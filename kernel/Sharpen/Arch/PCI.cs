using Sharpen.FileSystem;

namespace Sharpen.Arch
{
    public struct PciBar
    {
        public ulong Size;
        public ulong Address;
        public byte flags;
    }


    unsafe class PCI
    {
        public const ushort COMMAND = 0x04;

        public const ushort CONFIG_ADR = 0xCF8;
        public const ushort DATA_ADR = 0xCFC;


        public const ushort CONFIG_HEADER_MUTLI_FUNC = 0x80;

        /**
         * Config registers
         */
        public const ushort CONFIG_VENDOR_ID   = 0x00;
        public const ushort CONFIG_DEVICE_ID   = 0x02;
        public const ushort CONFIG_COMMAND     = 0x04;
        public const ushort CONFIG_STATUS      = 0x06;
        public const ushort CONFIG_REV_ID      = 0x08;
        public const ushort CONFIG_PROG_INTF   = 0x09;
        public const ushort CONFIG_SUB_CLASS   = 0x0A;
        public const ushort CONFIG_CLASS_CODE  = 0x0B;
        public const ushort CONFIG_CACHE_SIZE  = 0x0C;
        public const ushort CONFIG_LETENCY     = 0x0D;
        public const ushort CONFIG_HEADER_TYPE = 0x0E;
        public const ushort CONFIG_BIST        = 0x0F;

        public const ushort BAR_IO        = 0x01;
        public const ushort BAR_LOWMEM    = 0x02;
        public const ushort BAR_64        = 0x04;

        public const ushort BAR0 = 0x10;
        public const ushort BAR1 = 0x14;
        public const ushort BAR2 = 0x18;
        public const ushort BAR3 = 0x1C;
        public const ushort BAR4 = 0x20;
        public const ushort BAR5 = 0x24;

        public struct PciDriver
        {
            public string Name;

            public PciDriverInit Init;
            public PciDriverExit Exit;
        }

        public struct PciDevice
        {
            public ushort Bus;
            public ushort Slot;
            public ushort Function;

            public byte classCode;
            public byte SubClass; 

            public ushort Vendor;
            public ushort Device;
            public PciDriver Driver;
            public PciBar BAR0;
            public PciBar BAR1;

            public byte Type;
        }

        public unsafe delegate void PciDriverInit(PciDevice dev);
        public unsafe delegate void PciDriverExit(PciDevice dev);

        private static PciDevice[] m_devices;
        private static uint m_currentdevice = 0;

        /// <summary>
        /// Generates a PCI address
        /// </summary>
        /// <param name="lbus">Bus</param>
        /// <param name="lslot">Slot</param>
        /// <param name="lfun">Function</param>
        /// <param name="offset">Offset</param>
        /// <returns>Generates the address</returns>
        private static uint generateAddress(uint lbus, uint lslot, uint lfun, uint offset)
        {
            return lbus << 16 | lslot << 11 | lfun << 8 | (offset & 0xFC) | 0x80000000;
        }

        /// <summary>
        /// Read word from PCI
        /// </summary>
        /// <param name="bus">Bus</param>
        /// <param name="slot">Slot</param>
        /// <param name="function">Function</param>
        /// <param name="offset">Offset</param>
        /// <returns>Word value</returns>
        public static ushort ReadWord(ushort bus, ushort slot, ushort function, ushort offset)
        {
            return (ushort)PCIRead(bus, slot, function, offset, 2);
        }

        /// <summary>
        /// Read data from PCI
        /// </summary>
        /// <param name="bus">Bus</param>
        /// <param name="slot">Slot</param>
        /// <param name="function">Function</param>
        /// <param name="offset">Offset</param>
        /// <param name="size">The size of the data to read</param>
        /// <returns>The read data</returns>
        public static uint PCIRead(ushort bus, ushort slot, ushort function, ushort offset, uint size)
        {
            uint address = generateAddress(bus, slot, function, offset);

            PortIO.Out32(CONFIG_ADR, address);

            if (size == 4)
                return PortIO.In32(DATA_ADR);
            else if (size == 2)
                return PortIO.In16((ushort)(DATA_ADR + (offset & 0x02)));
            else if (size == 1)
                return PortIO.In8((ushort)(DATA_ADR + (offset & 0x03)));

            return 0xFFFFFFFF;
        }

        /// <summary>
        /// Write data to PCI
        /// </summary>
        /// <param name="bus">Bus</param>
        /// <param name="slot">Slot</param>
        /// <param name="function">Function</param>
        /// <param name="offset">Offset</param>
        /// <param name="value">The value to write</param>
        public static void PCIWrite(ushort bus, ushort slot, ushort function, ushort offset, uint value)
        {
            uint address = generateAddress(bus, slot, function, offset);

            PortIO.Out32(CONFIG_ADR, address);
            PortIO.Out32(DATA_ADR, value);
        }

        /// <summary>
        /// Write data to PCI
        /// </summary>
        /// <param name="dev">The PCI device</param>
        /// <param name="offset">Offset</param>
        /// <param name="value">The value to write</param>
        public static void PCIWrite(PciDevice dev, ushort offset, uint value)
        {
            PCIWrite(dev.Bus, dev.Slot, dev.Function, offset, value);
        }

        /// <summary>
        /// Read data from PCI
        /// </summary>
        /// <param name="dev">The PCI device</param>
        /// <param name="offset">Offset</param>
        /// <returns>The read data</returns>
        public static ushort PCIReadWord(PciDevice dev, ushort offset)
        {
            return ReadWord(dev.Bus, dev.Slot, dev.Function, offset);
        }

