using Microsoft.EntityFrameworkCore;
using SamaritanAPI.Data;
using SamaritanAPI.Models;
using SamaritanAPI.Repositories.Interfaces;

namespace SamaritanAPI.Repositories
{
    public class ServantCompanionRepository : IServantCompanionRepository
    {
        private readonly ApplicationDbContext context;

        public ServantCompanionRepository(ApplicationDbContext context)
            => this.context = context;
        

        public async Task CreateServantCompanion(ServantCompanion servantCompanion)
        {
            await context.ServantCompanions.AddAsync(servantCompanion);
            await context.SaveChangesAsync();
        }

        public async Task DeleteServantCompanion(int id)
        {
            var servantCompanion = await context.ServantCompanions.FirstOrDefaultAsync(x => x.Id == id);
            if (servantCompanion != null) 
            {
                context.ServantCompanions.Remove(servantCompanion);
                context.SaveChanges();
            }
        }

        public async Task<ServantCompanion?> GetServantCompanion(int id)
            => await context.ServantCompanions.FirstOrDefaultAsync(ser => ser.Id == id);

        public async Task<IEnumerable<ServantCompanion>> GetServantCompanions()
            => await context.ServantCompanions.ToListAsync();

        public async Task<List<Note>> GetServantNotes(int ServantId)
        {
            var servantCompanion = await context.ServantCompanions.Include(x => x.Notes).FirstOrDefaultAsync(x => x.Id == ServantId);
            if (servantCompanion is null)
                return new List<Note>();
            return servantCompanion.Notes;
        }

        public async Task UpdateServantCompanion(ServantCompanion servantCompanion)
        {
            context.ServantCompanions.Update(servantCompanion);
            await context.SaveChangesAsync();
        }
    }
}
