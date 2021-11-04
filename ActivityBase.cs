using AnimeDrive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive
{
  public class ActivityBase
  {
    public AnimeFileDatabase db;
    public ProgramSettings ps;

    public ActivityBase(AnimeFileDatabase db, ProgramSettings ps)
    {
      this.db = db;
      this.ps = ps;
    }
  }
}
