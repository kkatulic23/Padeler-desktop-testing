using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class MatchDto
    {
        [JsonProperty("matchId")]
        public int MatchId { get; set; }

        [JsonProperty("otherUserId")]
        public int OtherUserId { get; set; }

        [JsonProperty("otherName")]
        public string OtherName { get; set; }

        [JsonProperty("otherSurname")]
        public string OtherSurname { get; set; }

        [JsonProperty("otherPhone")]
        public string OtherPhone { get; set; }
    }
    public class GetMyMatchesResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("matches")]
        public List<MatchDto> Matches { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }


}
