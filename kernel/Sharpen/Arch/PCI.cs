using Sharpen.Power;
using Sharpen.Drivers.Power;
using Sharpen.Utilities;

namespace Sharpen.Arch
{
    public class PciDevice
    {
        public byte Bus;
        public byte Slot;
        public byte Function;

        public byte ClassCode;
        public byte SubClass;
        public byte ProgIntf;
        public int CombinedClass;

        public ushort Vendor;
        public ushort Device;
        public Pci.PciDriver Driver;
        public PciBar BAR0;
        public PciBar BAR1;
        public PciBar BAR2;
        public PciBar BAR3;
        public PciBar BAR4;
        public PciBar BAR5;

        public byte Type;

        public byte IRQPin;
        public byte IRQLine;
    }

    public struct PciBar
    {
        public ulong Size;
        public ulong Address;
        public byte flags;
    }


    public unsafe class Pci
    {
        public const ushort COMMAND = 0x04;

        public const ushort CONFIG_ADR = 0xCF8;
        public const ushort DATA_ADR = 0xCFC;

        public const ushort CONFIG_HEADER_MUTLI_FUNC = 0x80;

        /**
         * Config registers
         */
        public const ushort CONFIG_VENDOR_ID = 0x00;
        public const ushort CONFIG_DEVICE_ID = 0x02;
        public const ushort CONFIG_COMMAND = 0x04;
        public const ushort CONFIG_STATUS = 0x06;
        public const ushort CONFIG_REV_ID = 0x08;
        public const ushort CONFIG_PROG_INTF = 0x09;
        public const ushort CONFIG_SUB_CLASS = 0x0A;
        public const ushort CONFIG_CLASS_CODE = 0x0B;
        public const ushort CONFIG_CACHE_SIZE = 0x0C;
        public const ushort CONFIG_LETENCY = 0x0D;
        public const ushort CONFIG_HEADER_TYPE = 0x0E;
        public const ushort CONFIG_BIST = 0x0F;

        public const ushort IRQLINE = 0x3C;
        public const ushort IRQPIN = 0x3D;

        public const ushort BAR_IO = 0x01;
        public const ushort BAR_LOWMEM = 0x02;
        public const ushort BAR_64 = 0x04;

        public const ushort BAR0 = 0x10;
        public const ushort BAR1 = 0x14;
        public const ushort BAR2 = 0x18;
        public const ushort BAR3 = 0x1C;
        public const ushort BAR4 = 0x20;
        public const ushort BAR5 = 0x24;

        public const int PCI_BUS_DEV_MAX = 32;
        public const int PCI_PINS = 4;

        public struct PciDriver
        {
            public string Name;

            public PciDriverInit Init;
            public PciDriverExit Exit;
        }

        public unsafe delegate void PciDriverInit(PciDevice dev);
        public unsafe delegate void PciDriverExit(PciDevice dev);

        public static PciDevice[] Devices { get; private set; }

        private static uint m_currentdevice = 0;

        // If we wish to support more PCI busses, we should have a structure defining a PCI bus or bridge
        private static PciIRQEntry[] m_irqTable;

        private struct PciIRQEntry
        {
            public uint Irq;
            public uint Flags;
        }

        public static uint DeviceNum
        {
            get { return m_currentdevice; }
        }

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
            return (lbus << 16 | lslot << 11 | lfun << 8 | (offset & 0xFC) | 0x80000000);
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
        public static uint Read(ushort bus, ushort slot, ushort function, ushort offset, uint size)
        {
            uint address = generateAddress(bus, slot, function, offset);

            PortIO.Out32(CONFIG_ADR, address);

            if (size == 4)
                return PortIO.In32(DATA_ADR) & 0xFFFFFFFF;
            else if (size == 2)
                return (uint)PortIO.In16((ushort)(DATA_ADR + (offset & 0x02))) & 0xFFFF;
            else if (size == 1)
                return (uint)PortIO.In8((ushort)(DATA_ADR + (offset & 0x03))) & 0xFF;

            return 0xFFFFFFFF;
        }

        /// <summary>
        /// Read data from PCI
        /// </summary>
        /// <param name="dev">The device</param>
        /// <param name="offset">Offset</param>
        /// <param name="size">The size of the data to read</param>
        /// <returns>The read data</returns>
        public static uint Read(PciDevice dev, ushort offset, uint size)
        {
            return Read(dev.Bus, dev.Slot, dev.Function, offset, size);
        }

