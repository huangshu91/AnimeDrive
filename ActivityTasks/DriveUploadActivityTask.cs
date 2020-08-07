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

using GFile = Google.Apis.Drive.v3.Data.File;

namespace AnimeDrive
{
    public class DriveUploadActivityTask
    {
        static string[] Scopes = { DriveService.Scope.Drive };

        static string ApplicationName = @"AnimeSyncDrive";
        static int ChunkSize = 40;

        private long fileSize;
        private AnimeFileDatabase db;

        public DriveUploadActivityTask(AnimeFileDatabase db)
        {
            this.db = db;
        }

        public bool Execute(AnimeFile file)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            /*
            var req = service.Files.List();
            req.PageSize = 30;
            req.Fields = "nextPageToken, files(id, name, parents)";

            var resf = req.Execute().Files;
            */

            FileInfo fi = new FileInfo(file.encodePath);

            fileSize = fi.Length;

            var uploadStream = new FileStream(file.encodePath,
                                                FileMode.Open,
                                                FileAccess.Read);

            var insertRequest = service.Files.Create(
                    new GFile
                    {
                        Name = fi.Name,
                        Parents = new List<string>
                        {
                            "root"
                        }
                    },
                    uploadStream,
                    "application/octet-stream"
                );

            insertRequest.ChunkSize = ChunkSize * 1024 * 1024;
            insertRequest.ProgressChanged += Upload_ProgressChanged;
            insertRequest.ResponseReceived += Upload_ResponseReceived;

            Console.WriteLine("Uploading: " + fi.Name);

            var createFileTask = insertRequest.UploadAsync();
            createFileTask.ContinueWith(t =>
            {
                uploadStream.Dispose();
            }).Wait();

            if (createFileTask.Result.Status != UploadStatus.Failed)
            {
                file.fileStatus = FileStatus.UPLOAD;
                db.FlushData();
            }

            return true;
        }

        private void Upload_ProgressChanged(IUploadProgress progress)
        {
            Console.WriteLine(progress.Status + string.Format(" : Sent {0}/{1} bytes - {2}% completed.", progress.BytesSent, fileSize, progress.BytesSent / (float) fileSize * 100));

            if (progress.Status == UploadStatus.Failed)
            {
                Console.WriteLine(progress.Exception.Message);
                Console.WriteLine(progress.Exception.StackTrace);
            }
        }

        private void Upload_ResponseReceived(GFile file)
        {
            Console.WriteLine(file.Name + " was uploaded successfully.");
        }
    }
}
