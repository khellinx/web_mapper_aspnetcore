using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.SampleApi.Entities
{
    public class DataPage<TEntity>
    {
        public int PageNumber { get; set; }
        public int PageLength { get; set; }
        public long TotalEntityCount { get; set; }

        public IEnumerable<TEntity> Data { get; set; }
    }
}
