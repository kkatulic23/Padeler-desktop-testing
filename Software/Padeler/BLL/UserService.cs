using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UserService
    {
        private readonly UsersRepository _usersRepository;

        public UserService()
        {
            _usersRepository = new UsersRepository();
        }

        /// <summary>
        /// Dohvaća listu korisnika za prikaz kartica na HomeFormi
        /// prema trenutnoj lokaciji prijavljenog korisnika, radijusu
        /// i opcionalnim filtrima (spol, razina, pozicija, učestalost igranja).
        /// </summary>
        public async Task<List<UserCardDto>> GetUsersForCardAsync(  // Filip Grgac + Karlo Kršak (filter i geo)
            int radiusKm,
            string gender,
            string level,
            string position,
            string frequency
        ) 
        {
            if (!AuthContext.IsLoggedIn)
            {
                throw new Exception("The user is not logged in!");
            }

            if (radiusKm < 1) radiusKm = 1;
            if (radiusKm > 50) radiusKm = 50;

            int loggedId = AuthContext.CurrentUserId;

            var loggedUser = await _usersRepository.GetUserAsync(loggedId);
            if (loggedUser.latitude == null || loggedUser.longitude == null)
            {
                return new List<UserCardDto>();
            }

            gender = gender ?? "";
            level = level ?? "";
            position = position ?? "";
            frequency = frequency ?? "";

            var nearbyUser = await _usersRepository.GetNearbyUsersCardAsync(
                loggedId,
                loggedUser.latitude.Value,
                loggedUser.longitude.Value,
                radiusKm,
                gender,
                level,
                position,
                frequency
            );

            var cards = new List<UserCardDto>();

            foreach (var user in nearbyUser)
            {
                if (user.userId == loggedUser.userId)
                {
                    continue;
                }

                var image = await _usersRepository.GetImageForCardAsync(user.userId);

                cards.Add(UserCardInfo(user, image));
            }

            return cards;
        }

        /// <summary>
        /// Kreira i vraća UserCardDto objekt iz korisničkih podataka
        /// te izračunava dob korisnika i mapira podatke potrebne
        /// za prikaz kartice u korisničkom sučelju.
        /// </summary>
        private UserCardDto UserCardInfo(UserDto user, UserImageDto image) // Filip Grgac + Karlo Kršak (filter i geo)
        {
            var today = DateTime.Today;
            int age = today.Year - user.dateOfBirth.Year;
            if (user.dateOfBirth > today.AddYears(-age)) age--;

            return new UserCardDto
            {
                UserId = user.userId,
                FullName = $"{user.name} {user.surname}",
                Age = age,
                Level = user.levelOfPlay,
                Position = user.position,
                FrequencyOfPlaying = user.frequencyOfPlay,
                Latitude = user.latitude,
                Longitude = user.longitude,
                DistanceKm = user.distanceKm ?? 0.0,
                Image = image,
                Rating = user.rating
            };
        }

        
        /// <summary>
        /// Dohvaćanje slike za određenog korisnika tako da se može prikazati na kartici.
        /// </summary>
        public async Task<UserImageDto> GetUserImageCardAsync(int userId) // Filip Grgac
        {
            var image = await _usersRepository.GetImageForCardAsync(userId);
            return image;
        }
    }
}
