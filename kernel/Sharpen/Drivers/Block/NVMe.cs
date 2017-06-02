using Sharpen.Arch;
using Sharpen.Mem;
using Sharpen.MultiTasking;
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
        public ulong AdminSubmissionQueueAddress;
        public ulong AdminCompletionQueueAddress;
        public uint ControllerMemoryBufLocation;
        public uint ControllerMemoryBusSize;
    }

    unsafe struct NVMe_Submission_Item
    {
        public byte Opcode;
        public byte Flags;
        public ushort CommandID;
        public uint NSID;
        public ulong Res1;
        public ulong MPTR;
        public ulong PRP1;
        public ulong PRP2;
        public fixed uint CMDBlock[6];
    }

    unsafe struct NVMe_Completion_Item
    {
        public uint CommandSpec;
        public uint DW2;
        public byte SQHeadPtr;
        public byte SQID;
        public byte CommandID;
        public byte Status;
    }

    unsafe class NVMe_Queue
    {
        public int SQID;

        public NVMe_Submission_Item* SubmissionQueue;
        public NVMe_Completion_Item* CompletionQueue;
    }

    unsafe class NVMe
    {
        private const int MIN_QUEUE_DEPTH = 1024;

        private const int CAP_DSTRD_SHIFT = 32;
        private const int CAP_DSTRD_MASK = 0xF;
        private const int CAP_MQES_MASK = 0xFFFF;
        private const int CAP_TO_MASK = 0xF;
        private const int CAP_TO_SHIFT = 24;

        private const int CC_ENABLE = (1 << 0);
        private const int CC_CSS_NVM = (0 << 4);
        private const int CC_AMS_RR = (0 << 11);
        private const int CC_AMS_WRR = (1 << 11);
        private const int CC_AMS_VENDOR = (7 << 11);
        private const int CC_SHN_NO = (0 << 14);
        private const int CC_SHN_NML = (1 << 14);
        private const int CC_SHN_ABR = (2 << 14);
        private const int CC_IOSQES_SHIFT = 16;
        private const int CC_IOCQES_SHIFT = 20;

        private const int CSTS_RDY = (1 << 0);
        private const int CSTS_CFS = (1 << 1);
        private const int CSTS_SHST = (3 << 2);
        private const int CSTS_NSSRO = (1 << 4);
        private const int CSTS_PP = (1 << 5);

        private const int CC_MPS_SHIFT = 7;

        private PciDevice mDevice;
        private int mAddress;
        private int mAddressVirtual;
        private NVMe_Registers* mRegs;
        private int mTimeout;
        private int mStride;
        private int mVersion;
        private int mQueueSize;
        private NVMe_Queue mAdminQueue;

        private int AQA_ACQS_SHIFT = 16;
        private int AQA_ACQS_MASK = 0xFFF;
        private int AQA_ASQS_SHIFT = 0;
        private int AQA_ASQS_MASK = 0xFFF;

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
            ConfigureAdminQueue();
            EnableController();

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
            mQueueSize = (int)(mRegs->Capabilities & CAP_MQES_MASK);
            if (MIN_QUEUE_DEPTH > mQueueSize)
                mQueueSize = MIN_QUEUE_DEPTH;
        }

        /// <summary>
        /// Configure admin queue
        /// </summary>
        private void ConfigureAdminQueue()
        {

            mAdminQueue = AllocQueue(0);

            mRegs->AdminSubmissionQueueAddress = (ulong)Paging.GetPhysicalFromVirtual(mAdminQueue.SubmissionQueue);
            mRegs->AdminCompletionQueueAddress = (ulong)Paging.GetPhysicalFromVirtual(mAdminQueue.CompletionQueue);

            mRegs->AdminQueueAttribs = (uint)((uint)(mQueueSize - 1) & AQA_ASQS_MASK);
            mRegs->AdminQueueAttribs |= (uint)(((mQueueSize - 1) & AQA_ACQS_SHIFT) << AQA_ACQS_SHIFT);
        }

        /// <summary>
        /// Enable NVMe controller
        /// </summary>
        private void EnableController()
        {

            mRegs->ControllerStatus = 0x00;
            mRegs->ControllerConfig = (0) << CC_MPS_SHIFT; // 4KB
            mRegs->ControllerConfig |= CC_AMS_RR | CC_SHN_NO;
            mRegs->ControllerConfig |= (6 << CC_IOSQES_SHIFT) | (4 << CC_IOCQES_SHIFT);
            mRegs->ControllerConfig |= CC_ENABLE | CC_CSS_NVM;

            // Wait till device is ready
            uint status = mRegs->ControllerStatus;
            int timeOut = 0;

            while((status & CSTS_RDY) == 0)
            {

                Tasking.CurrentTask.CurrentThread.Sleep(0, 100);
                timeOut += 100;

                if (timeOut >= mTimeout)
                    Panic.DoPanic("[NVMe] Could not enable controller in time");

                status = mRegs->ControllerStatus;
            }
        }

        /// <summary>
        /// Alocate queue
        /// </summary>
        /// <param name="sqID">Submission Queue ID</param>
        /// <returns>Queue</returns>
        private unsafe NVMe_Queue AllocQueue(int sqID)
        {
            NVMe_Queue queue = new NVMe_Queue();
            queue.SQID = sqID;

            queue.SubmissionQueue = (NVMe_Submission_Item *)Heap.AlignedAlloc(0x1000, sizeof(NVMe_Submission_Item) * mQueueSize);
            queue.CompletionQueue = (NVMe_Completion_Item*)Heap.AlignedAlloc(0x1000, sizeof(NVMe_Completion_Item) * mQueueSize);
            Memory.Memclear(queue.SubmissionQueue, sizeof(NVMe_Submission_Item) * mQueueSize);
            Memory.Memclear(queue.CompletionQueue, sizeof(NVMe_Completion_Item) * mQueueSize);

            return queue;
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
