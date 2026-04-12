using EL;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BLL
{
    public class Validator
    {
        /// <summary>
        /// Provjerava ispravnost podataka korisnika prilikom ažuriranja profila.
        /// Vraća listu poruka o greškama ako neki podatak nije ispravno unesen.
        /// </summary>
        /// <param name="user">Objekt s podacima korisnika</param>
        /// <returns>Lista poruka o greškama</returns>
        public List<string> ValidateUser(UpdateUserRequest user)
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrWhiteSpace(user.name))
                errors.Add("Name is required.");
            if (string.IsNullOrWhiteSpace(user.surname))
                errors.Add("Surname is required.");
            if (string.IsNullOrWhiteSpace(user.username))
                errors.Add("Username is required.");
            if (string.IsNullOrWhiteSpace(user.email) || !IsValidEmail(user.email))
                errors.Add("A valid email is required.");
            if (string.IsNullOrWhiteSpace(user.phone) || !Regex.IsMatch(user.phone, @"^\+?[0-9]{7,15}$"))
                errors.Add("A valid phone number is required.");
            return errors;
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
