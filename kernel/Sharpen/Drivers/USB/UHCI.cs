#define __UHCI_DIAG
using Sharpen.Arch;
using Sharpen.Mem;
using Sharpen.USB;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.USB
{
    public struct UHCITransmitDescriptor
    {
        public int Link;
        public int Control;
        public int Token;
        public int BufferPointer;


        public bool Allocated { get; set; }
    }

    public unsafe struct UHCIQueueHead
    {
        public int Head;
        public int Element;

        public bool Allocated { get; set; }

        public USBTransfer *Transfer { get; set; }

        public UHCIQueueHead *Previous { get; set; }
        public UHCIQueueHead *Next { get; set; }
    }

    public unsafe class UHCIController : IUSBController
    {
        public USBHelpers.ControllerPoll Poll { get; set; }

        public USBControllerType Type { get { return USBControllerType.UHCI; } }

        public ushort IOBase { get; set; }

        public int* FrameList { get; set; }

        public UHCIQueueHead *QueueHeadPool { get; set; }

        public UHCITransmitDescriptor *TransmitPool { get; set; }
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
        


        /// <summary>
        /// 
        /// </summary>
        public static unsafe void Init()
        {
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

            UHCIController uhciDev = new UHCIController();
            uhciDev.IOBase = (ushort)dev.BAR4.Address;

            Console.Write("[UHCI] Initalize at 0x");
            Console.WriteHex(uhciDev.IOBase);
            Console.WriteLine("");

            uhciDev.FrameList = (int*)Heap.AlignedAlloc(0x1000, sizeof(int) * 1024);
            uhciDev.QueueHeadPool = (UHCIQueueHead *)Heap.AlignedAlloc(0x1000, sizeof(UHCIQueueHead) * MAX_HEADS);
            uhciDev.TransmitPool = (UHCITransmitDescriptor*)Heap.AlignedAlloc(0x1000, sizeof(UHCITransmitDescriptor) * MAX_TRANSMIT);



            UHCIQueueHead* head = GetQueueHead(uhciDev);
            head->Head = 0;
            head->Element = 0;

            for (int i = 0; i < 1024; i++)
                uhciDev.FrameList[i] = (1 << 1) | (int)head;

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

            Sharpen.USB.USB.RegisterController(uhciDev);

            probe(uhciDev);
            uhciDev.Poll = Poll;
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
            Sleep(60);

            /**
             * Unset reset bit
             */
            unsetPortBit(uhciDev, port, PORTSC_RESET);

            /**
             * Wait for atleast 150ms for link to go up
             */
            for(int i =0; i < 15; i++)
            {
                Sleep(10);

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
        /// Sleep for X ms
        /// </summary>
        /// <param name="cnt"></param>
        private static void Sleep(int cnt)
        {
            for (int i = 0; i < cnt; i++)
                PortIO.In32(0x80);
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
        /// Control USB Device
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="transfer"></param>
        private static void Control(USBDevice dev, USBTransfer transfer)
        {
#if __UHCI_DIAG
            USBDeviceRequest request = transfer.Request;

            Console.Write("------transfer---------");
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
#endif
            
        }

        #region Head allocation

        /// <summary>
        /// Get Queue head item
        /// </summary>
        /// <param name="dev">Device</param>
        /// <returns></returns>
        private static UHCIQueueHead* GetQueueHead(UHCIController dev)
        {
            int i = 0;
            while (i < MAX_HEADS)
            {
                if (!dev.QueueHeadPool[i].Allocated)
                {
                    dev.QueueHeadPool[i].Allocated = true;

                    return (UHCIQueueHead*)(((int)dev.QueueHeadPool) + (sizeof(UHCIQueueHead) * i));
                }
            }

            return null;
        }

        /// <summary>
        /// Process Queue Head
        /// </summary>
        /// <param name="device"></param>
        /// <param name="head"></param>
        public static void ProcessHead(UHCIController controller, UHCIQueueHead *head)
        {

        }

        /// <summary>
        /// Insert head
        /// </summary>
        /// <param name="controller">UHCIController</param>
        /// <param name="head"></param>
        public static void InsertHead(UHCIController controller, UHCIQueueHead* head)
        {
            UHCIQueueHead* end = head;

            while (true)
                if (end->Next != null)
                    end = end->Next;
                else
                    break;

            end->Next = head;
            end->Head = (int)head | 1;
            head->Head = 0;
        }

        public static void RemoveHead(UHCIController controller, UHCIQueueHead* head)
        {
            if (head->Previous != null)
            {
                if (head->Next != null)
                {
                    head->Previous->Head = head->Next->Head;
                    head->Previous->Next = head->Next;
                }
                else
                {
                    head->Previous->Head = 0;
                    head->Previous->Next = null;
                }
            }

            FreeHead(controller, head);
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

                    return (UHCITransmitDescriptor*)(((int)dev.TransmitPool) + (sizeof(UHCITransmitDescriptor) * i));
                }
            }

            return null;
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
                    ProcessHead(uhciController, (UHCIQueueHead *)((int)uhciController.QueueHeadPool) + (sizeof(UHCIQueueHead) * i));
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

                /**
                 * TODO: Handle connected device!
                 */

                USBDevice dev = new USBDevice();
                dev.Controller = uhciDev;

                /**
                 * Root hub
                 */
                dev.Parent = null;
                dev.Port = port;
                dev.State = USBDeviceState.ATTACHED;
                dev.Speed = (lowSpeed) ? USBDeviceSpeed.LOW_SPEED : USBDeviceSpeed.HIGH_SPEED;

                dev.Init();
            }
        }
    }
}
