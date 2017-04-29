//#define __UHCI_DIAG
using Sharpen.Arch;
using Sharpen.Mem;
using Sharpen.MultiTasking;
using Sharpen.Synchronisation;
using Sharpen.USB;

namespace Sharpen.Drivers.USB
{
    public unsafe struct UHCITransmitDescriptor
    {
        public int Link;
        public int Control;
        public uint Token;
        public int BufferPointer;


        public bool Allocated { get; set; }
        public UHCITransmitDescriptor* Previous { get; set; }
        public UHCITransmitDescriptor* Next { get; set; }
        public fixed byte padding[2];
    }
    
    public unsafe struct UHCIQueueHead
    {
        public int Head;
        public int Element;

        public bool Allocated { get; set; }

        public USBTransfer *Transfer { get; set; }
        public UHCITransmitDescriptor *Transmit { get; set; }

        public UHCIQueueHead *Previous { get; set; }
        public UHCIQueueHead *Next { get; set; }
        public fixed byte padding[20];
    }

    public unsafe class UHCIController : IUSBController
    {

        public USBControllerType Type { get { return USBControllerType.UHCI; } }

        public USBHelpers.ControllerPoll Poll { get; set; }

        public ushort IOBase { get; set; }

        public int* FrameList { get; set; }

        public UHCIQueueHead *QueueHeadPool { get; set; }

        public UHCITransmitDescriptor *TransmitPool { get; set; }

        public UHCIQueueHead * FirstHead { get; set; }
    }


    unsafe class UHCI
    {
        const ushort INTF_UHCI = 0x00;

        const ushort REG_USBCMD    = 0x00;
        const ushort REG_USBSTS    = 0x02;
        const ushort REG_USBINTR   = 0x04;
        const ushort REG_FRNUM     = 0x06;
        const ushort REG_FRBASEADD = 0x08;
        const ushort REG_SOFMOD    = 0x0C;
        const ushort REG_PORTSC1   = 0x10;
        const ushort REG_PORTSC2   = 0x12;
        const ushort REG_LEGSUP    = 0xC0;


        const ushort PORTSC_CUR_STAT            = (1 << 0);
        const ushort PORTSC_STAT_CHNG           = (1 << 1);
        const ushort PORTSC_CUR_ENABLE          = (1 << 2);
        const ushort PORTSC_ENABLE_STAT         = (1 << 3);
        const ushort PORTSC_LINE_STAT           = (1 << 4);
        const ushort PORTSC_RESUME_DETECT       = (1 << 6);
        const ushort PORTSC_LOW_SPEED           = (1 << 8);
        const ushort PORTSC_RESET               = (1 << 9);
        const ushort PORTSC_SUSPEND             = (1 << 12);

        const ushort FL_TERMINATE = (1 << 0);
        const ushort FL_QUEUEHEAD = (1 << 1);

        const ushort TD_TERMINATE = (1 << 0);
        const ushort TD_QUEUEHEAD = (1 << 1);
        const ushort TD_DEPTHSEL  = (1 << 2);

        const ushort USBCMD_RS = (1 << 0);
        const ushort USBCMD_HCRESET = (1 << 1);
        const ushort USBCMD_GRESET = (1 << 2);
        const ushort USBCMD_EGSM = (1 << 3);
        const ushort USBCMD_FGR = (1 << 4);
        const ushort USBCMD_SWDBG = (1 << 5);
        const ushort USBCMD_CF = (1 << 6);
        const ushort USBCMD_MAXP = (1 << 7);

        const ushort MAX_HEADS = 8;
        const ushort MAX_TRANSMIT = 32;


        const ushort TRANS_PACKET_SETUP = 0x2D;
        const ushort TRANS_PACKET_IN = 0x69;
        const ushort TRANS_PACKET_OUT = 0xE1;

