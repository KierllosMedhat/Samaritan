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
    public class DonorController : ControllerBase
    {
        private readonly IDonorRepository donorRepository;
        public DonorController(IDonorRepository donorRepository)
        {
            this.donorRepository = donorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetDonors()
        {
            var donors = await donorRepository.GetDonors();
            if (donors == null)
                return NotFound();
            return Ok(donors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDonorById(int id)
        {
            var donor = await donorRepository.GetDonorById(id);
            if (donor == null)
                return NotFound();
            return Ok(donor);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDonor([FromBody] Donor donor)
        {
            await donorRepository.CreateDonor(donor);
            return CreatedAtAction(nameof(GetDonorById), new { id = donor.Id }, donor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDonor(int id, [FromBody] Donor donor)
        {
            var existingDonor = await donorRepository.GetDonorById(id);
            if (id != donor.Id || existingDonor == null)
                return BadRequest();
            await donorRepository.UpdateDonor(donor);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonor(int id)
        {
            var donor = await donorRepository.GetDonorById(id);
            if (donor == null)
                return NotFound();
            await donorRepository.DeleteDonor(id);
            return Ok();
        }
    }
}