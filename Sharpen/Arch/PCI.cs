using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Arch
{
    class PCI
    {
        public static readonly ushort COMMAND = 0x04;

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

            public ushort Vendor;
            public ushort Device;
            public PciDriver Driver;
            public ushort Port1;
            public ushort Port2;
        }
        
        public unsafe delegate void PciDriverInit(PciDevice dev);
        public unsafe delegate void PciDriverExit(PciDevice dev);

        private static PciDevice[] m_devices = new PciDevice[300];
        private static uint m_currentdevice = 0;

        private static uint GenerateAddress(uint lbus, uint lslot, uint lfun, uint offset)
        {
            return lbus << 16 | lslot << 11 | lfun << 8 | (uint)(offset & 0xFC) | 0x80000000;
        }

        /// <summary>
        /// Read word from PCI
        /// </summary>
        /// <param name="bus">bus</param>
        /// <param name="slot">Slot</param>
        /// <param name="function">Function</param>
        /// <param name="offset">Offset</param>
        /// <returns>Word value</returns>
        private static ushort readWord(ushort bus, ushort slot, ushort function, ushort offset)
        {
            return (ushort)PciRead(bus, slot, function, offset, 2);
        }

        public static uint PciRead(ushort bus, ushort slot, ushort function, ushort offset, uint size)
        {
            uint address;
            
            address = GenerateAddress(bus, slot, function, offset);

            uint tmp = 0xFFFFFFFF;

            PortIO.Out32(0xCF8, address);

            if (size == 4)
                tmp = PortIO.In32(0xCFC);
            else if (size == 2)
                tmp = (ushort)((PortIO.In32(0xCFC) >> ((offset & 2) * 8)) & 0xffff);
            else if(size == 1)
                return (byte)((PortIO.In32(0xCFC) >> ((offset & 4) * 8)) & 0xff);

            return tmp;
        }

        public static void PciWrite(ushort bus, ushort slot, ushort function, ushort offset, uint value)
        {
            uint address = GenerateAddress(bus, slot, function, offset);

            PortIO.Out32(0xCF8, address);
            PortIO.Out32(0xCFC, value);
        }

        public static void PciWrite(PciDevice dev, ushort offset, uint value)
        {
            PciWrite(dev.Bus, dev.Slot, dev.Function, offset, value);
        }


        public static ushort PciReadWord(PciDevice dev, ushort offset, uint value)
        {
            return readWord(dev.Bus, dev.Slot, dev.Function, offset);
        }

        /// <summary>
        /// Get device ID
        /// </summary>
        /// <param name="bus">Bus</param>
        /// <param name="device">Device</param>
        /// <param name="function">Function</param>
        /// <returns>Device ID</returns>
        private static ushort GetDeviceID(ushort bus, ushort device, ushort function)
        {
            return readWord(bus, device, function, 0x2);
        }

        private static ushort GetHeaderType(ushort bus, ushort device, ushort function)
        {
            return (byte)(readWord(bus, device, function, 0xE) & 0xFF);
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
             return readWord(bus, device, function, 0);
        }

        /// <summary>
        /// Get Class ID
        /// </summary>
        /// <param name="bus">Bus</param>
        /// <param name="device">Device</param>
        /// <param name="function">Function</param>
        /// <returns>Class ID</returns>
        private static ushort GetClassID(ushort bus, ushort device, ushort function)
        {
            return (byte)(readWord(bus, device, function, 0XA) & 0xFF);
        }



        /// <summary>
        /// Get sub class ID
        /// </summary>
        /// <param name="bus">Bus</param>
        /// <param name="device">Device</param>
        /// <param name="function">Function</param>
        /// <returns>Class ID</returns>
        private static byte GetSubClassID(ushort bus, ushort device, ushort function)
        {
            return (byte)((readWord(bus, device, function, 0XA) >> 8) & 0xFF);
        }

        private static ushort getHeaderType(ushort bus, ushort device, ushort function)
        {
            return (byte)(readWord(bus, device, function, 0XE) & 0xFF);
        }

        /// <summary>
        /// Check PCI bus
        /// </summary>
        /// <param name="bus"></param>
        private static void checkBus(byte bus)
        {
            for (byte device = 0; device < 32; device++)
                checkDevice(bus, device);
        }

        private static void checkDevice(byte bus, byte device)
        {
            byte function = 0;

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
            dev.Port1 = (ushort)(readWord(bus, device, function, 0x10) & -1 << 1);
            dev.Port2 = (ushort)(readWord(bus, device, function, 0x14) & -1 << 1);

            m_devices[m_currentdevice++] = dev;
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
        /// Brute force scan over all busses
        /// </summary>
        public static void Probe()
        {
            checkBus(0);

            Console.Write("[PCI] ");
            Console.WriteNum((int)m_currentdevice - 1);
            Console.WriteLine(" devices detected");
        }
    }
}
