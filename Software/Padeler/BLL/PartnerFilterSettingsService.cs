using System;
namespace BLL
{
    public class FilterSettings
    {
        public int RadiusKm { get; set; }
        public string FilterGender { get; set; }
        public string FilterLevel { get; set; }
        public string FilterPosition { get; set; }
        public string FilterFrequency { get; set; }
    }

    public class PartnerFilterSettingsService // Minimal service returning defaults
    {
        public FilterSettings GetDefaultSettings()
        {
            return new FilterSettings
            {
                RadiusKm = 50,
                FilterGender = string.Empty,
                FilterLevel = string.Empty,
                FilterPosition = string.Empty,
                FilterFrequency = string.Empty
            };
        }
    }
}
