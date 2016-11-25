using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.UnitTests._Types
{
    public class PersonDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public PersonDetailModel Partner { get; set; }
        public IEnumerable<PersonDetailModel> Parents { get; set; }
    }
}
