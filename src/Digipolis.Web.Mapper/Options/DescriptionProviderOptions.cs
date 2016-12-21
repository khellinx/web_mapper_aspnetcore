using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.Options
{
    /// <summary>
    /// A basic class containing basic options for Description Providers
    /// </summary>
    /// <typeparam name="TDescriptionProvider"></typeparam>
    public class DescriptionProviderOptions<TDescriptionProvider>
        where TDescriptionProvider : IApiDescriptionProvider
    {
        /// <summary>
        /// Defines the order that can be used when adding the provider to the list of MVC API description providers.
        /// </summary>
        public int Order { get; set; }
    }
}
