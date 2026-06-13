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
            _recentlyViewedUserId.Add(userId);
        }

        public List<UserCardDto> FilterUsers(List<UserCardDto> users)
        {
            if (users == null)
            {
                return new List<UserCardDto>();
            }

            return users.Where(user => !_recentlyViewedUserId.Contains(user.UserId)).ToList();
        }
    }
}
