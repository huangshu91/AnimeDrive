using AnimeDrive.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeDrive
{
    public class EncodeActivityTask
    {
        public const string arguments = @"-i ""{0}"" -o ""{1}"" -e ""x264"" -f ""av_mp4"" --first-audio --aencoder ""copy"" --subtitle-lang-list ""eng"" --first-subtitle --ssa-lang ""eng"" --ssa-burn --subtitle-burned";

        public bool Execute(AnimeDriveSettings settings, string inputFile, string outputFile)
        {

            try
            {
                var formatArgs = string.Format(arguments, inputFile, outputFile);

                Process process = new Process();
                process.StartInfo.FileName = settings.HandbrakeCLIPath;
                process.StartInfo.Arguments = formatArgs;
                process.StartInfo.UseShellExecute = true;
                //process.StartInfo.RedirectStandardOutput = true;
                //process.StartInfo.RedirectStandardError = true;
                //process.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
                //process.ErrorDataReceived += (s, e) => Console.WriteLine(e.Data);

                process.Start();

                //process.BeginOutputReadLine();
                //process.BeginErrorReadLine();

                process.WaitForExit();

            }
            catch (Win32Exception ex)
            {
                Console.WriteLine(ex);
            }

            //Console.WriteLine("Handbrake result: ");

            return true;
        }
    }
}
