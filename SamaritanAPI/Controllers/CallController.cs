using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SamaritanAPI.Data;
using SamaritanAPI.Models;
using SamaritanAPI.Repositories.Interfaces;

namespace SamaritanAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CallController : ControllerBase
    {
        private readonly ICallRepository callRepository;

        public CallController(ICallRepository callRepository)
        {
            this.callRepository = callRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCalls()
        {
            var calls = await callRepository.GetCalls();
            return Ok(calls);
        }

        [HttpGet("donor/{donorId}")]
        public async Task<IActionResult> GetCallsByDonorId(int donorId)
        {
            var calls = await callRepository.GetCallsByDonorId(donorId);
            if (calls == null)
            {
                return NotFound();
            }
            return Ok(calls);
        }

        [HttpGet("{callId}")]
        public async Task<IActionResult> GetCallById(int callId)
        {
            var call = await callRepository.GetCallById(callId);
            if (call == null)
            {
                return NotFound();
            }
            return Ok(call);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCall([FromBody] Call call)
        {
            if(ModelState.IsValid)
            {
                await callRepository.CreateCall(call);
                return CreatedAtAction(nameof(GetCallById), new { callId = call.Id }, call);
            }
            ModelState.AddModelError("","Invalid Call Format!");
            return BadRequest(ModelState);
        }

        [HttpPost("{callId}")]
        public async Task<IActionResult> UpdateCall(int callId, [FromBody] Call call)
        {
            if(ModelState.IsValid)
            {
                var existingCall = await callRepository.GetCallById(callId);
                if (existingCall == null)
                {
                    return NotFound();
                }
                await callRepository.UpdateCall(call);
                return Ok(call);
            }
            ModelState.AddModelError("","Wrong Format!");
            return BadRequest(ModelState);
        }

        [HttpDelete("{callId}")]
        public async Task<ActionResult> DeleteCall(int callId)
        {
            var existingCall = await callRepository.GetCallById(callId);
            if (existingCall == null)
            {
                return NotFound();
            }
            await callRepository.DeleteCall(callId);
            return Ok();
        }
    }
}