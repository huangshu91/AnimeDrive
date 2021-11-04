using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive.Models.Parsers
{
    public interface IFilenameParser
    {
        double ParseEpisode(string fName);
        bool ParseGroup(string fName);
    }
}
