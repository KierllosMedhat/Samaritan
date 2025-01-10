using SamaritanAPI.Models;

namespace SamaritanAPI.Repositories.Interfaces
{
    public interface IServantCompanionRepository
    {
        Task<IEnumerable<ServantCompanion>> GetServantCompanions();
        Task<ServantCompanion?> GetServantCompanion(int id);
        Task<List<Note>> GetServantNotes(int ServantId);
        Task CreateServantCompanion(ServantCompanion servantCompanion);
        Task UpdateServantCompanion(ServantCompanion servantCompanion);
        Task DeleteServantCompanion(int id);
    }
}
