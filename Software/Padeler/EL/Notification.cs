using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class Notification // Filip Grgac
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }

    public class NotificationListResponse
    {
        public bool Success { get; set; }
        public List<Notification> Notifications { get; set; }
    }

}
