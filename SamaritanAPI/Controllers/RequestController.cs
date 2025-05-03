using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SamaritanAPI.Models;
using SamaritanAPI.Repositories.Interfaces;

namespace SamaritanAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepository requestRepository;

        public RequestController(IRequestRepository requestRepository)
        {
            this.requestRepository = requestRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Request>>> GetRequests()
        {
            var requests = await requestRepository.GetAllRequests();
            if(requests is null)
                return BadRequest();
            return Ok(requests);
        }

        [HttpGet("id")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await requestRepository.GetRequest(id);
            if(request is null)
                return NotFound();
            return Ok(request);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRequest([FromBody] Request request)
        {
            if(ModelState.IsValid)
            {
                bool requestCreated = await requestRepository.CreateRequest(request);
                if(!requestCreated)
                    return BadRequest();
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("id")]
        public async Task<ActionResult> UpdateRequest([FromBody] Request request, int id)
        {
            if(ModelState.IsValid && id == request.Id)
            {
                bool requestUpdated = await requestRepository.UpdateRequest(request);
                if(!requestUpdated)
                    return BadRequest();
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("id")]
        public async Task<ActionResult> DeleteRequest(int id)
        {
            if(ModelState.IsValid)
            {
                bool requestDeleted = await requestRepository.DeleteRequest(id);
                if(requestDeleted)
                    return Ok();
                else
                    return BadRequest();
            }
            return BadRequest();
        }
    }
}