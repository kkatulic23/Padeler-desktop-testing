using BLL;
using DAL;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BLLUnitTests
{
    public class CommentsServiceTests
    {
        [Fact]
        public void Constructor_ShouldCreateCommentsService()
        {
            // Act
            var service = new CommentsService();

            // Assert
            Assert.NotNull(service);
        }

        // New tests for comment length helper methods
        [Theory]
        [InlineData(null, 250)]
        [InlineData("", 250)]
        [InlineData("   ", 250)]
        [InlineData("Hello", 245)]
        public void GetRemainingCommentCharacters_VariousInputs_ReturnsExpected(string comment, int expected)
        {
            // Arrange
            var repository = A.Fake<ICommentsRepository>();
            var service = new CommentsService(repository);

            // Act
            var remaining = service.GetRemainingCommentCharacters(comment);

            // Assert
            Assert.Equal(expected, remaining);
        }

        [Fact]
        public void GetRemainingCommentCharacters_Exactly250_ReturnsZero()
        {
            // Arrange
            var repository = A.Fake<ICommentsRepository>();
            var service = new CommentsService(repository);
            var comment = new string('a', 250);

            // Act
            var remaining = service.GetRemainingCommentCharacters(comment);

            // Assert
            Assert.Equal(0, remaining);
        }

        [Fact]
        public void GetRemainingCommentCharacters_MoreThan250_ReturnsNegative()
        {
            // Arrange
            var repository = A.Fake<ICommentsRepository>();
            var service = new CommentsService(repository);
            var comment = new string('a', 251);

            // Act
            var remaining = service.GetRemainingCommentCharacters(comment);

            // Assert
            Assert.True(remaining < 0);
            Assert.Equal(-1, remaining);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        [InlineData("Hello", false)]
        [InlineData(null, false)]
        public void IsCommentTooLong_VariousInputs_ReturnsExpected(string comment, bool expected)
        {
            // Arrange
            var repository = A.Fake<ICommentsRepository>();
            var service = new CommentsService(repository);

            // Act
            var result = service.IsCommentTooLong(comment);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsCommentTooLong_Exactly250_ReturnsFalse()
        {
            // Arrange
            var repository = A.Fake<ICommentsRepository>();
            var service = new CommentsService(repository);
            var comment = new string('a', 250);

            // Act
            var result = service.IsCommentTooLong(comment);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsCommentTooLong_MoreThan250_ReturnsTrue()
        {
            // Arrange
            var repository = A.Fake<ICommentsRepository>();
            var service = new CommentsService(repository);
            var comment = new string('a', 251);

            // Act
            var result = service.IsCommentTooLong(comment);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(0, 2)]
        [InlineData(1, 0)]
        [InlineData(-1, 2)]
        [InlineData(1, -2)]
        public async Task AddRatingAsync_GivenInvalidUserIds_ThrowsArgumentException(int commentedId, int commenterId)
        {
            // Arrange
            var repository = A.Fake<ICommentsRepository>();
            var service = new CommentsService(repository);

            // Act
            Func<Task> act = async () => await service.AddRatingAsync(commentedId, commenterId, 5, "Good player");

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(act);
        }

        [Fact]
        public async Task AddRatingAsync_GivenSameUserIds_ThrowsArgumentException()
        {
            // Arrange
            var repository = A.Fake<ICommentsRepository>();
            var service = new CommentsService(repository);

            // Act
            Func<Task> act = async () => await service.AddRatingAsync(1, 1, 5, "Good player");

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(act);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(-1)]
        public async Task AddRatingAsync_GivenInvalidGrade_ThrowsArgumentException(int grade)
        {
            // Arrange
            var repository = A.Fake<ICommentsRepository>();
            var service = new CommentsService(repository);

            // Act
            Func<Task> act = async () => await service.AddRatingAsync(1, 2, grade, "Good player");

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(act);
        }

        [Fact]
        public async Task AddRatingAsync_GivenAlreadyRatedUser_ThrowsInvalidOperationException()
        {
            // Arrange
            var repository = A.Fake<ICommentsRepository>();

            A.CallTo(() => repository.GetRatedIdsAsync(2)).Returns(Task.FromResult(new List<int> { 1 }));

            var service = new CommentsService(repository);

            // Act
            Func<Task> act = async () => await service.AddRatingAsync(1, 2, 5, "Good player");

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(act);
            A.CallTo(() => repository.AddRatingAsync(A<int>._, A<int>._, A<int>._, A<string>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task AddRatingAsync_GivenValidData_ReturnsCommentId()
        {
            // Arrange
            var repository = A.Fake<ICommentsRepository>();

            A.CallTo(() => repository.GetRatedIdsAsync(2)).Returns(Task.FromResult(new List<int>()));

            A.CallTo(() => repository.AddRatingAsync(1, 2, 5, "Odlican igrac")).Returns(Task.FromResult(10));

            var service = new CommentsService(repository);

            // Act
            var result = await service.AddRatingAsync(1, 2, 5, "  Odlican igrac  ");

            // Assert
            Assert.Equal(10, result);
            A.CallTo(() => repository.AddRatingAsync(1, 2, 5, "Odlican igrac")).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddRatingAsync_GivenWhitespaceComment_SendsNullCommentToRepository()
        {
            // Arrange
            var repository = A.Fake<ICommentsRepository>();

            A.CallTo(() => repository.GetRatedIdsAsync(2)).Returns(Task.FromResult(new List<int>()));

            A.CallTo(() => repository.AddRatingAsync(1, 2, 4, null)).Returns(Task.FromResult(15));

            var service = new CommentsService(repository);

            // Act
            var result = await service.AddRatingAsync(1, 2, 4, "   ");

            // Assert
            Assert.Equal(15, result);
            A.CallTo(() => repository.AddRatingAsync(1, 2, 4, null)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddRatingAsync_GivenNullRatedIds_ReturnsCommentId()
        {
            // Arrange
            var repository = A.Fake<ICommentsRepository>();

            A.CallTo(() => repository.GetRatedIdsAsync(2)).Returns(Task.FromResult<List<int>>(null));

            A.CallTo(() => repository.AddRatingAsync(1, 2, 5, "Good player")).Returns(Task.FromResult(20));

            var service = new CommentsService(repository);

            // Act
            var result = await service.AddRatingAsync(1, 2, 5, "Good player");

            // Assert
            Assert.Equal(20, result);
        }

        // TDD dio - Katulić
        [Fact]
        public async Task AddRatingAsync_GivenCommentLongerThan250Characters_ThrowsArgumentException()
        {
            // Arrange
            var repository = A.Fake<ICommentsRepository>();
            var longComment = new string('a', 251);

            var service = new CommentsService(repository);

            // Act
            Func<Task> act = async () => await service.AddRatingAsync(1, 2, 5, longComment);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(act);
            A.CallTo(() => repository.AddRatingAsync(A<int>._, A<int>._, A<int>._, A<string>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task AddRatingAsync_GivenCommentWithExactly250Characters_AddsRating()
        {
            // Arrange
            var repository = A.Fake<ICommentsRepository>();
            var comment = new string('a', 250);

            A.CallTo(() => repository.GetRatedIdsAsync(1)).Returns(new List<int>());

            var service = new CommentsService(repository);

            // Act
            await service.AddRatingAsync(1, 2, 5, comment);

            // Assert
            A.CallTo(() => repository.AddRatingAsync(1, 2, 5, comment)).MustHaveHappenedOnceExactly();
        }
    }

}