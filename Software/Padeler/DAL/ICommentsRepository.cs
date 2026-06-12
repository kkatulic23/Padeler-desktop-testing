using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL
{
    public interface ICommentsRepository
    {
        Task<int> AddRatingAsync(int commentedId, int commenterId, int grade, string comment = null);
        Task<List<int>> GetRatedIdsAsync(int commenterId);
    }
}