using Sharpen.Collections;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.USB
{
    class USBDrivers
    {
        private static List mDriverList;

        /// <summary>
        /// Init driver list
        /// </summary>
        public static void Init()
        {
            mDriverList = new List();
        }

        /// <summary>
        /// Register driver
        /// </summary>
        /// <param name="driver"></param>
        public static void RegisterDriver(IUSBDriver driver)
        {
            mDriverList.Add(driver);
        }

        /// <summary>
        /// Load driver
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static unsafe IUSBDriver LoadDriver(USBDevice device)
        {
            for (int i = 0; i < mDriverList.Count; i++)
            {
                IUSBDriver driver = (IUSBDriver)mDriverList.Item[i];
                
                IUSBDriver loadedDriver = driver.Load(device);
                
                if (loadedDriver != null)
                    return loadedDriver;
            }

            return null;
        }
    }
}
