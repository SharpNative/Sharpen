using Sharpen.Arch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Sound
{
    class IntelHD
    {
        /// <summary>
        /// The initialization handler
        /// </summary>
        /// <param name="dev">This PCI device</param>
        private static void initHandler(PciDevice dev)
        {
        }

        /// <summary>
        /// The exit handler
        /// </summary>
        /// <param name="dev">This PCI device</param>
        private static void exitHander(PciDevice dev)
        {

        }
        
        /// <summary>
        /// Registers the Intel HD driver
        /// </summary>
        public static void Init()
        {
            Pci.PciDriver driver = new Pci.PciDriver();
            driver.Name = "Intel HD Driver";
            driver.Exit = exitHander;
            driver.Init = initHandler;

            Pci.RegisterDriver(0x8086, 0x2668, driver);
        }
    }
}
