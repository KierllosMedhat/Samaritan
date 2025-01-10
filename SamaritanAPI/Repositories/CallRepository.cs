using Microsoft.EntityFrameworkCore;
using SamaritanAPI.Data;
using SamaritanAPI.Models;
using SamaritanAPI.Repositories.Interfaces;

namespace SamaritanAPI.Repositories
{
    public class CallRepository : ICallRepository
    {
        private readonly ApplicationDbContext context;

        public CallRepository(ApplicationDbContext context)
            => this.context = context;

        public async Task CreateCall(Call call)
        {
            await context.Calls.AddAsync(call);
            await context.SaveChangesAsync();
        }

        public async Task DeleteCall(int callId)
        {
            var call = await context.Calls.FirstOrDefaultAsync(x => x.Id == callId);
            if (call != null) 
                context.Calls.Remove(call);
            await context.SaveChangesAsync();
        }

        public async Task<Call?> GetCallById(int callId)
        {
            var call = await context.Calls.FirstOrDefaultAsync(x => x.Id == callId);
            if(call != null)
                return call;
            else
                return null;
        }

        public async Task<IEnumerable<Call>> GetCalls()
            => await context.Calls.ToListAsync();

        public async Task<IEnumerable<Call>> GetCallsByDonorId(int donorId)
            => await context.Calls.Where(c => c.DonorId == donorId).ToListAsync();

        public async Task UpdateCall(Call call)
        {
            context.Calls.Update(call);
            await context.SaveChangesAsync();
        }
    }
}
