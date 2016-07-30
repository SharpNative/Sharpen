using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Arch
{
    class PCI
    {
        public struct PciDriver
        {
            public string Name;

            public PciDriverInit Init;
            public PciDriverExit Exit;
        }

        public struct PciDevice
        {
            public ushort Vendor;
            public ushort Device;
            public ushort Func;
            public PciDriver Driver;
        }
        
        public unsafe delegate void PciDriverInit(PciDevice dev);
        public unsafe delegate void PciDriverExit(PciDevice dev);

        private static PciDevice[] m_devices = new PciDevice[300];
        private static uint m_currentdevice = 0;

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
            uint address;
            uint lbus = bus;
            uint lslot = slot;
            uint lfun = function;

            ushort tmp;
            address = lbus << 16 | lslot << 11 | lfun << 8 | (uint)(offset & 0xFC) | 0x80000000;

            PortIO.Out32(0xCF8, address);
            tmp = (ushort)((PortIO.In32(0xCFC) >> ((offset & 2) * 8)) & 0xffff);
            return tmp;
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
            dev.Func = function;
            dev.Vendor = vendorID;

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
                if (m_devices[i].Vendor == vendorID && m_devices[i].Device == deviceID)
                {
                    foundIndex = i;
                    break;
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
