using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SamaritanAPI.Models;
using SamaritanAPI.Repositories.Interfaces;

namespace SamaritanAPI.Controllers
{
    [Authorize]
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
    
        [HttpGet("{id}/requestStatus")]
        public async Task<ActionResult> GetRequestStatus(int id)
        {
            if(ModelState.IsValid)
            {
                var requestStatus = await requestRepository.GetRequestStatus(id);
                if(requestStatus == null)
                    return NotFound();
                return Ok(requestStatus);
            }
            return BadRequest();
        }

        [HttpPut("update-timeline/{requestId}/{action}")]
        public async Task<ActionResult> UpdateTimelineAsync(int requestId, string action)
        {
            if (ModelState.IsValid)
            {
                var timelineUpdated = await requestRepository.UpdateTimelineAsync(requestId, action);
                if(timelineUpdated)
                    return Ok();
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPut("assign-subleader/{requestId}/{subleaderId}")]
        public async Task<ActionResult> AssignSubleader(int requestId, string subleaderId)
        {
            if (ModelState.IsValid)
            {
                var subleaderAssigned = await requestRepository.AssignSubleader(requestId,subleaderId);
                if(subleaderAssigned)
                    return Ok();
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPut("assign-dialler/{requestId}/{diallerId}")]
        public async Task<ActionResult> AssignDialler(int requestId, string diallerId)
        {
            if (ModelState.IsValid)
            {
                var diallerAssigned = await requestRepository.AssignDialler(requestId, diallerId);
                if(diallerAssigned)
                    return Ok();
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPut("{id}/no-dialler-found")]
        public async Task<ActionResult> RaiseNoDiallerFound(int id)
        {
            var alertRaised = await requestRepository.RaiseNoDiallerFound(id);
            if(alertRaised)
                return Ok();
            return NotFound();
        }

        
        [HttpPut("assign-donor/{requestId}/{donorId}")]
        public async Task<ActionResult> AssignDonor(int requestId, int donorId)
        {
            if (ModelState.IsValid)
            {
                var donorAssigned = await requestRepository.AssignDonor(requestId, donorId);
                if(donorAssigned)
                    return Ok();
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPut("{id}/no-donor-found")]
        public async Task<ActionResult> RaiseNoDonorFound(int id)
        {
            var alertRaised = await requestRepository.RaiseNoDonorFound(id);
            if(alertRaised)
                return Ok();
            return NotFound();
        }
        
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}/close-request")]
        public async Task<ActionResult> CloseRequest(int id)
        {
            var requestClosed = await requestRepository.CloseRequest(id);
            if (requestClosed)
                return Ok();
            return NotFound();
        }
    }
}