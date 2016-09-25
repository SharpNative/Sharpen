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
            public fixed byte Server[64];
            public fixed byte File[128];
            public fixed byte MagicCookie[4];
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct MessageTypeHeader
        {
            public byte Type;
            public byte Length;
            public byte DHCP;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ClientID
        {
            public byte Type;
            public byte Length;
            public byte HardwareType;
            public fixed byte ClientMac[6];
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct RequestedIpAddress
        {
            public byte Type;
            public byte Length;
            public fixed byte ClientMac[4];
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

            int size = sizeof(DHCPBootstrapHeader) + sizeof(MessageTypeHeader) + sizeof(ClientID) + sizeof(RequestedIpAddress) + 14;
            //UDPHeader *headerr = UDP.createHeader(broadCast, dest, 68, 67, (ushort)(size));
            void *ptr = Heap.Alloc(size + sizeof(UDPHeader));
            Memory.Memset(ptr, 0, size);
            //Memory.Memcpy(ptr, headerr, sizeof(UDPHeader));

            // DHCP Header

            byte* offset = (byte*)ptr + sizeof(UDPHeader);
            DHCPBootstrapHeader* headerDHCP = (DHCPBootstrapHeader*)offset;
            headerDHCP->MessageType = 1;
            headerDHCP->HardwareType = 1;
            headerDHCP->HardwareAddressLength = 6;
            headerDHCP->Hops = 0;
            headerDHCP->TransationID = ByteUtil.ReverseBytes((uint)0x3D1D);
            headerDHCP->SecondsElapsed = 0;
            headerDHCP->BootpFlags = 0x00;

            for (int i = 0; i < 6; i++)
                headerDHCP->ClientMac[i] = m_mac[i];
            for (int i = 0; i < 10; i++)
                headerDHCP->AddressPadding[i] = 0;

            headerDHCP->MagicCookie[0] = 0x63;
            headerDHCP->MagicCookie[1] = 0x82;
            headerDHCP->MagicCookie[2] = 0x53;
            headerDHCP->MagicCookie[3] = 0x63;

            // MessageType
            offset = offset + sizeof(DHCPBootstrapHeader);
            MessageTypeHeader* type = (MessageTypeHeader*)offset;
            type->Type = 0x35;
            type->DHCP = 1;
            type->Length = 1;

            // ClientID
            offset = offset + sizeof(MessageTypeHeader);
            ClientID* clientID = (ClientID*)offset;
            clientID->Type = 0x3D;
            clientID->HardwareType = 1;
            clientID->Length = 7;
            for (int i = 0; i < 6; i++)
                clientID->ClientMac[i] = m_mac[i];

            // Request IP
            offset = offset + sizeof(ClientID);
            RequestedIpAddress* reqIpAdr = (RequestedIpAddress*)offset;
            reqIpAdr->Type = 0x32;
            reqIpAdr->Length = 4;
            offset = offset + sizeof(RequestedIpAddress);

            // Request parameter list 
            offset[0] = 0x37;
            offset[1] = 4;
            offset[2] = 1;
            offset[3] = 3;
            offset[4] = 6;
            offset[5] = 42;
            offset[6] = 0xFF;
            
            //Network.Transmit((byte *)ptr, (uint)(size + sizeof(UDPHeader)));
        }
    }
}
