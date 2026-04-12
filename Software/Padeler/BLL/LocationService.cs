using System;
using System.Device.Location;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;

namespace BLL
{
    public class LocationService
    {
        private readonly UsersRepository _usersRepository;
        public LocationService() : this(new UsersRepository()) {}

        public LocationService(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// Pokušava dohvatiti trenutnu geografsku lokaciju prijavljenog korisnika
        /// te ju, ako je uspješno dohvaćena, sprema na poslužitelj.
        /// </summary>
        public async Task<bool> TryUpdateCurrentUserLocationAsync() // Karlo Kršak
        {
            if (!AuthContext.IsLoggedIn) return false;
            var location = await TryGetCurrentLocationAsync();
            if (location == null) return false;
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

        /// <summary>
        /// Asinkrono dohvaća trenutnu geografsku lokaciju korisnika
        /// koristeći GeoCoordinateWatcher i vraća koordinate ili null
        /// ako lokacija nije dostupna.
        /// </summary>
        private Task<(double lat, double lng)?> TryGetCurrentLocationAsync() // Karlo Kršak
        {
            var tcs = new TaskCompletionSource<(double, double)?>();
            var watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.PositionChanged += (s, e) =>
            {
                if (!e.Position.Location.IsUnknown)
                {
                    tcs.TrySetResult((
                        e.Position.Location.Latitude,
                        e.Position.Location.Longitude
                    ));
                    watcher.Stop();
                }
            };
            watcher.StatusChanged += (s, e) =>
            {
                if (e.Status == GeoPositionStatus.Disabled)
                {
                    tcs.TrySetResult(null);
                    watcher.Stop();
                }
            };
            watcher.Start();
            return tcs.Task;
        }
    }
}
