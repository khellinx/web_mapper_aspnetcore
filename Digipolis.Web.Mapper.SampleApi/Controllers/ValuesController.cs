using Digipolis.Web.Api;
using Digipolis.Web.Mapper.Filters;
using Digipolis.Web.Mapper.ModelBinders;
using Digipolis.Web.Mapper.SampleApi.Entities;
using Digipolis.Web.Mapper.SampleApi.Logic;
using Digipolis.Web.Mapper.SampleApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.SampleApi.Controllers
{
    [Route("api/[controller]")]
    [MapResult]
    public class ValuesController : Controller
    {
        public ValuesController(IValueLogic logic)
        {
            Logic = logic;
        }

        public IValueLogic Logic { get; private set; }

        // GET api/values
        [HttpGet]
        [ProducesResponseType(typeof(DataPage<Value>), 200)]
        public IActionResult GetAll(PageOptions pageOptions)
        {
            var result = Logic.GetAll(pageOptions.Page, pageOptions.PageSize);
            return Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Value), 200)]
        public IActionResult Get(int id)
        {
            return Ok(Logic.Get(id));
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(Value), 201)]
        public IActionResult Post([MapFromBody(typeof(ValueEdit))]Value value)
        {
            var result = Logic.Add(value);
            return CreatedAtAction(nameof(Get), result);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Value), 200)]
        public IActionResult Put(int id, [MapFromBody(typeof(ValueEdit))]Value value)
        {
            var result = Logic.Update(value);
            return Ok(result);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), 204)]
        public IActionResult Delete(int id)
        {
            Logic.Delete(id);
            return NoContent();
        }
    }
}
