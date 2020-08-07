using AnimeDrive.Irc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive.Models
{
    public class HorribleSubPack
    {
        [JsonProperty("b")]
        public string Bot { get; set; }

        [JsonProperty("n")]
        public string Number { get; set; }

        [JsonProperty("s")]
        public string Size { get; set; }

        [JsonProperty("f")]
        public string Filename { get; set; }

        [JsonIgnore]
        public double EpisodeNum
        {
            get
            {
                return HorribleSubParser.ParseEpisode(Filename);
            }
        }

        public override string ToString()
        {
            return $"/msg {Bot} xdcc send #{Number}";
        }
    }
}
