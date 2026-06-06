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
    }
}
