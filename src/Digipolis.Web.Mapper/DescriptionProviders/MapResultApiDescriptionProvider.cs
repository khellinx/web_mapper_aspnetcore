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
    public class MapResultApiDescriptionProvider : IApiDescriptionProvider
    {
        public MapResultApiDescriptionProvider(IOptions<DescriptionProviderOptions<MapResultApiDescriptionProvider>> options, IMapResultHelper helper)
        {
            Order = options?.Value?.Order ?? 0;
            Helper = helper;
        }

        public int Order { get; private set; }
        public IMapResultHelper Helper { get; private set; }

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

        public void OnProvidersExecuted(ApiDescriptionProviderContext context)
        {
        }
    }
}
