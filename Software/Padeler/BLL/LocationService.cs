using DAL;
using System.Threading.Tasks;
using System;

namespace BLL
{
    public class LocationService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ILocationProvider _locationProvider;

        public LocationService()
            : this(new UsersRepository(), new GeoCoordinateLocationProvider())
        {
        }

        public LocationService(IUsersRepository usersRepository)
            : this(usersRepository, new GeoCoordinateLocationProvider())
        {
        }

        public LocationService(IUsersRepository usersRepository, ILocationProvider locationProvider)
        {
            _usersRepository = usersRepository;
            _locationProvider = locationProvider;
        }

        /// <summary>
        /// Pokušava dohvatiti trenutnu geografsku lokaciju prijavljenog korisnika
        /// te ju, ako je uspješno dohvaćena, sprema na poslužitelj.
        /// </summary>
        public async Task<bool> TryUpdateCurrentUserLocationAsync() // Karlo Kršak
        {
            if (!AuthContext.IsLoggedIn)
            {
                return false;
            }

            var location = await _locationProvider.GetCurrentLocationAsync();

            if (location == null)
            {
                return false;
            }

            double lat = Math.Round(location.Value.lat, 7);
            double lng = Math.Round(location.Value.lng, 7);

            try
            {
                await _usersRepository.UpdateLocationAsync(AuthContext.CurrentUserId, lat, lng);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}