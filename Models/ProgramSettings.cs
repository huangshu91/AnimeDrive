using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive.Models
{
    public class ProgramSettings
    {
        public AnimeDriveSettings settings;

        static ProgramSettings()
        {
        }

        public ProgramSettings()
        {
            string settingsString = File.ReadAllText(@".\Settings.json");

            settings = JsonConvert.DeserializeObject<AnimeDriveSettings>(settingsString);
        }
    }
}
