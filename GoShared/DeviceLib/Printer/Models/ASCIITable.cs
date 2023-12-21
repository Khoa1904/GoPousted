using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoShared.DeviceLib.Printer.Models
{
    public class ASCIITable
    {
        public const byte DLE = 0x10;
        public const byte EOT = 0x04;  // end of transmission. ETX
        public const byte HT = 09; // horizonal tab 

        public const byte ESC = 0x1B;   // ESCAPE
        public const byte FS = 0x1C;
        public const byte GS = 0x1D;

        public const byte LF = 0x0A;   // line-feed
        public const byte CR = 0x0D;   // carriage-return

        public const byte FF = 0x0C;
        public const byte CAN = 0x18;

    }
}
