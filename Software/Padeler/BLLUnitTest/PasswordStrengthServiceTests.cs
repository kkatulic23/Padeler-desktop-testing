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

        [Fact]
        public void CalculateScore_GivenLowercasePasswordWithValidLength_ReturnsTwo()
        {
            // Arrange
            var service = new PasswordStrengthService();

            // Act
            var result = service.CalculateScore("abcdefgh");

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void GetStrengthLabel_GivenLowercasePasswordWithValidLength_ReturnsWeak()
        {
            // Arrange
            var service = new PasswordStrengthService();

            // Act
            var result = service.GetStrengthLabel("abcdefgh");

            // Assert
            Assert.Equal("Weak", result);
        }

        [Fact]
        public void CalculateScore_GivenPasswordWithUppercase_ReturnsThree()
        {
            // Arrange
            var service = new PasswordStrengthService();

            // Act
            var result = service.CalculateScore("Abcdefgh");

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public void GetStrengthLabel_GivenPasswordWithUppercase_ReturnsOkay()
        {
            // Arrange
            var service = new PasswordStrengthService();

            // Act
            var result = service.GetStrengthLabel("Abcdefgh");

            // Assert
            Assert.Equal("Okay", result);
        }

        [Fact]
        public void CalculateScore_GivenPasswordWithNumber_ReturnsFour()
        {
            // Arrange
            var service = new PasswordStrengthService();

            // Act
            var result = service.CalculateScore("Abcdefgh1");

            // Assert
            Assert.Equal(4, result);
        }

        [Fact]
        public void GetStrengthLabel_GivenPasswordWithNumber_ReturnsGood()
        {
            // Arrange
            var service = new PasswordStrengthService();

            // Act
            var result = service.GetStrengthLabel("Abcdefgh1");

            // Assert
            Assert.Equal("Good", result);
        }
    }
}
