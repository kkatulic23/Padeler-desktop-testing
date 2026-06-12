using DAL;
using EL;
using System.Threading.Tasks;

namespace BLL
{
    public class EditProfile // Kristian Katulić
    {
        private readonly IUsersRepository _usersRepository;
        public EditProfile() : this(new UsersRepository())
        {
        }
        public EditProfile(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
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
