using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.UnitTests._Types
{
    public class PersonEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public PersonEntity Partner { get; set; }
        public IList<PersonEntity> Parents { get; set; }
    }
}