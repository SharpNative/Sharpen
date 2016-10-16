using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    class NTP
    {
        private const byte NTP_VERSION = 0x04;

        private const byte MODE_CLIENT = 0x03;
        private const byte MODE_SERVER = 0x04;

        private static ushort sourcePort;


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        unsafe struct NTPHeader
        {
            public byte Mode;

            public byte Stratum;
            public byte Poll;
            public sbyte Percision;

            public uint RootDelay;
            public uint RootDispersion;
            public uint RefID;

            public ulong RefTimestamp;
            public ulong OrgTimestamp;
            public ulong RecTimestamp;
            public ulong TransmitTimestamp;
        }


        public static unsafe void Send(byte[] ip)
        {
            // Are we already doing a request?
            if (sourcePort != 0)
                return;

            sourcePort = UDP.RequestPort();

            NetPacketDesc *packet = NetPacket.Alloc();
            
            byte* buf = (byte*)(packet->buffer + packet->end);

            NTPHeader* header = (NTPHeader*)buf;
            header->Mode = (NTP_VERSION << 3) | MODE_CLIENT;
            header->Stratum = 0;
            header->Poll = 4;
            header->Percision = -6;
            header->RootDelay = ByteUtil.ReverseBytes((uint)(1 << 16));
            header->RootDispersion = ByteUtil.ReverseBytes((uint)(1 << 16));
            header->RefID = 0;

            header->RefTimestamp = 0;
            header->OrgTimestamp = 0;
            header->RecTimestamp = 0;
            header->TransmitTimestamp = 0;
            
            packet->end += (short)sizeof(NTPHeader);

            UDP.Send(packet, ip, sourcePort, 123);

            NetPacket.Free(packet);

            // And we wait :)
            UDP.Bind(sourcePort, PacketHandler);
        }

        private static unsafe void PacketHandler(byte[] ip, ushort port, byte* buffer, uint size)
        {
            NTPHeader* header = (NTPHeader*)buffer;

            ulong seconds = header->TransmitTimestamp;

            Console.Write("[NTP] Seconds since 1970 ");
            Console.WriteHex((int)seconds);
            Console.WriteLine("");

            sourcePort = 0;

            UDP.UnBind(sourcePort);
        }
    }
}
