using Sharpen.Arch;
using Sharpen.FileSystem;
using Sharpen.FileSystem.Cookie;
using Sharpen.Mem;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Block
{

    unsafe struct AHCI_Port_registers
    {
        public uint CLB; // 4
        public uint CLBU; // 8
        public uint FB; // 12
        public uint FBU; // 16
        public uint IS;
        public uint IE;
        public uint CMD;
        public uint Res;
        public uint TFD;
        public uint SIG;
        public uint SSTS;
        public uint SCTL;
        public uint SERR;
        public uint SACT;
        public uint CI;
        public uint SNTF;
        public uint Fbs;
        public fixed uint Res2[11];
        public fixed uint Vendor[4];
    }

    unsafe struct AHCI_Generic_registers
    {
        public uint CAP;
        public uint GHC;
        public uint IS;
        public uint PI;
        public uint VS;
        public uint CCC_CTL;
        public uint CCC_PORTS;
        public uint EM_LOC;
        public uint EM_CTL;
        public uint CAP2;
        public uint BOHC;

        public fixed byte Res[116];

        public fixed byte Vendor[96];
    }
    
    public enum AHCI_FIS
    {
        REG_H2D = 0x27,
        REG_D2H = 0x4,
        DMA_ACT = 0x39,
        DMA_SETUP = 0x41,
        TYPE_DATA = 0x46,
        TYPE_BIST = 0x58,
        PIO_SETUP = 0x5F,
        DEV_BITS = 0xA1
    }

    public enum AHCI_PORT_TYPE
    {
        NO,
        SATA,
        SATAPI,
        SEMB,
        PM
    }

    unsafe struct AHCI_Command_table_entry
    {
        public fixed byte CFIS[64];

        public fixed byte ACMD[16];

        public fixed byte Res[48];
    }

    unsafe struct AHCI_PRDT_Entry
    {
        public uint DBA;
        public uint DBAU;

        public int Res;

        public int Misc;
    }

    unsafe struct AHCI_REG_H2D
    {

        // DWORD 0
        public byte FisType;
        public byte Options;
        public byte Command;
        public byte Feature;

        // DWORD1
        public byte LBA0;
        public byte LBA1;
        public byte LBA2;
        public byte Device;

        // DWORD 2
        public byte LBA3;
        public byte LBA4;
        public byte LBA5;
        public byte FeatureExtended;

        // DWORD 3
        public byte CountLo;
        public byte CountHi;
        public byte ICC;
        public byte Control;

        // DWORD 4
        public uint Res;
    }

    unsafe struct AHCI_REG_D2H
    {

        // DWORD 0
        public byte FisType;
        public byte Options;
        public byte Status;
        public byte Error;

        // DWORD1
        public byte LBA0;
        public byte LBA1;
        public byte LBA2;
        public byte Device;

        // DWORD 2
        public byte LBA3;
        public byte LBA4;
        public byte LBA5;
        public byte Res;

        // DWORD 3
        public byte CountLo;
        public byte CountHi;
        public byte ICC;
        public ushort Res2;

        // DWORD 4
        public uint Res3;
    }

    unsafe struct AHCI_DATA
    {

        // DWORD 0
        public byte FisType;

        public byte Options;

        public ushort Res;

        // DWORD 1.. Data
    }

    unsafe struct AHCI_Command_header
    {
        // DW 0 - Description info
        public ushort Options;
        public ushort Prdtl;

        // DW1 - Command status
        public uint PRDBC;

        // DW2 - Command table base address
        public uint CTBA;

        // DW3 - Command table base address upper
        public uint CTBAU;

        public fixed int Res[4];
    }

    unsafe struct AHCI_Received_FIS
    {
        /// <summary>
        /// DMA Setup FIS
        /// </summary>
        public fixed byte DSFIS[28];

        /// <summary>
        /// 4 bytes padding
        /// </summary>
        public fixed byte Padding0[4];

        /// <summary>
        /// PIO Setup FIS
        /// </summary>
        public fixed byte PSFIS[20];

        /// <summary>
        /// 12 bytes padding
        /// </summary>
        public fixed byte Padding1[12];

        /// <summary>
        /// D2H Register FIS
        /// </summary>
        public fixed byte RFIS[20];

        /// <summary>
        /// 4 bytes padding
        /// </summary>
        public fixed byte Padding2[4];

        /// <summary>
        /// Set device bits FIS
        /// </summary>
        public fixed byte SDBFis[8];

        /// <summary>
        /// Unkown FIS
        /// </summary>
        public fixed byte UFIS[64];

        /// <summary>
        /// Reserved
        /// </summary>
        public fixed byte Reserved[96];
    }

    unsafe class AHCIPortInfo
    {

        public AHCI_PORT_TYPE Type { get; set; }

        public AHCI_Command_header *CommandHeader;

        public AHCI_Received_FIS* ReceivedFIS;

        public int PortNumber;

        public AHCI_Port_registers* PortRegisters;
    }

    unsafe class AHCI
    {
        private const int NUM_CMD_HEADERS = 32;

        private const int CAP_NUM_PORTS_MASK = 0x1F;

        private const int SSTS_DET_NO = 0x00;
        private const int SSTS_DET_NO_COM = 0x01;
        private const int SSTS_DET_COM = 0x03;
        private const int SSTS_DET_OFF = 0x04;

        private const int SSTS_DET_MASK = 0xF;

        private const int SSTS_SPD_NO = (0x00 << 4);
        private const int SSTS_SPD_GEN1 = (0x01 << 4);
        private const int SSTS_SPD_GEN2 = (0x03 << 4);
        private const int SSTS_SPD_GEN3 = (0x04 << 4);

        private const int SSTS_SPD_MASK = (0xF << 4);

        private const int SSTS_IPM_NO = (0x00 << 8);
        private const int SSTS_IPM_ACTIVE = (0x01 << 8);
        private const int SSTS_IPM_PARTIAL = (0x02 << 8);
        private const int SSTS_IPM_SLUMBER = (0x06 << 8);
        private const int SSTS_IPM_DEVSLEEP = (0x08 << 8);

        private const int SSTS_IPM_MASK = (0xF << 8);

        private const int PxCMD_ST = (1 << 0);
        private const int PxCMD_SUD = (1 << 1);
        private const int PxCMD_POD = (1 << 2);
        private const int PxCMD_CLO = (1 << 3);
        private const int PxCMD_FRE = (1 << 4);
        private const int PxCMD_CCS = (0xF << 8);
        private const int PxCMD_MPSS = (1 << 13);
        private const int PxCMD_FR = (1 << 14);
        private const int PxCMD_CR = (1 << 15);
        private const int PxCMD_CPS = (1 << 16);
        private const int PxCMD_PMA = (1 << 17);
        private const int PxCMD_HPCP = (1 << 18);
        private const int PxCMD_MPSP = (1 << 19);
        private const int PxCMD_CPD = (1 << 20);
        private const int PxCMD_ESP = (1 << 21);
        private const int PxCMD_FSSCP = (1 << 22);
        private const int PxCMD_APSTE = (1 << 23);
        private const int PxCMD_ATAPI = (1 << 24);
        private const int PxCMD_DLAE = (1 << 25);
        private const int PxCMD_ALPE = (1 << 26);
        private const int PxCMD_ASP = (1 << 27);
        private const int PxCMD_ICC = (0xF << 28);

        private const int PxCMD_ICC_IDLE = 0;
        private const int PxCMD_ICC_ACTIVE = 0x1;
        private const int PxCMD_ICC_PARTIAL = 0x2;
        private const int PxCMD_ICC_SLUMBER = 0x6;
        private const int PxCMD_ICC_DEVSLEEP = 0x8;

        private const int PxIS_DHRS = (1 << 0);
        private const int PxIS_PSS = (1 << 1);
        private const int PxIS_DSS = (1 << 2);
        private const int PxIS_SDBS = (1 << 3);
        private const int PxIS_UFS = (1 << 4);
        private const int PxIS_DPS = (1 << 5);
        private const int PxIS_PCS = (1 << 6);
        private const int PxIS_DMPS = (1 << 7);
        private const int PxIS_PRCS = (1 << 22);
        private const int PxIS_IPMS = (1 << 23);
        private const int PxIS_OFS = (1 << 24);
        private const int PxIS_INFS = (1 << 26);
        private const int PxIS_IFS = (1 << 27);
        private const int PxIS_HBDS = (1 << 28);
        private const int PxIS_HBFS = (1 << 29);
        private const int PxIS_TFES = (1 << 30);
        private const int PxIS_CPDS = (1 << 31);

        private const uint SIG_SATA = 0x00000101;
        private const uint SIG_SATAPI = 0xEB140101;
        private const uint SIG_SEMB = 0xC33C0101;
        private const uint SIG_PM = 0x96690101;

        private const byte ATA_CMD_READ_DMA = 0xC8;
        private const byte ATA_CMD_WRITE_DMA = 0xCA;
        private const byte ATA_CMD_READ_DMA_EX = 0x25;
        private const byte ATA_CMD_WRITE_DMA_EX = 0x35;
        private const byte ATA_CMD_PACKET = 0xA0;
        private const byte ATA_CMD_FLUSH_CACHE = 0xE7;
        private const byte ATA_CMD_IDENTIFY = 0xEC;
        private const byte ATA_CMD_SET_FEATURES = 0xEF;

        private const ushort CMD_HEAD_WRITE = (1 << 6);
        private const ushort CMD_HEAD_READ = (0 << 6);

        private const int ATA_DEV_BUSY = 0x80;
        private const int ATA_DEV_DRQ = 0x08;

        private static int mID;

        private PciDevice mPciDevice;
        private int mAddress;
        private int mAddressVirtual;

        private AHCI_Generic_registers* mGenericRegs;
        private AHCI_Port_registers* mPorts;
        private uint mSupportedPorts;
        
        private AHCIPortInfo[] PortInfo;

        public AHCI(PciDevice dev)
        {
            mPciDevice = dev;
        }

        /// <summary>
        /// Initalize device
        /// </summary>
        public unsafe void InitDevice()
        {

            // Check if its a memory bar
            if ((mPciDevice.BAR5.flags & Pci.BAR_IO) > 0)
            {
                Console.WriteLine("[AHCI] Device not MMIO!");
                return;
            }

            // Enable bus mastering
            Pci.EnableBusMastering(mPciDevice);


            int size = sizeof(AHCI_Generic_registers) + (sizeof(AHCI_Port_registers) * 32);
            mAddress = (int)mPciDevice.BAR5.Address;
            mAddressVirtual = (int)Paging.MapToVirtual(Paging.CurrentDirectory, mAddress, size, Paging.PageFlags.Writable | Paging.PageFlags.Present);

            mGenericRegs = (AHCI_Generic_registers*)mAddressVirtual;
            mPorts = (AHCI_Port_registers *)(mAddressVirtual + 0x100);

            ReadCapabilties();
            initPorts();

            Console.Write("[AHCI] Initalized at 0x");
            Console.WriteHex(mAddress & 0xFFFFFFFF);
            Console.WriteLine("");
        }

        /// <summary>
        /// Read capabilities from device
        /// </summary>
        private void ReadCapabilties()
        {
            mSupportedPorts = mGenericRegs->CAP & CAP_NUM_PORTS_MASK;
            mSupportedPorts++;
        }

        /// <summary>
        /// Initalize ports
        /// </summary>
        private void initPorts()
        {
            PortInfo = new AHCIPortInfo[mSupportedPorts];

            for (int i = 0; i < mSupportedPorts; i++)
            {
                PortInfo[i] = new AHCIPortInfo();

                PortInfo[i].PortNumber = i;

                initPort(PortInfo[i]);
            }
        }

        /// <summary>
        /// Get AHCI port type
        /// </summary>
        /// <param name="portReg">Port registers</param>
        /// <returns>Port type</returns>
        private AHCI_PORT_TYPE GetPortType(AHCI_Port_registers* portReg)
        {

            // Check if there is a device attached on the port.
            if ((portReg->SSTS & SSTS_DET_MASK) == SSTS_DET_NO)
                return AHCI_PORT_TYPE.NO;
            
            // Check if port is active.
            if ((portReg->SSTS & SSTS_IPM_MASK) != SSTS_IPM_ACTIVE)
                return AHCI_PORT_TYPE.NO;
            

            switch (portReg->SIG)
            {

                case SIG_SATA:
                    return AHCI_PORT_TYPE.SATA;

                case SIG_SATAPI:
                    return AHCI_PORT_TYPE.SATAPI;

                case SIG_SEMB:
                    return AHCI_PORT_TYPE.SEMB;

                case SIG_PM:
                    return AHCI_PORT_TYPE.PM;

                default:
                    return AHCI_PORT_TYPE.SATA;
            }
        }

        /// <summary>
        /// Initalize port
        /// </summary>
        /// <param name="portInfo">Port info</param>
        private void initPort(AHCIPortInfo portInfo)
        {

            AHCI_Port_registers *portReg = mPorts + portInfo.PortNumber;
            
            portInfo.CommandHeader = (AHCI_Command_header*)Heap.AlignedAlloc(4048, sizeof(AHCI_Command_header) * NUM_CMD_HEADERS);
            Memory.Memset(portInfo.CommandHeader, 0xFF, sizeof(AHCI_Command_header) * NUM_CMD_HEADERS);
            
            portInfo.Type = GetPortType(portReg);

            // Port found?
            if (portInfo.Type == AHCI_PORT_TYPE.NO)
                return;
            
            // NOTE: We support only SATA for now..
            if(portInfo.Type != AHCI_PORT_TYPE.SATA)
            {

                switch(portInfo.Type)
                {

                    case AHCI_PORT_TYPE.PM:
                        Console.Write("[AHCI] Unsupported type PM found on port ");
                        Console.WriteNum(portInfo.PortNumber);
                        Console.WriteLine("");
                        break;

                    case AHCI_PORT_TYPE.SATAPI:
                        Console.Write("[AHCI] Unsupported type SATAPI found on port ");
                        Console.WriteNum(portInfo.PortNumber);
                        Console.WriteLine("");
                        break;


                    case AHCI_PORT_TYPE.SEMB:
                        Console.Write("[AHCI] Unsupported type SEMB found on port ");
                        Console.WriteNum(portInfo.PortNumber);
                        Console.WriteLine("");
                        break;
                }

                return;
            }
            
            stopPort(portReg);

            AHCI_Received_FIS* Fises = (AHCI_Received_FIS*)Heap.AlignedAlloc(256, sizeof(AHCI_Received_FIS));
            Memory.Memclear(Fises, sizeof(AHCI_Received_FIS));

            portReg->CLB = (uint)Paging.GetPhysicalFromVirtual(portInfo.CommandHeader);
            portReg->CLBU = 0x00;

            portReg->FB = (uint)Paging.GetPhysicalFromVirtual(Fises);
            portReg->FBU = 0x00;

            AHCI_Command_table_entry* cmdTable = (AHCI_Command_table_entry*)Heap.AlignedAlloc(128, sizeof(AHCI_Command_table_entry));
            Memory.Memclear(cmdTable, sizeof(AHCI_Command_table_entry));

            Console.WriteLine("AD");
            for (int i = 0; i < NUM_CMD_HEADERS; i++)
            {

                portInfo.CommandHeader[i].Prdtl = 0;
                portInfo.CommandHeader[i].CTBA = (uint)cmdTable;
                portInfo.CommandHeader[i].CTBAU = 0x00;
            }

            startPort(portReg);
            
            portInfo.PortRegisters = portReg;


            char* name = (char*)Heap.Alloc(5);
            name[0] = 'H';
            name[1] = 'D';
            name[2] = 'A';
            name[3] = (char)('0' + portInfo.PortNumber);
            name[4] = '\0';
            string nameStr = Util.CharPtrToString(name);

            Node node = new Node();
            node.Read = readImpl;
            //node.Write = writeImpl;

            AHCICookie cookie = new AHCICookie();
            cookie.AHCI = this;
            cookie.PortInfo = portInfo;

            node.Cookie = cookie;

            Disk.InitalizeNode(node, nameStr);

            RootPoint dev = new RootPoint(nameStr, node);
            VFS.MountPointDevFS.AddEntry(dev);
        }

        /// <summary>
        /// Find free command header on port
        /// </summary>
        /// <param name="port">Port info</param>
        /// <returns>Command header number</returns>
        private int findFreeCommandHeader(AHCIPortInfo port)
        {

            AHCI_Port_registers* portReg = port.PortRegisters;
            
            uint slotValues = (portReg->CI | portReg->SACT);
            
            // Find free slot.
            for(int i = 0; i < NUM_CMD_HEADERS; i++)
                if ((slotValues & (1 << i)) == 0)
                    return i;

            // No free slot found.
            return -1;
        }

        /// <summary>
        /// Start port
        /// </summary>
        /// <param name="portReg">Port register</param>
        private void startPort(AHCI_Port_registers* portReg)
        {

            // Wait till device ready
            while ((portReg->CMD & PxCMD_CR) > 0)
                CPU.HLT();

            portReg->CMD |= (PxCMD_ST) | (PxCMD_FRE);

        }

        /// <summary>
        /// Stop port
        /// </summary>
        /// <param name="portReg">Port register</param>
        private void stopPort(AHCI_Port_registers *portReg)
        {
            portReg->CMD &= ~(uint)(PxCMD_ST);

            // Wait till device ready
            while ((portReg->CMD & PxCMD_CR) > 0)
                CPU.HLT();

            portReg->CMD &= ~(uint)(PxCMD_FRE);
        }

        /// <summary>
        /// ATA transfer
        /// </summary>
        /// <param name="info">AHCI port info</param>
        /// <param name="offset">LBA</param>
        /// <param name="count">Num of sectors</param>
        /// <param name="buffer">Buffer ptr</param>
        /// <param name="write">Write action?</param>
        /// <returns></returns>
        public int AtaTransfer(AHCIPortInfo info, int offset, int count, byte *buffer, bool write)
        {

            AHCI_Port_registers* portReg = info.PortRegisters;



            portReg->IS = 0;

            int fullCount = count * 512;
            
            int headerNumber = findFreeCommandHeader(info);
            if (headerNumber == -1)
                return 0;

            AHCI_Command_header* header = info.CommandHeader + headerNumber;
            header->Options = (ushort)((sizeof(AHCI_REG_H2D) / 4) | ((write? CMD_HEAD_WRITE: CMD_HEAD_READ)));
            

            int prdtl = (fullCount / 2024) + 1;
            header->Prdtl = (ushort)prdtl;
            
            byte* curBuf = buffer;
            int curOffset = 0;
            AHCI_Command_table_entry* cmdTable = (AHCI_Command_table_entry *)Heap.AlignedAlloc(128, sizeof(AHCI_Command_table_entry) + (sizeof(AHCI_PRDT_Entry) * prdtl));
            Memory.Memclear(cmdTable, sizeof(AHCI_Command_table_entry) + (sizeof(AHCI_PRDT_Entry) * prdtl));

            header->CTBA = (uint)Paging.GetPhysicalFromVirtual(cmdTable);


            for(int i = 0; i < header->Prdtl - 1; i++)
            {
                AHCI_PRDT_Entry* entry = (AHCI_PRDT_Entry *)((int)cmdTable + sizeof(AHCI_Command_table_entry) + (sizeof(AHCI_PRDT_Entry) * i));

                entry->DBA = (uint)curBuf;
                entry->DBAU = 0x00;
                entry->Misc = 2023;

                curBuf += 2024;
                curOffset += 2024;
            }

            AHCI_PRDT_Entry* lastEntry = (AHCI_PRDT_Entry*)((int)cmdTable + sizeof(AHCI_Command_table_entry) + (sizeof(AHCI_PRDT_Entry) * (prdtl - 1)));
            lastEntry->DBA = (uint)curBuf;
            lastEntry->DBAU = 0x00;
            lastEntry->Misc = (fullCount - curOffset) - 1;
            
            AHCI_REG_H2D* fis = (AHCI_REG_H2D*)cmdTable->CFIS;
            fis->FisType = (int)AHCI_FIS.REG_H2D;
            fis->Command = ATA_CMD_READ_DMA_EX;
            fis->Options = (1 << 7);

            fis->LBA0 = (byte)(offset & 0xFF);
            fis->LBA1 = (byte)((offset >> 8) & 0xFF);
            fis->LBA2 = (byte)((offset >> 16) & 0xFF);
            fis->Device = (1 << 6);

            fis->LBA3 = 0x00;
            fis->LBA4 = 0x00;
            fis->LBA5 = 0x00;

            fis->CountLo = (byte)(count & 0xFF);
            fis->CountHi = (byte)((count >> 8) & 0xFF);

            // Wait until port ready
            while ((info.PortRegisters->TFD & (ATA_DEV_BUSY | ATA_DEV_DRQ)) > 0)
                CPU.HLT();

            info.PortRegisters->CI = (uint)(1 << headerNumber);
            

            while (true)
            {
                if ((info.PortRegisters->CI & (1 << headerNumber)) == 0)
                    break;

                CPU.HLT();
            }

            if ((info.PortRegisters->IS & PxIS_TFES) > 0)
                return 0;
            
            return 0;
        }


        /// <summary>
        /// Write method for filesystem
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes written</returns>
        private static unsafe uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            // Only support sizes in magnitudes of 512
            if (size % 512 != 0)
                return 0;

            uint sz = size / 512;

            AHCICookie cookie = (AHCICookie)node.Cookie;

            byte* bufferPtr = (byte*)Util.ObjectToVoidPtr(buffer);

            if (cookie.AHCI.AtaTransfer(cookie.PortInfo, (int)offset, (int)sz, (byte*)Util.ObjectToVoidPtr(buffer), true) == 0)
                return 0;

            return (uint)size;
        }


        /// <summary>
        /// Read method for filesystem
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes read</returns>
        private static unsafe uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            // Only support sizes in magnitudes of 512
            if (size % 512 != 0)
                return 0;

            uint sz = size / 512;

            AHCICookie cookie = (AHCICookie)node.Cookie;

            byte* bufferPtr = (byte*)Util.ObjectToVoidPtr(buffer);

            if (cookie.AHCI.AtaTransfer(cookie.PortInfo, (int)offset, (int)sz, (byte*)Util.ObjectToVoidPtr(buffer), false) == 0)
                return 0;

            return (uint)size;
        }

        /// <summary>
        /// Initalize "Driver"
        /// </summary>
        public static void Init()
        {
            mID = 0;

            /**
             * Note: this cycles through PCI devices!
             */
            for (int i = 0; i < Pci.DeviceNum; i++)
            {
                PciDevice dev = Pci.Devices[i];

                if (dev.CombinedClass == 0x0106 && dev.ProgIntf == 0x01)
                {
                    AHCI ahci = new AHCI(dev);
                    ahci.InitDevice();
                }
            }
        }
    }
}
