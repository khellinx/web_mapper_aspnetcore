using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.SampleApi.Models
{
    public class FenomeenDetail
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public bool Disabled { get; set; }
        public int Sortindex { get; set; }
        public int? ParentId { get; set; }
    }
}