        /// <summary>
        /// Get device ID
        /// </summary>
        /// <param name="bus">Bus</param>
        /// <param name="device">Device</param>
        /// <param name="function">Function</param>
        /// <returns>Device ID</returns>
        public static ushort getDeviceID(ushort bus, ushort device, ushort function)
        {
            return ReadWord(bus, device, function, 0x2);
        }

        /// <summary>
        /// Get vendor ID
        /// </summary>
        /// <param name="bus">Bus</param>
        /// <param name="device">Device</param>
        /// <param name="function">Function</param>
        /// <returns>Vendor ID</returns>
        private static ushort GetVendorID(ushort bus, ushort device, ushort function)
        {
             return ReadWord(bus, device, function, 0);
        }


        public static PciDevice[] GetDevices()
        {
            return m_devices;
        }

        /// <summary>
        /// Register driver
        /// </summary>
        /// <param name="vendorID">VendorID</param>
        /// <param name="deviceID">Bus ID</param>
        /// <param name="driver">Driver</param>
        public static void RegisterDriver(ushort vendorID, ushort deviceID, PciDriver driver)
        {
            int foundIndex = -1;

            for (int i = 0; i < m_currentdevice; i++)
            {
                if (m_devices[i].Vendor == vendorID && m_devices[i].Device == deviceID)
                {
                    foundIndex = i;
                    break;
                }
            }

            if (foundIndex == -1)
                return;
            

            Console.Write("[PCI] Registered driver for ");
            Console.WriteHex(vendorID);
            Console.Write(":");
            Console.WriteHex(deviceID);
            Console.Write(" Name: ");
            Console.WriteLine(driver.Name);

            m_devices[foundIndex].Driver = driver;
            driver.Init(m_devices[foundIndex]);
        }

        /// <summary>
        /// Print the found devices
        /// </summary>
        public static void PrintDevices()
        {
            for (int i = 0; i < m_currentdevice; i++)
            {
                Console.Write("Device ");
                Console.WriteHex(m_devices[i].Vendor);
                Console.Write(":");
                Console.WriteHex(m_devices[i].Device);
                Console.WriteLine("");
            }
        }

        /// <summary>
        /// Brute force scan over bus 0
        /// </summary>
        public static unsafe void Probe()
        {
            checkBus(0);

            Console.Write("[PCI] ");
            Console.WriteNum((int)m_currentdevice - 1);
            Console.WriteLine(" devices detected");
        }
        

        private static Node findDirImpl(Node node, string name)
        {
            return null;
        }

        /// <summary>
        /// Initialize PCI
        /// </summary>
        public static void Init()
        {
            m_devices = new PciDevice[300];
            Probe();
        }

        public static uint PciGetMask(ushort bus, ushort slot, ushort function, uint index)
        {
            ushort reg = (ushort)(BAR0 + (index * sizeof(uint)));

            PCIWrite(bus, slot, function, reg, 0xffffffff);

            return PCIRead(bus, slot, function, reg, 4);
        }

        #region Bus Scanning


        /// <summary>
        /// Check PCI bus
        /// </summary>
        /// <param name="bus">The bus to check</param>
        private static void checkBus(byte bus)
        {
            for (byte device = 0; device < 32; device++)
            {
                uint headerType = PCIRead(bus, device, 0, CONFIG_HEADER_TYPE, 1);
                uint functionCount = (uint)(((headerType & CONFIG_HEADER_MUTLI_FUNC) > 0) ? 8 : 1);

                for (ushort i = 0; i < functionCount; i++)
                    checkDevice(bus, device, i);
            }
        }

        private static PciBar GetBar(byte bus, byte device, ushort function, ushort offset)
        {
            PciBar ret = new PciBar();

            uint address = PCIRead(bus, device, function, offset, 4);

            PCIWrite(bus, device, function, offset, 0xffffffff);
            uint mask = PCIRead(bus, device, function, offset, 4);

            PCIWrite(bus, device, function, offset, address);

            if ((address & BAR_64) > 0)
            {

                /**
                 * We don't support this yet
                 */

                return ret;
            }
            else if ((address & BAR_IO) > 0)
            {

                ret.Address = (ushort)(address & ~0x3);
                ret.Size    = (ushort)(~(mask & ~0x3) + 1);
                ret.flags   = (byte)(address & 0x3);
            }
            else
            {

                ret.Address = (ulong)(address & ~0xF);
                ret.Size = (ulong)(~(mask & ~0xF) + 1);
                ret.flags = (byte)(address & 0xF);
            }

            return ret;
        }

        /// <summary>
        /// Checks a device
        /// </summary>
        /// <param name="bus">The bus</param>
        /// <param name="device">The device</param>
        private static void checkDevice(byte bus, byte device, ushort function)
        {
            ushort vendorID = GetVendorID(bus, device, function);
            if (vendorID == 0xFFFF)
                return;

            ushort deviceID = getDeviceID(bus, device, function);
            if (deviceID == 0xFFFF)
                return;

            PciDevice dev = new PciDevice();
            dev.Device = deviceID;
            dev.Function = function;
            dev.Bus = bus;
            dev.Slot = device;

            dev.Vendor = vendorID;
            dev.BAR0 = GetBar(bus, device, function, BAR0);
            dev.BAR1 = GetBar(bus, device, function, BAR1);
            dev.Type = (byte)PCIRead(bus, device, function, CONFIG_HEADER_TYPE, 1);
            dev.classCode = (byte)PCIRead(bus, device, function, CONFIG_CLASS_CODE, 1);
            dev.SubClass = (byte)PCIRead(bus, device, function, CONFIG_SUB_CLASS, 1);
            
            m_devices[m_currentdevice++] = dev;
        }

        #endregion
    }
}
