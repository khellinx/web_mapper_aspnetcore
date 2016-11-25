using Digipolis.Web.Mapper.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.Helpers
{
    public class MapResultHelper : IMapResultHelper
    {
        private readonly IOptions<MapResultOptions> _options;

        public MapResultHelper(IOptions<MapResultOptions> options)
        {
            _options = options;
        }

        /// <summary>
        /// Use the ActionResult mapping configuration to determine which type to map to.
        /// </summary>
        /// <param name="source">The source type to find a result mapping for.</param>
        /// <param name="useSourceIfMappingNotFound">If no mapping is found for the specified type, you can specify if the source type should be returned, otherwise an exception will be thrown.</param>
        /// <returns>A type to map the result to based on the specified source type.</returns>
        public Type MapType(Type source, bool useSourceIfMappingNotFound)
        {
            Type destination = null;

            if (_options?.Value == null) throw new InvalidOperationException("No mapping configuration defined for MapResult filter.");

            if (_options.Value.IsMappingDefined(source))
            {
                destination = _options.Value.GetMapping(source);
            }
            else if (source.IsConstructedGenericType && _options.Value.IsGenericMappingDefined(source.GetGenericTypeDefinition()))
            {
                var temp = _options.Value.GetGenericMapping(source.GetGenericTypeDefinition());
                var genericTypeArguments = new List<Type>();

                foreach (var type in source.GenericTypeArguments)
                {
                    genericTypeArguments.Add(MapType(type, true));
                }

                destination = temp.MakeGenericType(genericTypeArguments.ToArray());
            }
            else
            {
                if (useSourceIfMappingNotFound)
                    destination = source;
                else
                    throw new InvalidOperationException($"No mapping found for type '{source.FullName}'.");
            }

            return destination;
        }
    }
}
