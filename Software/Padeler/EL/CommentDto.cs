using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace EL
{
    public sealed class AddCommentRequest // Kristian Katulić
    {
        [JsonProperty("commented_id")]
        public int CommentedId { get; set; }

        [JsonProperty("commenter_id")]
        public int CommenterId { get; set; }

        [JsonProperty("grade")]
        public double Grade { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; } = string.Empty;
    }

    public sealed class AddCommentResponse // Kristian Katulić
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("comment_id")]
        public int CommentId { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }

    public sealed class RatedIdsResponse // Kristian Katulić
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("rated_ids")]
        public List<int> RatedIds { get; set; } = new List<int>();

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
