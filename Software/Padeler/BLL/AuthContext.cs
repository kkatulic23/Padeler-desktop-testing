using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// Globalni kontekst prijavljenog korisnika (session u memoriji)
    /// </summary>
    public static class AuthContext
    {
        public static int CurrentUserId { get; private set; }
        public static string CurrentUsername { get; private set; } = "";


        public static bool IsLoggedIn => CurrentUserId > 0;

        public static void SetUser(int userId, string username) // Karlo Kršak
        {
            CurrentUserId = userId;
            CurrentUsername = username ?? "";
        }

        public static void Clear() // Karlo Kršak
        {
            CurrentUserId = 0;
            CurrentUsername = "";
        }
    }
}
