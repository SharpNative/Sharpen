using Sharpen.Arch;

namespace Sharpen.Drivers.Net
{
    class I217
    {
        private static ushort m_io_base;

        /// <summary>
        /// Initialization handler
        /// </summary>
        /// <param name="dev">This PCI device</param>
        private static void InitHandler(PCI.PciDevice dev)
        {
            m_io_base = dev.Port1;
        }

        /// <summary>
        /// Exit handler
        /// </summary>
        /// <param name="dev">This PCI device</param>
        private static void ExitHander(PCI.PciDevice dev)
        {
        }

        /// <summary>
        /// Initializes
        /// </summary>
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
