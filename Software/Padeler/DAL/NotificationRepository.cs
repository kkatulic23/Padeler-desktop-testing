using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class NotificationRepository // Filip Grgac
    {
        private readonly ApiClient _api = new ApiClient();

        public async Task<List<Notification>> GetNotificationsAsync(int userId)
        {
            var res = await _api.GetAsync<NotificationListResponse>($"api/notifications/list.php?user_id={userId}");

            return res.Notifications ?? new List<Notification>();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            await _api.PostJsonAsync<object>("api/notifications/mark_read.php", new { notification_id = notificationId });
        }
    }
}
