using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SamaritanAPI.Models;
using SamaritanAPI.Repositories;

namespace SamaritanAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly PatientRepository patientRepository;
        public PatientController(PatientRepository patientRepository)
            => this.patientRepository = patientRepository;
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            var patients = await patientRepository.GetPatients();
            return Ok(patients);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await patientRepository.GetPatient(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpGet("{id}/requests")]
        public async Task<ActionResult<IEnumerable<Request>>> GetPatientRequests(int id)
        {
            var requests = await patientRepository.GetPatientRequests(id);
            if (requests == null)
            {
                return NotFound();
            }
            return Ok(requests);
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> CreatePatient(Patient patient)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            await patientRepository.CreatePatient(patient);
            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Patient>> UpdatePatient(int id, [FromBody] Patient patient)
        {
            var existingPatient = await patientRepository.GetPatient(id);
            if (existingPatient == null)
                NotFound();
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            await patientRepository.UpdatePatient(patient);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient(int id)
        {
            var patient = await patientRepository.GetPatient(id);
            if (patient == null)
            {
                return NotFound();
            }
            await patientRepository.DeletePatient(id);
            return NoContent();
        }
    }
}