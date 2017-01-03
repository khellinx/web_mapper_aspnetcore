using Digipolis.Web.Api;
using Digipolis.Web.Api.ApiExplorer;
using Digipolis.Web.Mapper.Filters;
using Digipolis.Web.Mapper.ModelBinders;
using Digipolis.Web.Mapper.SampleApi.Entities;
using Digipolis.Web.Mapper.SampleApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.SampleApi.Controllers
{
    [Route("api/[controller]")]
    [MapResult]
    public class FenomenenController : Controller
    {
        public FenomenenController(ILogger<FenomenenController> logger)
        {
        }

        // GET /api/fenomenen
        [HttpGet]
        [ProducesDefaultResponses((int)HttpStatusCode.OK, SuccessResponseType = typeof(PagedCollection<Fenomeen>))]
        public async Task<IActionResult> GetAll([FromQuery]PageSortOptions pageQuery)
        {
            return Ok(null);
        }

        // GET /api/fenomenen/2
        [HttpGet("{id}")]
        [ProducesDefaultResponses((int)HttpStatusCode.OK, SuccessResponseType = typeof(Fenomeen))]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            return Ok(null);
        }

        // POST /api/fenomenen
        [HttpPost]
        //[ValidateModelState]
        [ProducesDefaultResponses((int)HttpStatusCode.Created, SuccessResponseType = typeof(Fenomeen))]
        public async Task<IActionResult> Add([MapFromBody(typeof(FenomeenEdit))]Fenomeen entity)
        {
            return Created("api/fenomenen/1", null);
        }

        // PUT /api/fenomenen/2
        [HttpPut("{id}")]
        //[ValidateModelState]
        [ProducesDefaultResponses((int)HttpStatusCode.OK, SuccessResponseType = typeof(Fenomeen))]
        public async Task<IActionResult> Update([FromRoute]int id, [MapFromBody(typeof(FenomeenEdit))]Fenomeen entity)
        {
            return Ok(null);
        }

        [HttpDelete("{id}")]
        [ProducesDefaultResponses((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            return NoContent();
        }
    }
}
