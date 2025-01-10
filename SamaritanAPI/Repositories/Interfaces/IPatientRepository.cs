using SamaritanAPI.Models;

namespace SamaritanAPI.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetPatients();
        Task<Patient?> GetPatient(int id);

        Task<IEnumerable<Request>?> GetPatientRequests(int patientId);
        Task CreatePatient(Patient patient);
        Task UpdatePatient(Patient patient);
        Task DeletePatient(int id);
    }
}
