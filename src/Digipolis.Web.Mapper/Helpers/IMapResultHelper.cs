using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.Helpers
{
    public interface IMapResultHelper
    {
        Type MapType(Type source, bool useSourceIfMappingNotFound);
    }
}