        /// <summary>
        /// Write data to PCI
        /// </summary>
        /// <param name="bus">Bus</param>
        /// <param name="slot">Slot</param>
        /// <param name="function">Function</param>
        /// <param name="offset">Offset</param>
        /// <param name="value">The value to write</param>
        /// <param name="size">The size of the data to write</param>
        public static void Write(ushort bus, ushort slot, ushort function, ushort offset, uint value, uint size)
        {
            uint address = generateAddress(bus, slot, function, offset);

            PortIO.Out32(CONFIG_ADR, address);

            if (size == 4)
                PortIO.Out32(DATA_ADR, value);
            else if (size == 2)
                PortIO.Out16((ushort)(DATA_ADR + (offset & 0x02)), (ushort)value);
            else if (size == 1)
                PortIO.Out8((ushort)(DATA_ADR + (offset & 0x03)), (byte)value);
        }

        /// <summary>
        /// Write data to PCI
        /// </summary>
        /// <param name="dev">The PCI device</param>
        /// <param name="offset">Offset</param>
        /// <param name="value">The value to write</param>
        /// <param name="size">The size of the data to write</param>
        public static void Write(PciDevice dev, ushort offset, uint value, uint size)
        {
            Write(dev.Bus, dev.Slot, dev.Function, offset, value, size);
        }
        
        /// <summary>
        /// Sets an interrupt handler for a PCI device
        /// </summary>
        /// <param name="dev">The device</param>
        public static void SetInterruptHandler(PciDevice dev, IRQ.IRQHandler handler)
        {
            // Get IRQ routing data from table
            PciIRQEntry irq = m_irqTable[(dev.Slot * PCI_PINS) + (dev.IRQPin - 1)];
            uint irqNum = irq.Irq;

            // If no routing exists, use the interrupt line
            if (irqNum == 0)
            {
                irqNum = dev.IRQLine;
            }
            
            // First set the IRQ handler to avoid race conditions
            IRQ.SetHandler(irqNum, handler);
            IOApicManager.CreateEntry(irqNum, irqNum, irq.Flags);

            // Info
            Console.Write("[PCI] Set device to use IRQ ");
            Console.WriteNum((int)irqNum);
            Console.Write('\n');
        }

        /// <summary>
        /// Enables bus mastering for a PCI device
        /// </summary>
        /// <param name="dev"></param>
        public static void EnableBusMastering(PciDevice dev)
        {
            ushort cmd = (ushort)Read(dev, COMMAND, 2);
            cmd |= (1 << 2);
            Write(dev.Bus, dev.Slot, dev.Function, COMMAND, cmd, 2);
        }

        /// <summary>
        /// Enables IO space for a PCI device
        /// </summary>
        /// <param name="dev"></param>
        public static void EnableIOSpace(PciDevice dev)
        {
            ushort cmd = (ushort)Read(dev, COMMAND, 2);
            cmd |= (1 << 0);
            Write(dev.Bus, dev.Slot, dev.Function, COMMAND, cmd, 2);
        }

        /// <summary>
        /// Get device ID
        /// </summary>
        /// <param name="bus">Bus</param>
        /// <param name="device">Device</param>
        /// <param name="function">Function</param>
        /// <returns>Device ID</returns>
        public static ushort GetDeviceID(ushort bus, ushort device, ushort function)
        {
            return (ushort)Read(bus, device, function, 0x2, 2);
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
            return (ushort)Read(bus, device, function, 0x0, 2);
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
                if (Devices[i].Vendor == vendorID && Devices[i].Device == deviceID)
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

            Devices[foundIndex].Driver = driver;
            driver.Init(Devices[foundIndex]);
        }
        
        /// <summary>
        /// Print the found devices
        /// </summary>
        public static void PrintDevices()
        {
            for (int i = 0; i < m_currentdevice; i++)
            {
                Console.Write("Device ");
                Console.WriteHex(Devices[i].Vendor);
                Console.Write(":");
                Console.WriteHex(Devices[i].Device);
                Console.Write(":");
                Console.WriteHex(Devices[i].ClassCode);
                Console.Write(":");
                Console.WriteHex(Devices[i].SubClass);
                Console.Write(".");
                Console.WriteHex(Devices[i].ProgIntf);
                Console.WriteLine("");
            }
        }