        const uint TD_CONTROL_BITSTUFF = (1 << 17);
        const uint TD_CONTROL_CRC = (1 << 18);
        const uint TD_CONTROL_NAK = (1 << 19);
        const uint TD_CONTROL_BABBLE = (1 << 20);
        const uint TD_CONTROL_OVERFLOW = (1 << 21);
        const uint TD_CONTROL_STALLED = (1 << 22);
        const uint TD_CONTROL_ACTIVE = (1 << 23);
        const uint TD_CONTROL_LOW_SPEED = (1 << 24);
        const uint TD_CONTROL_IOC = (1 << 25);
        const uint TD_CONTROL_IOS = (1 << 26);
        const uint TD_CONTROL_ERROR_MASK = (3 << 27);
        const uint TD_ERROR_SHIFT = 27;
        const uint TD_CONTROL_SPD = (1 << 29);

        const int TD_POINTER_TERMINATE = (1 << 0);
        const int TD_POINTER_QH = (1 << 1);
        const int TD_POINTER_DEPTH = (1 << 2);

        const int TD_TOKEN_ADDR = 8;
        const int TD_TOKEN_ENDP = 15;
        const int TD_TOKEN_D_SHIFT = 19;
        const int TD_TOKEN_MAXLEN = 21;

        private static Mutex m_mutex;

        /// <summary>
        /// 
        /// </summary>
        public static unsafe void Init()
        {
            m_mutex = new Mutex();
            /**
             * Note: this cycles through PCI devices!
             */
            for (int i = 0; i < PCI.DeviceNum; i++)
            {
                PciDevice dev = PCI.Devices[i];

                if (dev.CombinedClass == (int)PCIClassCombinations.USBController && dev.ProgIntf == INTF_UHCI)
                    initDevice(dev);
            }

        }

        private static void initDevice(PciDevice dev)
        {
            if ((dev.BAR4.flags & PCI.BAR_IO) == 0)
            {
                Console.WriteLine("[UHCI] Only Portio supported");
            }

            PCI.PCIEnableBusMastering(dev);

            UHCIController uhciDev = new UHCIController();
            uhciDev.IOBase = (ushort)dev.BAR4.Address;
            uhciDev.Poll = Poll;
            

            Console.Write("[UHCI] Initalize at 0x");
            Console.WriteHex(uhciDev.IOBase);
            Console.WriteLine("");

            uhciDev.FrameList = (int*)Heap.AlignedAlloc(0x1000, sizeof(int) * 1024);
            uhciDev.QueueHeadPool = (UHCIQueueHead *)Heap.AlignedAlloc(0x1000, sizeof(UHCIQueueHead) * MAX_HEADS);
            uhciDev.TransmitPool = (UHCITransmitDescriptor*)Heap.AlignedAlloc(0x1000, sizeof(UHCITransmitDescriptor) * MAX_TRANSMIT);
            

            UHCIQueueHead* head = GetQueueHead(uhciDev);
            head->Head = TD_POINTER_TERMINATE;
            head->Element = TD_POINTER_TERMINATE;

            uhciDev.FirstHead = head;
            

            for (int i = 0; i < 1024; i++)
                uhciDev.FrameList[i] = TD_POINTER_QH | (int)Paging.GetPhysicalFromVirtual(head);
            

            PortIO.Out16((ushort)(uhciDev.IOBase + REG_LEGSUP), 0x8f00);

            /**
             * Initalize framelist
             */
            PortIO.Out16((ushort)(uhciDev.IOBase + REG_FRNUM), 0);
            PortIO.Out32((ushort)(uhciDev.IOBase + REG_FRBASEADD), (uint)Paging.GetPhysicalFromVirtual(uhciDev.FrameList));
            PortIO.Out8(((ushort)(uhciDev.IOBase + REG_SOFMOD)), 0x40); // Ensure default value of 64 (aka cycle time of 12000)
            
            /**
             * We are going to poll!
             */
            PortIO.Out16((ushort)(uhciDev.IOBase + REG_USBINTR), 0x00);

            /**
             * Clear any pending statusses
             */
            PortIO.Out16((ushort)(uhciDev.IOBase + REG_USBSTS), 0xFFFF);

            /**
             * Enable device
             */
            PortIO.Out16((ushort)(uhciDev.IOBase + REG_USBCMD), USBCMD_RS);

            //TestLogic(uhciDev);

            probe(uhciDev);

            Sharpen.USB.USB.RegisterController(uhciDev);
        }

