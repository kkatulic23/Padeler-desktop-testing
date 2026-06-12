using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IUsersRepository
    {
        Task<UserDto> GetUserAsync(int userId);

        Task<List<UserDto>> GetNearbyUsersCardAsync(int currentUserId, double lat, double lng, int radius, string gender, string level, string position, string frequency);

        Task<UserImageDto> GetImageForCardAsync(int userId);
        Task UpdateUserAsync(UpdateUserRequest user);

        Task<bool> UpdateLocationAsync(int userId, double lat, double lng);
    }
}
