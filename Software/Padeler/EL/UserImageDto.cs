using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class UserImageDto // Filip Grgac
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("image_base64")]
        public string ImageBase64 { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
