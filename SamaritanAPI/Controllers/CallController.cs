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
        private readonly ApplicationDbContext context;
        private readonly ICallRepository callRepository;

        public CallController(ApplicationDbContext context, ICallRepository callRepository)
        {
            this.context = context;
            this.callRepository = callRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Call>>> GetCalls()
        {
            var calls = await callRepository.GetCalls();
            return Ok(calls);
        }

        [HttpGet($"{{donorId}}")]
        public async Task<ActionResult<List<Call>>> GetCallsByUserId(int donorId)
        {
            var calls = await callRepository.GetCallsByDonorId(donorId);
            if (calls == null)
            {
                return NotFound();
            }
            return Ok(calls);
        }

        [HttpGet($"{{callId}}")]
        public async Task<ActionResult<Call>> GetCallById(int callId)
        {
            var call = await callRepository.GetCallById(callId);
            if (call == null)
            {
                return NotFound();
            }
            return Ok(call);
        }

        [HttpPost]
        public async Task<ActionResult<Call>> CreateCall([FromBody] Call call)
        {
            await callRepository.CreateCall(call);
            return CreatedAtAction(nameof(GetCallById), new { callId = call.Id }, call);
        }

        [HttpPost("{callId}")]
        public async Task<ActionResult<Call>> UpdateCall(int callId, [FromBody] Call call)
        {
            var existingCall = await callRepository.GetCallById(callId);
            if (existingCall == null)
            {
                return NotFound();
            }
            call.Id = callId;
            await callRepository.UpdateCall(call);
            return Ok(call);
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