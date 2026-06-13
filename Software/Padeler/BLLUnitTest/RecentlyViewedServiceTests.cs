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
        [Fact]
        public async Task FilterUsers_GivenNullUsers_ReturnsEmptyList()
        {
            // Arrange
            var service = new RecentlyViewedService();

            // Act
            var result = service.FilterUsers(null);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task FilterUsers_GivenEmptyUsers_ReturnsEmptyList()
        {
            // Arrange
            var service = new RecentlyViewedService();

            // Act
            var result = service.FilterUsers(new List<UserCardDto>());

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task FilterUsers_GivenUsersAndNoRecentlyViewed_ReturnsAllUsers()
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

        private UserCardDto CreateUser(int userId)
        {
            return new UserCardDto
            {
                UserId = userId
            };
        }
    }
}
