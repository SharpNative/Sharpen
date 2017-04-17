using Sharpen.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.USB
{
    public class USB
    {
        const ushort USB_LOW_SPEED = 0;
        const ushort USB_HIGH_SPEED = 1;


        private static List Controllers;
        private static List Devices;

        public static void Init()
        {
            Controllers = new List();
            Devices = new List();
        }

        public static void RegisterController(IUSBController controller)
        {
            Controllers.Add(controller);
        }

        public static void RegisterDevice(USBDevice device)
        {
            Devices.Add(device);
        }


        public static void Poll()
        {
            for (int i = 0; i < Controllers.Count; i++)
            {
                IUSBController controller = (IUSBController)Controllers.Item[i];

                controller?.Poll(controller);
            }

            for (int i = 0; i < Devices.Count; i++)
            {
                USBDevice device = (USBDevice)Devices.Item[i];

                if (device.State == USBDeviceState.CONFIGURED)
                    device.Driver?.Poll(device);
            }
        }
    }
}
