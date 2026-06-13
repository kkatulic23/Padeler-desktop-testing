using System;
using System.Threading.Tasks;
using BLL;
using DAL;
using FakeItEasy;
using Xunit;

namespace BLLUnitTests
{
    public class LocationServiceTests
    {
        private IAuthContext CreateAuthContext(bool isLoggedIn, int userId = 0)
        {
            var authContext = A.Fake<IAuthContext>();

            A.CallTo(() => authContext.IsLoggedIn).Returns(isLoggedIn);
            A.CallTo(() => authContext.CurrentUserId).Returns(userId);

            return authContext;
        }

        [Fact]
        public void Constructor_GivenDefaultConstructor_CreatesService()
        {
            // Arrange & Act
            var service = new LocationService();

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_GivenRepositoryConstructor_CreatesService()
        {
            // Arrange
            var userRepository = A.Fake<IUsersRepository>();

            // Act
            var service = new LocationService(userRepository);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_GivenRepositoryAndLocationProviderConstructor_CreatesService()
        {
            // Arrange
            var userRepository = A.Fake<IUsersRepository>();
            var locationProvider = A.Fake<ILocationProvider>();

            // Act
            var service = new LocationService(userRepository, locationProvider);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public async Task TryUpdateCurrentUserLocationAsync_GivenUserIsNotLoggedIn_ReturnsFalse()
        {
            // Arrange
            var userRepository = A.Fake<IUsersRepository>();
            var locationProvider = A.Fake<ILocationProvider>();
            var authContext = CreateAuthContext(false);

            var service = new LocationService(userRepository, locationProvider, authContext);

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
            var userRepository = A.Fake<IUsersRepository>();
            var locationProvider = A.Fake<ILocationProvider>();
            var authContext = CreateAuthContext(true, 1);

            A.CallTo(() => locationProvider.GetCurrentLocationAsync())
                .Returns(Task.FromResult<(double lat, double lng)?>(null));

            var service = new LocationService(userRepository, locationProvider, authContext);

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
            var userRepository = A.Fake<IUsersRepository>();
            var locationProvider = A.Fake<ILocationProvider>();
            var authContext = CreateAuthContext(true, 1);

            A.CallTo(() => locationProvider.GetCurrentLocationAsync())
                .Returns(Task.FromResult<(double lat, double lng)?>((46.123456789, 16.987654321)));

            A.CallTo(() => userRepository.UpdateLocationAsync(1, 46.1234568, 16.9876543))
                .Returns(Task.FromResult(true));

            var service = new LocationService(userRepository, locationProvider, authContext);

            // Act
            var result = await service.TryUpdateCurrentUserLocationAsync();

            // Assert
            Assert.True(result);
            A.CallTo(() => userRepository.UpdateLocationAsync(1, 46.1234568, 16.9876543))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task TryUpdateCurrentUserLocationAsync_GivenRepositoryThrowsException_ReturnsFalse()
        {
            // Arrange
            var userRepository = A.Fake<IUsersRepository>();
            var locationProvider = A.Fake<ILocationProvider>();
            var authContext = CreateAuthContext(true, 1);

            A.CallTo(() => locationProvider.GetCurrentLocationAsync())
                .Returns(Task.FromResult<(double lat, double lng)?>((46.1234567, 16.9876543)));

            A.CallTo(() => userRepository.UpdateLocationAsync(A<int>._, A<double>._, A<double>._))
                .Throws(new Exception("API error"));

            var service = new LocationService(userRepository, locationProvider, authContext);

            // Act
            var result = await service.TryUpdateCurrentUserLocationAsync();

            // Assert
            Assert.False(result);
        }
    }
}