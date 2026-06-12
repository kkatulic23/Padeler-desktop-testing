using BLL;
using EL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Xunit;

namespace BLLUnitTests
{
    public class ImageTests
    {
        [Fact]
        public void ConvertUserImage_GivenNullUserImage_ReturnsFallbackImage()
        {
            // Arrange
            var service = new ImageService();
            var fallback = new Bitmap(1, 1);

            // Act
            var result = service.ConvertUserImage(null, fallback);

            // Assert
            Assert.Same(fallback, result);
        }

        [Fact]
        public void ConvertUserImage_GivenUnsuccessfulUserImage_ReturnsFallbackImage()
        {
            // Arrange
            var service = new ImageService();
            var fallback = new Bitmap(1, 1);
            var userImage = new UserImageDto
            {
                Success = false,
                ImageBase64 = CreateValidImageBase64()
            };

            // Act
            var result = service.ConvertUserImage(userImage, fallback);

            // Assert
            Assert.Same(fallback, result);
        }

        [Fact]
        public void ConvertUserImage_GivenEmptyBase64_ReturnsFallbackImage()
        {
            // Arrange
            var service = new ImageService();
            var fallback = new Bitmap(1, 1);
            var userImage = new UserImageDto
            {
                Success = true,
                ImageBase64 = ""
            };

            // Act
            var result = service.ConvertUserImage(userImage, fallback);

            // Assert
            Assert.Same(fallback, result);
        }

        [Fact]
        public void ConvertUserImage_GivenInvalidBase64_ReturnsFallbackImage()
        {
            // Arrange
            var service = new ImageService();
            var fallback = new Bitmap(1, 1);
            var userImage = new UserImageDto
            {
                Success = true,
                ImageBase64 = "invalid-base64"
            };

            // Act
            var result = service.ConvertUserImage(userImage, fallback);

            // Assert
            Assert.Same(fallback, result);
        }

        [Fact]
        public void ConvertUserImage_GivenValidBase64_ReturnsConvertedImage()
        {
            // Arrange
            var service = new ImageService();
            var fallback = new Bitmap(1, 1);
            var userImage = new UserImageDto
            {
                Success = true,
                ImageBase64 = CreateValidImageBase64()
            };

            // Act
            var result = service.ConvertUserImage(userImage, fallback);

            // Assert
            Assert.NotNull(result);
            Assert.NotSame(fallback, result);
            Assert.Equal(2, result.Width);
            Assert.Equal(3, result.Height);
        }

        [Fact]
        public void ImageToBytes_GivenNullImage_ReturnsNull()
        {
            // Arrange
            var converter = new BLL.ImageConverter();

            // Act
            var result = converter.ImageToBytes(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ImageToBytes_GivenValidImage_ReturnsByteArray()
        {
            // Arrange
            var converter = new BLL.ImageConverter();
            var image = new Bitmap(2, 3);

            // Act
            var result = converter.ImageToBytes(image);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void BytesToImage_GivenNullBytes_ReturnsNull()
        {
            // Arrange
            var converter = new BLL.ImageConverter();

            // Act
            var result = converter.BytesToImage(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void BytesToImage_GivenEmptyBytes_ReturnsNull()
        {
            // Arrange
            var converter = new BLL.ImageConverter();

            // Act
            var result = converter.BytesToImage(new byte[0]);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void BytesToImage_GivenValidBytes_ReturnsImage()
        {
            // Arrange
            var converter = new BLL.ImageConverter();
            var image = new Bitmap(2, 3);
            var bytes = converter.ImageToBytes(image);

            // Act
            var result = converter.BytesToImage(bytes);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Width);
            Assert.Equal(3, result.Height);
        }

        [Fact]
        public void LoadImage_GivenEmptyPath_ThrowsException()
        {
            // Arrange
            var converter = new BLL.ImageConverter();

            // Act
            Action act = () => converter.LoadImage("");

            // Assert
            Assert.Throws<Exception>(act);
        }

        [Fact]
        public void LoadImage_GivenValidImagePath_ReturnsImage()
        {
            // Arrange
            var converter = new BLL.ImageConverter();
            var path = Path.Combine(Path.GetTempPath(), "test-image.jpg");

            using (var bitmap = new Bitmap(2, 3))
            {
                bitmap.Save(path, ImageFormat.Jpeg);
            }

            // Act
            var result = converter.LoadImage(path);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Width);
            Assert.Equal(3, result.Height);

            File.Delete(path);
        }

        private string CreateValidImageBase64()
        {
            using (var bitmap = new Bitmap(2, 3))
            using (var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Png);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }
}