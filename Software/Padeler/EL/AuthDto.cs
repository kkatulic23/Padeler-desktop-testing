using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EL
{
    public class LoginRequestDto // Karlo Kršak
    {
        [JsonProperty("username")]
        public string Username { get; set; } = string.Empty;

        [JsonProperty("password")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto // Karlo Kršak
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }

    public class RegisterRequestDto // Karlo Kršak
    {
        [JsonProperty("username")]
        public string Username { get; set; } = string.Empty;

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("password")]
        public string Password { get; set; } = string.Empty;

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("date_of_birth")]
        public string DateOfBirth { get; set; }

        [JsonProperty("frequency")]
        public string Frequency { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("image_base64")]
        public string ImageBase64 { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }
    }

    public class RegisterResponseDto // Karlo Kršak
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }

}
