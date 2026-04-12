using EL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using System.Text;

namespace DAL
{
    public sealed class UsersRepository
    {
        private readonly ApiClient _api = new ApiClient();

        public async Task<UserDto> GetUserAsync(int userId) // Kristian Katulić
        {
            var res = await _api.GetAsync<UserResponse>($"api/users/get_user.php?user_id={userId}");
            var img = await _api.GetAsync<UserImageResponse>($"api/users/get_user_image.php?user_id={userId}");
            if (!res.Success) throw new Exception(res.Error ?? "API error");
            if (img.Success)
            {
                res.User.image = Convert.FromBase64String(img.imageBase64 ?? "");
                res.User.mimeType = img.mimeType ?? "";
            }
            return res.User;
        }

        /// <summary>
        /// Dohvaća korisnike u blizini zadane lokacije unutar određenog radijusa
        /// te primjenjuje opcionalne filtre (spol, razina, pozicija, učestalost igranja).
        /// </summary>
        public async Task<List<UserDto>> GetNearbyUsersCardAsync( // Filip Grgac + Karlo Kršak (filter i geo)
            int currentUserId,
            double lat,
            double lng,
            int radius,
            string gender,
            string level,
            string position,
            string frequency
        ) 
        {
            string latStr = lat.ToString(CultureInfo.InvariantCulture);
            string lngStr = lng.ToString(CultureInfo.InvariantCulture);
            var url = new StringBuilder();
            url.Append($"api/users/nearby.php?lat={latStr}&lng={lngStr}&radius={radius}&current_user_id={currentUserId}");
            if (!string.IsNullOrWhiteSpace(gender)) url.Append("&gender=" + Uri.EscapeDataString(gender));
            if (!string.IsNullOrWhiteSpace(level)) url.Append("&level=" + Uri.EscapeDataString(level));
            if (!string.IsNullOrWhiteSpace(position)) url.Append("&position=" + Uri.EscapeDataString(position));
            if (!string.IsNullOrWhiteSpace(frequency)) url.Append("&frequency=" + Uri.EscapeDataString(frequency));
            var res = await _api.GetAsync<GetNearbyUsersResponse>(url.ToString());
            if (!res.Success) throw new Exception(res.Error ?? "API error");
            return res.Users;
        }

        public async Task<UserImageDto> GetImageForCardAsync(int userId) // Filip Grgac
        {
            var res = await _api.GetAsync<UserImageDto>($"api/users/get_user_image.php?user_id={userId}");
            return res;
        }
        public async Task UpdateUserAsync(UpdateUserRequest user) // Kristian Katulić
        {
            var res = await _api.PostAsync<UserResponse>("api/users/update_profile.php", user);
            if (res == null)
                throw new Exception("API response is null (deserialization failed).");

            if (!res.Success)
                throw new Exception(res.Error ?? res.Debug ?? "Update failed (success=false).");
        }

        /// <summary>
        /// Ažurira geografsku lokaciju korisnika na poslužitelju
        /// slanjem trenutnih koordinata putem API poziva.
        /// </summary>
        public async Task<bool> UpdateLocationAsync(int userId, double lat, double lng) // Karlo Kršak
        {
            var req = new UpdateLocationRequest
            {
                UserId = userId,
                Latitude = lat,
                Longitude = lng
            };
            var res = await _api.PostJsonAsync<UpdateLocationResponse>(
                "api/users/update_location.php",
                req
            );
            if (res == null)
                throw new Exception("UpdateLocation: API response is null.");
            if (!res.Success)
                throw new Exception(res.Error ?? "Update location failed.");
            return true;
        }

    }
}
