using Microsoft.EntityFrameworkCore;
using SamaritanAPI.Data;
using SamaritanAPI.Models;
using SamaritanAPI.Repositories.Interfaces;

namespace SamaritanAPI.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext context;

        public PatientRepository(ApplicationDbContext context)
            => this.context = context;

        public async Task CreatePatient(Patient patient)
        {
            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();
        }

        public async Task DeletePatient(int id)
        {
            var patient = await context.Patients.FirstOrDefaultAsync(x => x.Id == id);
            if (patient != null)
            {
                context.Patients.Remove(patient);
                context.SaveChanges();
            }
        }

        public async Task<Patient?> GetPatient(int id)
            => await context.Patients.FirstOrDefaultAsync(pat => pat.Id == id);

        public async Task<IEnumerable<Patient>> GetPatients()
            => await context.Patients.ToListAsync();

        public async Task UpdatePatient(Patient patient)
        {
            context.Patients.Update(patient);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Request>?> GetPatientRequests(int patientId)
        {
            var patient = await context.Patients.Include(x => x.Requests).FirstOrDefaultAsync(x => x.Id == patientId);
            if (patient != null)
            {
                return patient.Requests;
            }
            return null;
        }
    }
}
