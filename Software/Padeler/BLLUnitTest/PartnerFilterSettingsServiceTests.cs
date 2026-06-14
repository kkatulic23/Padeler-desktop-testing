using BLL;
using BLL;
using Xunit;

namespace BLLUnitTests
{
    public class PartnerFilterSettingsServiceTests
    {
        [Fact]
        public void GetDefaultSettings_ReturnsNonNullObject()
        {
            // Arrange
            var svc = new PartnerFilterSettingsService();

            // Act
            var defaults = svc.GetDefaultSettings();

            // Assert
            Assert.NotNull(defaults);
        }

        [Fact]
        public void GetDefaultSettings_ReturnsRadius50()
        {
            // Arrange
            var svc = new PartnerFilterSettingsService();

            // Act
            var defaults = svc.GetDefaultSettings();

            // Assert
            Assert.Equal(50, defaults.RadiusKm);
        }

        [Fact]
        public void GetDefaultSettings_ReturnsEmptyStringsForFilters()
        {
            // Arrange
            var svc = new PartnerFilterSettingsService();

            // Act
            var defaults = svc.GetDefaultSettings();

            // Assert
            Assert.Equal(string.Empty, defaults.FilterGender);
            Assert.Equal(string.Empty, defaults.FilterLevel);
            Assert.Equal(string.Empty, defaults.FilterPosition);
            Assert.Equal(string.Empty, defaults.FilterFrequency);
        }

        [Fact]
        public void GetDefaultSettings_ConsecutiveCallsReturnSeparateButEqualObjects()
        {
            // Arrange
            var svc = new PartnerFilterSettingsService();

            // Act
            var a = svc.GetDefaultSettings();
            var b = svc.GetDefaultSettings();

            // Assert
            Assert.NotSame(a, b);
            Assert.Equal(a.RadiusKm, b.RadiusKm);
            Assert.Equal(a.FilterGender, b.FilterGender);
            Assert.Equal(a.FilterLevel, b.FilterLevel);
            Assert.Equal(a.FilterPosition, b.FilterPosition);
            Assert.Equal(a.FilterFrequency, b.FilterFrequency);
        }
    }
}
