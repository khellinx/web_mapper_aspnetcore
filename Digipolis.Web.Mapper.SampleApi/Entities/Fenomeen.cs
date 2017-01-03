using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.SampleApi.Entities
{
    public class Fenomeen : CodetableEntityBase
    {
        public int? ParentId { get; set; }
        public Fenomeen Parent { get; set; }
    }
}
