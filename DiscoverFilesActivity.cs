using AnimeDrive.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive
{
    public class DiscoverFilesActivity
    {
        public const char delim = ';';

        public AnimeFileDatabase db;
        public ProgramSettings ps;

        public DiscoverFilesActivity(AnimeFileDatabase db, ProgramSettings ps)
        {
            this.db = db;
            this.ps = ps;
        }

        public bool Execute()
        {
            var patterns = ps.settings.Patterns;
            var dbFiles = db.files;

            var files = Directory.GetFiles(ps.settings.FolderInput);
            var filesAdded = false;

            foreach(var f in files)
            {
                var fLower = f.ToLower();
                FileInfo fileInfo = new FileInfo(f);

                foreach (var p in patterns)
                {
                    if (p.Match(fileInfo.Name))
                    {
                        if (!dbFiles.ContainsKey(fileInfo.Name)) {
                            dbFiles.TryAdd(fileInfo.Name, new AnimeFile(fileInfo.FullName, FileStatus.DISCOVER));
                            filesAdded = true;
                        }
                            
                        if (dbFiles[fileInfo.Name].fileStatus == FileStatus.DOWNLOAD)
                        {
                            dbFiles[fileInfo.Name].fileStatus = FileStatus.DISCOVER;
                            filesAdded = true;
                        }
                    }
                }
            }

            if (filesAdded)
            {
                db.FlushData();
            }

            return true;
        }
    }
}
