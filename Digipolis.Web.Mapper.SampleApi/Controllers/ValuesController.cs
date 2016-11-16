using Digipolis.Web.Api;
using Digipolis.Web.Api.Models;
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
        public IActionResult GetAll(PageOptions pageOptions)
        {
            var result = Logic.GetAll(pageOptions.Page, pageOptions.PageSize);
            return Ok(result);
            //var pagedResult = pageOptions.ToPagedResult(result.Data, result.TotalEntitycount, nameof(GetAll), "Values");
            //return Ok(pagedResult);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(Logic.Get(id));
        }

        // POST api/values
        [HttpPost]
        [MapResult(typeof(ValueDetail))]
        public IActionResult Post([MapFromBody(typeof(ValueEdit))]Value value)
        {
            var isValid = ModelState.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            var result = Logic.Add(value);
            return CreatedAtAction(nameof(Get), result);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [MapResult(typeof(ValueDetail))]
        public IActionResult Put(int id, [MapFromBody(typeof(ValueEdit))]Value value)
        {
            var result = Logic.Update(value);
            return Ok(result);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Logic.Delete(id);
            return NoContent();
        }
    }
}
