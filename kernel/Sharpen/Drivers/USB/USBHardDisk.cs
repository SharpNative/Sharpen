using Sharpen.Mem;
using Sharpen.USB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.USB
{
    class USBHardDisk : IUSBDriver
    {
        private USBDevice _Device;

        private byte _MuxLun;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct CommandBlockWrapper
        {
            public int Signature { get; set; }

            public int Tag { get; set; }

            public int Length { get; set; }

            public byte Direction { get; set; }

            public byte Lun { get; set; }

            public byte CMDLen { get; set; }

            public fixed byte Data[16];
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct CommandStatusWrapper
        {
            public int Signature { get; set; }

            public int Tag { get; set; }

            public int Residue { get; set; }

            public byte Status { get; set; }
        }

        public static void Init()
        {
            USBHardDisk keyboard = new USBHardDisk();
            USBDrivers.RegisterDriver(keyboard);
        }

        public unsafe bool Init(USBDevice device)
        {
            _Device = device;
            _MuxLun = GetMaxLun();

            return true;
        }

        private unsafe byte GetMaxLun()
        {
            byte* data = (byte*)Heap.Alloc(1);
            
            if (!_Device.Request(USBDevice.TYPE_DEVICETOHOST | USBDevice.TYPE_CLASS | USBDevice.TYPE_INTF, 0xFE, 0, 0, 1, data))
                return 0xFF;
            

            byte ret = *data;
            
            Heap.Free(data);

            return ret;
        }

        public void Close(USBDevice device)
        {

        }

        public unsafe IUSBDriver Load(USBDevice device)
        {
            if (device.InterfaceDesc->Class == (byte)USBClassCodes.MASS_STORAGE && device.InterfaceDesc->SubClass == 0x06)
            {
                USBHardDisk hardDisk = new USBHardDisk();
                hardDisk.Init(device);


                device.Classifier = USBDeviceClassifier.FUNCTION;

                return hardDisk;
            }

            return null;
        }

        public void Poll(USBDevice device)
        {

        }
    }
}
