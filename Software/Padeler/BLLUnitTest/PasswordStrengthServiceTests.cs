using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using BLL;

namespace BLLUnitTests
{
    public class PasswordStrengthServiceTests
    {
        [Fact]
        public void CalculateScore_GivenEmptyPassword_ReturnsZero()
        {
            // Arrange
            var service = new PasswordStrengthService();

            // Act
            var result = service.CalculateScore("");

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetStrengthLabel_GivenEmptyPassword_ReturnsVeryWeak()
        {
            // Arrange
            var service = new PasswordStrengthService();

            // Act
            var result = service.GetStrengthLabel("");

            // Assert
            Assert.Equal("Very weak", result);
        }
    }
}
