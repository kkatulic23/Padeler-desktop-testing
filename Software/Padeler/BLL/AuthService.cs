using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using EL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace BLL
{
    public class AuthService
    {
        private readonly AuthRepository _repo;
        private const int MaxImageBytes = 600000;
        private const string ProfileMimeType = "image/jpeg";

        public AuthService()
        {
            _repo = new AuthRepository(new ApiClient());
        }

        public async Task<int> LoginAsync(string username, string password) // Karlo Kršak
        {
            int userId = await _repo.LoginAsync(username, password);
            AuthContext.SetUser(userId, username);
            return userId;
        }
        public async Task<int> RegisterAsync( // Karlo Kršak
            string username,
            string email,
            string password,
            string phone,
            string name,
            string surname,
            string gender,
            DateTime dob,
            string frequency,
            string level,
            string position,
            byte[] imagePngBytes
        )
        {
            name = Req(name, "Name");
            surname = Req(surname, "Surname");
            if (dob.Date > DateTime.Today) throw new Exception("Date of birth cannot be in the future.");
            username = Req(username, "Username");
            if (username.Length < 3) throw new Exception("Username must be at least 3 characters.");
            password = password ?? throw new Exception("Password is required.");
            if (password.Length == 0) throw new Exception("Password is required.");
            if (password.Length < 8) throw new Exception("Password must be at least 8 characters.");
            email = Req(email, "Email");
            if (!email.Contains("@") || !email.Contains(".")) throw new Exception("Email format is invalid.");
            phone = Req(phone, "Phone");
            ReqSelected(gender, "Gender");
            ReqSelected(frequency, "Frequency");
            ReqSelected(level, "Level");
            ReqSelected(position, "Position");

            string base64 = null;
            string mime = null;

            if (imagePngBytes != null && imagePngBytes.Length > 0)
            {
                const int MaxBytes = 600000;
                if (imagePngBytes.Length > MaxBytes)
                    throw new Exception("The selected image is too large. Please choose an image smaller than 600KB.");

                base64 = Convert.ToBase64String(imagePngBytes);
                mime = "image/jpeg";
            }

            var dto = new RegisterRequestDto
            {
                Username = username,
                Email = email,
                Password = password,
                Phone = phone,
                Name = name,
                Surname = surname,
                Gender = gender,
                DateOfBirth = dob.ToString("yyyy-MM-dd"),
                Frequency = frequency,
                Level = level,
                Position = position,
                ImageBase64 = base64,
                MimeType = mime
            };

            return await _repo.RegisterAsync(dto);
        }

        private static string Req(string value, string fieldName) // Karlo Kršak
        {
            if (value == null) throw new Exception($"{fieldName} is required.");
            var v = value.Trim();
            if (v.Length == 0) throw new Exception($"{fieldName} is required.");
            return v;
        }
        private static void ReqSelected(string value, string fieldName) // Karlo Kršak
        {
            if (string.IsNullOrWhiteSpace(value)) throw new Exception($"{fieldName} is required.");
        }
        public byte[] LoadProfileImageAsPngBytes(string path) // Karlo Kršak
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new Exception("Image path is required.");
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var img = Image.FromStream(fs))
            using (var bmp = new Bitmap(img))
            {
                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Jpeg);
                    var bytes = ms.ToArray();

                    if (bytes.Length > MaxImageBytes)
                        throw new Exception("The selected image is too large. Please choose an image smaller than 600KB.");

                    return bytes;
                }
            }
        }
        public void Logout() // Karlo Kršak
        {
            AuthContext.Clear();
        }

    }
}
