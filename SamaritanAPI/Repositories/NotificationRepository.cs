using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SamaritanAPI.Authentication;
using SamaritanAPI.Data;
using SamaritanAPI.Models;
using SamaritanAPI.Repositories.Interfaces;

namespace SamaritanAPI.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;

        public NotificationRepository(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task CreateNotification(Notification notification)
        {
            await context.Notifications.AddAsync(notification);
            await context.SaveChangesAsync();
        }

        public async Task DeleteNotification(int notificationId)
        {
            var notification = await context.Notifications.FirstOrDefaultAsync(x => x.Id == notificationId);
            if (notification != null)
                context.Notifications.Remove(notification);
            await context.SaveChangesAsync();
        }

        public async Task<Notification?> GetNotificationById(int Id)
            => await context.Notifications.FirstOrDefaultAsync(notification => notification.Id == Id);

        public async Task<IEnumerable<Notification>?> GetUserNotifications(string userId)
        {
            var user = await context.Users.Include(x => x.Notifications).FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                return null;
            return user.Notifications;
        }

        public async Task NotifyAll(int requestId, string title, string body)
        {
            var request = await context.Requests.FirstAsync(req => req.Id == requestId);
            if(request is null)
                return;
            foreach(AppUser user in request.Diallers)
                await SendNotification(user.Id,title,body);
            
            foreach(AppUser user in request.Subleaders)
                await SendNotification(user.Id,title,body); 

            var admin = context.Users.First(u => u.Role == "Administrator");
            await SendNotification(admin.Id, title, body);
        }

        public async Task NotifySubleaders(int requestId, string title, string body)
        {
            var request = await context.Requests.FirstAsync(req => req.Id == requestId);
            if(request is null)
                return;

            foreach(AppUser user in request.Subleaders)
                await SendNotification(user.Id,title,body); 

            var admin = context.Users.First(u => u.Role == "Administrator");
            await SendNotification(admin.Id, title, body);
        }

        public async Task SendNotification(string userId, string title, string body)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return;
            var notification = new Notification
            {
                UserId = userId,
                User = user,
                Title = title,
                Body = body,
                IsRead = false
            };
            await CreateNotification(notification);
            await context.SaveChangesAsync();
        }

        public async Task UpdateNotification(Notification notification)
        {
            context.Notifications.Update(notification);
            await context.SaveChangesAsync();
        }
    }
}
