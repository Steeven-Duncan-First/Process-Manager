using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenSourceMock.Models;

namespace GoldenSourceMock.Services
{
    public class MockNotificationService : IMockNotificationService
    {
        private static List<Notification> _notifications = new List<Notification>();

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        {
            await Task.Delay(300); // Simuler un délai réseau
            return _notifications.Where(n => n.UserId == userId).OrderByDescending(n => n.CreatedAt);
        }

        public async Task<Notification> CreateNotificationAsync(string userId, string message, string type)
        {
            await Task.Delay(200);
            var notification = new Notification
            {
                Id = _notifications.Count + 1,
                UserId = userId,
                Message = message,
                Type = type,
                IsRead = false,
                CreatedAt = DateTime.Now
            };
            _notifications.Add(notification);
            return notification;
        }

        public async Task MarkNotificationAsReadAsync(int notificationId)
        {
            await Task.Delay(200);
            var notification = _notifications.FirstOrDefault(n => n.Id == notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.Now;
            }
        }

        public async Task DeleteNotificationAsync(int notificationId)
        {
            await Task.Delay(200);
            _notifications.RemoveAll(n => n.Id == notificationId);
        }
    }
} 