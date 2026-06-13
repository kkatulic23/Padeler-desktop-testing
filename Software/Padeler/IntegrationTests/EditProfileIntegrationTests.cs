using BLL;
using DAL;
using EL;
using System;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace IntegrationTests
{
    public class EditProfileIntegrationTests : IDisposable 
    {
        private readonly WireMockServer _server;

        public EditProfileIntegrationTests()
        {
            _server = WireMockServer.Start();
        }

        [Fact]
        public async Task GetUserDataAsync_GivenExistingUserId_ReturnsUserData()
        {
            // Arrange
            StubGet("/api/users/get_user.php", "{\"success\":true,\"user\":{\"UserId\":1,\"Username\":\"kkatulic\",\"Email\":\"kristian@example.com\",\"Phone\":\"0911234567\",\"Name\":\"Kristian\",\"Surname\":\"Katulić\",\"Gender\":\"Male\",\"DateOfBirth\":\"2001-01-01\",\"FrequencyOfPlaying\":\"Weekly\",\"Level\":\"Intermediate\",\"Position\":\"Left\"}}");

            StubGet("/api/users/get_user_image.php", "{\"success\":true,\"image_base64\":\"AQID\",\"mime_type\":\"image/png\"}");

            var service = CreateDefaultEditProfileService();

            // Act
            var result = await service.GetUserDataAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.userId);
            Assert.Equal("kkatulic", result.username);
            Assert.Equal("kristian@example.com", result.email);
            Assert.Equal("Kristian", result.name);
            Assert.Equal("Katulić", result.surname);
            Assert.Equal("image/png", result.mimeType);
            Assert.Equal(new byte[] { 1, 2, 3 }, result.image);
        }

        [Fact]
        public async Task GetUserDataAsync_GivenExistingUserId_CallsUserAndImageApi()
        {
            // Arrange
            StubGet("/api/users/get_user.php", "{\"success\":true,\"user\":{\"UserId\":1,\"Username\":\"kkatulic\",\"Email\":\"kristian@example.com\",\"Name\":\"Kristian\",\"Surname\":\"Katulić\"}}");

            StubGet("/api/users/get_user_image.php", "{\"success\":false,\"image_base64\":\"\",\"mime_type\":\"\"}");

            var service = CreateDefaultEditProfileService();

            // Act
            await service.GetUserDataAsync(1);

            // Assert
            var userRequests = _server.FindLogEntries(Request.Create().WithPath("/api/users/get_user.php").WithParam("user_id", "1").UsingGet());

            var imageRequests = _server.FindLogEntries(Request.Create().WithPath("/api/users/get_user_image.php").WithParam("user_id", "1").UsingGet());

            Assert.Single(userRequests);
            Assert.Single(imageRequests);
        }

        [Fact]
        public async Task GetUserDataAsync_GivenApiError_ThrowsException()
        {
            // Arrange
            StubGet("/api/users/get_user.php", "{\"success\":false,\"error\":\"User not found.\"}");

            StubGet("/api/users/get_user_image.php", "{\"success\":false,\"image_base64\":\"\",\"mime_type\":\"\"}");

            var service = CreateDefaultEditProfileService();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => service.GetUserDataAsync(1));

            // Assert
            Assert.Equal("User not found.", exception.Message);
        }

        [Fact]
        public async Task UpdateUserDataAsync_GivenValidUserRequest_SendsProfileDataToApi()
        {
            // Arrange
            StubPost("/api/users/update_profile.php", "{\"success\":true,\"user\":null}");

            var request = new UpdateUserRequest
            {
                userId = 1,
                username = "kkatulic",
                email = "kristian.updated@example.com",
                phone = "0917654321",
                name = "Kristian",
                surname = "Katulić",
                gender = "Male",
                dateOfBirth = new DateTime(2001, 1, 1),
                frequency = "Weekly",
                level = "Intermediate",
                position = "Left"
            };

            var service = CreateDefaultEditProfileService();

            // Act
            await service.UpdateUserDataAsync(request);

            // Assert
            var requests = _server.FindLogEntries(Request.Create().WithPath("/api/users/update_profile.php").UsingPost());

            Assert.Single(requests);

            var body = requests[0].RequestMessage.Body ?? "";

            Assert.Contains("\"user_id\":1", body);
            Assert.Contains("\"username\":\"kkatulic\"", body);
            Assert.Contains("\"email\":\"kristian.updated@example.com\"", body);
            Assert.Contains("\"phone\":\"0917654321\"", body);
            Assert.Contains("\"name\":\"Kristian\"", body);
            Assert.Contains("\"surname\":\"Katulić\"", body);
            Assert.Contains("\"frequency\":\"Weekly\"", body);
            Assert.Contains("\"level\":\"Intermediate\"", body);
            Assert.Contains("\"position\":\"Left\"", body);
        }

        [Fact]
        public async Task UpdateUserDataAsync_GivenApiError_ThrowsException()
        {
            // Arrange
            StubPost("/api/users/update_profile.php", "{\"success\":false,\"error\":\"Update failed.\"}");

            var request = new UpdateUserRequest
            {
                userId = 1,
                username = "kkatulic",
                email = "kristian@example.com"
            };

            var service = CreateDefaultEditProfileService();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => service.UpdateUserDataAsync(request));

            // Assert
            Assert.Equal("Update failed.", exception.Message);
        }

        private EditProfile CreateDefaultEditProfileService()
        {
            var apiClient = new ApiClient(new Uri(_server.Url + "/"));
            var repository = new UsersRepository(apiClient);

            return new EditProfile(repository);
        }

        private void StubGet(string path, string body)
        {
            _server.Given(Request.Create().WithPath(path).UsingGet())
                    .RespondWith(Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(body));
        }

        private void StubPost(string path, string body)
        {
            _server.Given(Request.Create().WithPath(path).UsingPost())
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