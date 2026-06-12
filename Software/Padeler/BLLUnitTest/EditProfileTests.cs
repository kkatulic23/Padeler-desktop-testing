using BLL;
using DAL;
using EL;
using FakeItEasy;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BLLUnitTests
{
    public class EditProfileTests
    {
        [Fact]
        public void Constructor_ShouldCreateEditProfileService()
        {
            // Act
            var service = new EditProfile();

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public async Task GetUserDataAsync_GivenExistingUserId_ReturnsUserData()
        {
            // Arrange
            var repository = A.Fake<IUsersRepository>();

            var expectedUser = new UserDto
            {
                userId = 1,
                username = "kkatulic",
                email = "kristian@example.com",
                name = "Kristian",
                surname = "Katulić"
            };

            A.CallTo(() => repository.GetUserAsync(1)).Returns(Task.FromResult(expectedUser));

            var service = new EditProfile(repository);

            // Act
            var result = await service.GetUserDataAsync(1);

            // Assert
            Assert.Same(expectedUser, result);
            A.CallTo(() => repository.GetUserAsync(1)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetUserDataAsync_GivenRepositoryReturnsNull_ReturnsNull()
        {
            // Arrange
            var repository = A.Fake<IUsersRepository>();

            A.CallTo(() => repository.GetUserAsync(1)).Returns(Task.FromResult<UserDto>(null));

            var service = new EditProfile(repository);

            // Act
            var result = await service.GetUserDataAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserDataAsync_GivenRepositoryThrowsException_ThrowsException()
        {
            // Arrange
            var repository = A.Fake<IUsersRepository>();

            A.CallTo(() => repository.GetUserAsync(1)).Returns(Task.FromException<UserDto>(new Exception("Get user failed.")));

            var service = new EditProfile(repository);

            // Act
            Func<Task> act = async () => await service.GetUserDataAsync(1);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task UpdateUserDataAsync_GivenValidUserRequest_CallsRepositoryOnce()
        {
            // Arrange
            var repository = A.Fake<IUsersRepository>();

            var request = new UpdateUserRequest
            {
                userId = 1,
                username = "kkatulic",
                email = "kristian@example.com",
                phone = "0911234567",
                name = "Kristian",
                surname = "Katulić",
                gender = "Male",
                dateOfBirth = new DateTime(2001, 1, 1),
                frequency = "Weekly",
                level = "Intermediate",
                position = "Left"
            };

            var service = new EditProfile(repository);

            // Act
            await service.UpdateUserDataAsync(request);

            // Assert
            A.CallTo(() => repository.UpdateUserAsync(request)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateUserDataAsync_GivenRepositoryThrowsException_ThrowsException()
        {
            // Arrange
            var repository = A.Fake<IUsersRepository>();

            var request = new UpdateUserRequest
            {
                userId = 1,
                username = "kkatulic",
                email = "kristian@example.com"
            };

            A.CallTo(() => repository.UpdateUserAsync(request)).Returns(Task.FromException(new Exception("Update failed.")));

            var service = new EditProfile(repository);

            // Act
            Func<Task> act = async () => await service.UpdateUserDataAsync(request);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
        }
    }
}