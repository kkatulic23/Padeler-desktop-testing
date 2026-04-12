using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MatchListRepository : IMatchRepository
    {
        private readonly ApiClient _api = new ApiClient();
        public async Task<List<MatchEntryDto>> FindAllForUsersAsync(int userId) // Filip Grgac
        {
            var res = await _api.GetAsync<GetMatchEntriesResponse>($"api/match/get_match_entries.php?user_id={userId}");
            if (res == null || !res.Success) throw new Exception(res?.Error ?? "API error");

            return res.Entries ?? new List<MatchEntryDto>();
        }

        public async Task<MatchEntryDto> FindByUsersAsync(int currentUserId, int matchedUserId) // Filip Grgac
        {
            var res = await _api.GetAsync<GetMatchEntryResponse>($"api/match/get_match_entry.php?user_id={currentUserId}&matched_user_id={matchedUserId}");
            if (res == null || !res.Success) throw new Exception(res?.Error ?? "API error");

            return res.Entry;
        }

        public async Task<List<MatchDto>> GetMyMatchesAsync(int userId) // Filip Grgac
        {
            var res = await _api.GetAsync<GetMyMatchesResponse>($"api/match/get_my_matches.php?user_id={userId}");
            if (res == null || !res.Success) throw new Exception(res?.Error ?? "API error");

            return res.Matches ?? new List<MatchDto>();
        }

        public async Task HideAsync(int currentUserId, int matchedUserId) // Filip Grgac
        {
            var req = new HideMatchEntryRequest
            {
                UserId = currentUserId,
                MatchedUserId = matchedUserId
            };

            var res = await _api.PostJsonAsync<BasicResponse>("api/match/hide_match_entry.php", req);
            if (res == null || !res.Success) throw new Exception(res?.Error ?? "API error");
        }

        public async Task SaveAsync(MatchEntryDto matchEntryDto) // Filip Grgac
        {
            var req = new SaveMatchEntryRequest
            {
                UserId = matchEntryDto.CurrentUserId,
                MatchedUserId = matchEntryDto.MatchedUserId,
                CustomNickname = matchEntryDto.CustomNickname,
                IsHidden = matchEntryDto.IsHidden
            };

            var res = await _api.PostJsonAsync<BasicResponse>("api/match/save_match_entry.php", req);
            if (res == null || !res.Success) throw new Exception(res?.Error ?? "API error");
        }
    }
}
