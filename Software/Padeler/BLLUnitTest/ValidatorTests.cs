using BLL;
using EL;
using Xunit;

namespace BLLUnitTests
{
    public class ValidatorTests
    {
        [Fact]
        public void ValidateUser_GivenValidUser_ReturnsNoErrors()
        {
            // Arrange
            var validator = new Validator();
            var user = CreateValidUser();

            // Act
            var result = validator.ValidateUser(user);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ValidateUser_GivenMissingName_ReturnsNameError()
        {
            // Arrange
            var validator = new Validator();
            var user = CreateValidUser();
            user.name = "";

            // Act
            var result = validator.ValidateUser(user);

            // Assert
            Assert.Contains("Name is required.", result);
        }

        [Fact]
        public void ValidateUser_GivenMissingSurname_ReturnsSurnameError()
        {
            // Arrange
            var validator = new Validator();
            var user = CreateValidUser();
            user.surname = "   ";

            // Act
            var result = validator.ValidateUser(user);

            // Assert
            Assert.Contains("Surname is required.", result);
        }

        [Fact]
        public void ValidateUser_GivenMissingUsername_ReturnsUsernameError()
        {
            // Arrange
            var validator = new Validator();
            var user = CreateValidUser();
            user.username = null;

            // Act
            var result = validator.ValidateUser(user);

            // Assert
            Assert.Contains("Username is required.", result);
        }

        [Fact]
        public void ValidateUser_GivenInvalidEmail_ReturnsEmailError()
        {
            // Arrange
            var validator = new Validator();
            var user = CreateValidUser();
            user.email = "kriviEmail";

            // Act
            var result = validator.ValidateUser(user);

            // Assert
            Assert.Contains("A valid email is required.", result);
        }

        [Fact]
        public void ValidateUser_GivenInvalidPhone_ReturnsPhoneError()
        {
            // Arrange
            var validator = new Validator();
            var user = CreateValidUser();
            user.phone = "abc123";

            // Act
            var result = validator.ValidateUser(user);

            // Assert
            Assert.Contains("A valid phone number is required.", result);
        }

        private static UpdateUserRequest CreateValidUser()
        {
            return new UpdateUserRequest
            {
                name = "Karlo",
                surname = "Krsak",
                username = "karlo",
                email = "karlo@test.com",
                phone = "0911234567"
            };
        }
    }
}