using Sharpen.USB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.USB
{
    class USBHardDisk : IUSBDriver
    {
        public static void Init()
        {
            USBHardDisk keyboard = new USBHardDisk();
            USBDrivers.RegisterDriver(keyboard);
        }

        public unsafe bool Init(USBDevice device)
        {
            //device.Request(USBDevice.TYPE_DEVICETOHOST | USBDevice.TYPE_STANDARD, )
            

            return true;
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
