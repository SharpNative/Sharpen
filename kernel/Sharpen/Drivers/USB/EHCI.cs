using Sharpen.Arch;
using Sharpen.Mem;
using Sharpen.MultiTasking;
using Sharpen.Synchronisation;
using Sharpen.USB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.USB
{

    public unsafe class EHCIController : IUSBController
    {

        public USBControllerType Type { get { return USBControllerType.EHCI; } }

        public USBHelpers.ControllerPoll Poll { get; set; }

        public int MemoryBase { get; set; }

        public EHCIHostCapRegister * CapabilitiesRegisters { get; set; }

        public int OperationalRegisters { get; set; }

        public int* FrameList;

        public EHCIQueueHead* QueueHeadPool { get; set; }

        public EHCITransferDescriptor* TransferPool { get; set; }
        
        public EHCIQueueHead* FirstHead { get; set; }

        public EHCIQueueHead* AsyncQueueHead { get; set; }

        public EHCIQueueHead* PeriodicQueuehead { get; set; }

        public int PortNum { get; set; }
    }

    public unsafe struct EHCITransferDescriptor
    {
        public int NextLink;

        public int Reserved;

        public int Token;

        public fixed int Buffer[5];
        public fixed int ExtBuffer[5];


        public bool Allocated;
        public EHCITransferDescriptor* Previous;
        public EHCITransferDescriptor* Next;
    }
    
    public unsafe struct EHCIQueueHead
    {
        public int Head;
        public int EPCharacteristics;
        public int EPCapabilities;

        public int CurLink;


        // Transfer descriptor
        public int NextLink;

        public int Reserved;

        public int Token;

        public int BufferPointer;

        public bool Allocated;
        public USBTransfer* Transfer;
        public EHCITransferDescriptor* Transmit;
        public EHCIQueueHead* Previous;
        public EHCIQueueHead* Next;
        public fixed byte Padding[12];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct EHCIHostCapRegister
    {
        public byte CapLength;

        public byte Reserved;

        public ushort HCIVersion;

        public int HCSParams;

        public int HCCParams;

        public long HCSPPortroute;
    }
    
    public unsafe class EHCI
    {
        const ushort MAX_HEADS = 16;
        const ushort MAX_TRANSFERS = 32;

        const ushort INTF_EHCI = 0x20;

        const ushort REG_USBCMD = 0x00;
        const ushort REG_USBSTS = 0x04;
        const ushort REG_USBINTR = 0x08;
        const ushort REG_FRINDEX = 0x0C;
        const ushort REG_CTRLDSSEGMENT = 0x10;
        const ushort REG_PERIODICLISTBASE = 0x14;
        const ushort REG_ASYNCLISTADDR = 0x18;
        const ushort REG_CONFIGFLAG = 0x40;
        const ushort REG_PORTSC = 0x44;

        const int USBCMD_RUN = (1 << 0);
        const int USBCMD_HCRESET = (1 << 1);
        const int USBCMD_FLSize = (2 << 2);
        const int USBCMD_PSE = (1 << 4);
        const int USBCMD_ASE = (1 << 5);
        const int USBCMD_IOAAD = (1 << 6);
        const int USBCMD_LHCR = (1 << 7);
        const int USBCMD_ASPMC = (2 << 8);
        const int USBCMD_ASPME = (1 << 11);
        const int USBCMD_ITC = (7 << 16);

        const ushort ITC_1MICROFRAME = 0x01;
        const ushort ITC_2MICROFRAMES = 0x02;
        const ushort ITC_4MICROFRAMES = 0x04;
        const ushort ITC_8MICROFRAMES = 0x08;
        const ushort ITC_16MICROFRAMES = 0x10;
        const ushort ITC_32MICROFRAMES = 0x20;
        const ushort ITC_64MICROFRAMES = 0x40;

        const int PORTSC_CUR_STAT = (1 << 0);
        const int PORTSC_CON = (1 << 1);
        const int PORTSC_EN = (1 << 2);
        const int PORTSC_EN_CHNG = (1 << 3);
        const int PORTSC_OC_ACT = (1 << 4);
        const int PORTSC_OC_CHNG = (1 << 5);
        const int PORTSC_FP_RESUME = (1 << 6);
        const int PORTSC_SUSPEND = (1 << 7);
        const int PORTSC_RESET = (1 << 8);
        const int PORTSC_LINE_STAT = (2 << 10);
        const int PORTSC_POWER = (1 << 12);
        const int PORTSC_CPC = (1 << 13);
        const int PORTSC_PIC = (2 << 14);
        const int PORTSC_PTC = (0x7 << 16);
        const int PORTSC_WOCE = (1 << 20);
        const int PORTSC_WODE = (1 << 21);
        const int PORTSC_WOOCE = (1 << 22);
        const int PORTSC_CHANGE = (PORTSC_EN_CHNG | PORTSC_OC_CHNG | PORTSC_CON);

        const int INTR_TRANSFER = (1 << 0);
        const int INTR_ERROR = (1 << 1);
        const int INTR_STAT_CHANGE = (1 << 2);
        const int INTR_LFR = (1 << 3);
        const int INTR_HC_ERROR = (1 << 4);
        const int INTR_AA_ENABLE = (1 << 5);

        const int QH_EC_ADDR_SHIFT = 0;
        const int QH_EC_ADDR_MASK = 0x3F;
        const int QH_I = (1 << 7);
        const int QH_ENDP_SHIFT = (1 << 8);
        const int QH_ENDP_MASK = 0xF;
        const int QH_EPS_SHIFT = (1 << 12);
        const int QH_EPS_MASK = 0x3;
        const int QH_DTC = (1 << 14);
        const int QH_H = (1 << 15);
        const int QH_MAXLEN_SHIFT = 16;
        const int QH_MAXLEN_MASK = 0xA;

        const int TD_TOK_STATUS_SHIFT = 0;
        const int TD_TOK_STATUS_MASK = 0x7F;
        const int TD_TOK_STATUS_ACTIVE = (1 << 7);
        const int TD_TOK_PID_OUT = (0 << 8) | (0 << 9);
        const int TD_TOK_PID_IN = (1 << 8) | (0 << 9);
        const int TD_TOK_PID_SETUP = (0 << 8) | (1 << 9);
        const int TD_TOK_CERR_SHIFT = 10;
        const int TD_TOK_CPAGE_SHIFT = 12;
        const int TD_TOK_IOC_SHIFT = 15;
        const int TD_TOK_TBTT_SHIFT = 16;
        const int TD_TOK_TOGGLE_SHIFT = 31;
        const int TD_TOK_PID_SHIFT = 8;



        const ushort TRANS_PACKET_SETUP = 0x2D;
        const ushort TRANS_PACKET_IN = 0x69;
        const ushort TRANS_PACKET_OUT = 0xE1;

        const uint HCSPARAMS_PORTS_MASK = (0xF << 0);

        const ushort FL_TERMINATE = (1 << 0);
        const ushort FL_QUEUEHEAD = (1 << 1);

        const ushort TD_TERMINATE = (1 << 0);

        private static Mutex mMutex;

        /// <summary>
        /// Initialize
        /// </summary>
        public static unsafe void Init()
        {
            
            mMutex = new Mutex();
            /**
             * Note: this cycles through PCI devices!
             */
            for (int i = 0; i < Pci.DeviceNum; i++)
            {
                PciDevice dev = Pci.Devices[i];

                if (dev.CombinedClass == (int)PciClassCombinations.USBController && dev.ProgIntf == INTF_EHCI)
                    initDevice(dev);
            }

        }

        /// <summary>
        /// Read ports from controller
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        private unsafe static int ReadPorts(EHCIController controller)
        {
            return (int)((*controller.CapabilitiesRegisters).HCSParams & HCSPARAMS_PORTS_MASK);
        }

        private static unsafe void resetPort(EHCIController controller, int portNum)
        {

            /**
             * Set reset bit
             */
            setPortBit(controller, portNum, PORTSC_RESET);

            /**
             * Wait for 60 ms
             */
            Tasking.CurrentTask.CurrentThread.Sleep(0, 60);

            /**
             * Unset reset bit
             */
            unsetPortBit(controller, portNum, PORTSC_RESET);


            /**
             * Wait for atleast 150ms for link to go up
             */
            for (int i = 0; i < 15; i++)
            {
                Tasking.CurrentTask.CurrentThread.Sleep(0, 10);

                int status = *(int*)((controller.OperationalRegisters + REG_PORTSC) + (portNum * 4));

                /**
                 * Is it even connected?
                 */
                if (((status) & PORTSC_CUR_STAT) == 0)
                    break;

                /**
                 * Status changed?
                 */
                if (((status) & (PORTSC_CON | PORTSC_EN_CHNG)) > 0)
                {
                    unsetPortBit(controller, portNum, PORTSC_CON | PORTSC_EN_CHNG);

                    continue;
                }

                /**
                 * Enabled?
                 */
                if ((status & PORTSC_EN) > 0)
                    break;
                
            }
        }


        /// <summary>
        /// Set bit on port
        /// </summary>
        /// <param name="port">Port number</param>
        /// <param name="bit">Bit to setr</param>
        private unsafe static void setPortBit(EHCIController uhciDev, int port, ushort bit)
        {
            int* portAdr = (int*)((uhciDev.OperationalRegisters + REG_PORTSC) + (port * 4));

            int status = *portAdr;
            status |= bit;

            // Reset port changes
            status &= ~PORTSC_CHANGE;

            *portAdr = status;
        }

        /// <summary>
        /// Unset bit on port
        /// </summary>
        /// <param name="port">Port number</param>
        /// <param name="bit">Bit to unset</param>
        private unsafe static void unsetPortBit(EHCIController uhciDev, int port, ushort bit)
        {
            int* portAdr = (int*)((uhciDev.OperationalRegisters + REG_PORTSC) + (port * 4));

            int status = *portAdr;
            status &= ~PORTSC_CHANGE;
            status &= ~bit;

            // Reset port changes
            status |= PORTSC_CHANGE & bit;

            *portAdr = status;
        }


        /// <summary>
        /// Prepare interrupt
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="transfer"></param>
        private unsafe static void PrepareInterrupt(USBDevice dev, USBTransfer* transfer)
        {
            transfer->Executed = true;
            transfer->Success = false;
        }


        /// <summary>
        /// Transfer
        /// </summary>
        /// <param name="dev">Device</param>
        /// <param name="transfer">Transfers</param>
        /// <param name="length">Number of transfers</param>
        private static unsafe void TransferOne(USBDevice dev, USBTransfer* transfer)
        {
            transfer->Executed = true;
            transfer->Success = false;
        }

        /// <summary>
        /// Control USB Device
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="transfer"></param>
        private unsafe static void Control(USBDevice dev, USBTransfer* transfer)
        {

            USBDeviceRequest request = transfer->Request;
            transfer->Executed = false;

            EHCIController controller = (EHCIController)dev.Controller;

            EHCITransferDescriptor* td = AllocateEmptyTransmit(controller);

            EHCITransferDescriptor* head = td;
            EHCITransferDescriptor* prev = null;

            USBDeviceRequest* a = (USBDeviceRequest*)Heap.Alloc(sizeof(USBDeviceRequest));
            a->Request = request.Request;
            a->Index = request.Index;
            a->Length = request.Length;
            a->Type = request.Type;
            a->Value = request.Value;

            InitTransmit(td, prev, dev.Speed, dev.Address, 0, 0, TRANS_PACKET_SETUP, (uint)sizeof(USBDeviceRequest), (byte*)a);
            prev = td;

            uint packetType = ((request.Type & USBDevice.TYPE_DEVICETOHOST) > 0) ? TRANS_PACKET_IN : TRANS_PACKET_OUT;

            byte* ptr = transfer->Data;
            uint packetSize = transfer->Length;
            uint offset = 0;

            uint toggle = 0;

            uint remaining = packetSize;

            while (remaining > 0)
            {
                td = AllocateEmptyTransmit(controller);
                if (td == null)
                    return;

                packetSize = remaining;
                if (packetSize > dev.MaxPacketSize)
                    packetSize = dev.MaxPacketSize;

                remaining -= packetSize;

                toggle ^= 1;

                InitTransmit(td, prev, dev.Speed, dev.Address, 0, toggle, packetType, packetSize, ptr + offset);
                prev = td;

                offset += packetSize;
            }

            td = AllocateEmptyTransmit(controller);
            if (td == null)
                return;

            packetType = ((request.Type & USBDevice.TYPE_DEVICETOHOST) > 0) ? TRANS_PACKET_OUT : TRANS_PACKET_IN;

            toggle = 1;
            InitTransmit(td, prev, dev.Speed, dev.Address, 0, toggle, packetType, 0, null);

            EHCIQueueHead* qh = AllocateEmptyQH(controller);

            InitHead(qh, null, dev.Address, 0, packetSize);
            qh->NextLink = (int)td;
            qh->Transmit = td;
            qh->Transfer = transfer;

            InsertHead(controller, qh);
            WaitForQueueHead(controller, qh);
        }

        /// <summary>
        /// Probe usb devices on port
        /// </summary>
        /// <param name="uhciDev">The UHCI device</param>
        private static unsafe void probe(EHCIController controller)
        {
            /**
             * UHCI only supports 2 ports, so just 2 :-)
             */
            for (int portNum = 0; portNum < controller.PortNum; portNum++)
            {
                resetPort(controller, portNum);


                int status = *(int*)((controller.OperationalRegisters + REG_PORTSC) + (portNum * 4));

                /**
                 * Is the port even connected?
                 */
                if ((status & PORTSC_CUR_STAT) == 0)
                    continue;
                

                USBDevice dev = new USBDevice();
                dev.Controller = controller;
                dev.Control = Control;
                dev.PrepareInterrupt = PrepareInterrupt;
                dev.TransferOne = TransferOne;

                /**
                 * Root hub
                 */
                dev.Parent = null;
                dev.Port = (uint)portNum;
                dev.State = USBDeviceState.ATTACHED;
                dev.Speed = USBDeviceSpeed.HIGH_SPEED;

                if (!dev.Init())
                {
                    Console.Write("[EHCI] Device init failed on port ");
                    Console.WriteNum(portNum);
                    Console.WriteLine("");

                    Heap.Free(dev);
                }
            }
        }

        #region Head allocation

        /// <summary>
        /// Get Transmit descriptor item
        /// </summary>
        /// <param name="dev">Device</param>
        /// <returns></returns>
        private unsafe static EHCITransferDescriptor* GetTransmitDescriptor(EHCIController dev)
        {
            mMutex.Lock();
            int i = 0;
            while (i < MAX_HEADS)
            {
                if (!dev.TransferPool[i].Allocated)
                {
                    dev.TransferPool[i].Allocated = true;
                    dev.TransferPool[i].Next = null;
                    dev.TransferPool[i].Previous = null;

                    mMutex.Unlock();
                    return (EHCITransferDescriptor*)(((int)dev.TransferPool) + (sizeof(EHCITransferDescriptor) * i));
                }

                i++;
            }

            mMutex.Unlock();
            return null;
        }

        /// <summary>
        /// Get Queue head item
        /// </summary>
        /// <param name="dev">Device</param>
        /// <returns></returns>
        private unsafe static EHCIQueueHead* GetQueueHead(EHCIController dev)
        {
            mMutex.Lock();
            int i = 0;
            while (i < MAX_HEADS)
            {
                if (!dev.QueueHeadPool[i].Allocated)
                {
                    dev.QueueHeadPool[i].Allocated = true;
                    dev.QueueHeadPool[i].Next = null;
                    dev.QueueHeadPool[i].Previous = null;

                    mMutex.Unlock();
                    return (EHCIQueueHead*)(((int)dev.QueueHeadPool) + (sizeof(EHCIQueueHead) * i));
                }

                i++;
            }

            mMutex.Unlock();
            return null;
        }
        #endregion

        private unsafe static EHCITransferDescriptor* AllocateEmptyTransmit(EHCIController controller)
        {
            EHCITransferDescriptor* queueHead = GetTransmitDescriptor(controller);

            return queueHead;
        }

        private static void InitTransmit(EHCITransferDescriptor* td, EHCITransferDescriptor* previous,
            USBDeviceSpeed speed, uint address, uint endp, uint toggle, uint type, uint len, byte* data)
        {

            td->NextLink = TD_TERMINATE;
            td->Reserved = TD_TERMINATE;
            td->Next = null;

            // Add link
            if (previous != null)
            {
                previous->NextLink = (int)td;
                previous->Next = td;
            }

            // Set token
            td->Token = (int)((toggle << TD_TOK_TOGGLE_SHIFT) |
                        (len << TD_TOK_TBTT_SHIFT) |
                        (3 << TD_TOK_CERR_SHIFT) |
                        (type << TD_TOK_PID_SHIFT) |
                        TD_TOK_STATUS_ACTIVE);

            // Set data buffer
            int ptr = (int)data;
            td->Buffer[0] = ptr;
            td->ExtBuffer[0] = (ptr >> 32);
            ptr &= ~0xFFF;

            for(int i = 1; i < 4; i++)
            {
                ptr += 0x1000;
                td->Buffer[i] = ptr;
                td->ExtBuffer[i] = (ptr >> 32);
            }
        }

        /// <summary>
        /// Insert head
        /// </summary>
        /// <param name="controller">UHCIController</param>
        /// <param name="head"></param>
        public static void InsertHead(EHCIController controller, EHCIQueueHead* head)
        {
            mMutex.Lock();
            EHCIQueueHead* end = controller.FirstHead;

            while (true)
                if (end->Next != null)
                    end = end->Next;
                else
                    break;

            head->Head = TD_TERMINATE;
            head->Previous = end;
            head->Next = null;

            end->Next = head;
            end->Head = (int)Paging.GetPhysicalFromVirtual(head);

            mMutex.Unlock();
        }


        public static void RemoveHead(EHCIController controller, EHCIQueueHead* head)
        {
            mMutex.Lock();

            /**
             * Set next to previous
             */
            if (head->Previous != null)
            {
                if (head->Next != null)
                {
                    head->Previous->Head = head->Head;
                    head->Previous->Next = head->Next;
                }
                else
                {
                    head->Previous->Head = TD_TERMINATE;
                    head->Previous->Next = null;
                }
            }

            /**
             * Set previous to next
             */
            if (head->Next != null)
            {
                head->Next->Previous = head->Previous;
            }

            head->Allocated = false;
            mMutex.Unlock();
        }

        private static void InitHead(EHCIQueueHead* qh, EHCIQueueHead* previous, uint addr, uint endp, uint maxSize)
        {

            qh->NextLink = TD_TERMINATE;
            qh->Reserved = TD_TERMINATE;
            qh->Next = null;

            // Add link
            if (previous != null)
            {
                previous->NextLink = (int)qh | 1;
                previous->Next = qh;
            }

            // Setup chars and caps
            qh->EPCharacteristics = (int)(QH_DTC |
                        (0x3 << QH_EPS_SHIFT) |
                        (endp << QH_ENDP_SHIFT) |
                        (maxSize << QH_MAXLEN_SHIFT) |
                        (addr << QH_EC_ADDR_SHIFT));

            qh->EPCapabilities = (1 << 30);

            qh->Token = 0;
        }

        private unsafe static EHCIQueueHead *AllocateEmptyQH(EHCIController controller)
        {
            EHCIQueueHead* queueHead = GetQueueHead(controller);
            queueHead->Head = FL_TERMINATE;
            queueHead->EPCapabilities = 0x00;
            queueHead->EPCharacteristics = 0x00;
            queueHead->CurLink = 0x00;
            queueHead->NextLink = 0x00;
            queueHead->Token = 0x00;
            queueHead->BufferPointer = 0x00;
            queueHead->Next = null;
            queueHead->Previous = null;
            queueHead->Transfer = null;

            return queueHead;
        }

        /// <summary>
        /// Process Queue Head
        /// </summary>
        /// <param name="device"></param>
        /// <param name="head"></param>
        public static void ProcessHead(EHCIController controller, EHCIQueueHead* head)
        {
            USBTransfer* transfer = head->Transfer;

            EHCITransferDescriptor* td = head->Transmit;

            Console.WriteHex(td->Token);
            Console.WriteLine(" :)");

            if (transfer->Executed)
            {
                //if(transfer->ID > 0)
                //{
                //    PrintQueue(transfer->ID, head);
                //}

                head->Transfer = null;

                /**
                 * We need to toggle endpoint state here
                 */

                /**
                 * Remove head from schedule
                 */
                RemoveHead(controller, head);

                /**
                 * Free transmit descriptors
                 */
                EHCITransferDescriptor* tdE = td;

                while (tdE != null)
                {
                    EHCITransferDescriptor* next = tdE->Next;
                    FreeTransmit(controller, tdE);
                    tdE = next;
                }
            }
        }

        public static void FreeTransmit(EHCIController controller, EHCITransferDescriptor* transmit)
        {
            transmit->Allocated = false;
        }


        public static void WaitForQueueHead(EHCIController controller, EHCIQueueHead* head)
        {

            while (!head->Transfer->Executed)
                ProcessHead(controller, head);
        }

        private unsafe static void initDevice(PciDevice dev)
        {
            if ((dev.BAR0.flags & Pci.BAR_IO) != 0)
            {
                Console.WriteLine("[EHCI] Only Memory mapped IO supported");
            }

            /**
             * Enable bus mastering
             */
            Pci.EnableBusMastering(dev);

            ulong barAddress = dev.BAR0.Address;
            
            EHCIController controller = new EHCIController();
            controller.MemoryBase = (int)Paging.MapToVirtual(Paging.KernelDirectory, (int)barAddress, 20 * 0x1000, Paging.PageFlags.Writable | Paging.PageFlags.Present);

            controller.FrameList = (int*)Heap.AlignedAlloc(0x1000, sizeof(int) * 1024);
            controller.CapabilitiesRegisters = (EHCIHostCapRegister*)(controller.MemoryBase);
            controller.OperationalRegisters = controller.MemoryBase + (*controller.CapabilitiesRegisters).CapLength;
            controller.PortNum = ReadPorts(controller);
            controller.QueueHeadPool = (EHCIQueueHead*)Heap.AlignedAlloc(0x1000, sizeof(EHCIQueueHead) * MAX_HEADS); 
            controller.TransferPool = (EHCITransferDescriptor*)Heap.AlignedAlloc(0x1000, sizeof(EHCITransferDescriptor) * MAX_TRANSFERS);
            controller.AsyncQueueHead = AllocateEmptyQH(controller);

            // Link to itself
            controller.AsyncQueueHead[0].Head = (int)controller.AsyncQueueHead | FL_QUEUEHEAD;

            controller.PeriodicQueuehead = AllocateEmptyQH(controller);

            for (int i = 0; i < 1024; i++)
                controller.FrameList[i] = FL_QUEUEHEAD | (int)controller.PeriodicQueuehead;

            // Set device
            * (int*)(controller.OperationalRegisters + REG_FRINDEX) = 0;
            * (int *)(controller.OperationalRegisters + REG_PERIODICLISTBASE) = (int)Paging.GetPhysicalFromVirtual(controller.FrameList);
            *(int*)(controller.OperationalRegisters + REG_ASYNCLISTADDR) = (int)Paging.GetPhysicalFromVirtual(controller.AsyncQueueHead);
            *(int*)(controller.OperationalRegisters + REG_CTRLDSSEGMENT) = 0;

            Console.Write("Periodic: ");
            Console.WriteHex((int)Paging.GetPhysicalFromVirtual(controller.FrameList));
            Console.WriteLine("");
            Console.Write("Periodic: ");
            Console.WriteHex((int)Paging.GetPhysicalFromVirtual(controller.FrameList));
            Console.WriteLine("");
            Console.Write("FRAME LIST PHYS: ");
            Console.WriteHex((int)Paging.GetPhysicalFromVirtual(controller.FrameList));
            Console.WriteLine("");
            Console.Write("FRAME LIST ENTRY: ");
            Console.WriteHex((int)controller.FrameList);
            Console.WriteLine("");
            Console.Write("FRAME LIST ENTRY 1: ");
            Console.WriteHex(controller.FrameList[0]);
            Console.WriteLine("");

            // Reset status
            *(int*)(controller.OperationalRegisters + REG_USBSTS) = 0x3F;


            // enable device
            *(int*)(controller.OperationalRegisters + REG_USBCMD) = USBCMD_PSE | USBCMD_RUN | USBCMD_ASPME | (ITC_8MICROFRAMES << USBCMD_ITC);

            // Wait till done
            while ((*(int*)(controller.OperationalRegisters + REG_USBSTS) & (1 << 12)) > 0)
                CPU.HLT();

            Console.Write("[EHCI] Detected with ");
            Console.WriteHex(controller.PortNum);
            Console.WriteLine(" ports");

            probe(controller);
        }
    }
}
