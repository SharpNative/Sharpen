using Sharpen.Arch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Net
{
    class rtl8139
    {
        private static readonly ushort CONFIG_1 = 0x52;

        private static ushort m_io_base;

        /// <summary>
        /// Initialization handler
        /// </summary>
        /// <param name="dev">This PCI device</param>
        private static void initHandler(PCI.PciDevice dev)
        {
            m_io_base = dev.Port1;

            PortIO.Out8((ushort)(m_io_base + CONFIG_1), 0x00);


        }

        /// <summary>
        /// Exit handler
        /// </summary>
        /// <param name="dev">This PCI device</param>
        private static void exitHandler(PCI.PciDevice dev)
        {
        }

        /// <summary>
        /// Initializes
        /// </summary>
        public static void Init()
        {
            PCI.PciDriver driver = new PCI.PciDriver();
            driver.Name = "RTL8139 Driver";
            driver.Exit = exitHandler;
            driver.Init = initHandler;

            PCI.RegisterDriver(0x10EC, 0x8139, driver);
        }
    }
}
