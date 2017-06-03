using Sharpen.Arch;
using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.MultiTasking;
using Sharpen.Synchronisation;
using Sharpen.Utilities;
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

    unsafe struct NVMe_Identify_Cmd
    {
        public byte Opcode;
        public byte Flags;
        public ushort CommandID;
        public uint NSID;
        public ulong Res1;
        public ulong Res2;
        public ulong PRP1;
        public ulong PRP2;
        public uint CNS;
        public fixed uint CMDBlock[5];
    }

    unsafe struct NVMe_Create_Sq_Cmd
    {
        public byte Opcode;
        public byte Flags;
        public ushort CommandID;
        public fixed uint Res[5];
        public ulong PRP1;
        public ulong Res1;
        public ushort SqID;
        public ushort SqSize;
        public ushort SqFlags;
        public ushort CqID;
        public fixed uint Res2[4];

    }

    unsafe struct NVMe_Create_Cq_Cmd
    {
        public byte Opcode;
        public byte Flags;
        public ushort CommandID;
        public fixed uint Res[5];
        public ulong PRP1;
        public ulong Res1;
        public ushort CqID;
        public ushort CqSize;
        public ushort CqFlags;
        public ushort IrqVector;
        public fixed uint Res2[4];
    }

    unsafe struct NVMe_Rw_Cmd
    {
        public byte Opcode;
        public byte Flags;
        public ushort commandID;
        public uint NSId;
        public ulong Res;
        public ulong Mptr;
        public ulong Prp1;
        public ulong Prp2;
        public ulong Slba;
        public ushort Nlb;
        public ushort Control;
        public uint Dsmgmt;
        public uint RefTag;
        public ushort AppTag;
        public ushort AppMask;
    }


    unsafe struct NVMe_Completion_Item
    {
        public uint CommandSpec;
        public uint DW2;
        public ushort SQHead;
        public ushort SQID;
        public ushort CommandID;
        public ushort Status;
    }



    unsafe class NVMe_Queue
    {
        public int SQID;

        public NVMe_Submission_Item* SubmissionQueue;
        public NVMe_Completion_Item* CompletionQueue;

        public Mutex mSubmissionMutex;
        public int Tail;
        public int Max;
        public int* TailPtr;
        public int* HeadPtr;
    }

    unsafe struct NVMe_ID_Ctrl
    {
        public ushort Vid;
        public ushort SSVid;
        public fixed char SerialNumber[20];
        public fixed char ModelNumber[40];
        public fixed char FirmwareRevision[8];
        public byte RAB;
        public fixed byte IEEE[3];
        public byte CMIC;
        public byte MDTS;
        public fixed byte Res[178];
        public ushort OACS;
        public byte ACL;
        public byte AERL;
        public byte Firmware;
        public byte Lpa;
        public byte Elpe;
        public byte Npss;
        public fixed byte Res2[248];
        public byte Sqes;
        public byte Cqes;
        public ushort Res3;
        public uint Nn;
        public ushort Oncs;
        public ushort Fuses;
    }

    enum NVMe_RW_OP
    {
        READ,
        WRITE
    }

    unsafe class NVMe
    {
        private const int MIN_QUEUE_DEPTH = 1024;

        private const int ADMIN_OPCODE_DELETE_SQ = 0x00;
        private const int ADMIN_OPCODE_CREATE_SQ = 0x01;
        private const int ADMIN_OPCODE_GET_LOG_PAGE = 0x02;
        private const int ADMIN_OPCODE_DELETE_CQ = 0x04;
        private const int ADMIN_OPCODE_CREATE_CQ = 0x05;
        private const int ADMIN_OPCODE_IDENTIFY = 0x06;

        private const byte IO_OPCODE_FLUSH = 0x00;
        private const byte IO_OPCODE_WRITE = 0x01;
        private const byte IO_OPCODE_READ = 0x02;

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

        private int AQA_ACQS_SHIFT = 16;
        private int AQA_ACQS_MASK = 0xFFF;
        private int AQA_ASQS_SHIFT = 0;
        private int AQA_ASQS_MASK = 0xFFF;

        private int COMP_STATUS_DNR = (1 << 31);
        private int COMP_STATUS_MORE = (1 << 30);
        private int COMP_STATUS_SCT = (7 << 25);
        private int COMP_STATUS_SC = (0x1FF << 17);

        private int COMP_STATUS_SC_SHIFT = 17;
        private int COMP_STATUS_SC_MASK = 0x1FF;

        private int COMP_STATUS_SCT_GEN = 0x00;
        private int COMP_STATUS_SCT_CSS = 0x01;
        private int COMP_STATUS_SCT_MDIE = 0x02;
        private int COMP_STATUS_SCT_VS = 0x07;

        private int COMP_STATUS_SC_SUC = 0x00;
        private int COMP_STATUS_SC_INV_OP = 0x01;
        private int COMP_STATUS_SC_INV_FIL = 0x02;

        private ushort QUEUE_PHYS_CONFIG = (1 << 0);
        private ushort SQ_PRIO_MEDIUM = (2 << 1);

        private static int mID;

        private PciDevice mDevice;
        private int mAddress;
        private int mAddressVirtual;
        private int *mTailsAndHeads;

        private NVMe_Registers* mRegs;
        private int mTimeout;
        private int mStride;
        private int mVersion;
        private int mQueueSize;
        private NVMe_Queue mAdminQueue;
        private NVMe_Queue mIOQueue;


        private char[] mSerialNumber;
        private char[] mFirmwareRevision;
        private char[] mModel;
        private int nNumNamespaces;


        private ushort CurrentCID;

        public NVMe(PciDevice device)
        {
            mDevice = device;
        }



        /// <summary>
        /// Initialize nvme device
        /// </summary>
        public void InitDevice()
        {
            CurrentCID = 1;
            
            mAddress = (int)mDevice.BAR0.Address;
            mAddressVirtual = (int)Paging.MapToVirtual(Paging.CurrentDirectory, mAddress, 0x1000, Paging.PageFlags.Present | Paging.PageFlags.Writable);
            mRegs = (NVMe_Registers*)mAddressVirtual;

            Pci.EnableBusMastering(mDevice);
            
            // TODO: Make this map the right size
            mTailsAndHeads = (int*)Paging.MapToVirtual(Paging.CurrentDirectory, mAddress + 0x1000, 0x4000, Paging.PageFlags.Present | Paging.PageFlags.Writable);

            ReadCapabilities();
            ReadVersion();
            ConfigureAdminQueue();
            EnableController();
            DoIdentify();
            CreateQueues();



            char* name = (char*)Heap.Alloc(6);
            name[0] = 'N';
            name[1] = 'V';
            name[2] = 'M';
            name[3] = 'E';
            name[4] = (char)('0' + mID++);
            name[5] = '\0';
            string nameStr = Util.CharPtrToString(name);

            Node node = new Node();
            node.Read = readImpl;
            node.Write = writeImpl;

            NVMeCookie cookie = new NVMeCookie();
            cookie.NVMe = this;

            node.Cookie = cookie;

            RootPoint dev = new RootPoint(nameStr, node);
            VFS.MountPointDevFS.AddEntry(dev);

            Console.Write("[NVME] Init with serialnumber ");
            for(int i = 0; i < 20; i++)
                Console.Write(mSerialNumber[i]);
            Console.WriteLine("");
        }

        /// <summary>
        /// Do identify and read needed shizzle
        /// </summary>
        private void DoIdentify()
        {

            void* adr = Heap.Alloc(0x1000);

            int status = Identify(0, 1, adr);

            int statusCode = (status >> COMP_STATUS_SC_SHIFT) & COMP_STATUS_SC_MASK;

            if (statusCode != COMP_STATUS_SC_SUC)
                Panic.DoPanic("[NVMe] Couldn't identifiy");

            NVMe_ID_Ctrl* idCtrl = (NVMe_ID_Ctrl*)adr;

            mFirmwareRevision = new char[8];
            mSerialNumber = new char[20];
            mModel = new char[40];
            nNumNamespaces = (int)idCtrl->Nn;

            Memory.Memcpy(Util.ObjectToVoidPtr(mFirmwareRevision), idCtrl->FirmwareRevision, 8);
            Memory.Memcpy(Util.ObjectToVoidPtr(mSerialNumber), idCtrl->SerialNumber, 20);
            Memory.Memcpy(Util.ObjectToVoidPtr(mModel), idCtrl->ModelNumber, 40);

            Heap.Free(adr);
        }

        /// <summary>
        /// Create queues (for now just 1..)
        /// </summary>
        private void CreateQueues()
        {
            
            var queue = CreateQueue(1);

            if (queue == null)
                Panic.DoPanic("[NVMe] Couldn't make queue");
            
            
            mIOQueue = queue;
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
        /// Read write operation
        /// </summary>
        /// <param name="op"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public int RWOperation(NVMe_RW_OP op, byte *buffer, uint lba, uint blocks)
        {
            
            void* adr = Heap.Alloc(0x1000);


            NVMe_Rw_Cmd* item = (NVMe_Rw_Cmd*)Heap.Alloc(sizeof(NVMe_Rw_Cmd));
            Memory.Memclear(item, sizeof(NVMe_Rw_Cmd));

            item->Opcode = (op == NVMe_RW_OP.WRITE) ? IO_OPCODE_WRITE: IO_OPCODE_READ;
            item->NSId = 1;

            item->Slba = lba;
            item->Nlb = (ushort)(blocks - 1);
            item->Prp1 = (uint)adr;

            int cid = SubmitCMD(mIOQueue, (NVMe_Submission_Item*)item);
            
            NVMe_Completion_Item* compItem = PollAndWait(mIOQueue, cid);
            
            int statusCode = (compItem->Status >> COMP_STATUS_SC_SHIFT) & COMP_STATUS_SC_MASK;
            if (statusCode != COMP_STATUS_SC_SUC)
            {
                Heap.Free(adr);

                return 0;
            }

            Memory.Memcpy(buffer, adr, (int)blocks * 512);

            Heap.Free(adr);

            return (int)blocks * 512;
        }

        private unsafe int Identify(int nsID, int cns, void *adr)
        {
            NVMe_Identify_Cmd* item = (NVMe_Identify_Cmd*)Heap.Alloc(sizeof(NVMe_Submission_Item));
            Memory.Memclear(item, sizeof(NVMe_Submission_Item));
            
            item->Opcode = ADMIN_OPCODE_IDENTIFY;
            item->NSID = (uint)nsID;
            item->PRP1 = (ulong)adr;
            item->CNS = (uint)cns;

            int cid = SubmitCMD(mAdminQueue, (NVMe_Submission_Item*)item);

            NVMe_Completion_Item *compItem = PollAndWait(mAdminQueue, cid);

            return compItem->Status;
        }

        #region Byte swapping
        private ulong SwapBytes(ulong inLong)
        {
            ulong x = (ulong)inLong;
            x = (x & 0x00000000FFFFFFFF) << 32 | (x & 0xFFFFFFFF00000000) >> 32;
            x = (x & 0x0000FFFF0000FFFF) << 16 | (x & 0xFFFF0000FFFF0000) >> 16;
            x = (x & 0x00FF00FF00FF00FF) << 8 | (x & 0xFF00FF00FF00FF00) >> 8;
            return x;
        }

        private uint SwapBytes(uint inInt)
        {
            return ((inInt & 0x000000FF) << 24)
                | ((inInt & 0x0000FF00) << 8)
                | ((inInt & 0x00FF0000) >> 8)
                | ((inInt & 0xFF000000) >> 24);
        }

        private ushort SwapBytes(ushort inShort)
        {
            return (ushort)(((inShort & 0x00FF) << 8)
                | ((inShort & 0xFF00) >> 8));
        }
        #endregion

        private int CreateSubmissionQueue(NVMe_Queue queue)
        {
            

            NVMe_Create_Sq_Cmd* item = (NVMe_Create_Sq_Cmd*)Heap.Alloc(sizeof(NVMe_Create_Sq_Cmd));
            Memory.Memclear(item, sizeof(NVMe_Create_Sq_Cmd));

            item->Opcode = ADMIN_OPCODE_CREATE_SQ;
            item->PRP1 = (uint)Paging.GetPhysicalFromVirtual(queue.SubmissionQueue);
            item->SqID = (ushort)queue.SQID;
            item->SqSize = (ushort)(mQueueSize - 1);
            item->SqFlags = (ushort)(QUEUE_PHYS_CONFIG | SQ_PRIO_MEDIUM);
            item->CqID = (ushort)queue.SQID;

            int cid = SubmitCMD(mAdminQueue, (NVMe_Submission_Item*)item);

            NVMe_Completion_Item* compItem = PollAndWait(mAdminQueue, cid);

            return compItem->Status;
        }

        private int CreateCompletionQueue(NVMe_Queue queue)
        {


            NVMe_Create_Cq_Cmd* item = (NVMe_Create_Cq_Cmd*)Heap.Alloc(sizeof(NVMe_Create_Cq_Cmd));
            Memory.Memclear(item, sizeof(NVMe_Create_Cq_Cmd));

            item->Opcode = ADMIN_OPCODE_CREATE_CQ;
            item->PRP1 = (uint)Paging.GetPhysicalFromVirtual(queue.CompletionQueue);
            item->CqID = (ushort)queue.SQID;
            item->CqSize = (ushort)(mQueueSize - 1);
            item->CqFlags = (ushort)(QUEUE_PHYS_CONFIG | SQ_PRIO_MEDIUM);

            int cid = SubmitCMD(mAdminQueue, (NVMe_Submission_Item*)item);

            NVMe_Completion_Item* compItem = PollAndWait(mAdminQueue, cid);

            return compItem->Status;
        }

        private NVMe_Queue CreateQueue(int qid)
        {

            NVMe_Queue queue = AllocQueue(qid);

            
            int status = CreateCompletionQueue(queue);

            int statusCode = (status >> COMP_STATUS_SC_SHIFT) & COMP_STATUS_SC_MASK;
            if (statusCode != COMP_STATUS_SC_SUC)
                return null;
            

            status = CreateSubmissionQueue(queue);
            statusCode = (status >> COMP_STATUS_SC_SHIFT) & COMP_STATUS_SC_MASK;

            if (statusCode != COMP_STATUS_SC_SUC)
                return null;

            return queue;
        }

        /// <summary>
        /// Configure admin queue
        /// </summary>
        private void ConfigureAdminQueue()
        {

            mAdminQueue = AllocQueue(0);

            mRegs->ControllerStatus = 0x00;
            mRegs->AdminQueueAttribs = (uint)((uint)(mQueueSize - 1) & AQA_ASQS_MASK);
            mRegs->AdminQueueAttribs |= (uint)(((mQueueSize - 1) & AQA_ACQS_SHIFT) << AQA_ACQS_SHIFT);
            
            mRegs->AdminSubmissionQueueAddress = (ulong)Paging.GetPhysicalFromVirtual(mAdminQueue.SubmissionQueue);
            mRegs->AdminCompletionQueueAddress = (ulong)Paging.GetPhysicalFromVirtual(mAdminQueue.CompletionQueue);
        }

        private NVMe_Completion_Item *PollAndWait(NVMe_Queue queue, int cid)
        {

            while(true)
            {
                for (int i = 0; i < mQueueSize; i++)
                {
                    var item = (NVMe_Completion_Item*)((int)queue.CompletionQueue + (sizeof(NVMe_Completion_Item) * i));
                    
                    if(item->CommandID == cid)
                    {


                        return item;
                    }
                }
            }
        }

        /// <summary>
        /// Submit item in queue
        /// </summary>
        /// <param name="queue">Queue</param>
        /// <param name="item">Item</param>
        private int SubmitCMD(NVMe_Queue queue, NVMe_Submission_Item *item)
        {

            int tail = queue.Tail;

            queue.mSubmissionMutex.Lock();

            item->CommandID = CurrentCID;


            Memory.Memcpy((void *)((int)queue.SubmissionQueue + (sizeof(NVMe_Submission_Item) * tail)), item, sizeof(NVMe_Submission_Item));

            var itemm = (NVMe_Submission_Item*)((int)queue.SubmissionQueue + (sizeof(NVMe_Submission_Item) * tail));
            
            tail = tail + 1;

            if (tail == queue.Max)
                tail = 0;

            queue.Tail = tail;
            *queue.TailPtr = tail;

            if (++CurrentCID >= 65535)
                item->CommandID = 0;

            queue.mSubmissionMutex.Unlock();

            return CurrentCID - 1;
        }

        /// <summary>
        /// Enable NVMe controller
        /// </summary>
        private void EnableController()
        {

            mRegs->ControllerConfig = (0) << CC_MPS_SHIFT; // 4KB
            mRegs->ControllerConfig |= CC_AMS_RR | CC_SHN_NO; // Round robin, no shutdown notifcation
            mRegs->ControllerConfig |= (6 << CC_IOSQES_SHIFT) | (4 << CC_IOCQES_SHIFT); // Setr Queue sizes
            mRegs->ControllerConfig |= CC_ENABLE | CC_CSS_NVM; // Enable and NVM
            
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

            queue.mSubmissionMutex = new Mutex();
            queue.Max = mQueueSize;
            queue.Tail = 0;

            uint tail = (uint)mTailsAndHeads + (uint)((2 * sqID) * (sizeof(int) << mStride));
            uint head = (uint)mTailsAndHeads + (uint)((2 * sqID + 1) * (sizeof(int) << mStride));

            queue.TailPtr = (int *)tail;
            queue.HeadPtr = (int *)head;

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

        #region FS Commands

        /// <summary>
        /// Write method for filesystem
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes written</returns>
        private static uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            // Only support sizes in magnitudes of 512
            if (size % 512 != 0)
                return 0;

            uint inSize = size / 512;

            NVMeCookie cookie = (NVMeCookie)node.Cookie;
            return (uint)cookie.NVMe.RWOperation(NVMe_RW_OP.WRITE, (byte*)Util.ObjectToVoidPtr(buffer), offset, inSize);
        }

        /// <summary>
        /// Read method for filesystem
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes read</returns>
        private static uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            // Only support sizes in magnitudes of 512
            if (size % 512 != 0)
                return 0;

            uint inSize = size / 512;

            NVMeCookie cookie = (NVMeCookie)node.Cookie;
            return (uint)cookie.NVMe.RWOperation(NVMe_RW_OP.READ, (byte *)Util.ObjectToVoidPtr(buffer), offset, inSize);
        }


        #endregion


        /// <summary>
        /// Readout bus for NVMe devices
        /// </summary>
        public static unsafe void Init()
        {
            mID = 0;

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
