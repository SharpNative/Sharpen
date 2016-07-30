using Sharpen.Arch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Net
{
    class I217
    {

        private static ushort m_io_base;

        private static void InitHandler(PCI.PciDevice dev)
        {
            m_io_base = dev.Port1;


        }


        private static void ExitHander(PCI.PciDevice dev)
        {

        }

        public static void Init()
        {
            PCI.PciDriver driver = new PCI.PciDriver();
            driver.Name = "Intel I217 Driver";
            driver.Exit = ExitHander;
            driver.Init = InitHandler;

            PCI.RegisterDriver(0x8086, 0x100E, driver);
        }

    }
}
