using DAL;
using EL;
using System.Threading.Tasks;

namespace BLL
{
    public class EditProfile
    {
        private readonly UsersRepository _usersRepository = new UsersRepository();
        public async Task<UserDto> GetUserDataAsync(int userId)
        {
            return await _usersRepository.GetUserAsync(userId);
        }
        public async Task UpdateUserDataAsync(UpdateUserRequest user)
        {
            await _usersRepository.UpdateUserAsync(user);
        }
    }
}
