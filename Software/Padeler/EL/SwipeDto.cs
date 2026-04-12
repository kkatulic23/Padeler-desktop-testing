using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class SwipeDto // Filip Grgac
    {
        [JsonProperty("from_user_id")]
        public int FromUserId { get; set; }

        [JsonProperty("to_user_id")]
        public int ToUserId { get; set; }

        [JsonProperty("response")]
        public string Response { get; set; }
    }

    public class SwipeResponse // Filip Grgac
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("stauts")]
        public string Status { get; set; }

        [JsonProperty("matched")]
        public bool Matched { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
