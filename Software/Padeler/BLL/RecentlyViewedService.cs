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
        public List<UserCardDto> FilterUsers(List<UserCardDto> users)
        {
            if (users == null)
            {
                return new List<UserCardDto>();
            }

            return users;
        }
    }
}
