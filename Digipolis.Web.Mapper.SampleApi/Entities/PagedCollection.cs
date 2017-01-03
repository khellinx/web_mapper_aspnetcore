using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.SampleApi.Entities
{
    public class PagedCollection<T>
    {
        public IEnumerable<T> Data { get; set; }
        public Page Page { get; set; }
        public long TotalCount { get; set; }
    }
}
