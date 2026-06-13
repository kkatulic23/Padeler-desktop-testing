using BLL;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BLLUnitTests
{
    public class RecentlyViewedServiceTests
    {
        public RecentlyViewedServiceTests()
        {
            var service = new RecentlyViewedService();
            service.Clear();
        }

        [Fact]
        public void FilterUsers_GivenNullUsers_ReturnsEmptyList()
        {
            // Arrange
            var service = new RecentlyViewedService();

            // Act
            var result = service.FilterUsers(null);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FilterUsers_GivenEmptyUsers_ReturnsEmptyList()
        {
            // Arrange
            var service = new RecentlyViewedService();

            // Act
            var result = service.FilterUsers(new List<UserCardDto>());

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FilterUsers_GivenUsersAndNoRecentlyViewed_ReturnsAllUsers()
        {
            // Arrange
            var service = new RecentlyViewedService();

            var users = new List<UserCardDto>
            {
                CreateUser(1),
                CreateUser(2)
            };

            // Act
            var result = service.FilterUsers(users);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void FilterUsers_GivenOneRecentlyViewedUser_RemovesThatUser()
        {
            // Arrange
            var service = new RecentlyViewedService();
            service.AddSwipedUser(1);

            var users = new List<UserCardDto>
            {
                CreateUser(1),
                CreateUser(2)
            };

            // Act
            var result = service.FilterUsers(users);

            // Assert
            Assert.Single(result);
            Assert.Equal(2, result[0].UserId);
        }

        [Fact]
        public void FilterUsers_GivenAllUsersRecentlySwiped_ReturnsOriginalList()
        {
            // Arrange
            var service = new RecentlyViewedService();
            service.AddSwipedUser(1);
            service.AddSwipedUser(2);

            var users = new List<UserCardDto>
            {
                CreateUser(1),
                CreateUser(2)
            };

            // Act
            var result = service.FilterUsers(users);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].UserId);
            Assert.Equal(2, result[1].UserId);
        }

        [Fact]
        public void AddViewedUser_GivenSameUserAddTwice_DoesNotDuplicateUser()
        {
            // Arrange
            var service = new RecentlyViewedService();
            service.AddSwipedUser(1);
            service.AddSwipedUser(1);

            var users = new List<UserCardDto>
            {
                CreateUser(1),
                CreateUser(2)
            };

            // Act
            var result = service.FilterUsers(users);

            // Assert
            Assert.Single(result);
            Assert.Equal(2, result[0].UserId);
        }

        private UserCardDto CreateUser(int userId)
        {
            return new UserCardDto
            {
                UserId = userId
            };
        }
    }
}
