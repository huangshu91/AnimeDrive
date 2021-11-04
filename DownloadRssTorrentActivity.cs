using AnimeDrive.ActivityTasks;
using AnimeDrive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive
{
  public class DownloadRssTorrentActivity : ActivityBase
  {
    private ReadRssActivityTask rssTask;
    private LoadMagnetQBActivityTask qbtask;

    public DownloadRssTorrentActivity(AnimeFileDatabase db, ProgramSettings ps) : base(db, ps)
    {
      rssTask = new ReadRssActivityTask(db, ps);
      qbtask = new LoadMagnetQBActivityTask(db, ps);
    }

    public void Execute()
    {
      Console.WriteLine("Checking RSS Feed...");

      var rsslinks = rssTask.Execute();

      foreach(var link in rsslinks)
      {
        qbtask.Execute(link);
      }
    }
  }
}
