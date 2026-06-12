using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private UserService CreateDefaultUserService()
        {
            var apiClient = new ApiClient(new Uri(_server.Url + "/"));
            var repository = new UsersRepository(apiClient);

            return new UserService(repository);
        }

        public void Dispose()
        {
            AuthContext.Clear();
            _server.Stop();
            _server.Dispose();
        }
    }
}
