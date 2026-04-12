using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EL;

namespace DAL
{
    public class AuthRepository
    {
        private readonly ApiClient _api;
        public AuthRepository(ApiClient api)
        {
            _api = api;
        }
        public async Task<int> LoginAsync(string username, string password) // Karlo Kršak
        {
            var req = new LoginRequestDto
            {
                Username = username,
                Password = password
            };
            var res = await _api.PostJsonAsync<LoginResponseDto>(
                "api/users/login.php",
                req
            );
            if (!res.Success) throw new System.Exception(res.Error ?? "Login failed.");
            return res.UserId;
        }
        public async Task<int> RegisterAsync(RegisterRequestDto dto) // Karlo Kršak
        {
            var res = await _api.PostJsonAsync<RegisterResponseDto>(
                "api/users/register.php",
                dto
            );

            if (!res.Success) throw new Exception(res.Error ?? "Registration failed.");
            return res.UserId;
        }

    }
}
