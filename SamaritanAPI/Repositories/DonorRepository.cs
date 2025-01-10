using Microsoft.EntityFrameworkCore;
using SamaritanAPI.Data;
using SamaritanAPI.Models;
using SamaritanAPI.Repositories.Interfaces;

namespace SamaritanAPI.Repositories
{
    public class DonorRepository : IDonorRepository
    {
        private readonly ApplicationDbContext context;

        public DonorRepository(ApplicationDbContext context)
            => this.context = context;

        public async Task CreateDonor(Donor donor)
        {
            await context.Donors.AddAsync(donor);
            await context.SaveChangesAsync();
        }

        public async Task DeleteDonor(int id)
        {
            var donor = await context.Donors.FirstOrDefaultAsync(don => don.Id == id);
            if (donor != null)
                context.Donors.Remove(donor);
            await context.SaveChangesAsync();
        }

        public async Task<Donor?> GetDonorById(int id)
            => await context.Donors.FirstOrDefaultAsync(don => don.Id == id);

        public async Task<IEnumerable<Donor>> GetDonors()
            => await context.Donors.ToListAsync();

        public async Task UpdateDonor(Donor donor)
        {
            context.Donors.Update(donor);
            await context.SaveChangesAsync();
        }
    }
}
