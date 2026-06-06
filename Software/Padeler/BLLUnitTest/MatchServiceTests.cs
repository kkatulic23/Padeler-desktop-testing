using BLL;
using DAL;
using EL;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BLLUnitTests
{
    public class MatchServiceTests
    {
        [Fact]
        public async Task LikeAsync_GivenMatchedSwipe_ReturnsFalse()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();

            A.CallTo(() => swipeRepository.SwipeAsync(1, 2, "LIKE")).Returns(Task.FromResult(new SwipeResponse
            {
                Success = true,
                Matched = false
            }));

            var service = new MatchService(swipeRepository, matchRepository);

            // Act
            var result = await service.LikeAsync(1, 2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task LikeAsync_GivenMatchedSwipe_ReturnsTrue()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();

            A.CallTo(() => swipeRepository.SwipeAsync(1, 2, "LIKE")).Returns(Task.FromResult(new SwipeResponse
            {
                Success = true,
                Matched = true
            }));

            var service = new MatchService(swipeRepository, matchRepository);

            // Act
            var result = await service.LikeAsync(1, 2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task LikeAsync_GivenNullApiResponse_ThrowsException()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();

            A.CallTo(() => swipeRepository.SwipeAsync(1, 2, "LIKE")).Returns(Task.FromResult<SwipeResponse>(null));

            var service = new MatchService(swipeRepository, matchRepository);

            // Act
            Func<Task> act = async () => await service.LikeAsync(1, 2);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task LikeAsync_GivenUnsuccessfulApiResponse_ThrowsException()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();

            A.CallTo(() => swipeRepository.SwipeAsync(1, 2, "LIKE")).Returns(Task.FromResult(new SwipeResponse
            {
                Success = false,
                Error = "API error"
            }));

            var service = new MatchService(swipeRepository, matchRepository);

            // Act
            Func<Task> act = async () => await service.LikeAsync(1, 2);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task DislikeAsync_GivenSuccessfulResponse_DoesNotThrowException()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();


            A.CallTo(() => swipeRepository.SwipeAsync(1, 2, "DISLIKE")).Returns(Task.FromResult(new SwipeResponse
            {
                Success = true,
            }));

            var service = new MatchService(swipeRepository, matchRepository);

            // Act
            var exception = await Record.ExceptionAsync(() => service.DislikeAsync(1, 2));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task DislikeAsync_GivenNullApiResponse_ThrowsException()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();

            A.CallTo(() => swipeRepository.SwipeAsync(1, 2, "DISLIKE")).Returns(Task.FromResult<SwipeResponse>(null));

            var service = new MatchService(swipeRepository, matchRepository);

            // Act
            Func<Task> act = async () => await service.DislikeAsync(1, 2);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task DislikeAsync_GivenUnsuccessfulApiResponse_ThrowsException()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();

            A.CallTo(() => swipeRepository.SwipeAsync(1, 2, "DISLIKE")).Returns(Task.FromResult(new SwipeResponse
            {
                Success = false,
                Error = "API error"
            }));

            var service = new MatchService(swipeRepository, matchRepository);

            // Act
            Func<Task> act = async () => await service.DislikeAsync(1, 2);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
        }
    }
}
