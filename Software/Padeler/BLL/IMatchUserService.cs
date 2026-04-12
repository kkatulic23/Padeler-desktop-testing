using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IMatchUserService // Filip Grgac
    {
        Task<List<MatchRow>> GetMatchedEntries(int userId);
        Task<MatchEntryDto> GetEntry(int currentUserId, int matchedUserId);
        Task<bool> UpdateEntry(MatchEntryDto entry);
        Task<bool> DeleteEntry(MatchEntryDto entry);
    }
}
