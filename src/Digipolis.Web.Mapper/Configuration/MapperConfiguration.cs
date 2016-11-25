using AutoMapper;
using Digipolis.Web.Mapper.DescriptionProviders;
using Digipolis.Web.Mapper.Helpers;
using Digipolis.Web.Mapper.ModelBinders;
using Digipolis.Web.Mapper.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.Configuration
{
    public static class MapperConfiguration
    {
        /// <summary>
        /// This will add AutoMapper and result and body mapping functionality.
        /// </summary>
        /// <param name="services">The IServiceCollection to be extended.</param>
        /// <param name="configureAutoMapper">Optional AutoMapper configuration.</param>
        /// <param name="configureActionResultMapper">Optional configuration options where result and body mappings can be defined.</param>
        /// <param name="apiDescriptionProviderOrder">Optional order of the Api Description Provider for the MapFromBody functionality. Keep this value as low as possible.</param>
        /// <returns>The service collection with added mapper functionality.</returns>
        public static IServiceCollection AddMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> configureAutoMapper = null, Action<MapResultOptions> configureActionResultMapper = null, int apiDescriptionProviderOrder = 0)
        {
            // Add the Body Modelbinder with mapping functionality
            services.Configure<MvcOptions>(options =>
            {
                // Get required dependencies for the MappedBodyModelBinderProvider
                var serviceProvider = services.BuildServiceProvider();
                var mapper = serviceProvider.GetRequiredService<IMapper>();
                var readerFactory = serviceProvider.GetRequiredService<IHttpRequestStreamReaderFactory>();

                // Get the index of the current BodyModelBinderProvider (which we'll replace).
                var current = options.ModelBinderProviders.OfType<BodyModelBinderProvider>().FirstOrDefault();
                var currentIndex = 0;
                if (current != null)
                {
                    currentIndex = options.ModelBinderProviders.IndexOf(current);
                    options.ModelBinderProviders.RemoveAt(currentIndex);
                }
                options.ModelBinderProviders.Insert(currentIndex, new MappedBodyModelBinderProvider(mapper, options.InputFormatters, readerFactory));
            });

            // Add the helper service
            services.AddSingleton<IMapResultHelper, MapResultHelper>();

            // Add the Api Description provider for the MapBody functionality.
            services.Configure<DescriptionProviderOptions<MapFromBodyApiDescriptionProvider>>(options => options.Order = apiDescriptionProviderOrder);
            services.TryAddEnumerable(ServiceDescriptor.Transient<IApiDescriptionProvider, MapFromBodyApiDescriptionProvider>());

            // Add the Api Description provider for the MapResult functionality.
            services.Configure<DescriptionProviderOptions<MapResultApiDescriptionProvider>>(options => options.Order = apiDescriptionProviderOrder);
            services.TryAddEnumerable(ServiceDescriptor.Transient<IApiDescriptionProvider, MapResultApiDescriptionProvider>());

            // Add automapper itself
            services.AddAutoMapper();
            if (configureAutoMapper != null)
            {
                services.Configure<IMapperConfigurationExpression>(configureAutoMapper);
            }

            // Configure options if provided
            if (configureActionResultMapper != null)
            {
                services.Configure<MapResultOptions>(configureActionResultMapper);
            }

            return services;
        }
    }
}
