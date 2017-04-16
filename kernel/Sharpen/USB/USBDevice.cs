using Sharpen.Mem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.USB
{

    public class USBDevice
    {
        /**
         * Transfer types
         */
        const byte TYPE_HOSTODEVICE = 0;
        const byte TYPE_DEVICETOHOST = 1;
        const byte TYPE_STANDARD = 0;
        const byte TYPE_CLASS = 0x20;
        const byte TYPE_VENDOR = 0x40;

        const byte TYPE_DEV = 0x0;
        const byte TYPE_INTF = 0x01;
        const byte TYPE_ENDP = 0x02;
        const byte TYPE_OTHER = 0x03;

        /**
         * Requests
         */
        const byte REQ_GET_STATUS = 0x0;
        const byte REQ_CLEAR_FEATURE = 0x1;
        const byte REQ_SET_FEATURE = 0x3;
        const byte REQ_SET_ADDR = 0x05;
        const byte REQ_GET_DESCRIPTOR = 0x06;
        const byte REQ_SET_DESCRIPTOR = 0x07;
        const byte REQ_GET_CONFIGURATION = 0x08;
        const byte REQ_SET_CONFIGURATION = 0x09;
        const byte REQ_GET_INTERFACE = 0x0A;
        const byte REQ_SET_INTERFACE = 0x0B;
        const byte REQ_SYNC_FRAME = 0x0C;

        /**
         * Descriptors
         */
        const byte DESC_DEVICE = 0x01;
        const byte DESC_CONFIGURATION = 0x02;
        const byte DESC_STRING = 0x03;
        const byte DESC_INTERFACE = 0x04;
        const byte DESC_ENDPOINT = 0x05;
        const byte DESC_DEVICE_QUALIFIER = 0x06;
        const byte DESC_OTHER_SPEED_CONFIGURATION = 0x07;
        const byte DESC_INTERFACE_POWER = 0x08;

        public USBDevice Parent { get; set; }

        public IUSBController Controller { get; set; }

        public USBDeviceState State { get; set; }

        public USBDeviceSpeed Speed { get; set; }

        public ushort Port { get; set; }

        /// <summary>
        /// Control device state
        /// </summary>
        public USBHelpers.DeviceControl Control { get; set; }

        public unsafe bool Request(byte type, byte req, ushort value, ushort index, ushort len, byte *data)
        {
            USBDeviceRequest request = new USBDeviceRequest();
            request.Request = req;
            request.Length = len;
            request.Index = index;
            request.Type = type;
            request.Value = value;

            USBTransfer transfer = new USBTransfer();
            transfer.Request = request;
            transfer.Data = data;
            transfer.Length = len;

            transfer.Success = false;
            transfer.Executed = false;

            Control(this, transfer);

            return transfer.Success;
            
        }

        public unsafe bool Init()
        {
            byte* descriptor = (byte *)Heap.Alloc(8);
            if (!Request(TYPE_DEVICETOHOST, REQ_GET_DESCRIPTOR, (DESC_DEVICE << 8) | 0, 0, 8, descriptor))
                return false;

            return true;
        }
    }

}
