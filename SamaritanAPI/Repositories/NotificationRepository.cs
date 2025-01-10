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

        public async Task SendNotification(string userId, string message)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return;
            var notification = new Notification
            {
                UserId = userId,
                User = user,
                Text = message,
                IsRead = false
            };
            await CreateNotification(notification);
        }

        public async Task UpdateNotification(Notification notification)
        {
            context.Notifications.Update(notification);
            await context.SaveChangesAsync();
        }
    }
}
