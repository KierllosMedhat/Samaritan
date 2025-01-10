using SamaritanAPI.Models;

namespace SamaritanAPI.Repositories.Interfaces
{
    public interface IDonorRepository
    {
        Task<IEnumerable<Donor>> GetDonors();
        Task<Donor?> GetDonorById(int id);
        Task CreateDonor(Donor donor);
        Task UpdateDonor(Donor donor);
        Task DeleteDonor(int id);
    }
}
