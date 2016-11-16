using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.Options
{
    public class DescriptionProviderOptions<TDescriptionProvider>
        where TDescriptionProvider : IApiDescriptionProvider
    {
        public int Order { get; set; }
    }
}
