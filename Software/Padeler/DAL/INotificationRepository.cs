using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetNotificationsAsync(int userId);
        Task MarkAsReadAsync(int notificationId);
    }
}
