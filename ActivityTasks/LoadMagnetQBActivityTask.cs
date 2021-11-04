using AnimeDrive.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive.ActivityTasks
{
  public class LoadMagnetQBActivityTask : ActivityBase
  {
    public const string arguments = @"torrent add url -f {0} {1} --url {2}";

    public LoadMagnetQBActivityTask(AnimeFileDatabase db, ProgramSettings ps) : base(db, ps)
    { }

    public void Execute(SubsPleaseRss file)
    {
      try
      {
        var formatArgs = string.Format(arguments, ps.settings.FolderInput, file.magnet, ps.settings.QBittorrentURL);

        var p = new Process();
        p.StartInfo.FileName = ps.settings.QBittorrentCLIPath;
        p.StartInfo.Arguments = formatArgs;

        p.Start();

        p.WaitForExit();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }
    }

  }
}
