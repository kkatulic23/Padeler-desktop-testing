using BLL;
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
            var result = service.FIlterUsers(null);

            // Assert
            Assert.Empty(result);
        }
    }
}
