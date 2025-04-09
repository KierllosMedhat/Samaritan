using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SamaritanAPI.Authentication;
using SamaritanAPI.Data;
using SamaritanAPI.Models;
using SamaritanAPI.Models.Types;
using SamaritanAPI.Repositories.Interfaces;

namespace SamaritanAPI.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly NotificationRepository notificationRepository;

        public RequestRepository(ApplicationDbContext context, 
            UserManager<AppUser> userManager,
            NotificationRepository notificationRepository)
        {
            this.notificationRepository = notificationRepository;
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<Request>> GetAllRequests()
            => await context.Requests.ToListAsync();

        public async Task<Request?> GetRequest(int requestId)
        {
            var request = await context.Requests.FirstOrDefaultAsync(req => req.Id == requestId);
            if(request is null) return null;
            return request;
        }
        public async Task CreateRequest(Request request)
        {
            request.RequestStatus = RequestStatus.Pending;
            await context.Requests.AddAsync(request);
            await context.SaveChangesAsync();
            var admin = context.Users.First(u => u.Role == "Administrator");
            await notificationRepository.SendNotification(admin.Id, $"{request.Id}: Request Created", $"Request {request.Id} Successfully Created at {DateTime.UtcNow}");
            await UpdateTimelineAsync(request.Id, "Request Created");
        }

        public async Task DeleteRequest(int requestId)
        {
            var request = await context.Requests.FirstAsync(req => req.Id == requestId );
            if (request == null)
                return;
            context.Requests.Remove(request);
            await context.SaveChangesAsync();
            var admin = context.Users.First(u => u.Role == "Administrator");
            await notificationRepository.SendNotification(admin.Id, $"{requestId}: Request Deleted", $"Request {request.Id} Was Deleted at {DateTime.UtcNow}");
        }

        public async Task UpdateRequest(Request request)
        {
            var req = await context.Requests.FirstAsync(req => req.Id == request.Id);
            if (req is not null)
            {
                req = request;
                context.Requests.Update(req);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> UpdateTimelineAsync(int requestId, string action)
        {
            var request = await context.Requests.FindAsync(requestId);
            if (request is null) return false;

            // append new action to the timeline
            request.Timeline += $"[{DateTime.UtcNow}] {action} by: {userManager.GetUserName}\n";
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignSubleader(int requestId, string subleaderId)
        {
            var request = await context.Requests.Include(r => r.Subleaders).FirstAsync(r => r.Id == requestId);
            if (request is null) 
                return false;
            var leader = await context.Users.FindAsync(subleaderId);
            if(leader == null)
                return false;
            request.Subleaders.Add(leader);
            request.RequestStatus = RequestStatus.SubleaderAssigned; 
            context.Requests.Update(request);
            await context.SaveChangesAsync();
            await UpdateTimelineAsync(requestId, $"Subleader (User ID: {subleaderId}) assigned to Request.");
            await notificationRepository.SendNotification(subleaderId, $"New Request: {requestId}", $"You've been assigned to request {requestId} at {DateTime.UtcNow}");
            return true;
        }

        public async Task<bool> AssignDialler(int requestId, string diallerId)
        {
            var request = await context.Requests.Include(r => r.Diallers).FirstAsync(r => r.Id == requestId);
            if (request is null)
                return false;
            var dialler = await context.Users.FindAsync(diallerId);
            if(dialler is null)
                return false;
            request.Diallers.Add(dialler);
            request.RequestStatus = RequestStatus.DiallerAssigned; 
            context.Requests.Update(request);
            await context.SaveChangesAsync();
            await UpdateTimelineAsync(requestId, $"Dialler (User ID: {diallerId}) assigned to Request.");
            await notificationRepository.SendNotification(diallerId, $"New Request: {requestId}", $"You've been assigned to request {requestId} at {DateTime.UtcNow}");
            return true;
        }

        public async Task<bool> RaiseNoDiallerFound(int requestId)
        {
            var req = await context.Requests.FirstAsync(req => req.Id == requestId);
            if(req is null)
                return false;
            req.RequestStatus = RequestStatus.NoDiallerFound;
            context.Requests.Update(req);
            await context.SaveChangesAsync();
            await UpdateTimelineAsync(requestId, $"Raised No Diallers Found!");
            var admin = context.Users.First(u => u.Role == "Administrator");
            await notificationRepository.SendNotification(admin.Id, $"{requestId}: No Diallers Found", $"No Diallers were found for the request {requestId}, at {DateTime.UtcNow}");
            return true;
        }

        public async Task<bool> AssignDonor(int requestId, int donorId)
        {
            var request = await context.Requests.FindAsync(requestId);
            if(request is null)
                return false;
            var donor = await context.Donors.FindAsync(donorId);
            if(donor is null)
                return false;
            request.DonorId = donorId;
            request.Donor = donor;
            context.Requests.Update(request);
            await context.SaveChangesAsync();
            await UpdateTimelineAsync(requestId, $"Donor ({donorId}) assigned to Request");
            await notificationRepository.NotifyAll(requestId, $"{requestId}: Donor Assigned", $"Donor {donorId}, was assigned to request {requestId} at {DateTime.UtcNow}");
            return true;
        }

        public async Task<bool> RaiseNoDonorFound(int requestId)
        {
            var request = await context.Requests.FindAsync(requestId);
            if(request is null)
                return false;
            request.RequestStatus = RequestStatus.NoDonorFound;
            context.Requests.Update(request);
            await context.SaveChangesAsync();
            await UpdateTimelineAsync(requestId, $"Raised No Donors Found!");
            await notificationRepository.NotifySubleaders(requestId, $"{requestId}:No Donor Found", $"No Donor Was Found By Dialler {userManager.GetUserIdAsync} at {DateTime.UtcNow}");
            return true;
        }
        public async Task<bool> CloseRequest(int requestId)
        {
            var request = await context.Requests.FirstOrDefaultAsync(req => req.Id == requestId);
            if(request is null)
                return false;
            request.RequestStatus = RequestStatus.Completed;
            context.Requests.Update(request);
            await context.SaveChangesAsync();
            await UpdateTimelineAsync(requestId, $"Request Closed!");
            await notificationRepository.NotifyAll(requestId, $"{requestId}: Request Closed",$"{requestId} closed at {DateTime.UtcNow}");
            return true;
        }
        public async Task<RequestStatus?> GetRequestStatus(int requestId)
        {
            var request = await context.Requests.FirstOrDefaultAsync(req => req.Id == requestId);
            if(request is null)
                return null;
            return request.RequestStatus;
        }
    }
}