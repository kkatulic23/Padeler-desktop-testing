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

        [Fact]
        public async Task GetMyMatchesAsync_GivenExistingMatches_ReturnsMatches()
        {
            // Arrange
            StubGet("/api/match/get_my_matches.php", "{\"success\":true,\"matches\":[{\"matchId\":10,\"otherUserId\":2,\"otherName\":\"Kristian\",\"otherSurname\":\"Katulić\",\"otherPhone\":\"0924631257\"},{\"matchId\":11,\"otherUserId\":3,\"otherName\":\"Karlo\",\"otherSurname\":\"Kršak\",\"otherPhone\":\"0912415687\"}]}");

            var service = CreateDefaultMatchService();

            // Act
            var result = await service.GetMyMatchesAsync(1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(10, result[0].MatchId);
            Assert.Equal(11, result[1].MatchId);
            Assert.Equal(2, result[0].OtherUserId);
            Assert.Equal(3, result[1].OtherUserId);
        }

        [Fact]
        public async Task GetMatchedEntries_GivenVisibleAndHiddenEntries_ReturnsOnlyVisible()
        {

            // Arrange
            StubGet("/api/match/get_my_matches.php", "{\"success\":true,\"matches\":[{\"matchId\":10,\"otherUserId\":2,\"otherName\":\"Kristian\",\"otherSurname\":\"Katulić\",\"otherPhone\":\"0924631257\"},{\"matchId\":11,\"otherUserId\":3,\"otherName\":\"Karlo\",\"otherSurname\":\"Kršak\",\"otherPhone\":\"0912415687\"}]}");
            StubGet("/api/match/get_match_entries.php", "{\"success\":true,\"entries\":[{\"entryId\":1,\"currentUserId\":1,\"matchedUserId\":2,\"customNickname\":\"Kolega\",\"isHidden\":false},{\"entryId\":2,\"currentUserId\":1,\"matchedUserId\":3,\"customNickname\":\"Skriven\",\"isHidden\":true}]}");

            var service = CreateDefaultMatchService();

            // Act
            var result = await service.GetMatchedEntries(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(10, result[0].MatchId);
            Assert.Equal(2, result[0].OtherUserId);
            Assert.Equal("Kristian Katulić", result[0].FullName);
            Assert.Equal("Kolega", result[0].Nickname);
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

        private void StubGet(string path, string body)
        {
            _server.Given(Request.Create().WithPath(path).UsingGet()).RespondWith(Response.Create().WithStatusCode(200).WithHeader("Content-Type", "application/json").WithBody(body));
        }

        public void Dispose()
        {
            _server.Stop();
            _server.Dispose();
        }
    }
}