        private static void TestLogic(UHCIController control)
        {

            var head = GetQueueHead(control);
            var head2 = GetQueueHead(control);
            InsertHead(control, head);
            InsertHead(control, head2);

            PrintQueue(0, head);
            PrintQueue(1, head2);
            
            RemoveHead(control, head2);

            head2 = GetQueueHead(control);
            PrintQueue(0, head);
            PrintQueue(1, head2);

            InsertHead(control, head2);

            PrintQueue(0, head);
            PrintQueue(1, head2);
        }

        public static void PrintQueue(int num, UHCIQueueHead *head)
        {
            Console.Write("QUEUE HEAD (");
            Console.WriteNum(num);
            Console.Write(") , cur = ");
            Console.WriteHex((int)head);
            Console.Write(", Next = ");
            Console.WriteHex((int)head->Next);
            Console.Write(", Prev = ");
            Console.WriteHex((int)head->Previous);
            Console.WriteLine("");
        }

        /// <summary>
        /// Reset port
        /// </summary>
        /// <param name="port">Port num to reset</param>
        private static void resetPort(UHCIController uhciDev, ushort port)
        {
            /**
             * Set reset bit
             */
            setPortBit(uhciDev, port, PORTSC_RESET);

            /**
             * Wait for 60 ms
             */
            Tasking.CurrentTask.CurrentThread.Sleep(0, 60);

            /**
             * Unset reset bit
             */
            unsetPortBit(uhciDev, port, PORTSC_RESET);

            /**
             * Wait for atleast 150ms for link to go up
             */
            for(int i =0; i < 15; i++)
            {
                Tasking.CurrentTask.CurrentThread.Sleep(0, 10);

                ushort status = PortIO.In16((ushort)(uhciDev.IOBase + port));

                /**
                 * Is it even connected?
                 */
                if(((status) & PORTSC_CUR_STAT) == 0)
                    break;

                /**
                 * Status changed?
                 */
                if(((status) & (PORTSC_STAT_CHNG | PORTSC_ENABLE_STAT)) > 0)
                {
                    unsetPortBit(uhciDev, port, PORTSC_STAT_CHNG | PORTSC_ENABLE_STAT);
                    continue;
                }

                /**
                 * Enabled?
                 */
                if((status & PORTSC_CUR_ENABLE) > 0)
                    break;
                

            }
        }
        

        /// <summary>
        /// Set bit on port
        /// </summary>
        /// <param name="port">Port number</param>
        /// <param name="bit">Bit to setr</param>
        private static void setPortBit(UHCIController uhciDev, ushort port, ushort bit)
        {
            ushort status = PortIO.In16((ushort)(uhciDev.IOBase + port));
            status |= bit;
            PortIO.Out16((ushort)(uhciDev.IOBase + port), status);
        }

        /// <summary>
        /// Unset bit on port
        /// </summary>
        /// <param name="port">Port number</param>
        /// <param name="bit">Bit to unset</param>
        private static void unsetPortBit(UHCIController uhciDev, ushort port, ushort bit)
        {
            ushort status = PortIO.In16((ushort)(uhciDev.IOBase + port));
            status &= (ushort)~bit;
            PortIO.Out16((ushort)(uhciDev.IOBase + port), status);
        }

        /// <summary>
        /// Prepare interrupt
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="transfer"></param>
        private static void PrepareInterrupt(USBDevice dev, USBTransfer *transfer)
        {
            UHCIController controller = (UHCIController)dev.Controller;
            
            uint endp = (uint)(dev.EndPointDesc->Address & 0xF);
            
            UHCITransmitDescriptor* td = GetTransmit(controller);
            if(td == null)
            {
                transfer->Success = false;
                transfer->Executed = true;
            }

            UHCITransmitDescriptor* head = td;

            /**
             * Initalize read
             */
            InitTransmit(td, null, dev.Speed, dev.Address, endp, dev.Toggle, TRANS_PACKET_IN, transfer->Length, transfer->Data);


            UHCIQueueHead* qh = GetQueueHead(controller);
            qh->Element = (int)Paging.GetPhysicalFromVirtual(head);
            qh->Head = 0;
            qh->Transfer = transfer;
            qh->Transmit = head;
            
            InsertHead(controller, qh);
        }

