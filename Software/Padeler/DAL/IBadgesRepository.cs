using EL;
using System.Threading.Tasks;

namespace DAL
{
    public interface IBadgesRepository
    {
        Task<AddSwipeResponse> AddSwipeAsync(int userId);
        Task<GetUserBadgesResponse> GetUserBadgesAsync(int userId);
    }
}