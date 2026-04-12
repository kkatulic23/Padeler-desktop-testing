using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class MatchEntryDto
    {
        [JsonProperty("entryId")]
        public int EntryId { get; set; }

        [JsonProperty("currentUserId")]
        public int CurrentUserId { get; set; }

        [JsonProperty("matchedUserId")]
        public int MatchedUserId { get; set; }

        [JsonProperty("customNickname")]
        public string CustomNickname { get; set; }

        [JsonProperty("isHidden")]
        public bool IsHidden { get; set; }
    }

    public class GetMatchEntriesResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("entries")]
        public List<MatchEntryDto> Entries { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }

    public class GetMatchEntryResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("entry")]
        public MatchEntryDto Entry { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }

    public class SaveMatchEntryRequest
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("matched_user_id")]
        public int MatchedUserId { get; set; }

        [JsonProperty("custom_nickname")]
        public string CustomNickname { get; set; }

        [JsonProperty("is_hidden")]
        public bool IsHidden { get; set; }
    }

    public class HideMatchEntryRequest
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("matched_user_id")]
        public int MatchedUserId { get; set; }
    }

    public class BasicResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }

    public class MatchRow
    {
        public int MatchId { get; set; }
        public int OtherUserId { get; set; }

        public string FullName { get; set; }
        public string Phone { get; set; }

        public string Nickname { get; set; }
    }
}
