using Sharpen.Mem;
using Sharpen.Utilities;
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
        private static byte[] m_mac;

        private const byte OPT_REQUEST = 1;
        private const byte OPT_REPLY = 2;

        private const byte HARDTYPE_ETH = 1;

        private const byte OPT_DHCP_MESSAGE_TYPE = 53;
        private const byte OPT_PARAMETER_REQUEST = 55;
        private const byte OPT_PADDING = 0;
        private const byte OPT_SUBNET = 1;
        private const byte OPT_ROUTER = 3;
        private const byte OPT_DNS = 6;
        private const byte OPT_END = 255;

        private const uint MAGISCH_KOEKJE = 0x63825363;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct DHCPBootstrapHeader
        {
            public byte Opcode;

            public byte HardwareType;
            public byte HardwareAddressLength;
            public byte Hops;
            public uint TransationID;
            public UInt16 SecondsElapsed;
            public UInt16 BootpFlags;
            public fixed byte ClientIP[4];
            public fixed byte YourClientIP[4];
            public fixed byte NextServerIP[4];
            public fixed byte RelayServerIP[4];
            public fixed byte ClientMac[6];
            public fixed byte AddressPadding[10];
            public fixed byte Server[64];
            public fixed byte File[128];
        }

        /// <summary>
        /// Add DHCP header in packet
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="xid"></param>
        /// <param name="clientIP"></param>
        /// <param name="messageType"></param>
        private static unsafe void addHeader(NetPacketDesc *packet, uint xid, byte[] clientIP, byte messageType)
        {
            DHCPBootstrapHeader *header = (DHCPBootstrapHeader *)(packet->buffer + packet->start);
            Memory.Memset(header, 0, sizeof(DHCPBootstrapHeader));

            header->Opcode = OPT_REQUEST; // REQUEST
            header->HardwareType = HARDTYPE_ETH; // Ethernet
            header->HardwareAddressLength = 4; // IPV4
            header->Hops = 0;
            header->TransationID = ByteUtil.ReverseBytes(xid);
            header->SecondsElapsed = 0; // NULLLLL
            header->BootpFlags = 0; // NONNNN

            for (int i = 0; i < 4; i++)
                header->ClientIP[i] = clientIP[i];
            
            packet->end += (short)sizeof(DHCPBootstrapHeader);

            // Default options
            byte* opt = (byte*)(packet->buffer + packet->end);

            uint* topt = (uint*)opt;
            *topt = ByteUtil.ReverseBytes(MAGISCH_KOEKJE); // 4 bytes!;
            opt += 4; // Another FOUR!

            // Set message type
            *opt++ = OPT_DHCP_MESSAGE_TYPE; // OPT_DHCP_MESSAGE_TYPE
            *opt++ = 1;
            *opt++ = messageType;

            packet->end += 11;
        }

        /// <summary>
        /// DHCP discover (test version)
        /// </summary>
        public static unsafe void Discover()
        {
            m_mac = new byte[6];

            Network.GetMac((byte *)Util.ObjectToVoidPtr(m_mac));

            byte[] broadCast = new byte[6];
            for (int i = 0; i < 6; i++)
                broadCast[i] = 0xFF;

            byte[] dest = new byte[4];
            for (int i = 0; i < 4; i++)
                dest[i] = 0xFF;

            byte[] src = new byte[4];
            for (int i = 0; i < 4; i++)
                src[i] = 0x00;

            uint xid = 0x6666;

            NetPacketDesc* packet = NetPacket.Alloc();

            addHeader(packet, xid, src, 1); // DHCP discover

            // Specific options
            byte *buf = (byte *)(packet->buffer + packet->end);
            *buf++ = OPT_PARAMETER_REQUEST; // OPT_PARAMETER_REQUESt
            *buf++ = 3; // Length of 3 :)
            *buf++ = OPT_SUBNET; // SUBNET
            *buf++ = OPT_ROUTER; // ROUTER
            *buf++ = OPT_DNS; // DNS
            *buf++ = OPT_END; // And then

            packet->end += 6;

            UDP.Send(packet, broadCast, dest, 68, 67);

            NetPacket.Free(packet);
        }
    }
}
