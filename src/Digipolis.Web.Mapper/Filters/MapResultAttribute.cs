using AutoMapper;
using Digipolis.Web.Mapper.Extensions;
using Digipolis.Web.Mapper.Helpers;
using Digipolis.Web.Mapper.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class MapResultAttribute : Attribute, IFilterFactory
    {
        private MapResultFilter _filterInstance;

        /// <summary>
        /// Map the result.
        /// The Source type will be set to the original type of the result.
        /// The Destination type will be set to whatever is defined in the MapResult mapping configuration.
        /// </summary>
        public MapResultAttribute()
        {
            SourceType = null;
            DestinationType = null;
        }

        /// <summary>
        /// Map the result.
        /// The Source type will be set to the original type of the result.
        /// </summary>
        /// <param name="to">The type to map the result to.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="to"/> is a generic type but not constructed. E.g: typeof(List<>).</exception>
        public MapResultAttribute(Type to)
        {
            ValidateType(to, nameof(to));

            SourceType = null;
            DestinationType = to;
        }

        /// <summary>
        /// Map the result.
        /// </summary>
        /// <param name="from">The type to map the result from.</param>
        /// <param name="to">The type to map the result to.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="from"/> or <paramref name="to"/> is a generic type but not constructed. E.g: typeof(List<>).</exception>
        public MapResultAttribute(Type from, Type to)
        {
            ValidateType(from, nameof(from));
            ValidateType(to, nameof(to));

            SourceType = from;
            DestinationType = to;
        }

        /// <summary>
        /// The type to map the result from.
        /// </summary>
        public Type SourceType { get; private set; }
        /// <summary>
        /// The type to map the result to.
        /// </summary>
        public Type DestinationType { get; private set; }
        /// <summary>
        /// Determines if the mapping is executed, even when Source and Destination types are equal.
        /// </summary>
        public bool MapIfTypesMatch { get; set; }

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            _filterInstance = new MapResultFilter(serviceProvider.GetService<IMapResultHelper>(), serviceProvider.GetService<IMapper>(), SourceType, DestinationType, MapIfTypesMatch);
            return _filterInstance;
        }

        private void ValidateType(Type type, string argumentName)
        {
            if (type == null)
            {
                // Null values are valid but the underlying tests would fail, so we'll just return here.
                return;
            }

            if (type == typeof(void))
            {
                throw new ArgumentException("Void is not a valid MapResult type.");
            }

            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsGenericType && !type.IsConstructedGenericType)
            {
                throw new ArgumentException("If the type is a generic type, the generic type arguments should be specified. Try to add a generic mapping to the MapResult configuration instead.", argumentName);
            }
        }

        private class MapResultFilter : IResultFilter
        {
            public MapResultFilter(IMapResultHelper helper, IMapper mapper, Type sourceType, Type destinationType, bool mapIfTypesMatch)
            {
                Helper = helper;
                Mapper = mapper;
                SourceType = sourceType;
                DestinationType = destinationType;
                MapIfTypesMatch = mapIfTypesMatch;
            }

            public IMapResultHelper Helper { get; private set; }
            public IMapper Mapper { get; private set; }
            public Type SourceType { get; private set; }
            public Type DestinationType { get; private set; }
            public bool MapIfTypesMatch { get; private set; }

            public void OnResultExecuting(ResultExecutingContext context)
            {
                // Make sure only one MapResultFilter is applied.
                // The one with the highest scope and lowest order should be used (which is what the GetFilter extension method returns).
                var filter = context.ActionDescriptor.GetFilter<MapResultAttribute>();
                if (filter._filterInstance != this) return;

                // Get the original result
                var result = context.Result as ObjectResult;

                // Only map results that have an ok status
                if (result == null || !result.StatusCode.HasValue || (result.StatusCode != (int)HttpStatusCode.Created && result.StatusCode != (int)HttpStatusCode.OK)) return;

                // Infer the source and destination types if they are not specified.
                if (SourceType == null)
                {
                    SourceType = result.Value.GetType();
                }
                if (DestinationType == null)
                {
                    DestinationType = Helper.MapType(SourceType, false);
                }

                // Map the result
                if (SourceType != DestinationType || MapIfTypesMatch)
                {
                    result.Value = Mapper.Map(result.Value, SourceType, DestinationType);
                }
            }

            public void OnResultExecuted(ResultExecutedContext context)
            {
            }
        }
    }
}
