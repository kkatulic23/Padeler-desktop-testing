using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RecentlyViewedService
    {
        private readonly List<int> _recentlyViewedUserId = new List<int>();

        public void AddSwipedUser(int userId)
        {
            if (!_recentlyViewedUserId.Contains(userId))
            {
                _recentlyViewedUserId.Add(userId);
            }
        }

        public List<UserCardDto> FilterUsers(List<UserCardDto> users)
        {
            if (users == null)
            {
                return new List<UserCardDto>();
            }

            var filteredUsers = users.Where(user => !IsRecentlyViewed(user.UserId)).ToList();

            if(filteredUsers.Count == 0)
            {
                return users;
            }

            return filteredUsers;
        }

        private bool IsRecentlyViewed(int userId)
        {
            return _recentlyViewedUserId.Contains(userId);
        }
    }
}
