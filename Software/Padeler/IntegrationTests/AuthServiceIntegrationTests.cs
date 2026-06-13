using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using DAL;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace IntegrationTests
{
    public class AuthServiceIntegrationTests : IDisposable
    {
        private readonly WireMockServer _server;

        public AuthServiceIntegrationTests()
        {
            AuthContext.Clear();
            _server = WireMockServer.Start();
        }

        [Fact]
        public async Task LoginAsync_GivenSuccessfulApiResponse_ReturnsUserIdAndSetsAuthContext()
        {
            // Arrange
            StubPost("/api/users/login.php", "{\"success\":true,\"user_id\":25}");
            var service = CreateDefaultAuthService();

            // Act
            var result = await service.LoginAsync("kkrsak23", "Karlo123");

            // Assert
            Assert.Equal(25, result);
            Assert.True(AuthContext.IsLoggedIn);
            Assert.Equal(25, AuthContext.CurrentUserId);
            Assert.Equal("kkrsak23", AuthContext.CurrentUsername);
        }

        [Fact]
        public async Task LoginAsync_GivenUnsuccessfulApiResponse_ThrowsExceptionAndDoesNotLogIn()
        {
            // Arrange
            StubPost("/api/users/login.php", "{\"success\":false,\"user_id\":0,\"error\":\"Invalid credentials\"}");
            var service = CreateDefaultAuthService();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => service.LoginAsync("wrong", "wrong"));

            // Assert
            Assert.Equal("Invalid credentials", exception.Message);
            Assert.False(AuthContext.IsLoggedIn);
            Assert.Equal(0, AuthContext.CurrentUserId);
            Assert.Equal("", AuthContext.CurrentUsername);
        }

        [Fact]
        public async Task LoginAsync_GivenValidCredentials_SendsExpectedLoginRequest()
        {
            // Arrange
            StubPost("/api/users/login.php", "{\"success\":true,\"user_id\":25}");
            var service = CreateDefaultAuthService();

            // Act
            await service.LoginAsync("kkrsak23", "Karlo123");

            // Assert
            var requests = _server.LogEntries;

            Assert.Single(requests);

            var request = requests[0].RequestMessage;
            var body = request.Body ?? "";

            Assert.Contains("\"username\":\"kkrsak23\"", body);
            Assert.Contains("\"password\":\"Karlo123\"", body);
        }

        [Fact]
        public async Task RegisterAsync_GivenSuccessfulApiResponse_ReturnsUserId()
        {
            // Arrange
            StubPost("/api/users/register.php", "{\"success\":true,\"user_id\":30}");
            var service = CreateDefaultAuthService();

            // Act
            var result = await RegisterDefaultUserAsync(service);

            // Assert
            Assert.Equal(30, result);
        }

        [Fact]
        public async Task RegisterAsync_GivenValidData_SendsExpectedRegisterRequest()
        {
            // Arrange
            StubPost("/api/users/register.php", "{\"success\":true,\"user_id\":30}");
            var service = CreateDefaultAuthService();

            // Act
            await RegisterDefaultUserAsync(service);

            // Assert
            var requests = _server.LogEntries;

            Assert.Single(requests);

            var request = requests[0].RequestMessage;
            var body = request.Body ?? "";

            Assert.Contains("\"username\":\"noviuser\"", body);
            Assert.Contains("\"email\":\"noviuser@test.hr\"", body);
            Assert.Contains("\"password\":\"Lozinka123\"", body);
            Assert.Contains("\"phone\":\"0911111111\"", body);
            Assert.Contains("\"name\":\"Novi\"", body);
            Assert.Contains("\"surname\":\"Korisnik\"", body);
            Assert.Contains("\"gender\":\"Male\"", body);
            Assert.Contains("\"date_of_birth\":\"2000-01-01\"", body);
            Assert.Contains("\"frequency\":\"Often\"", body);
            Assert.Contains("\"level\":\"Beginner\"", body);
            Assert.Contains("\"position\":\"Left\"", body);
            Assert.Contains("\"image_base64\":null", body);
            Assert.Contains("\"mime_type\":null", body);
        }

        [Fact]
        public async Task RegisterAsync_GivenImageBytes_SendsImageBase64AndMimeType()
        {
            // Arrange
            StubPost("/api/users/register.php", "{\"success\":true,\"user_id\":31}");
            var service = CreateDefaultAuthService();

            var imageBytes = new byte[] { 1, 2, 3, 4 };
            var expectedBase64 = Convert.ToBase64String(imageBytes);

            // Act
            await RegisterDefaultUserAsync(
                service,
                username: "slikauser",
                email: "slikauser@test.hr",
                name: "Slika",
                imageBytes: imageBytes
            );

            // Assert
            var requests = _server.LogEntries;

            Assert.Single(requests);

            var request = requests[0].RequestMessage;
            var body = request.Body ?? "";

            Assert.Contains("\"image_base64\":\"" + expectedBase64 + "\"", body);
            Assert.Contains("\"mime_type\":\"image/jpeg\"", body);
        }

        [Fact]
        public async Task RegisterAsync_GivenExistingUsername_ThrowsException()
        {
            // Arrange
            StubPost("/api/users/register.php", "{\"success\":false,\"user_id\":0,\"error\":\"Username already exists\"}");
            var service = CreateDefaultAuthService();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                RegisterDefaultUserAsync(
                    service,
                    username: "postojeci",
                    email: "postojeci@test.hr",
                    name: "Postojeci"
                ));

            // Assert
            Assert.Equal("Username already exists", exception.Message);
        }

        [Fact]
        public async Task RegisterAsync_GivenExistingEmail_ThrowsException()
        {
            // Arrange
            StubPost("/api/users/register.php", "{\"success\":false,\"user_id\":0,\"error\":\"Email already exists\"}");
            var service = CreateDefaultAuthService();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                RegisterDefaultUserAsync(
                    service,
                    username: "noviuser",
                    email: "postojeci@test.hr"
                ));

            // Assert
            Assert.Equal("Email already exists", exception.Message);
        }

        private AuthService CreateDefaultAuthService()
        {
            var apiClient = new ApiClient(new Uri(_server.Url + "/"));
            var authRepository = new AuthRepository(apiClient);

            return new AuthService(authRepository);
        }

        private Task<int> RegisterDefaultUserAsync(
            AuthService service,
            string username = "noviuser",
            string email = "noviuser@test.hr",
            string password = "Lozinka123",
            string phone = "0911111111",
            string name = "Novi",
            string surname = "Korisnik",
            string gender = "Male",
            string frequency = "Often",
            string level = "Beginner",
            string position = "Left",
            byte[] imageBytes = null)
        {
            return service.RegisterAsync(
                username,
                email,
                password,
                phone,
                name,
                surname,
                gender,
                new DateTime(2000, 1, 1),
                frequency,
                level,
                position,
                imageBytes
            );
        }

        private void StubPost(string path, string body)
        {
            _server
                .Given(Request.Create().WithPath(path).UsingPost())
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
