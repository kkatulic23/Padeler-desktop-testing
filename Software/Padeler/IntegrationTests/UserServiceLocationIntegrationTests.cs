using BLL;
using DAL;
using FakeItEasy;
using System;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace IntegrationTests
{
    public class UserServiceLocationIntegrationTests : IDisposable
    {
        private readonly WireMockServer _server;
        private readonly IAuthContext _authContext;

        public UserServiceLocationIntegrationTests()
        {
            _server = WireMockServer.Start();
            _authContext = A.Fake<IAuthContext>();

            A.CallTo(() => _authContext.IsLoggedIn).Returns(true);
            A.CallTo(() => _authContext.CurrentUserId).Returns(1);
            A.CallTo(() => _authContext.CurrentUsername).Returns("karlo");
        }

        [Fact]
        public async Task GetUsersForCardAsync_GivenValidRadius_ReturnsNearbyUserCards()
        {
            // Arrange
            StubLoggedUserWithLocation();
            StubNearbyUsersWithExpectedParameters(
                radius: 25,
                body: "{\"success\":true,\"users\":[{\"UserId\":2,\"Name\":\"Ivan\",\"Surname\":\"Ivić\",\"DateOfBirth\":\"2000-05-10\",\"FrequencyOfPlaying\":\"Weekly\",\"Level\":\"Beginner\",\"Position\":\"Left\",\"Rating\":3.8,\"distance_km\":12.5}]}"
            );
            StubUserImage(2, "{\"success\":true,\"image_base64\":\"abc\",\"mime_type\":\"image/png\"}");

            var service = CreateDefaultUserService();

            // Act
            var result = await service.GetUsersForCardAsync(25, "", "", "", "");

            // Assert
            Assert.Single(result);
            Assert.Equal(2, result[0].UserId);
            Assert.Equal("Ivan Ivić", result[0].FullName);
            Assert.Equal("Beginner", result[0].Level);
            Assert.Equal("Left", result[0].Position);
            Assert.Equal("Weekly", result[0].FrequencyOfPlaying);
            Assert.Equal(12.5, result[0].DistanceKm, 1);
            Assert.Equal("image/png", result[0].Image.MimeType);
        }

        [Fact]
        public async Task GetUsersForCardAsync_GivenMinimumRadius_ReturnsNearbyUserCards()
        {
            // Arrange
            StubLoggedUserWithLocation();
            StubNearbyUsersWithExpectedParameters(
                radius: 1,
                body: "{\"success\":true,\"users\":[{\"UserId\":2,\"Name\":\"Ana\",\"Surname\":\"Anić\",\"DateOfBirth\":\"2001-04-15\",\"FrequencyOfPlaying\":\"Often\",\"Level\":\"Intermediate\",\"Position\":\"Right\",\"Rating\":4.5,\"distance_km\":0.8}]}"
            );
            StubUserImage(2, "{\"success\":true,\"image_base64\":\"abc\",\"mime_type\":\"image/png\"}");

            var service = CreateDefaultUserService();

            // Act
            var result = await service.GetUsersForCardAsync(1, "", "", "", "");

            // Assert
            Assert.Single(result);
            Assert.Equal(2, result[0].UserId);
            Assert.Equal("Ana Anić", result[0].FullName);
            Assert.Equal(0.8, result[0].DistanceKm, 1);
        }

        [Fact]
        public async Task GetUsersForCardAsync_GivenMaximumRadius_ReturnsNearbyUserCards()
        {
            // Arrange
            StubLoggedUserWithLocation();
            StubNearbyUsersWithExpectedParameters(
                radius: 50,
                body: "{\"success\":true,\"users\":[{\"UserId\":2,\"Name\":\"Marko\",\"Surname\":\"Marić\",\"DateOfBirth\":\"1999-08-20\",\"FrequencyOfPlaying\":\"Monthly\",\"Level\":\"Advanced\",\"Position\":\"Left\",\"Rating\":4.7,\"distance_km\":35.4}]}"
            );
            StubUserImage(2, "{\"success\":true,\"image_base64\":\"abc\",\"mime_type\":\"image/png\"}");

            var service = CreateDefaultUserService();

            // Act
            var result = await service.GetUsersForCardAsync(50, "", "", "", "");

            // Assert
            Assert.Single(result);
            Assert.Equal(2, result[0].UserId);
            Assert.Equal("Marko Marić", result[0].FullName);
            Assert.Equal(35.4, result[0].DistanceKm, 1);
        }

        [Fact]
        public async Task GetUsersForCardAsync_GivenNoNearbyUsers_ReturnsEmptyList()
        {
            // Arrange
            StubLoggedUserWithLocation();
            StubNearbyUsersWithExpectedParameters(
                radius: 10,
                body: "{\"success\":true,\"users\":[]}"
            );

            var service = CreateDefaultUserService();

            // Act
            var result = await service.GetUsersForCardAsync(10, "", "", "", "");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsersForCardAsync_GivenNearbyApiError_ThrowsException()
        {
            // Arrange
            StubLoggedUserWithLocation();
            StubNearbyUsersWithExpectedParameters(
                radius: 10,
                body: "{\"success\":false,\"error\":\"Nearby API error\",\"users\":[]}"
            );

            var service = CreateDefaultUserService();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                service.GetUsersForCardAsync(10, "", "", "", ""));

            // Assert
            Assert.Equal("Nearby API error", exception.Message);
        }

        private UserService CreateDefaultUserService()
        {
            var apiClient = new ApiClient(new Uri(_server.Url + "/"));
            var repository = new UsersRepository(apiClient);

            return new UserService(repository, _authContext);
        }

        private void StubLoggedUserWithLocation()
        {
            StubGet(
                "/api/users/get_user.php",
                "{\"success\":true,\"user\":{\"UserId\":1,\"Name\":\"Karlo\",\"Surname\":\"Kršak\",\"DateOfBirth\":\"2004-11-24\",\"Latitude\":45.5,\"Longitude\":15.8}}"
            );

            StubUserImage(1, "{\"success\":false,\"image_base64\":\"\",\"mime_type\":\"\"}");
        }

        private void StubNearbyUsersWithExpectedParameters(int radius, string body)
        {
            _server
                .Given(Request.Create()
                    .WithPath("/api/users/nearby.php")
                    .WithParam("lat", "45.5")
                    .WithParam("lng", "15.8")
                    .WithParam("radius", radius.ToString())
                    .WithParam("current_user_id", "1")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(body));
        }

        private void StubGet(string path, string body)
        {
            _server
                .Given(Request.Create().WithPath(path).UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(body));
        }

        private void StubUserImage(int userId, string body)
        {
            _server
                .Given(Request.Create()
                    .WithPath("/api/users/get_user_image.php")
                    .WithParam("user_id", userId.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(body));
        }

        public void Dispose()
        {
            _server.Stop();
            _server.Dispose();
        }
    }
}