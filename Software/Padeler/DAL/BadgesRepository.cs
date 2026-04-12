using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BadgesRepository // Kristian Katulić
    {
        private readonly ApiClient _api = new ApiClient();

        public async Task<AddSwipeResponse> AddSwipeAsync(int userId)
           => await _api.PostAsync<AddSwipeResponse>("api/badges/add_swipe.php", new { user_id = userId });

        public async Task<GetUserBadgesResponse> GetUserBadgesAsync(int userId)
            => await _api.PostAsync<GetUserBadgesResponse>("api/badges/get_user_badges.php", new { user_id = userId });
    }
}
