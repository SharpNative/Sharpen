using Sharpen.Arch;
using Sharpen.MultiTasking;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Net
{

    class ARP
    {

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        unsafe struct ARPHeader
        {
            public ushort HardwareType;
            public ushort ProtocolType;
            public byte HardwareAdrLength;
            public byte ProtocolAdrLength;
            public ushort Opcode;
            public fixed byte SrcHw[6];
            public fixed byte SrcIP[4];
            public fixed byte DstHw[6];
            public fixed byte DstIP[4];
        }

        public unsafe struct ARPEntry
        {
            public fixed byte MAC[6];
            public fixed byte IP[4];
        }

        public const byte OP_REQUEST = 0x01;
        public const byte OP_REPLY = 0x02;

        private static int m_offset = 0;
        private static ARPEntry[] m_arpTable;

        /// <summary>
        /// Register ARP protocol
        /// </summary>
        public static unsafe void Init()
        {
            Network.RegisterHandler(0x0806, handler);

            // Max 20
            m_arpTable = new ARPEntry[20];
        }

        /// <summary>
        /// Request ARP range
        /// </summary>
        /// <param name="ip">Sample 192.168.10.1 (Will do 192.168.10.1 - 192.168.10.255)</param>
        public static unsafe void Request(byte[] ip)
        {
            byte[] brdIP = new byte[6];
            for (int i = 0; i < 6; i++)
                brdIP[i] = 0xFF;
            
            ArpSend(OP_REQUEST, brdIP, ip);
        }

        public static unsafe void ArpSend(ushort op, byte[] hwAddr, byte[] ip)
        {
            byte[] mac = new byte[6];
            Network.GetMac((byte *)Util.ObjectToVoidPtr(mac));

            NetPacketDesc* packet = NetPacket.Alloc();

            ARPHeader *hdr = (ARPHeader *)(packet->buffer + packet->end);
            hdr->HardwareType = Utilities.Byte.ReverseBytes(0x01);
            hdr->ProtocolType = Utilities.Byte.ReverseBytes(0x800);

            hdr->HardwareAdrLength = 6;
            hdr->ProtocolAdrLength = 4;
            
            hdr->Opcode = Utilities.Byte.ReverseBytes(op);


            for (int i = 0; i < 6; i++)
                hdr->SrcHw[i] = mac[i];

            for (int i = 0; i < 4; i++)
                hdr->SrcIP[i] = Network.Settings->IP[i];

            if (op == OP_REPLY)
            {
                for (int i = 0; i < 6; i++)
                    hdr->DstHw[i] = hwAddr[i];
            }
            else
            {
                for (int i = 0; i < 6; i++)
                    hdr->DstHw[i] = 0x00;
            }

            for (int i = 0; i < 4; i++)
                hdr->DstIP[i] = ip[i];


            packet->end += (short)sizeof(ARPHeader);
            Ethernet.SendMAC(packet, hwAddr, EthernetTypes.ARP);

            NetPacket.Free(packet);
        }

        public static unsafe void FindOrAdd(byte[] srcIP, byte[] srcHW)
        {
            ARPEntry* entry = GetEntry(srcIP);

            if (entry == null)
            {
                // Add entry

                fixed (byte* ip = m_arpTable[m_offset].IP)
                {
                    for (int i = 0; i < 4; i++)
                        ip[i] = srcIP[i];
                }

                fixed (byte* mac = m_arpTable[m_offset].MAC)
                {
                    for (int i = 0; i < 6; i++)
                        mac[i] = srcHW[i];
                }

                m_offset++;
            }
            else
            {
                for (int i = 0; i < 6; i++)
                    entry->MAC[i] = srcHW[i];
            }
        }

        /// <summary>
        /// Handle packet
        /// </summary>
        /// <param name="buffer">Buffer pointer</param>
        /// <param name="size">Size</param>
        private static unsafe void handler(byte[] mac, byte* buffer, uint size)
        {
            ARPHeader* header = (ARPHeader*)buffer;

            // Only IPV4 - Ethernet ARP packages allowed! :)
            if (Utilities.Byte.ReverseBytes(header->ProtocolType) != 0x0800 || Utilities.Byte.ReverseBytes(header->HardwareType) != 1)
                return;

            if (Utilities.Byte.ReverseBytes(header->Opcode) == OP_REPLY)
            {
                FindOrAdd(Util.PtrToArray(header->SrcIP), Util.PtrToArray(header->SrcHw));
            }
            else
            {
                // Our IP?
                if (header->DstIP[0] == Network.Settings->IP[0] &&
                    header->DstIP[1] == Network.Settings->IP[1] &&
                    header->DstIP[2] == Network.Settings->IP[2] &&
                    header->DstIP[3] == Network.Settings->IP[3])
                    ArpSend(OP_REPLY, Util.PtrToArray(header->SrcHw), Util.PtrToArray(header->SrcIP));
            }
        }

        public static unsafe ARPEntry *GetEntry(byte[] IP)
        {
            for (int i = 0; i < 20; i++)
            {
                if (IPEqual(IP, m_arpTable[i]))
                {
                    return (ARPEntry*)((byte *)Util.ObjectToVoidPtr(m_arpTable) + (sizeof(ARPEntry) * i));
                }
            }

            return null;
        }

        /// <summary>
        /// Lookup IP in ARP
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="DestMac"></param>
        /// <returns></returns>
        public static unsafe bool Lookup(byte[] IP, byte *DestMac)
        {
            // Broadcast?
            if(IP[0] == 0xFF && IP[1] == 0xFF && IP[2] == 0xFF && IP[3] == 0xFF)
            {
                if (DestMac != null)
                    for (int i =0; i < 6; i++)
                    DestMac[i] = 0xFF;

                return true;
            }
            else
            {
                ARPEntry *entry = GetEntry(IP);

                if(entry != null)
                {
                    if(DestMac != null)
                        for (int i = 0; i < 6; i++)
                            DestMac[i] = entry->MAC[i];
                    
                    return true;
                }
            }

            return false;
        }

        public static unsafe bool IpExists(byte[] IP)
        {
            return Lookup(IP, null);
        }

        /// <summary>
        /// ARP IP matches?
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        private static unsafe bool IPEqual(byte[] IP, ARPEntry entry)
        {
            if (entry.IP[0] == IP[0] &&
                entry.IP[1] == IP[1] &&
                entry.IP[2] == IP[2] &&
                entry.IP[3] == IP[3])
                return true;

            return false;
        }
    }
}