        /// <summary>
        /// Brute force scan over bus 0
        /// </summary>
        private static unsafe void probe()
        {
            // TODO: this only scans the first 5 busses, we should implement recursive finding with acipca
            //       also: we only support irqs on first bus right now because that depends on recursive finding
            for (byte i = 0; i < 5; i++)
                checkBus(i);

            Console.Write("[PCI] ");
            Console.WriteNum((int)m_currentdevice - 1);
            Console.WriteLine(" devices detected");
        }

        /// <summary>
        /// Adds an IRQ to the table
        /// </summary>
        /// <param name="slot">The slot</param>
        /// <param name="pin">The pin</param>
        /// <param name="irq">The IRQ</param>
        private static void addIRQ(uint slot, uint pin, uint irq, uint polarity, uint trigger)
        {
            if (slot >= PCI_BUS_DEV_MAX || pin > PCI_PINS)
                Panic.DoPanic("[PCI] Invalid IRQ added");
            
            PciIRQEntry entry = new PciIRQEntry();
            entry.Irq = irq;

            entry.Flags = 0;
            if (polarity == 0 || polarity == 3)
                entry.Flags |= IOApic.IOAPIC_REDIR_POLARITY_LOW;
            else
                entry.Flags |= IOApic.IOAPIC_REDIR_POLARITY_HIGH;

            if (trigger == 0 || trigger == 3)
                entry.Flags |= IOApic.IOAPIC_REDIR_TRIGGER_LEVEL;
            else
                entry.Flags |= IOApic.IOAPIC_REDIR_TRIGGER_EDGE;
            
            m_irqTable[(slot * PCI_PINS) + pin] = entry;
        }

        /// <summary>
        /// Handles IRQ resources
        /// </summary>
        /// <param name="Resource">The resource</param>
        /// <param name="Context">User context</param>
        /// <returns>Status</returns>
        private static int onIRQResource(AcpiObjects.Resource* Resource, void* Context)
        {
            Acpica.PCIRoutingTable* table = (Acpica.PCIRoutingTable*)Context;
            uint slot = (uint)(table->Address >> 16);

            if (Resource->Type == AcpiObjects.ResourceType.IRQ)
            {
                AcpiObjects.ResourceIRQ* irq = (AcpiObjects.ResourceIRQ*)((byte*)Resource + sizeof(AcpiObjects.Resource));
                byte* interrupts = (byte*)irq + sizeof(AcpiObjects.ResourceIRQ);
                addIRQ(slot, table->Pin, interrupts[table->SourceIndex], irq->Polarity, irq->Trigger);
            }
            else if (Resource->Type == AcpiObjects.ResourceType.EXTENDED_IRQ)
            {
                AcpiObjects.ResourceExtendedIRQ* irq = (AcpiObjects.ResourceExtendedIRQ*)((byte*)Resource + sizeof(AcpiObjects.Resource));
                uint* interrupts = (uint*)((byte*)irq + sizeof(AcpiObjects.ResourceExtendedIRQ));
                addIRQ(slot, table->Pin, interrupts[table->SourceIndex], irq->Polarity, irq->Trigger);
            }

            return Acpica.AE_OK;
        }

        /// <summary>
        /// Called as callback when a root device is found
        /// </summary>
        /// <param name="Object">The object</param>
        /// <param name="NestingLevel">Nesting level</param>
        /// <param name="Context">User context</param>
        /// <param name="ReturnValue">Return value if early returned</param>
        /// <returns>Status</returns>
        private static int onRootDevice(void* Object, uint NestingLevel, void* Context, void** ReturnValue)
        {
            // Get routing table
            AcpiObjects.Buffer buffer;
            buffer.Length = Acpica.ACPI_ALLOCATE_BUFFER;
            int status = Acpica.AcpiGetIrqRoutingTable(Object, &buffer);
            if (status != Acpica.AE_OK)
            {
                Panic.DoPanic("[PCI] Couldn't get ACPI IRQ Routing Table");
            }

            // The last entry will have Length 0
            Acpica.PCIRoutingTable* table = (Acpica.PCIRoutingTable*)buffer.Pointer;
            while (table->Length > 0)
            {
                // No reference source
                if (table->Source[0] == 0)
                {
                    uint slot = (uint)(table->Address >> 16);
                    addIRQ(slot, table->Pin, table->SourceIndex, 0, 0);
                }
                // Source is not null, that means we need to lookup the reference resource
                else
                {
                    void* handle = null;
                    status = Acpica.AcpiGetHandle(Object, Util.CharPtrToString(table->Source), &handle);
                    if (status != Acpica.AE_OK)
                    {
                        Panic.DoPanic("[PCI] Couldn't load references handle");
                    }

                    status = Acpica.AcpiWalkResources(handle, Acpica.METHOD_NAME__CRS, onIRQResource, table);
                    if (status != Acpica.AE_OK)
                    {
                        Panic.DoPanic("[PCI] Couldn't process resources for IRQ");
                    }
                }

                // Next entry
                table = (Acpica.PCIRoutingTable*)((byte*)table + table->Length);
            }

            // The object returned should be freed by us
            Acpica.AcpiOsFree(buffer.Pointer);

            return Acpica.AE_OK;
        }

