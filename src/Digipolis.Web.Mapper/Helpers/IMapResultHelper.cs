using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.Helpers
{
    /// <summary>
    /// Web mapper configuration helper.
    /// </summary>
    public interface IMapResultHelper
    {
        /// <summary>
        /// Determine which type to map to using the ActionResult mapping configuration.
        /// </summary>
        /// <param name="source">The source type to find a result mapping for.</param>
        /// <param name="useSourceIfMappingNotFound">If no mapping is found for the specified type, you can specify if the source type should be returned, otherwise an exception will be thrown.</param>
        /// <returns>A type to map the result to based on the specified source type.</returns>
        Type MapType(Type source, bool useSourceIfMappingNotFound);
    }
}
