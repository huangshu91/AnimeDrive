using AnimeDrive.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace AnimeDrive
{
    public class UploadFilesActivity
    {
        private AnimeFileDatabase db;
        private ProgramSettings ps;

        public UploadFilesActivity(AnimeFileDatabase db, ProgramSettings ps)
        {
            this.db = db;
            this.ps = ps;
        }

        public bool Execute(List<AnimeFile> files)
        {
            
            foreach (var f in files)
            {
                DriveUploadActivityTask driveupload = new DriveUploadActivityTask(db);

                driveupload.Execute(f);
            }

            return true;
        }
    }
}
