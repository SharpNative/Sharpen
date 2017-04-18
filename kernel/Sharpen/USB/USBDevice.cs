using Sharpen.Arch;
using Sharpen.Mem;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.USB
{

    public unsafe class USBDevice
    {
        /**
         * Transfer types
         */
        public const byte TYPE_HOSTTODEVICE = 0;
        public const byte TYPE_DEVICETOHOST = 0x80;
        const byte TYPE_STANDARD = 0;
        const byte TYPE_CLASS = 0x20;
        const byte TYPE_VENDOR = 0x40;

        const byte TYPE_DEV = 0x0;
        const byte TYPE_INTF = 0x01;
        const byte TYPE_ENDP = 0x02;
        const byte TYPE_OTHER = 0x03;

        /**
         * Requests
         */
        const byte REQ_GET_STATUS = 0x0;
        const byte REQ_CLEAR_FEATURE = 0x1;
        const byte REQ_SET_FEATURE = 0x3;
        const byte REQ_SET_ADDR = 0x05;
        const byte REQ_GET_DESCRIPTOR = 0x06;
        const byte REQ_SET_DESCRIPTOR = 0x07;
        const byte REQ_GET_CONFIGURATION = 0x08;
        const byte REQ_SET_CONFIGURATION = 0x09;
        const byte REQ_GET_INTERFACE = 0x0A;
        const byte REQ_SET_INTERFACE = 0x0B;
        const byte REQ_SYNC_FRAME = 0x0C;

        /**
         * Descriptors
         */
        const byte DESC_DEVICE = 0x01;
        const byte DESC_CONFIGURATION = 0x02;
        const byte DESC_STRING = 0x03;
        const byte DESC_INTERFACE = 0x04;
        const byte DESC_ENDPOINT = 0x05;
        const byte DESC_DEVICE_QUALIFIER = 0x06;
        const byte DESC_OTHER_SPEED_CONFIGURATION = 0x07;
        const byte DESC_INTERFACE_POWER = 0x08;

        public USBDevice Parent { get; set; }

        public IUSBController Controller { get; set; }

        public USBDeviceState State { get; set; }

        public uint Toggle { get; set; }

        public USBDeviceSpeed Speed { get; set; }

        public ushort Port { get; set; }

        public uint MaxPacketSize { get; set; } = 8;

        public ushort Address { get; set; } = 0;

        private static ushort NextAddress { get; set; } = 0;

        public IUSBDriver Driver { get; set; }

        public USBDeviceClassifier Classifier { get; set; }

        /// <summary>
        /// Control device state
        /// </summary>
        public USBHelpers.DeviceControl Control { get; set; }

        /// <summary>
        /// Prepare device
        /// </summary>
        public USBHelpers.PrepareDevice PrepareInterrupt { get; set; }

        /// <summary>
        /// Number of languages
        /// </summary>
        public int NumLanguages { get; set; }

        /// <summary>
        /// Languages
        /// </summary>
        public ushort[] Languages { get; set; }

        /// <summary>
        /// Manufacturer in first lang
        /// </summary>
        public string Manfacturer { get; set; }

        /// <summary>
        /// Product name in first lang
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Serial number in first lang
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Set confiugration value
        /// </summary>
        public byte ConfigValue { get; set; }

        /// <summary>
        /// USB endpoint descriptor (we only use 1 for now)
        /// </summary>
        public USBEndpointDescriptor * EndPointDesc { get; set; }

        /// <summary>
        /// USB interface descritpro (we only use 1 for now)
        /// </summary>
        public USBInterfaceDescriptor *InterfaceDesc { get; set; }

        /// <summary>
        /// Do device request
        /// </summary>
        /// <param name="type"></param>
        /// <param name="req"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <param name="len"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public unsafe bool Request(byte type, byte req, ushort value, ushort index, ushort len, byte *data)
        {
            USBDeviceRequest request = new USBDeviceRequest();
            request.Request = req;
            request.Length = len;
            request.Index = index;
            request.Type = type;
            request.Value = value;

            USBTransfer* transfer = (USBTransfer*)Heap.Alloc(sizeof(USBTransfer));
            transfer->Request = request;
            transfer->Data = data;
            transfer->Length = len;

            transfer->Success = false;
            transfer->Executed = false;

            Control(this, transfer);

            return transfer->Success;
        }


        private static void Sleep(int cnt)
        {
            for (int i = 0; i < cnt; i++)
                PortIO.In32(0x80);
        }

        /// <summary>
        /// Initalize device
        /// </summary>
        /// <returns>Success?</returns>
        public unsafe bool Init()
        {
            byte* data = (byte *)Heap.Alloc(sizeof(USBDeviceDescriptor));
            for (int i = 0; i < sizeof(USBDeviceDescriptor);i++)
                data[i] = 0x1;


            /**
             * Get max packet size
             */
            USBDeviceDescriptor* descriptor = (USBDeviceDescriptor*)data;
            
            if (!Request(TYPE_DEVICETOHOST, REQ_GET_DESCRIPTOR, (DESC_DEVICE << 8) | 0, 0, 8, data))
                return false;

            MaxPacketSize = descriptor->MaxpacketSize0;

            /**
             * Set new address
             */
            ushort tempAdr = ++NextAddress;

            if (!Request(TYPE_HOSTTODEVICE, REQ_SET_ADDR, tempAdr, 0, 0, null))
                return false;

            Address = tempAdr;

            Sleep(20);

            /**
             * Get descriptor
             */

            if (!Request(TYPE_DEVICETOHOST, REQ_GET_DESCRIPTOR, (DESC_DEVICE << 8) | 0, 0, (ushort)sizeof(USBDeviceDescriptor), data))
                return false;

            /**
             * Load languages and set default
             */
            LoadLanguages();
            
            if(NumLanguages > 0)
            {
                Manfacturer = GetString(Languages[0], descriptor->ManufacturerIndex);
                Product = GetString(Languages[0], descriptor->ProductIndex);
                SerialNumber = GetString(Languages[0], descriptor->SerialNumberIndex);
            }

            /**
             * Get configurations
             */
            byte[] buffer = new byte[512];
            byte* ptrToBuf = (byte *)Util.ObjectToVoidPtr(buffer);
            USBConfigurationDescriptor* descBuf = (USBConfigurationDescriptor*)ptrToBuf;

            for (int i = 0; i < descriptor->NumConfigurations; i++)
            {


                if (!Request(TYPE_DEVICETOHOST, REQ_GET_DESCRIPTOR, (ushort)((DESC_CONFIGURATION << 8) | i), 0, 4, (byte*)Util.ObjectToVoidPtr(buffer)))
                    continue;

                if (!Request(TYPE_DEVICETOHOST, REQ_GET_DESCRIPTOR, (ushort)((DESC_CONFIGURATION << 8) | i), 0, descBuf->TotalLength, (byte*)Util.ObjectToVoidPtr(buffer)))
                    continue;
                
                // Prevent overflow
                if (descBuf->TotalLength > 512)
                    continue;

                ConfigValue = descBuf->ConfigurationValue;

                int remaining = descBuf->TotalLength - descBuf->Length;
                byte* dataStart = ptrToBuf + descBuf->Length;

                while(remaining > 0)
                {
                    int len = dataStart[0];
                    int type = dataStart[1];

                    if(type == DESC_INTERFACE)
                    {
                        InterfaceDesc = (USBInterfaceDescriptor*)dataStart;
                    }
                    else if(type == DESC_ENDPOINT)
                    {
                        EndPointDesc = (USBEndpointDescriptor*)dataStart;
                    }

                    dataStart += len;
                    remaining -= len;
                }
            }

            if(ConfigValue != 0 && InterfaceDesc != null && EndPointDesc != null)
            {

                if (!Request(TYPE_HOSTTODEVICE, REQ_SET_CONFIGURATION, ConfigValue, 0, 0, null))
                    return false;

                /**
                 * Init driver here
                 */
                Console.WriteLine("[USB] Device configured!");
                Console.WriteHex(InterfaceDesc->Class);
                Console.WriteLine("");
                State = USBDeviceState.CONFIGURED;

                IUSBDriver test = USBDrivers.LoadDriver(this);

                if(test == null)
                {
                    Console.WriteLine("test");
                }

                Driver = test;
            }

            return true;
        }

        /// <summary>
        /// Get string from descriptor
        /// </summary>
        /// <param name="languageID">Language ID</param>
        /// <param name="entryIndex">Index</param>
        /// <returns></returns>
        public unsafe string GetString(ushort languageID, uint entryIndex)
        {
            /**
             * Get length
             */
            byte[] buf = new byte[1];

            if (!Request(TYPE_DEVICETOHOST, REQ_GET_DESCRIPTOR, (ushort)((DESC_STRING << 8) | entryIndex), languageID, 1, (byte*)Util.ObjectToVoidPtr(buf)))
                return null;

            int len = *(byte*)Util.ObjectToVoidPtr(buf);

            /**
             * Load full object
             */
            byte[] buffer = new byte[len];

            if (!Request(TYPE_DEVICETOHOST, REQ_GET_DESCRIPTOR, (ushort)((DESC_STRING << 8) | entryIndex), languageID, (ushort)(len), (byte*)Util.ObjectToVoidPtr(buffer)))
                return null;

            /**
             * Set string
             */
            USBStringDescriptor* ptr = (USBStringDescriptor*)Util.ObjectToVoidPtr(buffer);
            char* entryData = (char*)ptr + 2;

            int size = (len - sizeof(USBStringDescriptor)) / 2;
            char* str = (char*)Heap.Alloc(size);

            int i = 0;
            for (; i < size; i++)
                str[i] = entryData[i * 2];
            str[i] = (char)0x00;

            /**
             * Free unused shizzle
             */
            Heap.Free(buffer);
            Heap.Free(buf);

            return Util.CharPtrToString(str);
        }

        /// <summary>
        /// Load all languages
        /// </summary>
        /// <returns></returns>
        public unsafe bool LoadLanguages()
        {
            /**
             * Read length
             */
            byte[] buf = new byte[1];

            if (!Request(TYPE_DEVICETOHOST, REQ_GET_DESCRIPTOR, (DESC_STRING << 8) | 0, 0, 1, (byte *)Util.ObjectToVoidPtr(buf)))
                return false;

            int len = *(byte*)Util.ObjectToVoidPtr(buf);

            /**
             * Read full table
             */
            byte[] buffer = new byte[len];

            if (!Request(TYPE_DEVICETOHOST, REQ_GET_DESCRIPTOR, (DESC_STRING << 8) | 0, 0, (ushort)(len), (byte*)Util.ObjectToVoidPtr(buffer)))
                return false;

            USBStringDescriptor* ptr = (USBStringDescriptor*)Util.ObjectToVoidPtr(buffer);
            ushort* entryData = (ushort*)ptr + 1;
           
           
            int entries = (ptr->Length - sizeof(USBStringDescriptor)) / 2;

            /**
             * Set entries
             */
            Languages = new ushort[entries];
            NumLanguages = entries;

            for (int i = 0; i < entries; i++)
                Languages[i] = *(entryData + i);

            /**
             * Free unused shizzle
             */
            Heap.Free(buf);
            Heap.Free(buffer);

            return true;
        }
    }

}