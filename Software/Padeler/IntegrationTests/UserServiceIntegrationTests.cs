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
