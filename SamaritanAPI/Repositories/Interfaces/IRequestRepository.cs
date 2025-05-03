using SamaritanAPI.Models;
using SamaritanAPI.Models.Types;

namespace SamaritanAPI.Repositories.Interfaces
{
    public interface IRequestRepository
    {
        Task<IEnumerable<Request>> GetAllRequests();
        Task<Request?> GetRequest(int requestId);
        Task<bool> CreateRequest(Request request);
        Task<bool> UpdateRequest(Request request);
        Task<bool> DeleteRequest(int requestId);

        Task<RequestStatus?> GetRequestStatus(int requestId);
        Task<bool> UpdateTimelineAsync(int requestId, string action);
        Task<bool> AssignSubleader(int requestId, string subleaderId);
        Task<bool> AssignDialler(int requestId, string diallerId);
        Task<bool> RaiseNoDiallerFound(int requestId);
        Task<bool> AssignDonor(int requestId, int donorId);
        Task<bool> RaiseNoDonorFound(int requestId);
        Task<bool> CloseRequest(int requestId);
    }
}
