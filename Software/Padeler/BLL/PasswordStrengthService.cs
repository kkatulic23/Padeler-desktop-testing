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

            int score = 0;

            if (HasMinimumLength(password))
            {
                score++;
            }

            if (HasLowercaseLetter(password))
            {
                score++;
            }

            if (HasUppercaseLetter(password))
            {
                score++;
            }

            return score;
        }

        public string GetStrengthLabel(string password)
        {
            int score = CalculateScore(password);

            if (score <= 1)
            {
                return "Very weak";
            }

            if (score == 2)
            {
                return "Weak";
            }

            if (score == 3)
            {
                return "Okay";
            }

            return "Very weak";
        }

        private bool HasMinimumLength(string password)
        {
            return password.Length >= 8;
        }

        private bool HasLowercaseLetter(string password)
        {
            foreach (char character in password)
            {
                if (char.IsLower(character))
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasUppercaseLetter(string password)
        {
            foreach (char character in password)
            {
                if (char.IsUpper(character))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
