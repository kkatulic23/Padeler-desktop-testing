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
    public class CommentsServiceIntegrationTests : IDisposable
    {
        private readonly WireMockServer _server;

        public CommentsServiceIntegrationTests()
        {
            _server = WireMockServer.Start();
        }

        [Fact]
        public async Task AddRatingAsync_GivenValidData_ReturnsCommentId()
        {
            // Arrange
            StubGet("/api/comments/my_rated.php", "{\"success\":true,\"rated_ids\":[]}");
            StubPost("/api/comments/add_comment.php", "{\"success\":true,\"comment_id\":15}");

            var service = CreateDefaultCommentsService();

            // Act
            var result = await service.AddRatingAsync(1, 2, 5, "Odlican igrac");

            // Assert
            Assert.Equal(15, result);
        }

        [Fact]
        public async Task AddRatingAsync_GivenValidData_SendsTrimmedCommentToApi()
        {
            // Arrange
            StubGet("/api/comments/my_rated.php", "{\"success\":true,\"rated_ids\":[]}");
            StubPost("/api/comments/add_comment.php", "{\"success\":true,\"comment_id\":16}");

            var service = CreateDefaultCommentsService();

            // Act
            await service.AddRatingAsync(1, 2, 5, "  Odlican igrac  ");

            // Assert
            var requests = _server.LogEntries;

            Assert.Equal(2, requests.Count);

            var postRequest = requests[1].RequestMessage;
            var body = postRequest.Body ?? "";

            Assert.Contains("\"commented_id\":1", body);
            Assert.Contains("\"commenter_id\":2", body);
            Assert.Contains("\"grade\":5", body);
            Assert.Contains("\"comment\":\"Odlican igrac\"", body);
        }

        [Fact]
        public async Task AddRatingAsync_GivenWhitespaceComment_SendsNullCommentToApi()
        {
            // Arrange
            StubGet("/api/comments/my_rated.php", "{\"success\":true,\"rated_ids\":[]}");
            StubPost("/api/comments/add_comment.php", "{\"success\":true,\"comment_id\":17}");

            var service = CreateDefaultCommentsService();

            // Act
            await service.AddRatingAsync(1, 2, 4, "   ");

            // Assert
            var requests = _server.LogEntries;

            Assert.Equal(2, requests.Count);

            var postRequest = requests[1].RequestMessage;
            var body = postRequest.Body ?? "";

            Assert.Contains("\"comment\":null", body);
        }

        [Fact]
        public async Task AddRatingAsync_GivenAlreadyRatedUser_ThrowsInvalidOperationException()
        {
            // Arrange
            StubGet("/api/comments/my_rated.php", "{\"success\":true,\"rated_ids\":[1]}");
            StubPost("/api/comments/add_comment.php", "{\"success\":true,\"comment_id\":18}");

            var service = CreateDefaultCommentsService();

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddRatingAsync(1, 2, 5, "Good player"));

            // Assert
            Assert.Equal("You have already graded this user.", exception.Message);

            var requests = _server.LogEntries;

            Assert.Single(requests);
            Assert.Contains("/api/comments/my_rated.php", requests[0].RequestMessage.Path);
        }

        [Fact]
        public async Task AddRatingAsync_GivenApiError_ThrowsException()
        {
            // Arrange
            StubGet("/api/comments/my_rated.php", "{\"success\":true,\"rated_ids\":[]}");
            StubPost("/api/comments/add_comment.php", "{\"success\":false,\"error\":\"Failed to add comment.\"}");

            var service = CreateDefaultCommentsService();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => service.AddRatingAsync(1, 2, 5, "Good player"));

            // Assert
            Assert.Equal("Failed to add comment.", exception.Message);
        }

        [Fact]
        public async Task AddRatingAsync_GivenRatedIdsApiError_ThrowsException()
        {
            // Arrange
            StubGet("/api/comments/my_rated.php", "{\"success\":false,\"error\":\"Failed to fetch rated ids.\"}");

            var service = CreateDefaultCommentsService();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => service.AddRatingAsync(1, 2, 5, "Good player"));

            // Assert
            Assert.Equal("Failed to fetch rated ids.", exception.Message);
        }

        private CommentsService CreateDefaultCommentsService()
        {
            var apiClient = new ApiClient(new Uri(_server.Url + "/"));
            var repository = new CommentsRepository(apiClient);

            return new CommentsService(repository);
        }

        private void StubPost(string path, string body)
        {
            _server.Given(Request.Create().WithPath(path).UsingPost())
                    .RespondWith(Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(body));
        }

        private void StubGet(string path, string body)
        {
            _server.Given(Request.Create().WithPath(path).UsingGet())
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