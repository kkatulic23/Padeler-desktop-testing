using BLL;
using DAL;
using EL;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BLLUnitTests
{
    public class MatchServiceTests
    {
        [Fact]
        public void Constructor_GivenDefaultConstructor_CreatesService()
        {
            // Arrange & Act
            var service = new MatchService();

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_GivenRepositoriesConstructor_CreatesService()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();

            // Act
            var service = new MatchService(swipeRepository, matchRepository);

            // Assert
            Assert.NotNull(service);
        }

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

        [Fact]
        public async Task DeleteEntry_GivenMatchEntry_SavesEntryAndReturnsTrue()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();

            var entry = new MatchEntryDto
            {
                CurrentUserId = 1,
                MatchedUserId = 2
            };

            var service = new MatchService(swipeRepository, matchRepository);

            // Act
            var result = await service.DeleteEntry(entry);

            // Assert
            Assert.True(result);
            A.CallTo(() => matchRepository.HideAsync(1, 2)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetMyMatchesAsync_GivenUserId_ReturnsMatches()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();

            var matches = new List<MatchDto>
            {
                new MatchDto
                {
                    MatchId = 10,
                    OtherUserId = 2,
                    OtherName = "Kristian",
                    OtherSurname = "Katulić",
                    OtherPhone = "0912543678"
                }
            };

            A.CallTo(() => matchRepository.GetMyMatchesAsync(1)).Returns(Task.FromResult(matches));

            var service = new MatchService(swipeRepository, matchRepository);

            // Act
            var result = await service.GetMyMatchesAsync(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(10, result[0].MatchId);
            Assert.Equal(2, result[0].OtherUserId);
            Assert.Equal("Kristian", result[0].OtherName);
            Assert.Equal("Katulić", result[0].OtherSurname);
            Assert.Equal("0912543678", result[0].OtherPhone);
            A.CallTo(() => matchRepository.GetMyMatchesAsync(1)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetEntry_GivenCurrentUserIdAndMatchedUserId_ReturnsEntry()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();

            var entry = new MatchEntryDto
            {
                CurrentUserId = 1,
                MatchedUserId = 2,
                CustomNickname = "Nadimak",
                IsHidden = false
            };

            A.CallTo(() => matchRepository.FindByUsersAsync(1, 2)).Returns(Task.FromResult(entry));
            var service = new MatchService(swipeRepository, matchRepository);

            // Act
            var result = await service.GetEntry(1, 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.CurrentUserId);
            Assert.Equal(2, result.MatchedUserId);
            Assert.Equal("Nadimak", result.CustomNickname);
            Assert.False(result.IsHidden);
            A.CallTo(() => matchRepository.FindByUsersAsync(1, 2)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void FilterMatchesByName_GivenNullMatches_ReturnsEmptyList()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();
            var service = new MatchService(swipeRepository, matchRepository);

            // Act
            var result = service.FilterMatchesByName(null, "test");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void FilterMatchesByName_GivenEmptySearch_ReturnsOriginalList()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();
            var service = new MatchService(swipeRepository, matchRepository);

            var rows = new List<MatchRow>
            {
                new MatchRow { OtherUserId = 1, FullName = "Ivan Horvat", Phone = "", Nickname = "" },
                new MatchRow { OtherUserId = 2, FullName = "Ana Ivić", Phone = "", Nickname = "" }
            };

            // Act
            var result = service.FilterMatchesByName(rows, "");

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void FilterMatchesByName_IsCaseInsensitiveAndPartialMatch()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();
            var service = new MatchService(swipeRepository, matchRepository);

            var rows = new List<MatchRow>
            {
                new MatchRow { OtherUserId = 1, FullName = "Kristian Katulić" },
                new MatchRow { OtherUserId = 2, FullName = "Karlo Kršak" },
                new MatchRow { OtherUserId = 3, FullName = "Ana" }
            };

            // Act
            var result1 = service.FilterMatchesByName(rows, "kristian"); // full name, different case
            var result2 = service.FilterMatchesByName(rows, "krš"); // partial surname

            // Assert
            Assert.Single(result1);
            Assert.Equal(1, result1[0].OtherUserId);

            Assert.Single(result2);
            Assert.Equal(2, result2[0].OtherUserId);
        }

        [Fact]
        public void FilterMatchesByName_NoMatches_ReturnsEmptyList()
        {
            // Arrange
            var swipeRepository = A.Fake<ISwipeRepository>();
            var matchRepository = A.Fake<IMatchRepository>();
            var service = new MatchService(swipeRepository, matchRepository);

            var rows = new List<MatchRow>
            {
                new MatchRow { OtherUserId = 1, FullName = "Ivan Horvat" },
                new MatchRow { OtherUserId = 2, FullName = "Ana Ivić" }
            };

            // Act
            var result = service.FilterMatchesByName(rows, "zzz");

            // Assert
            Assert.Empty(result);
        }
    }
}