        /// <summary>
        /// Sets the IRQ routing
        /// </summary>
        private static void setIRQRouting()
        {
            m_irqTable = new PciIRQEntry[PCI_BUS_DEV_MAX * PCI_PINS];

            // Find root PCI device and handle it
            int status = Acpica.AcpiGetDevices("PNP0A03", onRootDevice, null, null);
            if (status != Acpica.AE_OK)
            {
                Panic.DoPanic("[PCI] Couldn't get root device");
            }
        }

        /// <summary>
        /// Initialize PCI
        /// </summary>
        public static void Init()
        {
            Devices = new PciDevice[300];
            setIRQRouting();
            probe();
            
        }
        
        #region Bus Scanning


        /// <summary>
        /// Check PCI bus
        /// </summary>
        /// <param name="bus">The bus to check</param>
        private static void checkBus(byte bus)
        {
            for (byte device = 0; device < PCI_BUS_DEV_MAX; device++)
            {
                uint headerType = Read(bus, device, 0, CONFIG_HEADER_TYPE, 1);
                uint functionCount = (uint)(((headerType & CONFIG_HEADER_MUTLI_FUNC) > 0) ? 8 : 1);

                for (byte i = 0; i < functionCount; i++)
                    checkDevice(bus, device, i);
            }
        }

        /// <summary>
        /// Gets a bar
        /// </summary>
        /// <param name="bus">The bus</param>
        /// <param name="device">The device</param>
        /// <param name="function">The function</param>
        /// <param name="offset">The field offset</param>
        /// <returns>The bar</returns>
        private static PciBar GetBar(byte bus, byte device, ushort function, ushort offset)
        {
            PciBar ret = new PciBar();

            uint address = Read(bus, device, function, offset, 4);

            Write(bus, device, function, offset, 0xFFFFFFFF, 4);
            uint mask = Read(bus, device, function, offset, 4);

            Write(bus, device, function, offset, address, 4);

            if ((address & BAR_64) > 0)
            {
                /**
                 * We do support this, but we convert it to 32bit address (So we only take the low ones)
                 */

                ret.Address = (ulong)(address & ~0xF);
                ret.Size = (ulong)(~(mask & ~0xF) + 1);
                ret.flags = (byte)(address & 0xF);

                return ret;
            }
            else if ((address & BAR_IO) > 0)
            {
                ret.Address = (ushort)(address & ~0x3);
                ret.Size = (ushort)(~(mask & ~0x3) + 1);
                ret.flags = (byte)(address & 0x3);
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
        /// <param name="function">The function</param>
        private static void checkDevice(byte bus, byte device, byte function)
        {
            ushort vendorID = GetVendorID(bus, device, function);
            if (vendorID == 0xFFFF)
                return;

            ushort deviceID = GetDeviceID(bus, device, function);
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
            dev.BAR2 = GetBar(bus, device, function, BAR2);
            dev.BAR3 = GetBar(bus, device, function, BAR3);
            dev.BAR4 = GetBar(bus, device, function, BAR4);
            dev.BAR5 = GetBar(bus, device, function, BAR5);
            dev.Type = (byte)Read(bus, device, function, CONFIG_HEADER_TYPE, 1);
            dev.ClassCode = (byte)Read(bus, device, function, CONFIG_CLASS_CODE, 1);
            dev.SubClass = (byte)Read(bus, device, function, CONFIG_SUB_CLASS, 1);
            dev.ProgIntf = (byte)Read(bus, device, function, CONFIG_PROG_INTF, 1);
            dev.CombinedClass = dev.ClassCode << 8 | dev.SubClass;
            dev.IRQPin = (byte)Read(bus, device, function, IRQPIN, 1);
            dev.IRQLine = (byte)Read(bus, device, function, IRQLINE, 1);

            Devices[m_currentdevice++] = dev;
        }

        #endregion
    }
}
