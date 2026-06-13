using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class AuthContextAdapter : IAuthContext
    {
        public int CurrentUserId => AuthContext.CurrentUserId;
        public string CurrentUsername => AuthContext.CurrentUsername;
        public bool IsLoggedIn => AuthContext.IsLoggedIn;

        public void SetUser(int userId, string username)
        {
            AuthContext.SetUser(userId, username);
        }

        public void Clear()
        {
            AuthContext.Clear();
        }
    }
}