using BLL;
using DAL;
using FakeItEasy;
using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
