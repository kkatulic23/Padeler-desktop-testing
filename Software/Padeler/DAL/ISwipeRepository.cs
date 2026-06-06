using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface ISwipeRepository //Filip Grgac
    {
        Task<SwipeResponse> SwipeAsync(int fromUserId, int toUserId, string response);
    }
}
