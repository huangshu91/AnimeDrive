using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive.Models.Parsers
{
    public class HorribleSubParser : IFilenameParser
    {
        const string _group = "horriblesubs";

        public double ParseEpisode(string fName)
        {
            var lastDash = fName.LastIndexOf('-') + 1;
            var lastRes = fName.LastIndexOf('[');
            var numString = fName.Substring(lastDash, lastRes - lastDash);
            numString = numString.Trim();

            double res = 0;
            Double.TryParse(numString, out res);

            return res;
        }

        public bool ParseGroup(string fName)
        {
            return fName.Contains(_group);
        }
    }
}
