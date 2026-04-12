using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class NotificationService : INotificationService // Filip Grgac
    {
        private readonly NotificationRepository _repo = new NotificationRepository();
        private readonly INotificationPresenter _present;

        public NotificationService(INotificationPresenter present)
        {
            _present = present;
        }

        /// <summary>
        /// Provjerava postoje li nepročitane MATCH notifikacije za korisnika
        /// te prikazuje prvu pronađenu notifikaciju kroz prezentacijski sloj.
        /// Nakon korisničke potvrde, notifikacija se označava kao pročitana.
        /// </summary>
        public async Task CheckAndShowNotificationAsync(int userId)
        {
            var notifications = await _repo.GetNotificationsAsync(userId);

            var unreadMatch = notifications.FirstOrDefault(x => !x.IsRead && x.Type == "MATCH");

            if (unreadMatch == null)
            {
                return;
            }

            _present.Show(unreadMatch, async () => await _repo.MarkAsReadAsync(unreadMatch.NotificationId));
        }

        /// <summary>
        /// Označava najnoviju nepročitanu MATCH notifikaciju korisnika kao pročitanu.
        /// Metoda se koristi za scenarije u kojima se notifikacija potvrđuje kroz MessageBox.
        /// </summary>
        public async Task MarkLatestMatchAsReadAsync(int userId)
        {
            var notifications = await _repo.GetNotificationsAsync(userId);

            var matchNotification = notifications
                .FirstOrDefault(n => n.Type == "MATCH" && !n.IsRead);

            if (matchNotification != null)
            {
                await _repo.MarkAsReadAsync(matchNotification.NotificationId);
            }
        }

    }
}
