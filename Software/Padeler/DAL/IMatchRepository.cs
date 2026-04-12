using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IMatchRepository // Filip Grgac
    {
        Task<MatchEntryDto> FindByUsersAsync(int currentUserId, int matchedUserId);
        Task<List<MatchEntryDto>> FindAllForUsersAsync(int userId);
        Task SaveAsync(MatchEntryDto matchEntryDto);

        Task<List<MatchDto>> GetMyMatchesAsync(int userId);

        Task HideAsync(int currentUserId, int matchedUserId);
    }
}
