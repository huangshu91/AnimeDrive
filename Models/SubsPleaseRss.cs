using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive.Models
{
  public class SubsPleaseRss
  {
    public string title { get; set; }
    public string magnet { get; set; }

    public SubsPleaseRss()
    {

    }

    public SubsPleaseRss(string t, string m)
    {
      title = t;
      magnet = m;
    }
  }
}
