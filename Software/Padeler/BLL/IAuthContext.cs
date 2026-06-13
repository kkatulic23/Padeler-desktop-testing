using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IAuthContext
    {
        int CurrentUserId { get; }
        string CurrentUsername { get; }
        bool IsLoggedIn { get; }

        void SetUser(int userId, string username);
        void Clear();
    }
}