        /// <summary>
        /// Control USB Device
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="transfer"></param>
        private static void Control(USBDevice dev, USBTransfer *transfer)
        {
            USBDeviceRequest request = transfer->Request;

#if __UHCI_DIAG

            Console.WriteLine("------ UHCI Control message ---------");
            Console.Write("Request: ");
            Console.WriteHex(request.Request);
            Console.WriteLine("");
            Console.Write("Index: ");
            Console.WriteHex(request.Index);
            Console.WriteLine("");
            Console.Write("Length:");
            Console.WriteHex(request.Length);
            Console.WriteLine("");
            Console.Write("Type:");
            Console.WriteHex(request.Type);
            Console.WriteLine("");
            Console.Write("Value:");
            Console.WriteHex(request.Value);
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------");
#endif

            UHCIController controller = (UHCIController)dev.Controller;

            UHCITransmitDescriptor *td = GetTransmit(controller);

            UHCITransmitDescriptor* head = td;
            UHCITransmitDescriptor* prev = null;

            USBDeviceRequest* a = (USBDeviceRequest *)Heap.Alloc(sizeof(USBDeviceRequest));
            a->Request = request.Request;
            a->Index = request.Index;
            a->Length = request.Length;
            a->Type = request.Type;
            a->Value = request.Value;

            InitTransmit(td, prev, dev.Speed, dev.Address, 0, 0, TRANS_PACKET_SETUP, (uint)sizeof(USBDeviceRequest), (byte *)a);
            prev = td;

            uint packetType = ((request.Type & USBDevice.TYPE_DEVICETOHOST) > 0) ? TRANS_PACKET_IN : TRANS_PACKET_OUT;

            byte* ptr = transfer->Data;
            uint packetSize = transfer->Length;
            uint offset = 0;

            uint toggle = 0;

            uint remaining = packetSize;

            while (remaining > 0)
            {
                td = GetTransmit(controller);
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
            
            td = GetTransmit(controller);
            if (td == null)
                return;

            packetType = ((request.Type & USBDevice.TYPE_DEVICETOHOST) > 0) ? TRANS_PACKET_OUT : TRANS_PACKET_IN;

            toggle = 1;
            InitTransmit(td, prev, dev.Speed, dev.Address, 0, toggle, packetType, 0, null);

            
            UHCIQueueHead *qh = GetQueueHead(controller);
            qh->Element = (int)Paging.GetPhysicalFromVirtual(head);
            qh->Head = 0;
            qh->Transfer = transfer;
            qh->Transmit = head;

            InsertHead(controller, qh);
            WaitForQueueHead(controller, qh);
        }

        public static void WaitForQueueHead(UHCIController controller, UHCIQueueHead *head)
        {

            while (!head->Transfer->Executed)
                ProcessHead(controller, head);
        }

        #region Head allocation

        /// <summary>
        /// Get Queue head item
        /// </summary>
        /// <param name="dev">Device</param>
        /// <returns></returns>
        private static UHCIQueueHead* GetQueueHead(UHCIController dev)
        {
            m_mutex.Lock();
            int i = 0;
            while (i < MAX_HEADS)
            {
                if (!dev.QueueHeadPool[i].Allocated)
                {
                    dev.QueueHeadPool[i].Allocated = true;
                    dev.QueueHeadPool[i].Next = null;
                    dev.QueueHeadPool[i].Previous = null;

                    m_mutex.Unlock();
                    return (UHCIQueueHead*)(((int)dev.QueueHeadPool) + (sizeof(UHCIQueueHead) * i));
                }

                i++;
            }

            m_mutex.Unlock();
            return null;
        }

        /// <summary>
        /// Process Queue Head
        /// </summary>
        /// <param name="device"></param>
        /// <param name="head"></param>
        public static void ProcessHead(UHCIController controller, UHCIQueueHead *head)
        {
            USBTransfer *transfer = head->Transfer;
            
            UHCITransmitDescriptor* td = head->Transmit;


            //Console.WriteLine("Test:");
            //Console.WriteHex((int)head);
            //Console.WriteLine("");
            //Console.WriteHex((int)head->Head);
            //Console.WriteLine("");

            if ((head->Element & ~0xF) == 0)
            {
                transfer->Executed = true;
                transfer->Success = true;
            }
            else
            {
                if((td->Control & TD_CONTROL_NAK) > 0)
                {

                }

                if((td->Control & TD_CONTROL_STALLED) > 0)
                {
                    Console.WriteLine("Stalled");
                    transfer->Executed = true;
                    transfer->Success = false;
                }


                if ((td->Control & TD_CONTROL_BABBLE) > 0)
                {
                    Console.WriteLine("Control Babble error");
                }


                if ((td->Control & TD_CONTROL_CRC) > 0)
                {
                    Console.WriteLine("CRC Timeout");

                    transfer->Executed = true;
                    transfer->Success = false;
                }


                if ((td->Control & TD_CONTROL_BITSTUFF) > 0)
                {
                    Console.WriteLine("Bitstuff error");
                }
            }
            
            if (transfer->Executed)
            {
                if(transfer->ID > 0)
                {
                    PrintQueue(transfer->ID, head);
                }

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
                UHCITransmitDescriptor* tdE = td;

                while(tdE != null)
                {
                    UHCITransmitDescriptor* next = tdE->Next;
                    FreeTransmit(controller, tdE);
                    tdE = next;
                }
            }
        }

        /// <summary>
        /// Insert head
        /// </summary>
        /// <param name="controller">UHCIController</param>
        /// <param name="head"></param>
        public static void InsertHead(UHCIController controller, UHCIQueueHead* head)
        {
            m_mutex.Lock();
            UHCIQueueHead* end = controller.FirstHead;

            while (true)
                if (end->Next != null)
                    end = end->Next;
                else
                    break;


            head->Head = TD_POINTER_TERMINATE;
            head->Previous = end;
            head->Next = null;

            end->Next = head;
            end->Head = (int)Paging.GetPhysicalFromVirtual(head) | TD_POINTER_QH;
            end->Element = TD_POINTER_TERMINATE;

            m_mutex.Unlock();
            //Console.WriteHex((int)controller.FirstHead);
            //Console.WriteLine("");
            //Console.WriteHex((int)end);
            //Console.WriteLine("");
            //Console.WriteHex((int)head);
            //Console.WriteLine("");
        }

        public static void RemoveHead(UHCIController controller, UHCIQueueHead* head)
        {
            m_mutex.Lock();

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
                    head->Previous->Head = TD_POINTER_TERMINATE;
                    head->Previous->Next = null;
                }
            }

