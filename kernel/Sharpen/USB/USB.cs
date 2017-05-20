using Sharpen.Collections;
using Sharpen.MultiTasking;
using Sharpen.Utilities;
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
        
        public unsafe static void Init()
        {
            Controllers = new List();
            Devices = new List();

            Thread pollThread = new Thread();
            pollThread.Context.CreateNewContext(Util.MethodToPtr(Poll), 0, null, true);
            Tasking.KernelTask.AddThread(pollThread);

        }

        public static void RegisterController(IUSBController controller)
        {
            Controllers.Add(controller);
        }

        public static void RegisterDevice(USBDevice device)
        {
            Devices.Add(device);
        }


        public static unsafe void Poll()
        {
            while(true)
            {
                for (int i = 0; i < Controllers.Count; i++)
                {
                    IUSBController controller = (IUSBController)Controllers.Item[i];
                    
                    //tada
                    controller?.Poll(controller);
                }

                for (int i = 0; i < Devices.Count; i++)
                {
                    USBDevice device = (USBDevice)Devices.Item[i];
                    
                    if (device.State == USBDeviceState.CONFIGURED)
                    {
                        device.Driver?.Poll(device);
                    }
                }
                
                MultiTasking.Tasking.CurrentTask.CurrentThread.Sleep(0, 100);
            }
        }
    }
}
