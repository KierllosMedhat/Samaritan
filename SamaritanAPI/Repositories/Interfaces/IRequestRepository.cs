using SamaritanAPI.Models;
using SamaritanAPI.Models.Types;

namespace SamaritanAPI.Repositories.Interfaces
{
    public interface IRequestRepository
    {
        Task<IEnumerable<Request>> GetAllRequests();
        Task<Request> GetRequest(int requestId);
        Task CreateRequest(Request request);
        void UpdateRequest(Request request);
        Task DeleteRequest(int requestId);

        Task<RequestLevel> GetRequestLevel(int requestId);
        
    }
}
