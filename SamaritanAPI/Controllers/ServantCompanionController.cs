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
    public class ServantCompanionController : ControllerBase
    {
        private readonly IServantCompanionRepository servantCompanionRepository;

        public ServantCompanionController(IServantCompanionRepository servantCompanionRepository)
            => this.servantCompanionRepository = servantCompanionRepository;

        [HttpGet]
        public async Task<IActionResult> GetServantCompanions()
        {
            var servantCompanions = await servantCompanionRepository.GetServantCompanions();
            if (servantCompanions is null)
            {
                return NotFound();
            }
            return Ok(servantCompanions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServantCompanion(int id)
        {
            var servantCompanion = await servantCompanionRepository.GetServantCompanion(id);
            if (servantCompanion is null)
            {
                return NotFound();
            }
            return Ok(servantCompanion);
        }

        [HttpGet("{ServantId}/notes")]
        public async Task<IActionResult> GetServantNotes(int ServantId)
        {
            var servantNotes = await servantCompanionRepository.GetServantNotes(ServantId);
            if (servantNotes is null)
            {
                return NotFound();
            }
            return Ok(servantNotes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateServantCompanion([FromBody] ServantCompanion servantCompanion)
        {
            if(ModelState.IsValid)
            {
                await servantCompanionRepository.CreateServantCompanion(servantCompanion);
                return Ok();
            }
            ModelState.AddModelError("","Invalid Format!");
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServantCompanion(int id, [FromBody] ServantCompanion servantCompanion)
        {
            if(ModelState.IsValid)
            {
                var existingServant = await servantCompanionRepository.GetServantCompanion(id);
                if (id != servantCompanion.Id || existingServant is null)
                {
                    return NotFound();
                }
                await servantCompanionRepository.UpdateServantCompanion(servantCompanion);
                return Ok();
            }
            ModelState.AddModelError("","Invalid Format!");
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServantCompanion(int id)
        {
            var existingServant = await servantCompanionRepository.GetServantCompanion(id);
            if (existingServant is null)
            {
                return NotFound();
            }
            await servantCompanionRepository.DeleteServantCompanion(id);
            return Ok();
        }
    }
}