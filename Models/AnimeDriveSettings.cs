using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive.Models
{
  public class AnimeDriveSettings
  {
    public string HandbrakeCLIPath { get; set; }

    public string QBittorrentCLIPath { get; set; }

    public string QBittorrentURL { get; set; }

    public string FolderInput { get; set; }

    public string FolderOutput { get; set; }

    public PatternFilter[] Patterns { get; set; }

    public string Extensions { get; set; }

    public string[] AllowedExt
    {
      get
      {
        return Extensions.Split(',');
      }
    }

    public int SyncTime { get; set; }

    public int FileExpiry { get; set; }
  }
}
