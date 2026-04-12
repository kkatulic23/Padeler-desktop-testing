using Newtonsoft.Json;
using System.Collections.Generic;
namespace EL
{
    public class BadgeDto // Kristian Katulić
    {
        public int BadgeId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; }
        public string Type { get; set; }
        public int PointsRequired { get; set; }
        public string AwardedAt { get; set; }
    }
    public class AddSwipeResponse // Kristian Katulić
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("swipeNum")]
        public int SwipeNum { get; set; }
        [JsonProperty("awardedBadges")]
        public List<BadgeDto> AwardedBadges { get; set; } = new List<BadgeDto>();
        [JsonProperty("error")]
        public string Error { get; set; }
    }
    public class GetUserBadgesResponse // Kristian Katulić
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("badges")]
        public List<BadgeDto> Badges { get; set; } = new List<BadgeDto>();
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
