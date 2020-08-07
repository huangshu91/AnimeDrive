using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AnimeDrive.Util
{
    public static class Util
    {
        public static void TimedRun(int milliseconds, Action func)
        {
            Timer runTimer = new Timer();
            runTimer.Start();


            Console.WriteLine("running cycle...");
            func();
            Console.WriteLine("cycle finished.");


            runTimer.Stop();

        }
    }
}
