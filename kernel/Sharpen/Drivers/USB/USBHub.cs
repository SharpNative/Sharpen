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
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct USBHubDescriptor
        {
            public byte Length;
            public byte Type;
            public byte NumPorts;
            public ushort Characterstics;
            public byte PWR;
            public byte ControlConcurrent;
            public byte DeviceRemovable;
            public byte PWRMask;
        }

        private USBHubDescriptor *descriptor;

        public void Close(USBDevice device)
        {

        }

        public unsafe void Init(USBDevice device)
        {

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
