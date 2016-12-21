using Digipolis.Web.Mapper.Extensions;
using Digipolis.Web.Mapper.Filters;
using Digipolis.Web.Mapper.ModelBinders;
using Digipolis.Web.Mapper.Options;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.DescriptionProviders
{
    /// <summary>
    /// An API description provider for the MapFromBody filter.
    /// This will set the body parameter type in all api descriptions to a mapped type when the MapFromBody is used.
    /// </summary>
    public class MapFromBodyApiDescriptionProvider : IApiDescriptionProvider
    {
        /// <summary>
        /// A constructor accepting default decription provider options.
        /// </summary>
        /// <param name="options">Options for this API description provider.</param>
        public MapFromBodyApiDescriptionProvider(IOptions<DescriptionProviderOptions<MapFromBodyApiDescriptionProvider>> options)
        {
            Order = options?.Value?.Order ?? 0;
        }

        /// <summary>
        /// The order used when adding this API description providers to the collection of MVC API description providers.
        /// </summary>
        public int Order { get; private set; }

        /// <summary>
        /// Sets the body parameter type in the api description to a mapped type specified in the MapFromBody filter.
        /// </summary>
        /// <param name="context">The context</param>
        public void OnProvidersExecuting(ApiDescriptionProviderContext context)
        {
            foreach (var apiDescription in context.Results)
            {
                ControllerParameterDescriptor mapFromBodyParameter;
                MapFromBodyAttribute mapFromBodyAttribute;
                apiDescription.ActionDescriptor.GetMapFromBodyParameter(out mapFromBodyParameter, out mapFromBodyAttribute);

                if (mapFromBodyParameter != null && mapFromBodyAttribute != null)
                {
                    var resultParameter = apiDescription.ParameterDescriptions.FirstOrDefault(x => x.Source.Id.Equals("Body") && x.Name == mapFromBodyParameter.Name);
                    if (resultParameter != null)
                    {
                        resultParameter.Type = mapFromBodyAttribute.SourceType;
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
