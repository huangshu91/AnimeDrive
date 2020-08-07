using AnimeDrive.Irc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive.Models
{
    public class PatternFilter
    {
        public string p;

        [DefaultValue(-1)]
        public double f;

        [DefaultValue("720p")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string r;

        public bool Match(HorribleSubPack pack)
        {
            return Match(pack.Filename);
        }

        public bool Match(string fileName)
        {
            var lowerName = fileName.ToLower();
            var lowerNameSplit = lowerName.Split(' ');

            var pSplit = p.Split(' ');

            foreach (var pat in pSplit)
            {
                if (!lowerNameSplit.Contains(pat))
                {
                    return false;
                }
            }

            return lowerName.Contains(r) &&
                   HorribleSubParser.ParseEpisode(fileName) > f;
        }
    }
}
