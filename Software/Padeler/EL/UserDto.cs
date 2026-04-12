using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EL
{
    public class UserDto // Kristian Katulić
    {
        [JsonProperty("UserId")]
        public int userId { get; set; }

        [JsonProperty("Username")]
        public string username { get; set; } = string.Empty;

        [JsonProperty("Email")]
        public string email { get; set; } = string.Empty;

        [JsonProperty("Phone")]
        public string phone { get; set; }

        [JsonProperty("Name")]
        public string name { get; set; }

        [JsonProperty("Surname")]
        public string surname { get; set; }

        [JsonProperty("Gender")]
        public string gender { get; set; }

        [JsonProperty("DateOfBirth")]
        public DateTime dateOfBirth { get; set; }

        [JsonProperty("FrequencyOfPlaying")]
        public string frequencyOfPlay { get; set; }

        [JsonProperty("Level")]
        public string levelOfPlay { get; set; }

        [JsonProperty("Position")]
        public string position { get; set; }

        [JsonProperty("Rating")]
        public double? rating { get; set; }

        [JsonProperty("NumberOfRatings")]
        public int? numberOfRatings { get; set; }

        [JsonProperty("SwipeNum")]
        public int numOfSwipes { get; set; }

        [JsonProperty("Latitude")]
        public double? latitude { get; set; }

        [JsonProperty("Longitude")]
        public double? longitude { get; set; }

        [JsonProperty("distance_km")]
        public double? distanceKm { get; set; }

        [JsonProperty("Image")]
        public byte[] image { get; set; }

        [JsonProperty("Mime_type")]
        public string mimeType { get; set; }
    }
    public class UserResponse // Kristian Katulić
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("user")]
        public UserDto User { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("debug")]
        public string Debug { get; set; }
    }

    public class UserImageResponse // Kristian Katulić
    { 
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("image_base64")]
        public string imageBase64 { get; set; }

        [JsonProperty("mime_type")]
        public string mimeType { get; set; }
    }
    public class UpdateUserRequest // Kristian Katulić
    {
        [JsonProperty("user_id")]
        public int userId { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("phone")]
        public string phone { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("username")]
        public string username { get; set; }

        [JsonProperty("surname")]
        public string surname { get; set; }

        [JsonProperty("gender")]
        public string gender { get; set; }

        [JsonProperty("date_of_birth")]
        public DateTime dateOfBirth { get; set; }

        [JsonProperty("frequency")]
        public string frequency { get; set; }

        [JsonProperty("level")]
        public string level { get; set; }

        [JsonProperty("position")]
        public string position { get; set; }

        [JsonProperty("image_base64")]
        public byte[] imageBase64 { get; set; }

        [JsonProperty("mime_type")]
        public string mimeType { get; set; }
    }

    public class GetNearbyUsersResponse // Filip Grgac
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("users")]
        public List<UserDto> Users { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}