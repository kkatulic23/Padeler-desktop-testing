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
    public class BadgeServiceIntegrationTests : IDisposable
    {
        private readonly WireMockServer _server;

        public BadgeServiceIntegrationTests()
        {
            _server = WireMockServer.Start();
        }

        [Fact]
        public async Task RegisterSwipeAsync_GivenSuccessfulApiResponse_ReturnsSwipeNumberAndAwardedBadges()
        {
            // Arrange
            StubPost("/api/badges/add_swipe.php", "{\"success\":true,\"userId\":1,\"swipeNum\":10,\"awardedBadges\":[{\"BadgeId\":1,\"Name\":\"First swipe\",\"Description\":\"Prva interakcija\",\"Type\":\"Swipe\",\"PointsRequired\":10}]}");

            var service = CreateDefaultBadgeService();

            // Act
            var result = await service.RegisterSwipeAsync(1);

            // Assert
            Assert.Equal(10, result.newSwipeNum);
            Assert.Single(result.newlyAwarded);
            Assert.Equal(1, result.newlyAwarded[0].BadgeId);
            Assert.Equal("First swipe", result.newlyAwarded[0].Name);
        }

        [Fact]
        public async Task RegisterSwipeAsync_GivenSuccessfulApiResponse_SendsUserIdToApi()
        {
            // Arrange
            StubPost("/api/badges/add_swipe.php", "{\"success\":true,\"userId\":1,\"swipeNum\":5,\"awardedBadges\":[]}");

            var service = CreateDefaultBadgeService();

            // Act
            await service.RegisterSwipeAsync(1);

            // Assert
            var requests = _server.FindLogEntries(Request.Create().WithPath("/api/badges/add_swipe.php").UsingPost());

            Assert.Single(requests);

            var body = requests[0].RequestMessage.Body ?? "";

            Assert.Contains("\"user_id\":1", body);
        }

        [Fact]
        public async Task RegisterSwipeAsync_GivenApiError_ThrowsException()
        {
            // Arrange
            StubPost("/api/badges/add_swipe.php", "{\"success\":false,\"error\":\"Add swipe failed.\"}");

            var service = CreateDefaultBadgeService();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => service.RegisterSwipeAsync(1));

            // Assert
            Assert.Equal("Add swipe failed.", exception.Message);
        }

        [Fact]
        public async Task GetUserBadgesAsync_GivenSuccessfulApiResponse_ReturnsUserBadges()
        {
            // Arrange
            StubPost("/api/badges/get_user_badges.php", "{\"success\":true,\"userId\":1,\"badges\":[{\"BadgeId\":2,\"Name\":\"Active player\",\"Description\":\"Aktivan korisnik\",\"Type\":\"Activity\",\"PointsRequired\":20}]}");

            var service = CreateDefaultBadgeService();

            // Act
            var result = await service.GetUserBadgesAsync(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(2, result[0].BadgeId);
            Assert.Equal("Active player", result[0].Name);
            Assert.Equal("Activity", result[0].Type);
        }

        [Fact]
        public async Task GetUserBadgesAsync_GivenEmptyBadgeList_ReturnsEmptyList()
        {
            // Arrange
            StubPost("/api/badges/get_user_badges.php", "{\"success\":true,\"userId\":1,\"badges\":[]}");

            var service = CreateDefaultBadgeService();

            // Act
            var result = await service.GetUserBadgesAsync(1);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUserBadgesAsync_GivenApiError_ThrowsException()
        {
            // Arrange
            StubPost("/api/badges/get_user_badges.php", "{\"success\":false,\"error\":\"Get badges failed.\"}");

            var service = CreateDefaultBadgeService();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => service.GetUserBadgesAsync(1));

            // Assert
            Assert.Equal("Get badges failed.", exception.Message);
        }

        private BadgeService CreateDefaultBadgeService()
        {
            var apiClient = new ApiClient(new Uri(_server.Url + "/"));
            var repository = new BadgesRepository(apiClient);

            return new BadgeService(repository);
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