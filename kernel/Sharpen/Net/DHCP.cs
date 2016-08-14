using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    class DHCP
    {

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct DHCPHeader
        {
            public byte Opcode;
            public byte Htype;
            public byte Hlen;
            public byte HopCount;
            public uint Xid;
            public ushort SecCount;
            public ushort Flags;
            public uint ClientIp;
            public uint YourIp;
            public uint ServerIP;
            public uint ClientEth;
            fixed byte Reserved[10];
            public fixed char ServerName[64];
            public fixed char bootFileName[128];
        }

        private static DHCPHeader makeHeader(uint xid, uint clientAddr, byte type)
        {
            return new DHCPHeader();
        }

        public static void Sample()
        {

        }
    }
}
