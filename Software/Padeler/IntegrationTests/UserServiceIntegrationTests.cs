using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace IntegrationTests
{
    public class UserServiceIntegrationTests : IDisposable
    {
        private readonly WireMockServer _server;

        public UserServiceIntegrationTests()
        {
            _server = WireMockServer.Start();
            AuthContext.SetUser(1, "filip");
        }

        [Fact]
        public async Task GetUsersForCardAsync_GivenUserIsNotLoggedIn_ThrowsException()
        {
            // Arrange
            AuthContext.Clear();
            var service = CreateDefaultUserService();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => service.GetUsersForCardAsync(10, "", "", "", ""));

            // Assert
            Assert.Equal("The user is not logged in!", exception.Message);
        }

        [Fact]
        public async Task GetUsersForCardAsync_GivenLoggedUserWithoutLocation_ReturnsEmptyList()
        {
            // Arrange
            StubLoggedUserWithoutLocation();
            var service = CreateDefaultUserService();

            // Act
            var result = await service.GetUsersForCardAsync(10, "", "", "", "");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsersForCardAsync_GiveNearbyUsers_ReturnsUserCards()
        {
            // Arrange
            StubLoggedUserWithLocation();
            StubGet("/api/users/nearby.php", "{\"success\":true,\"users\":[{\"UserId\":2,\"Name\":\"Ivan\",\"Surname\":\"Ivić\",\"DateOfBirth\":\"2000-05-10\",\"FrequencyOfPlaying\":\"Weekly\",\"Level\":\"Begginer\",\"Position\":\"Left\",\"Rating\":3.8,\"distance_km\":5.1}]}");
            StubUserImage(2, "{\"success\":true,\"image_base64\":\"abc\",\"mime_type\":\"image/png\"}");

            var service = CreateDefaultUserService();

            // Act
            var result = await service.GetUsersForCardAsync(10, "", "", "", "");

            // Assert
            Assert.Equal(2, result[0].UserId);
            Assert.Equal("Ivan Ivić", result[0].FullName);
            Assert.Equal("Left", result[0].Position);
            Assert.Equal("Begginer", result[0].Level);
            Assert.Equal("Weekly", result[0].FrequencyOfPlaying);
            Assert.Equal("image/png", result[0].Image.MimeType);
        }

        private UserService CreateDefaultUserService()
        {
            var apiClient = new ApiClient(new Uri(_server.Url + "/"));
            var repository = new UsersRepository(apiClient);

            return new UserService(repository);
        }

        private void StubLoggedUserWithoutLocation()
        {
            StubGet("/api/users/get_user.php", "{\"success\":true,\"user\":{\"UserId\":1,\"Name\":\"Filip\",\"Surname\":\"Grgac\",\"DateOfBirth\":\"2004-11-24\",\"Latitude\":null,\"Longitude\":null}}");
            StubUserImage(1, "{\"success\":false,\"image_base64\":\"\",\"mime_type\":\"\"}");
        }

        private void StubLoggedUserWithLocation()
        {
            StubGet("/api/users/get_user.php", "{\"success\":true,\"user\":{\"UserId\":1,\"Name\":\"Filip\",\"Surname\":\"Grgac\",\"DateOfBirth\":\"2004-11-24\",\"Latitude\":45.5,\"Longitude\":15.8}}");
            StubUserImage(1, "{\"success\":false,\"image_base64\":\"\",\"mime_type\":\"\"}");
        }

        private void StubGet(string path, string body)
        {
            _server.Given(Request.Create().WithPath(path).UsingGet()).RespondWith(Response.Create().WithStatusCode(200).WithHeader("Content-Type", "application/json").WithBody(body));
        }

        private void StubUserImage(int userId, string body)
        {
            _server.Given(Request.Create().WithPath("/api/users/get_user_image.php").WithParam("user_id", userId.ToString()).UsingGet()).RespondWith(Response.Create().WithStatusCode(200).WithHeader("Content-Type", "application/json").WithBody(body));
        }

        public void Dispose()
        {
            AuthContext.Clear();
            _server.Stop();
            _server.Dispose();
        }
    }
}
