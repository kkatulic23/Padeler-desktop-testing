using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface INotificationService // Filip Grgac
    {
        Task CheckAndShowNotificationAsync(int userId);
    }
}
