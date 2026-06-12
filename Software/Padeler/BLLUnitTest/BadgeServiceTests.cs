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
    public class BadgeServiceTests
    {
        [Fact]
        public void Constructor_ShouldCreateBadgeService()
        {
            // Act
            var service = new BadgeService();

            // Assert
            Assert.NotNull(service);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task RegisterSwipeAsync_GivenInvalidUserId_ThrowsException(int userId)
        {
            // Arrange
            var repository = A.Fake<IBadgesRepository>();
            var service = new BadgeService(repository);

            // Act
            Func<Task> act = async () => await service.RegisterSwipeAsync(userId);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task RegisterSwipeAsync_GivenNullApiResponse_ThrowsException()
        {
            // Arrange
            var repository = A.Fake<IBadgesRepository>();

            A.CallTo(() => repository.AddSwipeAsync(1))
                .Returns(Task.FromResult<AddSwipeResponse>(null));

            var service = new BadgeService(repository);

            // Act
            Func<Task> act = async () => await service.RegisterSwipeAsync(1);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task RegisterSwipeAsync_GivenUnsuccessfulApiResponse_ThrowsException()
        {
            // Arrange
            var repository = A.Fake<IBadgesRepository>();

            A.CallTo(() => repository.AddSwipeAsync(1))
                .Returns(Task.FromResult(new AddSwipeResponse
                {
                    Success = false,
                    Error = "Add swipe failed."
                }));

            var service = new BadgeService(repository);

            // Act
            Func<Task> act = async () => await service.RegisterSwipeAsync(1);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task RegisterSwipeAsync_GivenSuccessfulApiResponse_ReturnsSwipeNumberAndAwardedBadges()
        {
            // Arrange
            var repository = A.Fake<IBadgesRepository>();
            var awardedBadges = new List<BadgeDto>
            {
                new BadgeDto
                {
                    BadgeId = 1,
                    Name = "First swipe"
                }
            };

            A.CallTo(() => repository.AddSwipeAsync(1))
                .Returns(Task.FromResult(new AddSwipeResponse
                {
                    Success = true,
                    SwipeNum = 10,
                    AwardedBadges = awardedBadges
                }));

            var service = new BadgeService(repository);

            // Act
            var result = await service.RegisterSwipeAsync(1);

            // Assert
            Assert.Equal(10, result.newSwipeNum);
            Assert.Single(result.newlyAwarded);
            Assert.Equal("First swipe", result.newlyAwarded[0].Name);
        }

        [Fact]
        public async Task RegisterSwipeAsync_GivenNullAwardedBadges_ReturnsEmptyBadgeList()
        {
            // Arrange
            var repository = A.Fake<IBadgesRepository>();

            A.CallTo(() => repository.AddSwipeAsync(1))
                .Returns(Task.FromResult(new AddSwipeResponse
                {
                    Success = true,
                    SwipeNum = 5,
                    AwardedBadges = null
                }));

            var service = new BadgeService(repository);

            // Act
            var result = await service.RegisterSwipeAsync(1);

            // Assert
            Assert.Empty(result.newlyAwarded);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetUserBadgesAsync_GivenInvalidUserId_ThrowsException(int userId)
        {
            // Arrange
            var repository = A.Fake<IBadgesRepository>();
            var service = new BadgeService(repository);

            // Act
            Func<Task> act = async () => await service.GetUserBadgesAsync(userId);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task GetUserBadgesAsync_GivenNullApiResponse_ThrowsException()
        {
            // Arrange
            var repository = A.Fake<IBadgesRepository>();

            A.CallTo(() => repository.GetUserBadgesAsync(1))
                .Returns(Task.FromResult<GetUserBadgesResponse>(null));

            var service = new BadgeService(repository);

            // Act
            Func<Task> act = async () => await service.GetUserBadgesAsync(1);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task GetUserBadgesAsync_GivenUnsuccessfulApiResponse_ThrowsException()
        {
            // Arrange
            var repository = A.Fake<IBadgesRepository>();

            A.CallTo(() => repository.GetUserBadgesAsync(1))
                .Returns(Task.FromResult(new GetUserBadgesResponse
                {
                    Success = false,
                    Error = "Get badges failed."
                }));

            var service = new BadgeService(repository);

            // Act
            Func<Task> act = async () => await service.GetUserBadgesAsync(1);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task GetUserBadgesAsync_GivenSuccessfulApiResponse_ReturnsUserBadges()
        {
            // Arrange
            var repository = A.Fake<IBadgesRepository>();
            var badges = new List<BadgeDto>
            {
                new BadgeDto
                {
                    BadgeId = 1,
                    Name = "Active player"
                }
            };

            A.CallTo(() => repository.GetUserBadgesAsync(1))
                .Returns(Task.FromResult(new GetUserBadgesResponse
                {
                    Success = true,
                    Badges = badges
                }));

            var service = new BadgeService(repository);

            // Act
            var result = await service.GetUserBadgesAsync(1);

            // Assert
            Assert.Single(result);
            Assert.Equal("Active player", result[0].Name);
        }

        [Fact]
        public async Task GetUserBadgesAsync_GivenNullBadges_ReturnsEmptyBadgeList()
        {
            // Arrange
            var repository = A.Fake<IBadgesRepository>();

            A.CallTo(() => repository.GetUserBadgesAsync(1))
                .Returns(Task.FromResult(new GetUserBadgesResponse
                {
                    Success = true,
                    Badges = null
                }));

            var service = new BadgeService(repository);

            // Act
            var result = await service.GetUserBadgesAsync(1);

            // Assert
            Assert.Empty(result);
        }
    }
}