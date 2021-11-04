using AnimeDrive.Irc;
using AnimeDrive.Models.Parsers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive.Models
{
    public class PatternFilter
    {
        public string p;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public double f;

        [DefaultValue("720p")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string r;

        [DefaultValue(FilenameParserEnum.subsplease)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public FilenameParserEnum ps;

        private IFilenameParser parser;

        [OnDeserialized]
        internal void DeserializeInitialize(StreamingContext context)
        {
            switch (ps)
            {
                case FilenameParserEnum.subsplease:
                    parser = new SubsPleaseParser();
                    break;
                case FilenameParserEnum.erai:
                    parser = new EraiRawParser();
                    break;
                case FilenameParserEnum.hs:
                    parser = new HorribleSubParser();
                    break;
            }
        }

        public bool Match(HorribleSubPack pack)
        {
            return Match(pack.Filename);
        }

        public bool Match(string fileName)
        {
            var lowerName = fileName.ToLower();

            if (!parser.ParseGroup(lowerName))
            {
                return false;
            }

            var lowerNameSplit = lowerName.Split(' ');

            var pSplit = p.Split(' ');

            foreach (var pat in pSplit)
            {
                var patlow = pat.ToLower();
                if (!lowerNameSplit.Contains(patlow))
                {
                    return false;
                }
            }

            if (!lowerName.Contains(r))
            {
                return false;
            }

            var parseEp = parser.ParseEpisode(fileName);

            return parseEp > f;
        }
    }
}
