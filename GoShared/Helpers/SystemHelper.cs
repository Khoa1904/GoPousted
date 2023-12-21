using CoreWCF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GoShared.Helpers
{
    public class SystemHelper
    {
        //**----------------------------------------------------------------------------------

        public static Size CalendarSize = new Size(450, 480);
        public static Size CalendarMinSize = new Size(450, 480);
        public static Size CalendarMaxSize = new Size(450, 480);

        //**----------------------------------------------------------------------------------
        public const string ReqSignPadCode = "PN";
        //**----------------------------------------------------------------------------------
        public const double MainHeight = 768;
        public const double MainWidth = 1024;

        //**----------------------------------------------------------------------------------
        public const int SerialPortBitrateDefault = 9600;

        public static void ProgramRestart(bool shutdownOnly = false)
        {
            if (!shutdownOnly)
            {
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            }

            // clear environment
            // Environment.Exit(0);
        }
    }
}
