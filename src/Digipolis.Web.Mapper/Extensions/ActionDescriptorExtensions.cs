using Digipolis.Web.Mapper.ModelBinders;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.Extensions
{
    /// <summary>
    /// A static class containing extension methods for ActionDescriptors
    /// </summary>
    public static class ActionDescriptorExtensions
    {
        /// <summary>
        /// Get a filter of the specified type <typeparamref name="TFilter"/> for the actiondescriptor.
        /// </summary>
        /// <typeparam name="TFilter">The type of filter to search for.</typeparam>
        /// <param name="actionDescriptor">The action descriptor on which to search for the filter.</param>
        /// <returns>A filter of type <typeparamref name="TFilter"/> that has been applied to the action. If more than one filter is applied, the one with the highest scope (Action > Controller > Global) followed by the lowest order will be returned.</returns>
        public static TFilter GetFilter<TFilter>(this ActionDescriptor actionDescriptor)
        {
            // Note: OrderBy(Descending) is deterministic for LINQ to Objects.
            //       Thus, it is safe to assume this will always return the same filter if multiple filters are applied with the same scope and order.
            var result = actionDescriptor.FilterDescriptors
                .OrderByDescending(desc => desc.Scope).ThenBy(desc => desc.Order)
                .Select(desc => desc.Filter)
                .OfType<TFilter>()
                .FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Get the first parameter with the MapFromBody attribute for the specified action descriptor.
        /// </summary>
        /// <param name="actionDescriptor">The action descriptor on which to search for the parameter.</param>
        /// <param name="parameterDescriptor">Parameter descriptor result.</param>
        /// <param name="mapFromBodyAttribute">MapFromBodyAttribute instance result.</param>
        public static void GetMapFromBodyParameter(this ActionDescriptor actionDescriptor, out ControllerParameterDescriptor parameterDescriptor, out MapFromBodyAttribute mapFromBodyAttribute)
        {
            ControllerParameterDescriptor resultParamDescriptor = null;
            MapFromBodyAttribute resultAttribute = null;

            var controllerParameters = actionDescriptor.Parameters.OfType<ControllerParameterDescriptor>();
            foreach (var param in controllerParameters)
            {
                resultAttribute = param.ParameterInfo.GetCustomAttribute<MapFromBodyAttribute>();
                if (resultAttribute != null)
                {
                    resultParamDescriptor = param;
                    break;
                }
            }

            parameterDescriptor = resultParamDescriptor;
            mapFromBodyAttribute = resultAttribute;
        }
    }
}
