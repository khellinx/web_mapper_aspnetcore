using Digipolis.Errors.Exceptions;
using Digipolis.Web.Mapper.SampleApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.SampleApi.Logic
{
    public class ValueLogic : IValueLogic
    {
        private static List<Value> _values = new List<Value>(new Value[]
        {
            new Value() { Id = 1, Code = "ONE", Description = "First value" },
            new Value() { Id = 2, Code = "TWO", Description = "Second value" },
            new Value() { Id = 3, Code = "THREE", Description = "Third value" },
            new Value() { Id = 4, Code = "FOUR", Description = "Fourth value" },
            new Value() { Id = 5, Code = "FIVE", Description = "Fifth value" }
        });

        public Value Get(int id)
        {
            var result = _values.FirstOrDefault(x => x.Id == id);

            if (result == null) throw new NotFoundException();

            return result;
        }

        public DataPage<Value> GetAll(int page, int pageLength)
        {
            var result = new DataPage<Value>();
            result.PageNumber = page;
            result.PageLength = pageLength;
            result.TotalEntityCount = _values.Count;

            result.Data = _values.Skip((page - 1) * pageLength).Take(pageLength);

            return result;
        }

        public Value Add(Value value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (_values.Count > 0)
            {
                value.Id = _values.Max(x => x.Id) + 1;
            }
            else
            {
                value.Id = 1;
            }

            _values.Add(value);

            return value;
        }

        public Value Update(Value value)
        {
            var result = _values.FirstOrDefault(x => x.Id == value.Id);

            if (result == null) throw new NotFoundException();

            result.Code = value.Code;
            result.Description = value.Description;

            return result;
        }

        public void Delete(int id)
        {
            _values.RemoveAll(x => x.Id == id);
        }
    }
}
