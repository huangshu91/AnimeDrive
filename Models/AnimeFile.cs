using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive.Models
{
    public enum FileStatus
    {
        DOWNLOAD,
        DISCOVER,
        ENCODE,
        UPLOAD,
        CLEAN
    }

    public class AnimeFile
    {
        public string filePath { get; set; }

        public string encodePath { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FileStatus fileStatus { get; set; }

        public string driveFileId{ get; set; }

        public AnimeFile(string path, FileStatus status = FileStatus.DOWNLOAD)
        {
            filePath = path;
            fileStatus = status;
        }
    }
}
