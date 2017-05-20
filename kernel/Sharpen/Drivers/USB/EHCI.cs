using Sharpen.Arch;
using Sharpen.Synchronisation;
using Sharpen.USB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.USB
{

    public unsafe class EHCIController : IUSBController
    {

        public USBControllerType Type { get { return USBControllerType.EHCI; } }

        public USBHelpers.ControllerPoll Poll { get; set; }

        public ushort IOBase { get; set; }

        public int* FrameList { get; set; }

        public UHCIQueueHead* QueueHeadPool { get; set; }

        public UHCITransmitDescriptor* TransmitPool { get; set; }

        public UHCIQueueHead* FirstHead { get; set; }
    }

    public class EHCI
    {
        const ushort INTF_EHCI = 0x20;

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

        private static void initDevice(PciDevice dev)
        {

        }
}
