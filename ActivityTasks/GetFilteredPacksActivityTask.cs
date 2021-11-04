using AnimeDrive.Irc;
using AnimeDrive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeDrive.ActivityTasks
{
    public class GetFilteredPacksActivityTask : ActivityBase
    {
        private HttpClient client;

        private GetPacklistActivityTask gpat;

        const string newBotName = "CR-ARUTHA|NEW";
        const string archiveBotName = "ARUTHA-BATCH|720p";

        const string filterRes = "720p";

        public GetFilteredPacksActivityTask(AnimeFileDatabase db, ProgramSettings ps) : base(db, ps)
        {
            client = new HttpClient();
            gpat = new GetPacklistActivityTask(client);
        }

        public List<HorribleSubPack> Execute()
        {
            var terms = ps.settings.Patterns;

            var newPacks = gpat.FetchBotPackListAsync(newBotName, CancellationToken.None).Result;
            var oldPacks = gpat.FetchBotPackListAsync(archiveBotName, CancellationToken.None).Result;

            newPacks = newPacks.Concat(oldPacks);

            //newPacks = newPacks.Where(pack => { return pack.Filename.Contains("Yesterday"); });

            var filteredPacks = newPacks.Where((pack) =>
            {
                var lowerFile = pack.Filename.ToLower();

                foreach (var term in terms)
                {
                    if (term.Match(pack) &&
                        !db.files.ContainsKey(pack.Filename))
                    {
                        return true;
                    }
                }

                return false;
            });

            var dedupPacks = filteredPacks.Distinct(new PackComparer());

            return dedupPacks.ToList();
        }
    }
}
