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

        [Fact]
        public async Task GetMatchedEntries_GivenVisibleMatches_ReturnsRows()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();

            A.CallTo(() => matchRepository.GetMyMatchesAsync(1)).Returns(Task.FromResult(new List<MatchDto>
            {
                new MatchDto
                {
                    MatchId = 10,
                    OtherUserId = 2,
                    OtherName = "Kristian",
                    OtherSurname = "Katulić",
                    OtherPhone = "0912543678"
                }
            }));

            A.CallTo(() => matchRepository.FindAllForUsersAsync(1)).Returns(Task.FromResult(new List<MatchEntryDto>
            {
                new MatchEntryDto
                {
                    CurrentUserId = 1,
                    MatchedUserId = 2,
                    CustomNickname = "Kolega",
                    IsHidden = false
                }
            }));

            var service = new MatchService(swipeRepository, matchRepository);

            // Act
            var result = await service.GetMatchedEntries(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(10, result[0].MatchId);
            Assert.Equal(2, result[0].OtherUserId);
            Assert.Equal("Kristian Katulić", result[0].FullName);
            Assert.Equal("0912543678", result[0].Phone);
            Assert.Equal("Kolega", result[0].Nickname);
        }

        [Fact]
        public async Task GetMatchedEntries_GivenHiddenEntry_DoesNotReturnRow()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();

            A.CallTo(() => matchRepository.GetMyMatchesAsync(1)).Returns(Task.FromResult(new List<MatchDto>
            {
                new MatchDto
                {
                    MatchId = 10,
                    OtherUserId = 2,
                    OtherName = "Kristian",
                    OtherSurname = "Katulić",
                    OtherPhone = "0912543678"
                }
            }));

            A.CallTo(() => matchRepository.FindAllForUsersAsync(1)).Returns(Task.FromResult(new List<MatchEntryDto>
            {
                new MatchEntryDto
                {
                    CurrentUserId = 1,
                    MatchedUserId = 2,
                    CustomNickname = "Kolega",
                    IsHidden = true
                }
            }));

            var service = new MatchService(swipeRepository, matchRepository);

            // Act
            var result = await service.GetMatchedEntries(1);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetMatchedEntries_GivenNoEntry_ReturnsRowWithEmptyNickname()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();

            A.CallTo(() => matchRepository.GetMyMatchesAsync(1)).Returns(Task.FromResult(new List<MatchDto>
            {
                new MatchDto
                {
                    MatchId = 10,
                    OtherUserId = 2,
                    OtherName = "Karlo",
                    OtherSurname = "Kršak",
                    OtherPhone = null
                }
            }));

            A.CallTo(() => matchRepository.FindAllForUsersAsync(1)).Returns(Task.FromResult(new List<MatchEntryDto>()));

            var service = new MatchService(swipeRepository, matchRepository);

            // Act
            var result = await service.GetMatchedEntries(1);

            // Assert
            Assert.Single(result);
            Assert.Equal("Karlo Kršak", result[0].FullName);
            Assert.Equal("", result[0].Nickname);
            Assert.Equal("", result[0].Phone);
        }

        [Fact]
        public async Task UpdateEntry_GivenMatchEntry_SavesEntryAndReturnsTrue()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();

            var entry = new MatchEntryDto
            {
                CurrentUserId = 1,
                MatchedUserId = 2,
                CustomNickname = "Nadimak"
            };

            var service = new MatchService(swipeRepository, matchRepository);

            // Act
            var result = await service.UpdateEntry(entry);

            // Assert
            Assert.True(result);
            A.CallTo(() => matchRepository.SaveAsync(entry)).MustHaveHappenedOnceExactly();
        }
    }
}
