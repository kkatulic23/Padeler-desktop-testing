using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BadgeService // Kristian Katulić
    {
        private readonly IBadgesRepository _repo;

        public BadgeService() : this(new BadgesRepository())
        {
        }

        public BadgeService(IBadgesRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Registrira jedan novi swipe korisnika.
        /// Povećava ukupan broj swipeova te vraća ažurirani broj swipeova
        /// i listu novo dodijeljenih znački (ako ih ima).
        /// </summary>
        /// <param name="userId">Identifikator korisnika</param>
        /// <returns>
        /// Tuple koji sadrži novi broj swipeova i listu novo dodijeljenih znački.
        /// </returns>
        public async Task<(int newSwipeNum, List<BadgeDto> newlyAwarded)> RegisterSwipeAsync(int userId)
        {
            if (userId <= 0)
                throw new Exception("Invalid user id.");
            var res = await _repo.AddSwipeAsync(userId);

            if (res == null)
                throw new Exception("API response is null.");
            if (!res.Success)
                throw new Exception(res.Error ?? "Add swipe failed.");

            return (res.SwipeNum, res.AwardedBadges ?? new List<BadgeDto>());
        }

        /// <summary>
        /// Dohvaća sve značke koje su dodijeljene korisniku.
        /// </summary>
        /// <param name="userId">Identifikator korisnika</param>
        /// <returns>
        /// Lista znački koje korisnik ima.
        /// </returns>
        public async Task<List<BadgeDto>> GetUserBadgesAsync(int userId)
        {
            if (userId <= 0)
                throw new Exception("Invalid user id.");

            var res = await _repo.GetUserBadgesAsync(userId);

            if (res == null)
                throw new Exception("API response is null.");

            if (!res.Success)
                throw new Exception(res.Error ?? "Get badges failed.");

            return res.Badges ?? new List<BadgeDto>();
        }
    }
}
