using Sharpen.FileSystem.Cookie;
using Sharpen.USB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.USB
{
    class USBMSCCookie: ICookie
    {

        public USBDevice Device;

        public USBMSC USBMSC;

        /// <summary>
        /// Creates a new ID cookie
        /// </summary>
        /// <param name="device">The ID</param>
        public USBMSCCookie(USBDevice device, USBMSC Usbmsc)
        {
            Device = device;
            USBMSC = Usbmsc;
        }

        /// <summary>
        /// Cleans up
        /// </summary>
        public void Dispose()
        {
        }
    }
}
