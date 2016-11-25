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
    public class MapFromBodyApiDescriptionProvider : IApiDescriptionProvider
    {
        public MapFromBodyApiDescriptionProvider(IOptions<DescriptionProviderOptions<MapFromBodyApiDescriptionProvider>> options)
        {
            Order = options?.Value?.Order ?? 0;
        }

        public int Order { get; private set; }

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

        public void OnProvidersExecuted(ApiDescriptionProviderContext context)
        {
        }
    }
}
