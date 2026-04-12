using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class UserCardDto // Filip Grgac
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string FrequencyOfPlaying { get; set; }
        public string Level { get; set; }
        public string Position { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double DistanceKm { get; set; }
        public UserImageDto Image { get; set; }

        public double? Rating { get; set; }
    }
}