            /**
             * Set previous to next
             */
            if (head->Next != null)
            {
                if (head->Previous != null)
                {
                    head->Next->Previous = head->Previous;
                }
                else
                {
                    head->Next->Previous = null;
                }
            }

            FreeHead(controller, head);
            m_mutex.Unlock();
        }

        public static void FreeHead(UHCIController controller, UHCIQueueHead* head)
        {
            head->Allocated = false;
        }

        #endregion

        #region Transmit allocation


        /// <summary>
        /// Get Queue head item
        /// </summary>
        /// <param name="dev">Device</param>
        /// <returns></returns>
        private static UHCITransmitDescriptor* GetTransmit(UHCIController dev)
        {
            int i = 0;
            while (i < MAX_TRANSMIT)
            {
                if (!dev.TransmitPool[i].Allocated)
                {
                    dev.TransmitPool[i].Allocated = true;
                    dev.TransmitPool[i].Next = null;
                    dev.TransmitPool[i].Previous = null;

                    return (UHCITransmitDescriptor*)(((int)dev.TransmitPool) + (sizeof(UHCITransmitDescriptor) * i));
                }

                i++;
            }

            Console.WriteLine("NO TRANSMIT LEFT");

            return null;
        }

        private static void InitTransmit(UHCITransmitDescriptor* td, UHCITransmitDescriptor *previous, 
            USBDeviceSpeed speed, uint address, uint endp, uint toogle, uint type, uint len, byte *data)
        {

            len = (len - 1) & 0x7FFF;

            if (previous != null)
            {
                previous->Link = (int)Paging.GetPhysicalFromVirtual(td) | TD_POINTER_DEPTH;
                previous->Next = td;
            }

            td->Link = TD_POINTER_TERMINATE;
            td->Next = null;

            td->Control = (int)((3 << (int)TD_ERROR_SHIFT) | TD_CONTROL_ACTIVE);
            if (speed == USBDeviceSpeed.LOW_SPEED)
                td->Control |= (int)TD_CONTROL_LOW_SPEED;


            td->Token = (len << TD_TOKEN_MAXLEN) |
                (toogle << TD_TOKEN_D_SHIFT) |
                (endp << TD_TOKEN_ENDP) | 
                (address << TD_TOKEN_ADDR) |
                type;

            td->BufferPointer = (int)Paging.GetPhysicalFromVirtual(data);
        }

