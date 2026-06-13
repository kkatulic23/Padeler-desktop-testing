using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PasswordStrengthService
    {
        public int CalculateScore(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return 0;
            }
            return 0;
        }

        public string GetStrengthLabel(string password)
        {
            int score = CalculateScore(password);

            if (score <= 1)
            {
                return "Very weak";
            }
            return "Very weak";
        }
    }
}
