using BLL;
using DAL;
using System;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace IntegrationTests
{
    public class UserServiceFilterIntegrationTests : IDisposable
    {
        private readonly WireMockServer _server;

        public UserServiceFilterIntegrationTests()
        {
            _server = WireMockServer.Start();
            AuthContext.SetUser(1, "karlo");
        }

        [Fact]
        public async Task GetUsersForCardAsync_GivenGenderFilter_ReturnsFilteredUserCards()
        {
            // Arrange
            StubLoggedUserWithLocation();
            StubNearbyUsersWithExpectedParameters(
                radius: 25,
                gender: "Female",
                body: "{\"success\":true,\"users\":[{\"UserId\":2,\"Name\":\"Ana\",\"Surname\":\"Anić\",\"DateOfBirth\":\"2001-04-15\",\"FrequencyOfPlaying\":\"Often\",\"Level\":\"Intermediate\",\"Position\":\"Right\",\"Rating\":4.5,\"distance_km\":4.2}]}"
            );
            StubUserImage(2, "{\"success\":true,\"image_base64\":\"abc\",\"mime_type\":\"image/png\"}");

            var service = CreateDefaultUserService();

            // Act
            var result = await service.GetUsersForCardAsync(25, "Female", "", "", "");

            // Assert
            Assert.Single(result);
            Assert.Equal(2, result[0].UserId);
            Assert.Equal("Ana Anić", result[0].FullName);
            Assert.Equal("Intermediate", result[0].Level);
            Assert.Equal("Right", result[0].Position);
            Assert.Equal("Often", result[0].FrequencyOfPlaying);
            Assert.Equal(4.2, result[0].DistanceKm, 1);
        }

        [Fact]
        public async Task GetUsersForCardAsync_GivenLevelFilter_ReturnsFilteredUserCards()
        {
            // Arrange
            StubLoggedUserWithLocation();
            StubNearbyUsersWithExpectedParameters(
                radius: 25,
                level: "Advanced",
                body: "{\"success\":true,\"users\":[{\"UserId\":2,\"Name\":\"Marko\",\"Surname\":\"Marić\",\"DateOfBirth\":\"1999-08-20\",\"FrequencyOfPlaying\":\"Weekly\",\"Level\":\"Advanced\",\"Position\":\"Left\",\"Rating\":4.7,\"distance_km\":6.3}]}"
            );
            StubUserImage(2, "{\"success\":true,\"image_base64\":\"abc\",\"mime_type\":\"image/png\"}");

            var service = CreateDefaultUserService();

            // Act
            var result = await service.GetUsersForCardAsync(25, "", "Advanced", "", "");

            // Assert
            Assert.Single(result);
            Assert.Equal(2, result[0].UserId);
            Assert.Equal("Marko Marić", result[0].FullName);
            Assert.Equal("Advanced", result[0].Level);
            Assert.Equal("Left", result[0].Position);
            Assert.Equal("Weekly", result[0].FrequencyOfPlaying);
            Assert.Equal(6.3, result[0].DistanceKm, 1);
        }

        [Fact]
        public async Task GetUsersForCardAsync_GivenPositionAndFrequencyFilters_ReturnsFilteredUserCards()
        {
            // Arrange
            StubLoggedUserWithLocation();
            StubNearbyUsersWithExpectedParameters(
                radius: 25,
                position: "Left",
                frequency: "Weekly",
                body: "{\"success\":true,\"users\":[{\"UserId\":2,\"Name\":\"Ivan\",\"Surname\":\"Ivić\",\"DateOfBirth\":\"2000-05-10\",\"FrequencyOfPlaying\":\"Weekly\",\"Level\":\"Beginner\",\"Position\":\"Left\",\"Rating\":3.8,\"distance_km\":8.1}]}"
            );
            StubUserImage(2, "{\"success\":true,\"image_base64\":\"abc\",\"mime_type\":\"image/png\"}");

            var service = CreateDefaultUserService();

            // Act
            var result = await service.GetUsersForCardAsync(25, "", "", "Left", "Weekly");

            // Assert
            Assert.Single(result);
            Assert.Equal(2, result[0].UserId);
            Assert.Equal("Ivan Ivić", result[0].FullName);
            Assert.Equal("Left", result[0].Position);
            Assert.Equal("Weekly", result[0].FrequencyOfPlaying);
            Assert.Equal(8.1, result[0].DistanceKm, 1);
        }

        [Fact]
        public async Task GetUsersForCardAsync_GivenMultipleFilters_ReturnsUserCardsMatchingAllFilters()
        {
            // Arrange
            StubLoggedUserWithLocation();
            StubNearbyUsersWithExpectedParameters(
                radius: 25,
                gender: "Male",
                level: "Advanced",
                position: "Left",
                frequency: "Weekly",
                body: "{\"success\":true,\"users\":[{\"UserId\":2,\"Name\":\"Luka\",\"Surname\":\"Lukić\",\"DateOfBirth\":\"1998-03-12\",\"FrequencyOfPlaying\":\"Weekly\",\"Level\":\"Advanced\",\"Position\":\"Left\",\"Rating\":4.9,\"distance_km\":10.4}]}"
            );
            StubUserImage(2, "{\"success\":true,\"image_base64\":\"abc\",\"mime_type\":\"image/png\"}");

            var service = CreateDefaultUserService();

            // Act
            var result = await service.GetUsersForCardAsync(25, "Male", "Advanced", "Left", "Weekly");

            // Assert
            Assert.Single(result);
            Assert.Equal(2, result[0].UserId);
            Assert.Equal("Luka Lukić", result[0].FullName);
            Assert.Equal("Advanced", result[0].Level);
            Assert.Equal("Left", result[0].Position);
            Assert.Equal("Weekly", result[0].FrequencyOfPlaying);
            Assert.Equal(10.4, result[0].DistanceKm, 1);
        }

        [Fact]
        public async Task GetUsersForCardAsync_GivenFiltersWithoutMatchingUsers_ReturnsEmptyList()
        {
            // Arrange
            StubLoggedUserWithLocation();
            StubNearbyUsersWithExpectedParameters(
                radius: 25,
                gender: "Female",
                level: "Advanced",
                position: "Right",
                frequency: "Monthly",
                body: "{\"success\":true,\"users\":[]}"
            );

            var service = CreateDefaultUserService();

            // Act
            var result = await service.GetUsersForCardAsync(25, "Female", "Advanced", "Right", "Monthly");

            // Assert
            Assert.Empty(result);
        }

        private UserService CreateDefaultUserService()
        {
            var apiClient = new ApiClient(new Uri(_server.Url + "/"));
            var repository = new UsersRepository(apiClient);

            return new UserService(repository);
        }

        private void StubLoggedUserWithLocation()
        {
            StubGet(
                "/api/users/get_user.php",
                "{\"success\":true,\"user\":{\"UserId\":1,\"Name\":\"Karlo\",\"Surname\":\"Kršak\",\"DateOfBirth\":\"2004-11-24\",\"Latitude\":45.5,\"Longitude\":15.8}}"
            );

            StubUserImage(1, "{\"success\":false,\"image_base64\":\"\",\"mime_type\":\"\"}");
        }

        private void StubNearbyUsersWithExpectedParameters(
            int radius,
            string body,
            string gender = "",
            string level = "",
            string position = "",
            string frequency = "")
        {
            var request = Request.Create()
                .WithPath("/api/users/nearby.php")
                .WithParam("lat", "45.5")
                .WithParam("lng", "15.8")
                .WithParam("radius", radius.ToString())
                .WithParam("current_user_id", "1");

            if (!string.IsNullOrWhiteSpace(gender))
            {
                request = request.WithParam("gender", gender);
            }

            if (!string.IsNullOrWhiteSpace(level))
            {
                request = request.WithParam("level", level);
            }

            if (!string.IsNullOrWhiteSpace(position))
            {
                request = request.WithParam("position", position);
            }

            if (!string.IsNullOrWhiteSpace(frequency))
            {
                request = request.WithParam("frequency", frequency);
            }

            _server
                .Given(request.UsingGet())
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
            AuthContext.Clear();
            _server.Stop();
            _server.Dispose();
        }
    }
}