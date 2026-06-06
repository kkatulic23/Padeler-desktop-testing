using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class NotificationService : INotificationService // Filip Grgac
    {
        private readonly INotificationRepository _repo;
        private readonly INotificationPresenter _present;
        private const string SuccessfulMatch = "MATCH";

        [ExcludeFromCodeCoverage]
        public NotificationService(INotificationPresenter present) : this(new NotificationRepository(), present)
        {
        }

        public NotificationService(INotificationRepository repo, INotificationPresenter present)
        {
            _repo = repo;
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

            var unreadMatch = notifications.FirstOrDefault(x => !x.IsRead && x.Type == SuccessfulMatch);

            if (unreadMatch == null || _present == null)
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
                .FirstOrDefault(n => n.Type == SuccessfulMatch && !n.IsRead);

            if (matchNotification != null)
            {
                await _repo.MarkAsReadAsync(matchNotification.NotificationId);
            }
        }

    }
}
