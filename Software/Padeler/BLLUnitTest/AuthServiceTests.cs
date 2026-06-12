using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using BLL;
using DAL;
using EL;
using FakeItEasy;
using Xunit;

namespace BLLUnitTests
{
    public class AuthServiceTests : IDisposable
    {
        public AuthServiceTests()
        {
            AuthContext.Clear();
        }

        public void Dispose()
        {
            AuthContext.Clear();
        }

        [Fact]
        public async Task LoginAsync_GivenValidCredentials_ReturnsUserIdAndSetsAuthContext()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);

            A.CallTo(() => authRepository.LoginAsync("karlo", "lozinka123")).Returns(5);

            // Act
            var result = await service.LoginAsync("karlo", "lozinka123");

            // Assert
            Assert.Equal(5, result);
            Assert.True(AuthContext.IsLoggedIn);
            Assert.Equal(5, AuthContext.CurrentUserId);
            Assert.Equal("karlo", AuthContext.CurrentUsername);
        }

        [Fact]
        public async Task LoginAsync_GivenEmptyUsername_ThrowsException()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);

            A.CallTo(() => authRepository.LoginAsync("", "lozinka123"))
                .ThrowsAsync(new Exception("Username is required."));

            // Act
            Func<Task> act = async () => await service.LoginAsync("", "lozinka123");

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Username is required.", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_GivenEmptyPassword_ThrowsException()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);

            A.CallTo(() => authRepository.LoginAsync("karlo", ""))
                .ThrowsAsync(new Exception("Password is required."));

            // Act
            Func<Task> act = async () => await service.LoginAsync("karlo", "");

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Password is required.", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_GivenRepositoryThrowsException_ThrowsException()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);

            A.CallTo(() => authRepository.LoginAsync("karlo", "krivaLozinka")).ThrowsAsync(new Exception("Login failed."));

            // Act
            Func<Task> act = async () => await service.LoginAsync("karlo", "krivaLozinka");

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task LoginAsync_GivenValidCredentials_CallsRepositoryOnce()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);

            A.CallTo(() => authRepository.LoginAsync("karlo", "lozinka123"))
                .Returns(5);

            // Act
            await service.LoginAsync("karlo", "lozinka123");

            // Assert
            A.CallTo(() => authRepository.LoginAsync("karlo", "lozinka123"))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task RegisterAsync_GivenValidData_ReturnsUserId()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);
            var data = CreateValidRegisterData();

            A.CallTo(() => authRepository.RegisterAsync(A<RegisterRequestDto>._)).Returns(10);

            // Act
            var result = await CallRegisterAsync(service, data);

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task RegisterAsync_GivenValidData_CreatesCorrectRegisterDto()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);
            var data = CreateValidRegisterData();

            RegisterRequestDto capturedDto = null;

            A.CallTo(() => authRepository.RegisterAsync(A<RegisterRequestDto>._))
                .Invokes((RegisterRequestDto dto) => capturedDto = dto)
                .Returns(10);

            // Act
            await CallRegisterAsync(service, data);

            // Assert
            Assert.NotNull(capturedDto);
            Assert.Equal("karlo", capturedDto.Username);
            Assert.Equal("karlo@test.com", capturedDto.Email);
            Assert.Equal("lozinka123", capturedDto.Password);
            Assert.Equal("0911234567", capturedDto.Phone);
            Assert.Equal("Karlo", capturedDto.Name);
            Assert.Equal("Krsak", capturedDto.Surname);
            Assert.Equal("Male", capturedDto.Gender);
            Assert.Equal("2000-01-01", capturedDto.DateOfBirth);
            Assert.Equal("Weekly", capturedDto.Frequency);
            Assert.Equal("Beginner", capturedDto.Level);
            Assert.Equal("Right", capturedDto.Position);
            Assert.Null(capturedDto.ImageBase64);
            Assert.Null(capturedDto.MimeType);
        }

        [Fact]
        public async Task RegisterAsync_GivenTextFieldsWithSpaces_TrimsValues()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);
            var data = CreateValidRegisterData();

            data.Username = "  karlo  ";
            data.Email = "  karlo@test.com  ";
            data.Phone = "  0911234567  ";
            data.Name = "  Karlo  ";
            data.Surname = "  Krsak  ";

            RegisterRequestDto capturedDto = null;

            A.CallTo(() => authRepository.RegisterAsync(A<RegisterRequestDto>._))
                .Invokes((RegisterRequestDto dto) => capturedDto = dto)
                .Returns(10);

            // Act
            await CallRegisterAsync(service, data);

            // Assert
            Assert.NotNull(capturedDto);
            Assert.Equal("karlo", capturedDto.Username);
            Assert.Equal("karlo@test.com", capturedDto.Email);
            Assert.Equal("0911234567", capturedDto.Phone);
            Assert.Equal("Karlo", capturedDto.Name);
            Assert.Equal("Krsak", capturedDto.Surname);
        }

        [Fact]
        public async Task RegisterAsync_GivenNameIsNull_ThrowsException()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);
            var data = CreateValidRegisterData();
            data.Name = null;

            // Act
            Func<Task> act = async () => await CallRegisterAsync(service, data);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Name is required.", exception.Message);

            A.CallTo(() => authRepository.RegisterAsync(A<RegisterRequestDto>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task RegisterAsync_GivenSurnameIsEmpty_ThrowsException()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);
            var data = CreateValidRegisterData();
            data.Surname = "   ";

            // Act
            Func<Task> act = async () => await CallRegisterAsync(service, data);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Surname is required.", exception.Message);

            A.CallTo(() => authRepository.RegisterAsync(A<RegisterRequestDto>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task RegisterAsync_GivenFutureDateOfBirth_ThrowsException()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);
            var data = CreateValidRegisterData();
            data.DateOfBirth = DateTime.Today.AddDays(1);

            // Act
            Func<Task> act = async () => await CallRegisterAsync(service, data);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Date of birth cannot be in the future.", exception.Message);

            A.CallTo(() => authRepository.RegisterAsync(A<RegisterRequestDto>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task RegisterAsync_GivenUsernameTooShort_ThrowsException()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);
            var data = CreateValidRegisterData();
            data.Username = "kk";

            // Act
            Func<Task> act = async () => await CallRegisterAsync(service, data);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Username must be at least 3 characters.", exception.Message);

            A.CallTo(() => authRepository.RegisterAsync(A<RegisterRequestDto>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task RegisterAsync_GivenPasswordIsNull_ThrowsException()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);
            var data = CreateValidRegisterData();
            data.Password = null;

            // Act
            Func<Task> act = async () => await CallRegisterAsync(service, data);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Password is required.", exception.Message);

            A.CallTo(() => authRepository.RegisterAsync(A<RegisterRequestDto>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task RegisterAsync_GivenPasswordTooShort_ThrowsException()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);
            var data = CreateValidRegisterData();
            data.Password = "1234567";

            // Act
            Func<Task> act = async () => await CallRegisterAsync(service, data);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Password must be at least 8 characters.", exception.Message);

            A.CallTo(() => authRepository.RegisterAsync(A<RegisterRequestDto>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task RegisterAsync_GivenInvalidEmailFormat_ThrowsException()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);
            var data = CreateValidRegisterData();
            data.Email = "karlo.test.com";

            // Act
            Func<Task> act = async () => await CallRegisterAsync(service, data);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Email format is invalid.", exception.Message);

            A.CallTo(() => authRepository.RegisterAsync(A<RegisterRequestDto>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task RegisterAsync_GivenPhoneIsEmpty_ThrowsException()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);
            var data = CreateValidRegisterData();
            data.Phone = "";

            // Act
            Func<Task> act = async () => await CallRegisterAsync(service, data);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Phone is required.", exception.Message);

            A.CallTo(() => authRepository.RegisterAsync(A<RegisterRequestDto>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task RegisterAsync_GivenGenderIsMissing_ThrowsException()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);
            var data = CreateValidRegisterData();
            data.Gender = "";

            // Act
            Func<Task> act = async () => await CallRegisterAsync(service, data);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Gender is required.", exception.Message);

            A.CallTo(() => authRepository.RegisterAsync(A<RegisterRequestDto>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task RegisterAsync_GivenImageTooLarge_ThrowsException()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);
            var data = CreateValidRegisterData();
            data.ImagePngBytes = new byte[600001];

            // Act
            Func<Task> act = async () => await CallRegisterAsync(service, data);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("The selected image is too large. Please choose an image smaller than 600KB.", exception.Message);

            A.CallTo(() => authRepository.RegisterAsync(A<RegisterRequestDto>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task RegisterAsync_GivenValidImage_SendsBase64AndMimeType()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);
            var data = CreateValidRegisterData();
            data.ImagePngBytes = new byte[] { 1, 2, 3 };

            RegisterRequestDto capturedDto = null;

            A.CallTo(() => authRepository.RegisterAsync(A<RegisterRequestDto>._))
                .Invokes((RegisterRequestDto dto) => capturedDto = dto)
                .Returns(10);

            // Act
            await CallRegisterAsync(service, data);

            // Assert
            Assert.NotNull(capturedDto);
            Assert.Equal(Convert.ToBase64String(data.ImagePngBytes), capturedDto.ImageBase64);
            Assert.Equal("image/jpeg", capturedDto.MimeType);
        }

        [Fact]
        public void LoadProfileImageAsPngBytes_GivenEmptyPath_ThrowsException()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);

            // Act
            Action act = () => service.LoadProfileImageAsPngBytes("");

            // Assert
            var exception = Assert.Throws<Exception>(act);
            Assert.Equal("Image path is required.", exception.Message);
        }

        [Fact]
        public void LoadProfileImageAsPngBytes_GivenValidImage_ReturnsBytes()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".jpg");

            using (var image = new Bitmap(20, 20))
            {
                image.Save(path, ImageFormat.Jpeg);
            }

            try
            {
                // Act
                var result = service.LoadProfileImageAsPngBytes(path);

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Length > 0);
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        [Fact]
        public void Logout_GivenLoggedUser_ClearsAuthContext()
        {
            // Arrange
            var authRepository = A.Fake<IAuthRepository>();
            var service = new AuthService(authRepository);

            AuthContext.SetUser(5, "karlo");

            // Act
            service.Logout();

            // Assert
            Assert.False(AuthContext.IsLoggedIn);
            Assert.Equal(0, AuthContext.CurrentUserId);
            Assert.Equal("", AuthContext.CurrentUsername);
        }

        private static RegisterData CreateValidRegisterData()
        {
            return new RegisterData
            {
                Username = "karlo",
                Email = "karlo@test.com",
                Password = "lozinka123",
                Phone = "0911234567",
                Name = "Karlo",
                Surname = "Krsak",
                Gender = "Male",
                DateOfBirth = new DateTime(2000, 1, 1),
                Frequency = "Weekly",
                Level = "Beginner",
                Position = "Right",
                ImagePngBytes = null
            };
        }

        private static Task<int> CallRegisterAsync(AuthService service, RegisterData data)
        {
            return service.RegisterAsync(
                data.Username,
                data.Email,
                data.Password,
                data.Phone,
                data.Name,
                data.Surname,
                data.Gender,
                data.DateOfBirth,
                data.Frequency,
                data.Level,
                data.Position,
                data.ImagePngBytes
            );
        }

        private class RegisterData
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Phone { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Gender { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string Frequency { get; set; }
            public string Level { get; set; }
            public string Position { get; set; }
            public byte[] ImagePngBytes { get; set; }
        }
    }
}