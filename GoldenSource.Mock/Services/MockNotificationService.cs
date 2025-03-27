using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenSource.Mock.Models;

namespace GoldenSource.Mock.Services
{
    public interface IMockNotificationService
    {
        Task<List<Notification>> GetUserNotificationsAsync(string userId);
        Task<Notification> CreateNotificationAsync(string userId, string title, string message, string type);
        Task<bool> MarkNotificationAsReadAsync(int notificationId);
        Task<int> GetUnreadNotificationCountAsync(string userId);
    }

    public class MockNotificationService : IMockNotificationService
    {
        public async Task<List<Notification>> GetUserNotificationsAsync(string userId)
        {
            // Simuler un délai réseau
            await Task.Delay(300);

            return MockData.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedDate)
                .Take(20)
                .ToList();
        }

        public async Task<Notification> CreateNotificationAsync(string userId, string title, string message, string type)
        {
            // Simuler un délai réseau
            await Task.Delay(400);

            var notification = new Notification
            {
                Id = MockData.Notifications.Count + 1,
                UserId = userId,
                Title = title,
                Message = message,
                CreatedDate = DateTime.Now,
                IsRead = false,
                Type = type
            };

            MockData.Notifications.Add(notification);
            return notification;
        }

        public async Task<bool> MarkNotificationAsReadAsync(int notificationId)
        {
            // Simuler un délai réseau
            await Task.Delay(300);

            var notification = MockData.Notifications.FirstOrDefault(n => n.Id == notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                return true;
            }
            return false;
        }

        public async Task<int> GetUnreadNotificationCountAsync(string userId)
        {
            // Simuler un délai réseau
            await Task.Delay(200);

            return MockData.Notifications.Count(n => n.UserId == userId && !n.IsRead);
        }
    }
} 