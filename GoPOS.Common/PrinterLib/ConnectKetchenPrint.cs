using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Common.PrinterLib
{
    internal class ConnectKetchenPrint
    {

        [StructLayout(LayoutKind.Explicit)]
        public struct IN_ADDR
        {
            [FieldOffset(0)]
            public S_un_union S_un;

            [FieldOffset(0)]
            public uint S_addr; // Equivalent to s_addr

            public byte s_host
            {
                get { return S_un.S_un_b.s_b2; }
                set { S_un.S_un_b.s_b2 = value; }
            }

            public byte s_net
            {
                get { return S_un.S_un_b.s_b1; }
                set { S_un.S_un_b.s_b1 = value; }
            }

            public ushort s_imp
            {
                get { return S_un.S_un_w.s_w2; }
                set { S_un.S_un_w.s_w2 = value; }
            }

            public byte s_impno
            {
                get { return S_un.S_un_b.s_b4; }
                set { S_un.S_un_b.s_b4 = value; }
            }

            public byte s_lh
            {
                get { return S_un.S_un_b.s_b3; }
                set { S_un.S_un_b.s_b3 = value; }
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct S_un_union
        {
            [FieldOffset(0)]
            public S_un_b_union S_un_b;

            [FieldOffset(0)]
            public S_un_w_union S_un_w;

            [FieldOffset(0)]
            public uint S_addr;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct S_un_b_union
        {
            public byte s_b1;
            public byte s_b2;
            public byte s_b3;
            public byte s_b4;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct S_un_w_union
        {
            public ushort s_w1;
            public ushort s_w2;
        }
    }
}
