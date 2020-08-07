using AnimeDrive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive.Irc
{
    class PackComparer : IEqualityComparer<HorribleSubPack>
    {
        public bool Equals(HorribleSubPack p1, HorribleSubPack p2)
        {
            if (p1.Filename.Equals(p2.Filename))
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(HorribleSubPack pack)
        {
            return pack.Filename.GetHashCode();
        }
    }
}
