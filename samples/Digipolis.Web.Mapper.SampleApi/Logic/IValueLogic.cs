using Digipolis.Web.Mapper.SampleApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.SampleApi.Logic
{
    public interface IValueLogic
    {
        Value Get(int id);
        DataPage<Value> GetAll(int page, int pageLength);
        Value Add(Value value);
        Value Update(Value value);
        void Delete(int id);
    }
}
