using Sharpen.Arch;
using Sharpen.Mem;
using Sharpen.MultiTasking;
using Sharpen.USB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.USB
{
    unsafe class USBHub : IUSBDriver
    {
        const ushort USB_HUB_DESCRIPTOR_TYPE = 0x29;

        /**
         * USB Clear features
         */
        const ushort USB_HUB_FEATURE_C_HUB_LOCAL_POWER = 0x00;
        const ushort USB_HUB_FEATURE_C_HUB_OVER_CURRENT = 0x01;

        /**
         * USB features
         */
        const byte USB_HUB_FEATURE_CONNECTION = 0x00;
        const byte USB_HUB_FEATURE_ENABLE = 0x01;
        const byte USB_HUB_FEATURE_SUSPEND = 0x02;
        const byte USB_HUB_FEATURE_OVER_CURRENT = 0x03;
        const byte USB_HUB_FEATURE_RESET = 0x04;
        const byte USB_HUB_FEATURE_POWER = 0x08;
        const byte USB_HUB_FEATURE_LOW_SPEED = 0x09;


        /**
         * Descriptor chars
         */
        const ushort USB_HUB_POWER_MASK = 0x03;
        const ushort USB_HUB_POWER_GLOBAL = 0x00;
        const ushort USB_HUB_POWER_PER_DEVICE = 0x01;

        /**
         * Status bitmasks
         */
        const ushort USB_HUB_PORT_CONNECTION = 0x01;
        const ushort USB_HUB_PORT_ENABLE = 0x02;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct USBHubDescriptor
        {
            public byte Length;
            public byte Type;
            public byte NumPorts;
            public ushort Characteristics;
            public byte PowerTime;
            public byte ControlConcurrent;
            public byte DeviceRemovable;
            public byte PWRMask;
        }

        private USBHubDescriptor *descriptor;

        public static void Init()
        {
            USBHub hub = new USBHub();
            USBDrivers.RegisterDriver(hub);
        }

        public void Close(USBDevice device)
        {

        }

        private static void Sleep(int cnt)
        {
            // Wait 10 ms
            for (int i = 0; i < cnt; i++)
                PortIO.In32(0x80);
        }

        private bool ResetPort(USBDevice device, int port)
        {
            /**
             * Reset port
             */
            if (!device.Request(USBDevice.TYPE_HOSTTODEVICE | USBDevice.TYPE_CLASS | USBDevice.TYPE_OTHER,
                USBDevice.REQ_SET_FEATURE, USB_HUB_FEATURE_RESET, (ushort)(port + 1), 0, null))
                return false;

            /**
             * Wait a maximum of 150ms for port to reset
             */
            for (int i = 0; i < 15; i++)
            {
                Tasking.CurrentTask.CurrentThread.Sleep(0, 10000);


                uint status = ReadStatus(device, port);

                if (status == 0)
                    continue;
                
                if ((status & USB_HUB_PORT_CONNECTION) > 0)
                    return true;


                if ((status & USB_HUB_PORT_ENABLE) > 0)
                    return true;
            }
            
            return false;
        }

        /// <summary>
        /// Read status of port
        /// </summary>
        /// <param name="device">USB device</param>
        /// <param name="port">Port number</param>
        /// <returns></returns>
        private uint ReadStatus(USBDevice device, int port)
        {
            uint* status = (uint*)Heap.Alloc(sizeof(uint));

            if(!device.Request(USBDevice.TYPE_DEVICETOHOST | USBDevice.TYPE_CLASS | USBDevice.TYPE_OTHER, USBDevice.REQ_GET_STATUS, 0, (ushort)(port + 1), 1, (byte *)status))
            {
                Heap.Free(status);

                return 0;
            }

            uint ret = *status;

            Heap.Free(status);

            return ret;
        }

        private unsafe void Probe(USBDevice device)
        {
            int numPorts = descriptor->NumPorts;
            
            /**
             * Do we need to power ports manually? then enable power on all devices
             */
            if((descriptor->Characteristics & USB_HUB_POWER_MASK) == USB_HUB_POWER_PER_DEVICE)
            {

                for(int i = 0; i < numPorts; i++)
                {
                    if (!device.Request(USBDevice.TYPE_HOSTTODEVICE | USBDevice.TYPE_CLASS | USBDevice.TYPE_OTHER, USBDevice.REQ_SET_FEATURE, USB_HUB_FEATURE_POWER, (ushort)(i + 1), 0, null))
                        continue;

                    // Note: this does wait powertime * 10 ms
                    Tasking.CurrentTask.CurrentThread.Sleep(0, (uint)descriptor->PowerTime * 10000);
                }
            }


            /**
             * We can now reset port and wait
             */
            for(int i = 0; i < numPorts; i++)
            {

                if (!ResetPort(device, i))
                    continue;
                
                uint status = ReadStatus(device, i);
                

                if((status & USB_HUB_PORT_ENABLE) > 0)
                {
                    /**
                     * Port enable, we can init here now!
                     */
                    USBDevice newDevice = new USBDevice();

                    newDevice.Controller = device.Controller;
                    newDevice.Control = device.Control;
                    newDevice.PrepareInterrupt = device.PrepareInterrupt;

                    newDevice.Port = (ushort)i;

                    newDevice.Init();
                }
            }
        }

        public unsafe bool Init(USBDevice device)
        {

            /**
             * Loaad descriptor
             */
            descriptor = (USBHubDescriptor*)Heap.Alloc(sizeof(USBHubDescriptor));
            
            if (!device.Request(USBDevice.TYPE_DEVICETOHOST | USBDevice.TYPE_CLASS | USBDevice.TYPE_DEV, USBDevice.REQ_GET_DESCRIPTOR, (USB_HUB_DESCRIPTOR_TYPE << 8) | 0, 0, (ushort)sizeof(USBHubDescriptor), (byte *)descriptor))
                return false;
            

            Probe(device);

            return true;
        }

        public unsafe IUSBDriver Load(USBDevice device)
        {
            if(device.InterfaceDesc->Class == (byte)USBClassCodes.HUB)
            {
                USBHub hub = new USBHub();
                hub.Init(device);

                device.Classifier = USBDeviceClassifier.HUB;
                
                return hub;
            }

            return null;
        }

        public void Poll(USBDevice device)
        {

        }
    }
}
