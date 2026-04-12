using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface INotificationPresenter // Filip Grgac
    {
        void Show(Notification notification, Action onRead);
    }
}
