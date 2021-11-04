using AnimeDrive.Models;
using CodeHollow.FeedReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive.ActivityTasks
{
  public class ReadRssActivityTask : ActivityBase
  {
    private string feedUrl = @"https://subsplease.org/rss/?r=720";

    public ReadRssActivityTask(AnimeFileDatabase db, ProgramSettings ps) : base(db, ps)
    {
    }

    public List<SubsPleaseRss> Execute()
    {
      var terms = ps.settings.Patterns;
      List<SubsPleaseRss> res = new List<SubsPleaseRss>();

      var feed = FeedReader.ReadAsync(feedUrl).Result;

      foreach(var item in feed.Items)
      {
        var title = item.Title;

        foreach(var term in terms)
        {
          if (term.Match(title) && !db.files.ContainsKey(title))
          {
            var file = new SubsPleaseRss(item.Title, item.Link);
            res.Add(file);
          }
        }
      }

      return res;
    }
  }
}
