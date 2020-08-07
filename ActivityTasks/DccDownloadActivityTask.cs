using AnimeDrive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive.ActivityTasks
{
    public class DccDownloadActivityTask
    {
        public AnimeFileDatabase db;
        public ProgramSettings ps;

        public DccDownloadActivityTask(AnimeFileDatabase db, ProgramSettings ps)
        {
            this.db = db;
            this.ps = ps;
        }

        public void Execute()
        {

        }
    }
}
