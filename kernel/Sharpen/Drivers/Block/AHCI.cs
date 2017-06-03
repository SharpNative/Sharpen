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

    unsafe class AHCI
    {
        private const int NUM_CMD_HEADERS = 32;

        private const int CAP_NUM_PORTS_MASK = 0x1F;

        private static int mID;

        private PciDevice mPciDevice;
        private int mAddress;
        private int mAddressVirtual;

        private AHCI_Generic_registers* mGenericRegs;
        private AHCI_Port_registers* mPorts;
        private uint mSupportedPorts;

        private AHCI_Command_header* CmdHeaders;

        public AHCI(PciDevice dev)
        {
            mPciDevice = dev;
        }

        public unsafe void InitDevice()
        {
            /**
             * Check if there is a memory bar
             */
            if ((mPciDevice.BAR5.flags & Pci.BAR_IO) > 0)
            {
                Console.WriteLine("[E1000] Device not MMIO!");
                return;
            }

            Pci.EnableBusMastering(mPciDevice);


            int size = sizeof(AHCI_Generic_registers) + (sizeof(AHCI_Port_registers) * 32);
            mAddress = (int)mPciDevice.BAR5.Address;
            mAddressVirtual = (int)Paging.MapToVirtual(Paging.CurrentDirectory, mAddress, size, Paging.PageFlags.Writable | Paging.PageFlags.Present);

            mGenericRegs = (AHCI_Generic_registers*)mAddressVirtual;
            mPorts = (AHCI_Port_registers *)(mAddressVirtual + 0x100);

            ReadCapabilties();
            initPorts();

        }

        private void ReadCapabilties()
        {
            mSupportedPorts = mGenericRegs->CAP & CAP_NUM_PORTS_MASK;
            mSupportedPorts++;
        }

        private void initPorts()
        {
            for (int i = 0; i < mSupportedPorts; i++)
                initPort(i);
        }

        private void initPort(int portNum)
        {

            AHCI_Port_registers* portReg = mPorts + portNum;


            // Note: 1k alligned?
            CmdHeaders = (AHCI_Command_header *) Heap.Alloc(sizeof(AHCI_Command_header) * NUM_CMD_HEADERS);


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
