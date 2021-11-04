using AnimeDrive.Models;
using System;
using System.Diagnostics;
using System.Threading;

namespace AnimeDrive
{
  public class AnimeDrive
  {
    static void Main(string[] args)
    {
      AnimeFileDatabase db = new AnimeFileDatabase();
      ProgramSettings ps = new ProgramSettings();

      /*
            DownloadFilesActivity downloadActivity = new DownloadFilesActivity(db, ps);
            DiscoverFilesActivity discoverActivity = new DiscoverFilesActivity(db, ps);
            EncodeFilesActivity encodeActivity = new EncodeFilesActivity(db, ps);
            UploadFilesActivity uploadActivity = new UploadFilesActivity(db, ps);
      */
      /*
            Action runCycle = () =>
            {
                //downloadActivity.Execute();

                discoverActivity.Execute();

                var discovered = db.GetDiscoveredFiles();

                encodeActivity.Execute(discovered);

                var encoded = db.GetEncodedFiles();

                uploadActivity.Execute(encoded);
            };
      */

      Stopwatch timer = new Stopwatch();

      // cycle time from 20 minutes to milliseconds
      var cycleMilliseconds = ps.settings.SyncTime * 60 * 1000;

      while (true)
      {
        timer.Reset();
        timer.Start();

        ps = new ProgramSettings();

        DiscoverFilesActivity discoverActivity = new DiscoverFilesActivity(db, ps);
        EncodeFilesActivity encodeActivity = new EncodeFilesActivity(db, ps);
        UploadFilesActivity uploadActivity = new UploadFilesActivity(db, ps);
        DownloadRssTorrentActivity rssDownloadActivity = new DownloadRssTorrentActivity(db, ps);

        //runCycle();
        //-----------

        //downloadActivity.Execute();

        rssDownloadActivity.Execute();

        discoverActivity.Execute();

        var discovered = db.GetDiscoveredFiles();

        encodeActivity.Execute(discovered);

        var encoded = db.GetEncodedFiles();

        uploadActivity.Execute(encoded);

        //--------

        timer.Stop();

        var elapsed = timer.ElapsedMilliseconds;

        // wait if there was no enough work
        if (cycleMilliseconds > elapsed)
        {
          Thread.Sleep((int)(cycleMilliseconds - elapsed));
        }
      }

    }
  }
}
