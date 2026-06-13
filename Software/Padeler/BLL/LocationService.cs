using DAL;
using System.Threading.Tasks;
using System;

namespace BLL
{
    public class LocationService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ILocationProvider _locationProvider;
        private readonly IAuthContext _authContext;

        public LocationService()
            : this(new UsersRepository(), new GeoCoordinateLocationProvider(), new AuthContextAdapter())
        {
        }

        public LocationService(IUsersRepository usersRepository)
            : this(usersRepository, new GeoCoordinateLocationProvider(), new AuthContextAdapter())
        {
        }

        public LocationService(IUsersRepository usersRepository, ILocationProvider locationProvider)
            : this(usersRepository, locationProvider, new AuthContextAdapter())
        {
        }

        public LocationService(IUsersRepository usersRepository, ILocationProvider locationProvider, IAuthContext authContext)
        {
            _usersRepository = usersRepository;
            _locationProvider = locationProvider;
            _authContext = authContext;
        }

        /// <summary>
        /// Pokušava dohvatiti trenutnu geografsku lokaciju prijavljenog korisnika
        /// te ju, ako je uspješno dohvaćena, sprema na poslužitelj.
        /// </summary>
        public async Task<bool> TryUpdateCurrentUserLocationAsync() // Karlo Kršak
        {
            if (!_authContext.IsLoggedIn)
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
                await _usersRepository.UpdateLocationAsync(_authContext.CurrentUserId, lat, lng);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}