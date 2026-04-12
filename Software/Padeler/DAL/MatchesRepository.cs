using EL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public sealed class MatchesRepository
    {
        private readonly ApiClient _api = new ApiClient();

        /// <summary>
        /// Šalje korisničku swipe akciju ("LIKE" ili "DISLIKE") prema poslužitelju.
        /// Metoda prosljeđuje identifikatore korisnika i vrstu odgovora te vraća
        /// rezultat obrade, uključujući informaciju je li došlo do matcha.
        /// </summary>
        public async Task<SwipeResponse> SwipeAsync(int fromUserId, int toUserId, string response) // Filip Grgac
        {
            var dto = new SwipeDto
            {
                FromUserId = fromUserId,
                ToUserId = toUserId,
                Response = response
            };

            var res = await _api.PostMatchAsync<SwipeDto, SwipeResponse>("api/match/swipe.php", dto);

            return res;
        }
    }
}
