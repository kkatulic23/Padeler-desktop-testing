using BLL;
using System.Collections.Generic;
using Xunit;

namespace BLLUnitTests
{
    public class ComboFillerTests
    {
        [Fact]
        public void GetFrequenciesOfPlay_GivenNoParameters_ReturnsExpectedFrequencies()
        {
            // Arrange
            var comboFiller = new ComboFiller();
            var expected = new List<string>
            {
                "Daily",
                "Weekly",
                "Monthly"
            };

            // Act
            var result = comboFiller.GetFrequenciesOfPlay();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetLevelsOfPlay_GivenNoParameters_ReturnsExpectedLevels()
        {
            // Arrange
            var comboFiller = new ComboFiller();
            var expected = new List<string>
            {
                "Beginner",
                "Lower intermediate",
                "Intermediate",
                "Advanced",
                "Professional"
            };

            // Act
            var result = comboFiller.GetLevelsOfPlay();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetPositions_GivenNoParameters_ReturnsExpectedPositions()
        {
            // Arrange
            var comboFiller = new ComboFiller();
            var expected = new List<string>
            {
                "Left",
                "Right"
            };

            // Act
            var result = comboFiller.GetPositions();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetGenders_GivenNoParameters_ReturnsExpectedGenders()
        {
            // Arrange
            var comboFiller = new ComboFiller();
            var expected = new List<string>
            {
                "Male",
                "Female",
                "Other"
            };

            // Act
            var result = comboFiller.GetGenders();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}