        /// <summary>
        /// Insert head
        /// </summary>
        /// <param name="controller">UHCIController</param>
        /// <param name="head"></param>
        public static void InsertTransmit(UHCIController controller, UHCITransmitDescriptor* transmit)
        {
            UHCITransmitDescriptor* end = transmit;

            while (true)
                if (end->Next != null)
                    end = end->Next;
                else
                    break;

            end->Next = transmit;
            end->Link = (int)transmit | 1;
            transmit->Link = 0;
        }

        public static void RemoveTransmit(UHCIController controller, UHCITransmitDescriptor* transmit)
        {
            if (transmit->Previous != null)
            {
                if (transmit->Next != null)
                {
                    transmit->Previous->Link = transmit->Next->Link;
                    transmit->Previous->Next = transmit->Next;
                }
                else
                {
                    transmit->Previous->Link = 0;
                    transmit->Previous->Next = null;
                }
            }

            FreeTransmit(controller, transmit);
        }

        public static void FreeTransmit(UHCIController controller, UHCITransmitDescriptor* transmit)
        {
            transmit->Allocated = false;
        }

        #endregion

        /// <summary>
        /// Poll queue heads
        /// </summary>
        /// <param name="controller"></param>
        public static void Poll(IUSBController controller)
        {
            UHCIController uhciController = (UHCIController)controller;

            for (int i = 0; i < MAX_HEADS; i++)
                if (uhciController.QueueHeadPool[i].Transfer != null)
                {
                    int address = (int)uhciController.QueueHeadPool;
                    address += sizeof(UHCIQueueHead) * i;

                    ProcessHead(uhciController, (UHCIQueueHead*)(address));
                }
        }

        /// <summary>
        /// Probe usb devices on port
        /// </summary>
        /// <param name="uhciDev">The UHCI device</param>
        private static void probe(UHCIController uhciDev)
        {

            /**
             * UHCI only supports 2 ports, so just 2 :-)
             */
            for(int i = 0; i < 2; i++)
            {
                ushort port = (i == 0 )? REG_PORTSC1 : REG_PORTSC2;

                resetPort(uhciDev, port);


                ushort status = PortIO.In16((ushort)(uhciDev.IOBase + port));

                /**
                 * Is the port even connected?
                 */
                if ((status & PORTSC_CUR_STAT) == 0)
                    continue;

                bool lowSpeed = ((status & PORTSC_LOW_SPEED) > 0);
                
                USBDevice dev = new USBDevice();
                dev.Controller = uhciDev;
                dev.Control = Control;
                dev.PrepareInterrupt = PrepareInterrupt;

                /**
                 * Root hub
                 */
                dev.Parent = null;
                dev.Port = port;
                dev.State = USBDeviceState.ATTACHED;
                dev.Speed = (lowSpeed) ? USBDeviceSpeed.LOW_SPEED : USBDeviceSpeed.HIGH_SPEED;

                if (!dev.Init())
                    Heap.Free(dev);
            }
        }
    }
}
