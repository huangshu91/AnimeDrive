using AnimeDrive.Irc;
using AnimeDrive.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeDrive.ActivityTasks
{

    public class GetPacklistActivityTask : IDisposable
    {
        private const string BASE_URL = "https://xdcc.horriblesubs.info/";
        private const string SEARCH_IN_ALL_PACKLISTS_URL = "https://xdcc.horriblesubs.info/search.php?t={0}";
        private const string SEARCH_IN_BOT_PACKLIST_URL = "https://xdcc.horriblesubs.info/search.php?t={0}&nick={1}";
        private const string BOT_PACKS_URL = "https://xdcc.horriblesubs.info/search.php?nick={0}";

        private readonly HttpClient _http;
        private readonly HorribleSubIrcParser _parser;

        public GetPacklistActivityTask(HttpClient httpClient)
        {
            _http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _parser = new HorribleSubIrcParser();
        }

        /// <summary>
        /// Searches for packs which match the term on all bots.
        /// </summary>
        /// <param name="term">The search term.</param>
        /// <param name="token">The cancellation token which can be used to cancel the operation.</param>
        /// <returns>A list which contains all matching packs.</returns>
        public async Task<IEnumerable<HorribleSubPack>> FindPacksAsync(
            string term,
            CancellationToken token)
        {
            var uri = string.Format(SEARCH_IN_ALL_PACKLISTS_URL, term);
            var stream = await _http.GetStreamAsync(uri);

            return await _parser.ParsePacklistAsync(stream, token);
        }

        /// <summary>
        /// Searches for packs which match the term on a specific bot.
        /// </summary>
        /// <param name="term">The search term.</param>
        /// <param name="bot">The bot.</param>
        /// <param name="token">The cancellation token which can be used to cancel the operation.</param>
        /// <returns>A list which contains all matching packs.</returns>
        public async Task<IEnumerable<HorribleSubPack>> FindBotPacksAsync(
            string term,
            string bot,
            CancellationToken token)
        {
            var packList = await FetchBotPackListAsync(bot, token);

            var uri = string.Format(SEARCH_IN_BOT_PACKLIST_URL, term, bot);
            var stream = await _http.GetStreamAsync(uri);

            return await _parser.ParsePacklistAsync(stream, token);
        }

        /// <summary>
        /// Fetches the pack list of all bots.
        /// </summary>
        /// <param name="token">The cancellation token which can be used to cancel the operation.</param>
        /// <returns>A list which contains all packs.</returns>
        public async Task<IEnumerable<HorribleSubPack>> FetchPackListsAsync(
            CancellationToken token)
        {
            var packList = new List<HorribleSubPack>();
            var botList = (await FetchBotListAsync(token)).ToList();

            var tasks = botList.Select(async bot =>
            {
                var packs = await FetchBotPackListAsync(bot, token);
                packList.AddRange(packs);
            });

            await Task.WhenAll(tasks);

            return packList;
        }

        /// <summary>
        /// Fetches the pack list of a specific bot.
        /// </summary>
        /// <param name="bot">The bot.</param>
        /// <param name="token">The cancellation token which can be used to cancel the operation.</param>
        /// <returns>A list which contains all packs of the bot.</returns>
        public async Task<IEnumerable<HorribleSubPack>> FetchBotPackListAsync(
            string bot,
            CancellationToken token)
        {
            var uri = string.Format(BOT_PACKS_URL, bot);
            var stream = await _http.GetStreamAsync(uri);

            return await _parser.ParsePacklistAsync(stream, token);
        }

        /// <summary>
        /// Fetches all available bot names.
        /// </summary>
        /// <param name="token">The cancellation token which can be used to cancel the operation.</param>
        /// <returns>A list which contains all bot names.</returns>
        public async Task<IEnumerable<string>> FetchBotListAsync(
            CancellationToken token)
        {
            var stream = await _http.GetStreamAsync(BASE_URL);
            return await _parser.ParseBotsAsync(stream, token);
        }

        public void Dispose()
        {
            _http?.Dispose();
        }
    }
}
