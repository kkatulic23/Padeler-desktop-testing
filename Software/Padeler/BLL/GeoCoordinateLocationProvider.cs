using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class GeoCoordinateLocationProvider : ILocationProvider
    {
        public Task<(double lat, double lng)?> GetCurrentLocationAsync()
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
