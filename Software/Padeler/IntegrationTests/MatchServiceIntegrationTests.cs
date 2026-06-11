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
    public class MatchServiceIntegrationTests : IDisposable
    {
        private readonly WireMockServer _server;

        public MatchServiceIntegrationTests()
        {
            _server = WireMockServer.Start();
        }

        [Fact]
        public async Task LikeAsync_GivenSuccessfulMatch_ReturnsTrue()
        {
            // Arrange
            StubPost("/api/match/swipe.php", "{\"success\":true,\"matched\":true,\"message\":\"Match created\"}");
            var service = CreateDefaultMatchService();

            // Act
            var result = await service.LikeAsync(1, 2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task LikeAsync_GivenSuccessfulLikeWithoutMatch_ReturnsFalse()
        {
            // Arrange
            StubPost("/api/match/swipe.php", "{\"success\":true,\"matched\":false,\"message\":\"Like saved\"}");
            var service = CreateDefaultMatchService();

            // Act
            var result = await service.LikeAsync(1, 2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task LikeAsync_GivenApiError_ThrowsException()
        {
            // Arrange
            StubPost("/api/match/swipe.php", "{\"success\":false,\"matched\":false,\"error\":\"API error\"}");
            var service = CreateDefaultMatchService();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => service.LikeAsync(1, 2));

            // Assert
            Assert.Equal("API error", exception.Message);
        }

        [Fact]
        public async Task DislikeAsync_GivenSuccessfulResponse_SendsDislikeRequest()
        {

            // Arrange
            StubPost("/api/match/swipe.php", "{\"success\":true,\"matched\":false,\"message\":\"Dislike saved\"}");
            var service = CreateDefaultMatchService();

            // Act
            await service.DislikeAsync(1, 2);

            // Assert
            var requests = _server.LogEntries;

            Assert.Single(requests);

            var request = requests[0].RequestMessage;
            var body = request.Body ?? "";

            Assert.Contains("\"from_user_id\":1", body);
            Assert.Contains("\"to_user_id\":2", body);
            Assert.Contains("\"response\":\"DISLIKE\"", body);
        }

        private MatchService CreateDefaultMatchService()
        {
            var apiClient = new ApiClient(new Uri(_server.Url + "/"));
            var swipeRepository = new MatchesRepository(apiClient);
            var matchRepository = new MatchListRepository(apiClient);

            return new MatchService(swipeRepository, matchRepository);
        }

        private void StubPost(string path, string body)
        {
            _server.Given(Request.Create().WithPath(path).UsingPost()).RespondWith(Response.Create().WithStatusCode(200).WithHeader("Content-Type", "application/json").WithBody(body));
        }

        public void Dispose()
        {
            _server.Stop();
            _server.Dispose();
        }
    }
}
