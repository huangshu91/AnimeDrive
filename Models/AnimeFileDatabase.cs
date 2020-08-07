using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive.Models
{
    public class AnimeFileDatabase
    {
        public ConcurrentDictionary<string, AnimeFile> files = new ConcurrentDictionary<string, AnimeFile>();

        private const string DataPath = @".\SaveData\data.json";

        public AnimeFileDatabase()
        {
            string dataString = File.ReadAllText(DataPath);

            files = JsonConvert.DeserializeObject<ConcurrentDictionary<string, AnimeFile>>(dataString);
        }

        public List<AnimeFile> GetDiscoveredFiles()
        {
            var toEncode = files.Values.Where((file) =>
            {
                return file.fileStatus == FileStatus.DISCOVER;
            });

            return toEncode.ToList();
        }

        public List<AnimeFile> GetEncodedFiles()
        {
            var toUpload = files.Values.Where((file) =>
            {
                return file.fileStatus == FileStatus.ENCODE;
            });

            return toUpload.ToList();
        }

        public bool FlushData()
        {
            try
            {
                string saveData = JsonConvert.SerializeObject(files, Formatting.Indented);

                File.WriteAllText(DataPath, saveData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }
    }
}
