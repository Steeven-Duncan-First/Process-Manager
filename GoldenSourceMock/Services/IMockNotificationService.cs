using System.Collections.Generic;
using System.Threading.Tasks;
using GoldenSourceMock.Models;

namespace GoldenSourceMock.Services
{
    public interface IMockNotificationService
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);
        Task<Notification> CreateNotificationAsync(string userId, string message, string type);
        Task MarkNotificationAsReadAsync(int notificationId);
        Task DeleteNotificationAsync(int notificationId);
    }
} 