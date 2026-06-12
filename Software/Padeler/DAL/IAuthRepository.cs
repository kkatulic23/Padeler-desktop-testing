using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EL;

namespace DAL
{
    public interface IAuthRepository
    {
        Task<int> LoginAsync(string username, string password);
        Task<int> RegisterAsync(RegisterRequestDto dto);
    }
}
