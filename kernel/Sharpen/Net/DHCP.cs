using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{
    enum ProtocolTypes
    {
        IPV4 = 0x0800
    }

    class DHCP
    {
        private static byte[] m_mac;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct PacketHeader
        {
            public fixed byte Destination[6];
            public fixed byte Source[6];
            public UInt16 Protocol;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct IPV4Header
        {
            public byte Version;
            public byte ServicesField;
            public UInt16 totalLength;
            public UInt16 ID;
            public byte Flags;
            public UInt16 FragmentOffset;
            public byte TTL;
            public byte Protocol;
            public UInt16 HeaderChecksum;
            public fixed byte Source[4];
            public fixed byte Destination[4];
            public fixed byte SourceGeoIP[4];
            public fixed byte DestinationGeoIP[4];
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct UserDiagramProtocol
        {
            public UInt16 SourcePort;
            public UInt16 DestinationPort;
            public UInt16 Length;
            public UInt16 Checksum;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct DHCPBootstrapHeader
        {
            public byte MessageType;

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
            public fixed byte Server[63];
            public fixed byte File[127];
            public fixed byte DHCPMessageType[3];

        }
        


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct DHCPDiscoverHeader
        {
            public PacketHeader packet;
            public IPV4Header IPV4;
            public UserDiagramProtocol Protocol;
            public DHCPBootstrapHeader Bootstrap;
        }

        private static unsafe PacketHeader CreateHeader(ProtocolTypes type, byte[] destination)
        {
            PacketHeader header = new PacketHeader();

            for (int i = 0; i < 6; i++)
                header.Destination[i] = destination[i];

            for (int i = 0; i < 6; i++)
                header.Source[i] = m_mac[i];

            header.Protocol = (UInt16)type;

            return header;
        }
            
        private static unsafe IPV4Header CreateIpv4Header()
        {
            IPV4Header header = new IPV4Header();

            header.Version = 0x45;
            header.ServicesField = 0;
            header.totalLength = 0x12C;
            header.ID = 0xA836;
            header.Flags = 0;
            header.FragmentOffset = 0;
            header.TTL = 250;
            header.Protocol = 0x11;
            header.HeaderChecksum = 0x178B;


            return header;
        }


        public static unsafe void Sample()
        {
            m_mac = new byte[6];

            Network.GetMac((byte *)Util.ObjectToVoidPtr(m_mac));

            byte[] broadCast = new byte[6];
            for (int i = 0; i < 6; i++)
                broadCast[i] = 0xFF;

            PacketHeader header = CreateHeader(ProtocolTypes.IPV4, broadCast);
            IPV4Header headerr = CreateIpv4Header();

            DHCPDiscoverHeader* headerrr = (DHCPDiscoverHeader*)Heap.Alloc(sizeof(DHCPDiscoverHeader));
            headerrr->packet = header;
            headerrr->IPV4 = headerr;
            headerrr->Protocol.SourcePort = 68;
            headerrr->Protocol.DestinationPort = 67;
            headerrr->Protocol.Length = 280;
            headerrr->Protocol.Checksum = 0x591F;

            headerrr->Bootstrap.MessageType = 0x01;

            Network.Transmit((byte *)headerrr, (uint)sizeof(DHCPDiscoverHeader));
        }
    }
}
