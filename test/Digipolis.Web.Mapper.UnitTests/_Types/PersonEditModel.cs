using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.UnitTests._Types
{
    public class PersonEditModel
    {
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public int PartnerId { get; set; }
        public IEnumerable<int> ParentIds { get; set; }
    }
}
