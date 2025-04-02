using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GoldenSourceMock.Services;
using GoldenSourceMock.Models;

namespace GoldenSourceMock.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IMockNotificationService _notificationService;

        public NotificationController(IMockNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            // Pour la démo, on utilise un ID utilisateur fixe
            var userId = "1";
            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return View(notifications);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationService.MarkNotificationAsReadAsync(id);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _notificationService.DeleteNotificationAsync(id);
            return Json(new { success = true });
        }
    }
} 