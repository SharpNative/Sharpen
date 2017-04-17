using Sharpen.USB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.USB
{
    class USBHIDMouse : IUSBDriver
    {
        public static void Init()
        {
            USBHIDMouse mouse = new USBHIDMouse();
            USBDrivers.RegisterDriver(mouse);
        }

        public void Close(USBDevice device)
        {

        }

        public unsafe IUSBDriver Load(USBDevice device)
        {
            USBInterfaceDescriptor* desc = device.InterfaceDesc;
            if (!(desc->Class == (byte)USBClassCodes.HID &&
                desc->SubClass == 0x01 &&
                desc->Protocol == 0x02))
                return null;

            Console.WriteLine("Loading mouse ;)");

            return null;
        }

        public void Poll(USBDevice device)
        {

        }
    }
}
