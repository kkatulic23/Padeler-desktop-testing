using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using DAL;
using FakeItEasy;
using Xunit;

namespace BLLUnitTests
{
    public class LocationServiceTests
    {
        public LocationServiceTests()
        {
            AuthContext.Clear();
        }

        [Fact]
        public async Task TryUpdateCurrentUserLocationAsync_GivenDefaultConstructorAndUserIsNotLoggedIn_ReturnsFalse()
        {
            // Arrange
            AuthContext.Clear();
            var service = new LocationService();

            // Act
            var result = await service.TryUpdateCurrentUserLocationAsync();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task TryUpdateCurrentUserLocationAsync_GivenRepositoryConstructorAndUserIsNotLoggedIn_ReturnsFalse()
        {
            // Arrange
            AuthContext.Clear();
            var userRepository = A.Fake<IUsersRepository>();
            var service = new LocationService(userRepository);

            // Act
            var result = await service.TryUpdateCurrentUserLocationAsync();

            // Assert
            Assert.False(result);

            A.CallTo(() => userRepository.UpdateLocationAsync(A<int>._, A<double>._, A<double>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task TryUpdateCurrentUserLocationAsync_GivenUserIsNotLoggedIn_ReturnsFalse()
        {
            // Arrange
            var userRepository = A.Fake<IUsersRepository>();
            var locationProvider = A.Fake<ILocationProvider>();
            var service = new LocationService(userRepository, locationProvider);

            // Act
            var result = await service.TryUpdateCurrentUserLocationAsync();

            // Assert
            Assert.False(result);
            A.CallTo(() => locationProvider.GetCurrentLocationAsync()).MustNotHaveHappened();
            A.CallTo(() => userRepository.UpdateLocationAsync(A<int>._, A<double>._, A<double>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task TryUpdateCurrentUserLocationAsync_GivenLocationIsNotAvailable_ReturnsFalse()
        {
            // Arrange
            AuthContext.SetUser(1, "kkrsak23");
            var userRepository = A.Fake<IUsersRepository>();
            var locationProvider = A.Fake<ILocationProvider>();

            A.CallTo(() => locationProvider.GetCurrentLocationAsync())
                .Returns(Task.FromResult<(double lat, double lng)?>(null));

            var service = new LocationService(userRepository, locationProvider);

            // Act
            var result = await service.TryUpdateCurrentUserLocationAsync();

            // Assert
            Assert.False(result);
            A.CallTo(() => userRepository.UpdateLocationAsync(A<int>._, A<double>._, A<double>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task TryUpdateCurrentUserLocationAsync_GivenValidLocation_UpdatesLocationAndReturnsTrue()
        {
            // Arrange
            AuthContext.SetUser(1, "kkrsak23");

            var userRepository = A.Fake<IUsersRepository>();
            var locationProvider = A.Fake<ILocationProvider>();

            A.CallTo(() => locationProvider.GetCurrentLocationAsync())
                .Returns(Task.FromResult<(double lat, double lng)?>((46.123456789, 16.987654321)));

            A.CallTo(() => userRepository.UpdateLocationAsync(1, 46.1234568, 16.9876543))
                .Returns(Task.FromResult(true));

            var service = new LocationService(userRepository, locationProvider);

            // Act
            var result = await service.TryUpdateCurrentUserLocationAsync();

            // Assert
            Assert.True(result);
            A.CallTo(() => userRepository.UpdateLocationAsync(1, 46.1234568, 16.9876543)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task TryUpdateCurrentUserLocationAsync_GivenRepositoryThrowsException_ReturnsFalse()
        {
            // Arrange
            AuthContext.SetUser(1, "kkrsak23");

            var userRepository = A.Fake<IUsersRepository>();
            var locationProvider = A.Fake<ILocationProvider>();

            A.CallTo(() => locationProvider.GetCurrentLocationAsync())
                .Returns(Task.FromResult<(double lat, double lng)?>((46.1234567, 16.9876543)));

            A.CallTo(() => userRepository.UpdateLocationAsync(A<int>._, A<double>._, A<double>._))
                .Throws(new Exception("API error"));

            var service = new LocationService(userRepository, locationProvider);

            // Act
            var result = await service.TryUpdateCurrentUserLocationAsync();

            // Assert
            Assert.False(result);
        }
    }
}
