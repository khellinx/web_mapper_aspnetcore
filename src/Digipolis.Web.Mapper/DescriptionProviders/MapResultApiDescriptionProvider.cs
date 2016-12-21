using Digipolis.Web.Mapper.Extensions;
using Digipolis.Web.Mapper.Filters;
using Digipolis.Web.Mapper.Helpers;
using Digipolis.Web.Mapper.Options;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.DescriptionProviders
{
    /// <summary>
    /// An API description provider for the MapResult filter.
    /// This will set the response type in all api descriptions to a mapped type as configured in the mapping configuration or the filter itself.
    /// </summary>
    public class MapResultApiDescriptionProvider : IApiDescriptionProvider
    {
        /// <summary>
        /// A constructor accepting default decription provider options and an implementation for MapResultHelper
        /// </summary>
        /// <param name="options">Options for this API description provider.</param>
        /// <param name="helper">Implementation for the MapResultHelper which is used to get the mapped type.</param>
        public MapResultApiDescriptionProvider(IOptions<DescriptionProviderOptions<MapResultApiDescriptionProvider>> options, IMapResultHelper helper)
        {
            Order = options?.Value?.Order ?? 0;
            Helper = helper;
        }

        /// <summary>
        /// The order used when adding this API description providers to the collection of MVC API description providers.
        /// </summary>
        public int Order { get; private set; }

        /// <summary>
        /// The implementation of the MapResultHelper that is used to get the correct mapped type.
        /// </summary>
        public IMapResultHelper Helper { get; private set; }

        /// <summary>
        /// Sets the response type in the api description to a mapped type as configured in the mapping configuration or the filter itself.
        /// </summary>
        /// <param name="context">The context</param>
        public void OnProvidersExecuting(ApiDescriptionProviderContext context)
        {
            foreach (var apiDescription in context.Results)
            {
                var mapResultAttribute = apiDescription.ActionDescriptor.GetFilter<MapResultAttribute>();

                if (mapResultAttribute != null)
                {
                    foreach (var responseType in apiDescription.SupportedResponseTypes)
                    {
                        if (responseType.StatusCode != (int)HttpStatusCode.Created && responseType.StatusCode != (int)HttpStatusCode.OK)
                        {
                            break;
                        }

                        var sourceType = mapResultAttribute.SourceType ?? responseType.Type;
                        var destinationType = mapResultAttribute.DestinationType ?? Helper.MapType(sourceType, true);

                        responseType.Type = destinationType;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnProvidersExecuted(ApiDescriptionProviderContext context)
        {
        }
    }
}
