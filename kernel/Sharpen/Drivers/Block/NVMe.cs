using Sharpen.Arch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Block
{
    struct NVMe_Registers
    {
        public ulong Capabilities;
        public uint Version;
        public uint InterruptMaskSet;
        public uint InterruptMaskClear;
        public uint ControllerConfig;
        public uint Reserved;
        public uint ControllerStatus;
        public uint NVMReset;
        public uint AdminQueueAttribs;
        public ulong AdminSubmitQueueAddress;
        public ulong AdminTransmitQueueAddress;
        public uint ControllerMemoryBufLocation;
        public uint ControllerMemoryBusSize;
    }

    unsafe class NVMe
    {
        private const int MIN_QUEUE_DEPTH = 1024;

        private const int CAP_DSTRD_SHIFT = 32;
        private const int CAP_DSTRD_MASK = 0xF;

        private const int CAP_MQES_MASK = 0xFFFF;

        private const int CAP_TO_MASK = 0xF;
        private const int CAP_TO_SHIFT = 24;

        private PciDevice mDevice;
        private int mAddress;
        private int mAddressVirtual;
        private NVMe_Registers* mRegs;
        private int mTimeout;
        private int mStride;
        private int mVersion;

        public NVMe(PciDevice device)
        {
            mDevice = device;
        }

        /// <summary>
        /// Initialize nvme device
        /// </summary>
        public void InitDevice()
        {
            mAddress = (int)mDevice.BAR0.Address;
            mAddressVirtual = (int)Paging.MapToVirtual(Paging.CurrentDirectory, mAddress, 0x4000, Paging.PageFlags.Present | Paging.PageFlags.Writable);
            mRegs = (NVMe_Registers*)mAddressVirtual;

            ReadCapabilities();
            ReadVersion();

            Console.Write("[NVME] Init at 0x");
            Console.WriteHex(mAddress & 0xFFFFFFFF);
            Console.Write(", vers = ");
            Console.WriteNum(mVersion / 100);
            Console.Write(".0");
            Console.WriteNum(mVersion % 100);
            Console.Write(", Stride = ");
            Console.WriteNum(mStride);
            Console.Write(", TO = ");
            Console.WriteNum(mTimeout);
            Console.WriteLine("");
        }

        /// <summary>
        /// Read capabilities
        /// </summary>
        private void ReadCapabilities()
        {
            mTimeout = (int)((mRegs->Capabilities >> CAP_TO_SHIFT) & CAP_TO_MASK) * 500;
            mStride = (int)((mRegs->Capabilities >> CAP_DSTRD_SHIFT) & CAP_DSTRD_MASK);
        }

        /// <summary>
        /// Read version
        /// </summary>
        private void ReadVersion()
        {

            uint version = mRegs->Version;

            if (version == 0x00010200)
                mVersion = 102;
            else if (version == 0x00010100)
                mVersion = 101;
            else if (version == 0x00010000)
                mVersion = 100;
            else
                Panic.DoPanic("[NVMe] Wrong NVMe version");
        }

        /// <summary>
        /// Readout bus for NVMe devices
        /// </summary>
        public static unsafe void Init()
        {
            /**
             * Note: this cycles through PCI devices!
             */
            for (int i = 0; i < Pci.DeviceNum; i++)
            {
                PciDevice dev = Pci.Devices[i];
                
                if(dev.CombinedClass == 0x0108 && dev.ProgIntf == 0x02)
                {
                    NVMe nvme = new NVMe(dev);
                    nvme.InitDevice();
                }
            }
        }
    }
}
