using SamaritanAPI.Models;

namespace SamaritanAPI.Repositories.Interfaces
{
    public interface ICallRepository
    {
        Task<IEnumerable<Call>> GetCalls();
        Task<IEnumerable<Call>> GetCallsByDonorId(int donorId);
        Task<Call?> GetCallById(int callId);
        Task DeleteCall(int callId);
        Task CreateCall(Call call);
        Task UpdateCall(Call call);

    }
}
