using AnimeDrive.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive
{
    public class EncodeFilesActivity
    {
        private AnimeFileDatabase db;
        private ProgramSettings ps;

        public EncodeFilesActivity(AnimeFileDatabase db, ProgramSettings ps)
        {
            this.db = db;
            this.ps = ps;
        }

        public bool Execute(List<AnimeFile> encodeFiles)
        {
            Console.WriteLine(string.Format("Encoding {0} Files...", encodeFiles.Count));

            foreach (var f in encodeFiles)
            {
                EncodeActivityTask task = new EncodeActivityTask();

                FileInfo fi = new FileInfo(f.filePath);

                string outFile = Path.Combine(ps.settings.FolderOutput, fi.Name);

                Console.WriteLine("Encoding - " + fi.Name);

                task.Execute(ps.settings, f.filePath, outFile);

                f.fileStatus = FileStatus.ENCODE;
                f.encodePath = outFile;

                db.FlushData();
            }

            return true;
        }
    }
}
