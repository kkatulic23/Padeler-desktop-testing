using BLL;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Padeler
{
    public class WinFormsNotificationPresenter : INotificationPresenter // Filip Grgac
    {
        private readonly Form _owner;

        public WinFormsNotificationPresenter(Form owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// Prikazuje push notifikaciju korisniku u obliku zasebne forme.
        /// Nakon korisničke potvrde, izvršava zadanu akciju za označavanje
        /// notifikacije kao pročitane.
        /// </summary>
        public void Show(Notification notification, Action onRead)
        {
            var form = new PushNotificationForm(notification);

            form.Owner = _owner;

            form.NotificationRead += (s, e) =>
            {
                onRead();
            };

            form.PositionOwner();

            form.Show(_owner);
        }
    }
}
