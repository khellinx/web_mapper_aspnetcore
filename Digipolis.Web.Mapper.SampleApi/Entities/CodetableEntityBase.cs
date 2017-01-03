using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.SampleApi.Entities
{
    public class CodetableEntityBase : EntityBase
    {
        public string Code { get; set; }
        public bool Disabled { get; set; }
        public int Sortindex { get; set; }
        public string Value { get; set; }
    }
}
