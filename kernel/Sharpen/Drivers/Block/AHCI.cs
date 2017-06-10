using Sharpen.Arch;
using Sharpen.Mem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Block
{

    unsafe struct AHCI_Port_registers
    {
        public uint CLB;
        public uint CLBU;
        public uint FB;
        public uint FBU;
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
        public uint CTBUA;

        public fixed uint Res[4];
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

    unsafe class PortInfo
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

        private const uint SIG_SATA = 0x00000101;
        private const uint SIG_SATAPI = 0xEB140101;
        private const uint SIG_SEMB = 0xC33C0101;
        private const uint SIG_PM = 0x96690101;

        private static int mID;

        private PciDevice mPciDevice;
        private int mAddress;
        private int mAddressVirtual;

        private AHCI_Generic_registers* mGenericRegs;
        private AHCI_Port_registers* mPorts;
        private uint mSupportedPorts;
        
        private PortInfo[] PortInfo;

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
            PortInfo = new PortInfo[mSupportedPorts];

            for (int i = 0; i < mSupportedPorts; i++)
            {
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

            if ((portReg->SSTS & SSTS_DET_MASK) == SSTS_DET_NO)
                return AHCI_PORT_TYPE.NO;
            

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
        private void initPort(PortInfo portInfo)
        {

            AHCI_Port_registers *portReg = mPorts + portInfo.PortNumber;

            portInfo.PortRegisters = portReg;

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

            AHCI_Command_header * CmdHeaders = (AHCI_Command_header *) Heap.AlignedAlloc(1024, sizeof(AHCI_Command_header) * NUM_CMD_HEADERS);
            Memory.Memclear(CmdHeaders, sizeof(AHCI_Command_header) * NUM_CMD_HEADERS);

            AHCI_Command_header* Fises = (AHCI_Command_header*)Heap.AlignedAlloc(256, sizeof(AHCI_Received_FIS));
            Memory.Memclear(Fises, sizeof(AHCI_Received_FIS));

            portReg->CLB = (uint)CmdHeaders;
            portReg->CLBU = 0x00;

            portReg->FB = (uint)Fises;
            portReg->FBU = 0x00;


            for(int i = 0; i < NUM_CMD_HEADERS; i++)
            {

                CmdHeaders[i].Prdtl = 8;

                CmdHeaders[i].CTBA = (uint)(CmdHeaders + i);
                CmdHeaders[i].CTBUA = 0x00;
            }


            startPort(portReg);
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
