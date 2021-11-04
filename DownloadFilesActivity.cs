using AnimeDrive.ActivityTasks;
using AnimeDrive.Irc;
using AnimeDrive.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeDrive
{
    public class DownloadFilesActivity
    {
        private AnimeFileDatabase db;
        private ProgramSettings ps;

        GetFilteredPacksActivityTask gfpat;
        DccDownloadActivityTask ddat;

        private IrcClient ircClient;
        private DCCClient dccClient;

        private const string rizon = @"irc.rizon.net";
        private const string hsChannel = @"#horriblesubs";
        private const string userName = @"maievBot";

        public DownloadFilesActivity(AnimeFileDatabase db, ProgramSettings ps)
        {
            this.db = db;
            this.ps = ps;

            ddat = new DccDownloadActivityTask(db, ps);
            gfpat = new GetFilteredPacksActivityTask(db, ps);

            dccClient = new DCCClient();

            dccClient.OnDccDebugMessage += dccDebug;

            ircClient = new IrcClient();
            ircClient.SetConnectionInformation(
                rizon, "maievBot", hsChannel, dccClient, ps.settings.FolderInput);

            //ircClient.OnMessageReceived += IrcClient_OnMessageReceived;
            //ircClient.OnRawMessageReceived += IrcClient_OnRawMessageReceived;

            var res = ircClient.Connect();
        }

        private void IrcClient_OnRawMessageReceived(object sender, IrcRawReceivedEventArgs e)
        {
            Console.WriteLine(sender.ToString());
            Console.WriteLine(e.Message);
        }

        private void IrcClient_OnMessageReceived(object sender, IrcReceivedEventArgs e)
        {
            Console.WriteLine(sender.ToString());
            Console.WriteLine(e.User + "::" + e.Message);
        }

        public void dccDebug(object Sender, DCCDebugMessageArgs args)
        {
            Console.WriteLine(args.Message);
        }

        public void Execute()
        {
            Console.WriteLine("Downloading Files...");

            var packs = gfpat.Execute();
            var limitPacks = packs.Take(5);

            foreach (var pack in limitPacks)
            {
                ircClient.SendMessageToAll(pack.ToString());

                Thread.Sleep(10000);

                var down = ircClient.CheckIfDownloading();

                while(ircClient.CheckIfDownloading())
                {
                    Thread.Sleep(5000);
                }

                // Check filesize is correct before marking as done
                FileInfo fi = new FileInfo(Path.Combine(ps.settings.FolderInput, pack.Filename));
                double packSize = 0;
                Double.TryParse(pack.Size, out packSize);

                // make sure the file exists and only differs by 1 mb (rounding)
                if (fi.Exists && Math.Abs(packSize - fi.Length/(1024*1024)) < 2)
                {
                    db.files.TryAdd(fi.Name, new AnimeFile(fi.FullName));
                    db.FlushData();
                }
                else
                {
                    Console.WriteLine("Could not download: " + pack.Filename);
                }
            }

            ircClient.StopClient();
        }
    }
}
