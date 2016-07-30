using Sharpen.Arch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Sound
{
    class AC97
    {

        private static void InitHandler(PCI.PciDevice dev)
        {

        }

        private static void ExitHander(PCI.PciDevice dev)
        {

        }

        public static void Init()
        {
            PCI.PciDriver driver = new PCI.PciDriver();
            driver.Name = "AC97 Driver";
            driver.Exit = ExitHander;
            driver.Init = InitHandler;

            PCI.RegisterDriver(0x8086, 0x2415, driver);
        }
    }
}
