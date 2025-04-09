using SamaritanAPI.Models;

namespace SamaritanAPI.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>?> GetUserNotifications(string userId);
        Task<Notification?> GetNotificationById(int Id);
        Task CreateNotification(Notification notification);
        Task UpdateNotification(Notification notification);
        Task DeleteNotification(int notificationId);
        Task SendNotification(string userId, string title ,string body);
        Task NotifyAll(int requestId, string title, string body);
        Task NotifySubleaders(int requestId, string title, string body);
    }
}
