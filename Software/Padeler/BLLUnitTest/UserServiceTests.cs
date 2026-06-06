using BLL;
using DAL;
using FakeItEasy;
using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EL;

namespace BLLUnitTests
{
    public class UserServiceTests
    {
        public UserServiceTests()
        {
            AuthContext.Clear();
        }

        [Fact]
        public async Task GetUSersForCardAsync_GivenUserIsNotLoggedIn_ThrowsException()
        {
            //Arrange
            var userRepository = A.Fake<IUsersRepository>();
            var service = new UserService(userRepository);

            //Act
            Func<Task> act = async () => await service.GetUsersForCardAsync(10, "", "", "", "");

            //Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task GetUsersForCardAsync_GivenNearbyUsers_ReturnsUserCards()
        {
            // Arrange
            AuthContext.SetUser(1, "fgrgac23");

            var userRepository = A.Fake<IUsersRepository>();
            var image = new UserImageDto
            {
                Success = true,
                ImageBase64 = "abc",
                MimeType = "image/png"
            };

            A.CallTo(() => userRepository.GetUserAsync(1)).Returns(Task.FromResult(CreateLoggedUser()));

            A.CallTo(() => userRepository.GetNearbyUsersCardAsync(1, 21, 10, 10, "M", "Beginner", "Left", "Often")).Returns(Task.FromResult(new List<UserDto>
            {
                new UserDto
                {
                    userId = 2,
                    name = "Karlo",
                    surname = "Kršak",
                    dateOfBirth = DateTime.Today.AddYears(-21),
                    levelOfPlay = "Beginner",
                    position = "Left",
                    frequencyOfPlay = "Often",
                    latitude = 50,
                    longitude = 16,
                    distanceKm = 5,
                    rating = 4.5
                }
            }));

            A.CallTo(() => userRepository.GetImageForCardAsync(2)).Returns(Task.FromResult(image));

            var service = new UserService(userRepository);

            //Act
            var result = await service.GetUsersForCardAsync(10, "M", "Beginner", "Left", "Often");

            //Assert
            Assert.Single(result);
            Assert.Equal(2, result[0].UserId);
            Assert.Equal("Karlo Kršak", result[0].FullName);
            Assert.Equal(21, result[0].Age);
            Assert.Equal("Beginner", result[0].Level);
            Assert.Equal("Left", result[0].Position);
            Assert.Equal("Often", result[0].FrequencyOfPlaying);
            Assert.Equal(5, result[0].DistanceKm);
            Assert.Equal(4.5, result[0].Rating);
            Assert.Equal(image, result[0].Image);
        }

        [Fact]
        public async Task GetUsersForCardAsync_GivenNearbyListContainsLoggesUser_SkipsLoggedUser()
        {
            // Arrange
            AuthContext.SetUser(1, "fgrgac23");

            var userRepository = A.Fake<IUsersRepository>();

            A.CallTo(() => userRepository.GetUserAsync(1)).Returns(Task.FromResult(CreateLoggedUser()));
            A.CallTo(() => userRepository.GetNearbyUsersCardAsync(1, 21, 10, 10, "", "", "", "")).Returns(Task.FromResult(new List<UserDto>
            {
                CreateLoggedUser(),
                new UserDto()
                {
                    userId = 3,
                    name = "Kristian",
                    surname = "Katulić",
                    dateOfBirth = DateTime.Today.AddYears(-21),
                    latitude = 51,
                    longitude = 15
                }
            }));

            A.CallTo(() => userRepository.GetImageForCardAsync(3)).Returns(Task.FromResult(new UserImageDto()));

            var service = new UserService(userRepository);

            //Act
            var result = await service.GetUsersForCardAsync(10, "", "", "", "");

            //Assert
            Assert.Single(result);
            Assert.Equal(3, result[0].UserId);
            A.CallTo(() => userRepository.GetImageForCardAsync(1)).MustNotHaveHappened();
        }

        private UserDto CreateLoggedUser()
        {
            return new UserDto
            {
                userId = 1,
                name = "Filip",
                surname = "Grgac",
                dateOfBirth = DateTime.Today.AddYears(-21),
                latitude = 21,
                longitude = 10
            };
        }
    }
}
