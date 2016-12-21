using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.Options
{
    /// <summary>
    /// An options class for configuring the response mappings.
    /// </summary>
    public class MapResultOptions
    {
        private Dictionary<Type, Type> _mappings;
        private Dictionary<Type, Type> _genericMappings;

        /// <summary>
        /// Add a response mapping.
        /// </summary>
        /// <typeparam name="TSource">The source type to map from.</typeparam>
        /// <typeparam name="TDestination">The destination type to map to.</typeparam>
        public void AddMapping<TSource, TDestination>()
        {
            AddMapping(typeof(TSource), typeof(TDestination));
        }

        /// <summary>
        /// Add a response mapping.
        /// </summary>
        /// <param name="source">The source type to map from. If the type is generic, is must be a closed generic type.</param>
        /// <param name="destination">The destination type to map to. If the type is generic, is must be a closed generic type.</param>
        public void AddMapping(Type source, Type destination)
        {
            ValidateForMapping(source, nameof(source));
            ValidateForMapping(destination, nameof(destination));

            if (_mappings != null && _mappings.ContainsKey(source)) throw new ArgumentException("There is already a mapping defined for this source type.", nameof(source));

            if (_mappings == null) _mappings = new Dictionary<Type, Type>();

            _mappings.Add(source, destination);
        }

        private void ValidateForMapping(Type type, string argumentName)
        {
            if (type == null)
            {
                throw new NullReferenceException(argumentName);
            }
            if (type == typeof(void))
            {
                throw new ArgumentException("Source and destination types cannot be of type void.", argumentName);
            }

            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsGenericType && !type.IsConstructedGenericType)
            {
                throw new ArgumentException("If the source or destination is a generic type, the generic type arguments should be specified. Try to add a generic mapping instead.", argumentName);
            }
        }

        /// <summary>
        /// Check whether a mapping for a specified source type is defined.
        /// </summary>
        /// <param name="source">The source type to search for.</param>
        /// <returns>True if a mapping is already defined. Otherwise, false.</returns>
        public bool IsMappingDefined(Type source)
        {
            return _mappings.ContainsKey(source);
        }

        /// <summary>
        /// Get the mapping destination type for the specified source type.
        /// </summary>
        /// <param name="source">The source type to return the destination type for.</param>
        /// <returns>The destination type.</returns>
        public Type GetMapping(Type source)
        {
            return _mappings[source];
        }

        /// <summary>
        /// Add a generic mapping.
        /// </summary>
        /// <param name="source">The source type to map from. This type must be an open generic type.</param>
        /// <param name="destination">The destination type to map to. This type must be an open generic type.</param>
        /// <example>
        /// AddGenericMapping(typeof(List&lt;&gt;), typeof(MyCustomList&lt;&gt;)):
        /// This will add a generic mapping where responses of type List will be mapped to MyCustomList.
        /// The generic parameter inside List will also be mapped if a mapping configuration exists for that type.
        /// </example>
        public void AddGenericMapping(Type source, Type destination)
        {
            ValidateForGenericMapping(source, nameof(source));
            ValidateForGenericMapping(destination, nameof(destination));

            if (_genericMappings != null && _genericMappings.ContainsKey(source)) throw new ArgumentException("There is already a generic mapping defined for this source type.", nameof(source));

            if (_genericMappings == null) _genericMappings = new Dictionary<Type, Type>();

            _genericMappings.Add(source, destination);
        }

        private void ValidateForGenericMapping(Type type, string argumentName)
        {
            if (type == null)
            {
                throw new NullReferenceException(argumentName);
            }

            var typeInfo = type.GetTypeInfo();

            if (!typeInfo.IsGenericType)
            {
                throw new ArgumentException("Both the source as destination type should be Generic types.", argumentName);
            }
            if (type.GenericTypeArguments.Length != 0)
            {
                throw new ArgumentException("No generic type arguments should be given to both the source as destination type when using AddGenericMapping. Try adding a normal mapping instead.", argumentName);
            }
        }

        /// <summary>
        /// Check whether a generic mapping for a specified source type is defined.
        /// </summary>
        /// <param name="source">The source type to search for.</param>
        /// <returns>True if a mapping is already defined. Otherwise, false.</returns>
        public bool IsGenericMappingDefined(Type source)
        {
            return _genericMappings.ContainsKey(source);
        }

        /// <summary>
        /// Get the generic mapping destination type for the specified source type.
        /// </summary>
        /// <param name="source">The source type to return the destination type for.</param>
        /// <returns>The destination type.</returns>
        public Type GetGenericMapping(Type source)
        {
            return _genericMappings[source];
        }

        /// <summary>
        /// Try to add all mappings in an Automapper profile.
        /// </summary>
        /// <typeparam name="TProfile">The Automapper profile.</typeparam>
        public void TryAddMappingsFromAutoMapperProfile<TProfile>()
            where TProfile : Profile, new()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<TProfile>();
            });

            var typeMaps = config.GetAllTypeMaps();
            foreach (var typeMap in typeMaps)
            {
                try
                {
                    AddMapping(typeMap.SourceType, typeMap.DestinationType);
                }
                catch (Exception)
                {
                    // TODO: commented because options classes should have an empty constructor, thus a logger cannot be injected.
                    // Log a warning but don't throw back.
                    //_logger.LogWarning($"Could not add mapping from AutoMapper profile '{typeof(TProfile).Name}' for source type '{typeMap.SourceType.Name}' and destination type '{typeMap.DestinationType.Name}'.");
                }
            }
        }
    }
}
