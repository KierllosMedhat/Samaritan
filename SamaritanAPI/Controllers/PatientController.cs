using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SamaritanAPI.Models;
using SamaritanAPI.Repositories.Interfaces;

namespace SamaritanAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository patientRepository;
        public PatientController(IPatientRepository patientRepository)
            => this.patientRepository = patientRepository;
        
        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var patients = await patientRepository.GetPatients();
            return Ok(patients);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(int id)
        {
            var patient = await patientRepository.GetPatient(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpGet("{id}/requests")]
        public async Task<IActionResult> GetPatientRequests(int id)
        {
            var requests = await patientRepository.GetPatientRequests(id);
            if (requests == null)
            {
                return NotFound();
            }
            return Ok(requests);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatient(Patient patient)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("","Invalid Format!");
                return BadRequest(ModelState);
            }
            await patientRepository.CreatePatient(patient);
            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patient patient)
        {
            var existingPatient = await patientRepository.GetPatient(id);
            if (existingPatient == null)
                NotFound();
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("","Invalid Format!");
                return BadRequest(ModelState);
            }
            await patientRepository.UpdatePatient(patient);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await patientRepository.GetPatient(id);
            if (patient == null)
            {
                return NotFound();
            }
            await patientRepository.DeletePatient(id);
            return Ok();
        }
    }
}