using Sharpen.FileSystem;
using Sharpen.FileSystem.Cookie;
using Sharpen.Mem;
using Sharpen.USB;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.USB
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CommandBlockWrapper
    {
        public int Signature { get; set; }

        public int Tag { get; set; }

        public int Length { get; set; }

        public byte Direction { get; set; }

        public byte Lun { get; set; }

        public byte CMDLen { get; set; }

        public fixed byte Data[16];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CommandStatusWrapper
    {
        public int Signature { get; set; }

        public int Tag { get; set; }

        public int Residue { get; set; }

        public byte Status { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCSIReadCap
    {

        public byte Opcode { get; set; }

        public byte Reserved { get; set; }

        public int LBA { get; set; }

        public ushort Reserved2 { get; set; }

        public byte PMI { get; set; }

        public byte Control { get; set; }
    }


    public unsafe struct SCSIReadCapData
    {
        public int LBA { get; set; }

        public int BlockLength { get; set; }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCSIInquiry
    {
        public byte Opcode { get; set; }

        public byte CommandDataSupportVitalProductData { get; set; }

        public byte PageCode { get; set; }

        public byte Reserved { get; set; }

        public byte AllocLength { get; set; }

        public byte Control { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCSIInquiryData
    {
        public byte PeripheralInfo { get; set; }

        public byte RMB { get; set; }

        public byte Version { get; set; }

        public byte RespData1 { get; set; }

        public byte AddLength { get; set; }

        public byte SCSS { get; set; }

        public byte RespData2 { get; set; }

        public byte RespData3 { get; set; }

        public fixed char VendorID[8];

        public fixed char ProductID[16];

        public fixed char ProductRevLevel[4];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCSIRead
    {

        public byte Opcode { get; set; }

        public byte Flags { get; set; }

        public int LBA { get; set; } // 2, 3, 4, 5

        // 6
        public byte Reserved { get; set; }

        // 7, 8, 9, 10
        public ushort Length { get; set; }

        public byte Control { get; set; }
    }


    
    class USBMSC : IUSBDriver
    {
        private const byte SCSI_INQUIRY = 0x12;
        private const byte SCSI_READ_CAPACITY = 0x25;
        private const byte SCSI_READ = 0x28;
        private const byte SCSI_WRITE = 0x2A;

        private const byte DATA_IN = 0x80;

        private const byte INQUIRY_PDT_DIRECT_ACCESS = 0x00;

        private const int SUPPORTED_BLOCK_LENGTH = 512;

        private static int _DeviceNum;

        private USBDevice _Device;

        private byte _MuxLun;

        private uint _ReturnedLBA;
        private uint _BlockLength;

        private uint _Capacity;

        private int _CurTag;

        private int _EndPointIn;
        private int _EndPointOut;

        public static void Init()
        {
            USBMSC msc = new USBMSC();
            USBDrivers.RegisterDriver(msc);
            _DeviceNum = 0;
        }

        public unsafe bool Init(USBDevice device)
        {
            // TODO: Read from descriptors
            _EndPointIn = 0x01;
            _EndPointOut = 0x02;

            _Device = device;
            _MuxLun = GetMaxLun();
            _CurTag = 0;

            if (!Initalize())
            {

                return false;
            }
            
            return true;
        }
        
        private unsafe bool Initalize()
        {
            // TODO: Read endpoint descriptors here

            SCSIInquiryData* inquiryResp = Inquiry();
            if (inquiryResp == null)
            {
                Console.WriteLine("[USB-MSC] Inquiry failed");

                Heap.Free(inquiryResp);
                return false;
            }
            
            // Check if device supports Direct access SBC-2 or above
            if((inquiryResp->PeripheralInfo & 0xF) != INQUIRY_PDT_DIRECT_ACCESS)
            {
                Console.WriteLine("[USB-MSC] Unsupported device type");

                Heap.Free(inquiryResp);
                return false;
            }
            
            if (!ReadCapacity())
            {
                Console.WriteLine("[USB-MSC] Cannot read capacity");

                return false;
            }

            if(_BlockLength != SUPPORTED_BLOCK_LENGTH)
            {
                Console.Write("[USB-MSC] Unsupported block length ");
                Console.WriteNum((int)_BlockLength);
                Console.WriteLine("");
            }

            // We can mount it here :D

            int deviceNum = _DeviceNum++;

            char* name = (char*)Heap.Alloc(6);
            name[0] = 'U';
            name[1] = 'S';
            name[2] = 'B';
            name[3] = 'D';
            name[4] = (char)('0' + deviceNum);
            name[5] = '\0';
            string nameStr = Util.CharPtrToString(name);

            Node node = new Node();
            node.Read = readImpl;
            node.Write = writeImpl;

            USBMSCCookie cookie = new USBMSCCookie(_Device, this);
            node.Cookie = cookie;

            RootPoint dev = new RootPoint(nameStr, node);
            VFS.MountPointDevFS.AddEntry(dev);

            return true;
        }

        /// <summary>
        /// Write method for filesystem
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes written</returns>
        private static uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            // Only support sizes in magnitudes of 512
            if (size % 512 != 0)
                return 0;

            USBMSCCookie cookie = (USBMSCCookie)node.Cookie;

            return cookie.USBMSC.WriteSector(offset, buffer, (int)size);
        }

        /// <summary>
        /// Read method for filesystem
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes read</returns>
        private static uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            // Only support sizes in magnitudes of 512
            if (size % 512 != 0)
                return 0;

            USBMSCCookie cookie = (USBMSCCookie)node.Cookie;

            return cookie.USBMSC.ReadSector(offset, buffer, (int)size);
        }

        public unsafe uint ReadSector(uint offset, byte[] buffer, int size)
        {

            SCSIRead* cmd = (SCSIRead*)Heap.Alloc(sizeof(SCSIRead));
            Memory.Memclear(cmd, sizeof(SCSIRead));
            
            cmd->Opcode = SCSI_READ;
            cmd->LBA = (int)SwapBytes((offset));
            cmd->Length = SwapBytes((ushort)(size / 512));

            

            if (!Command((byte*)cmd, (byte)sizeof(SCSIReadCap), (byte*)Util.ObjectToVoidPtr(buffer), size, true))
            {
                Heap.Free(cmd);

                Console.WriteLine("FAIL");

                return 0;
            }

            return (uint)size;
        }

        public unsafe uint WriteSector(uint offset, byte[] buffer, int size)
        {

            SCSIRead* cmd = (SCSIRead*)Heap.Alloc(sizeof(SCSIRead));
            Memory.Memclear(cmd, sizeof(SCSIRead));

            cmd->Opcode = SCSI_WRITE;
            cmd->Flags = 0x08;
            cmd->LBA = (int)SwapBytes((offset));
            cmd->Length = SwapBytes((ushort)(size / 512));


            if (!Command((byte*)cmd, (byte)sizeof(SCSIReadCap), (byte*)Util.ObjectToVoidPtr(buffer), size, false))
            {
                Heap.Free(cmd);

                Console.WriteLine("FAIL");

                return 0;
            }

            return (uint)size;
        }

        private unsafe SCSIInquiryData* Inquiry()
        {
            SCSIInquiryData* data = (SCSIInquiryData*)Heap.Alloc(sizeof(SCSIInquiryData));
            Memory.Memclear(data, sizeof(SCSIReadCapData));

            SCSIInquiry* cmd = (SCSIInquiry*)Heap.Alloc(sizeof(SCSIInquiry));
            Memory.Memclear(cmd, sizeof(SCSIInquiry));
            cmd->Opcode = SCSI_INQUIRY;
            cmd->AllocLength = (byte)sizeof(SCSIInquiryData);

            if (!Command((byte*)cmd, (byte)sizeof(SCSIReadCap), (byte*)data, sizeof(SCSIInquiryData), true))
            {
                Heap.Free(cmd);
                Heap.Free(data);

                return null;
            }

            Heap.Free(cmd);

            return data;
        }

        /// <summary>
        /// Read device capacity
        /// </summary>
        /// <returns></returns>
        private unsafe bool ReadCapacity()
        {
            SCSIReadCapData* data = (SCSIReadCapData*)Heap.Alloc(sizeof(SCSIReadCapData));
            Memory.Memclear(data, sizeof(SCSIReadCapData));


            // CMD transfer
            SCSIReadCap* cmd = (SCSIReadCap*)Heap.Alloc(sizeof(SCSIReadCap));
            Memory.Memclear(cmd, sizeof(SCSIReadCap));
            cmd->Opcode = SCSI_READ_CAPACITY;
            cmd->LBA = 0;
            cmd->PMI = 0;
            cmd->Control = 0;

            if (!Command((byte*)cmd, (byte)sizeof(SCSIReadCap), (byte*)data, sizeof(SCSIReadCapData), true))
            {
                Heap.Free(cmd);
                Heap.Free(data);

                return false;
            }

            _BlockLength = SwapBytes((uint)data->BlockLength);
            _ReturnedLBA = SwapBytes((uint)data->LBA);
            
            Heap.Free(cmd);
            Heap.Free(data);

            return true;
        }

        #region Byte swapping
        private uint SwapBytes(uint inInt)
        {
            return ((inInt & 0x000000FF) << 24)
                | ((inInt & 0x0000FF00) << 8)
                | ((inInt & 0x00FF0000) >> 8)
                | ((inInt & 0xFF000000) >> 24);
        }

        private ushort SwapBytes(ushort inShort)
        {
            return (ushort)(((inShort & 0x00FF) << 8)
                | ((inShort & 0xFF00) >> 8));
        }
        #endregion


        /// <summary>
        /// Do command
        /// </summary>
        /// <param name="cmdBlockData">SCSI command data</param>
        /// <param name="cmdBlockLength">Command length</param>
        /// <param name="buffer">Data buffer</param>
        /// <param name="bufferLength">Data buffer length</param>
        /// <param name="In">In or out?</param>
        /// <returns></returns>
        private unsafe bool Command(byte *cmdBlockData, byte cmdBlockLength, byte *buffer, int bufferLength, bool In)
        {

            USBTransfer* transfer = (USBTransfer*)Heap.Alloc(sizeof(USBTransfer));

            /**
             * Command transport
             */
            CommandBlockWrapper* cbw = (CommandBlockWrapper*)Heap.Alloc(sizeof(CommandBlockWrapper));
            cbw->Signature = 0x43425355;
            cbw->Tag = ++_CurTag;

            cbw->Length = bufferLength;
            cbw->Direction = (byte)(In ? DATA_IN : 0);
            cbw->Lun = 0;
            cbw->CMDLen = cmdBlockLength;

            Memory.Memclear(cbw->Data, 16);
            for (int i = 0; i < cmdBlockLength; i++)
                cbw->Data[i] = cmdBlockData[i];

            transfer->Success = false;
            transfer->Type = 1;

            transfer->Executed = false;
            transfer->Endpoint = (uint)_EndPointOut;
            transfer->Length = (uint)sizeof(CommandBlockWrapper);
            transfer->Data = (byte*)cbw;

            _Device.TransferOne(_Device, transfer);

            if(!transfer->Success)
            {
                Console.WriteLine("[USB-MSC] CBW transfer failed.");

                Heap.Free(transfer);

                return false;
            }


            /**
             * Data transport
             */
            if(bufferLength > 0)
            {
                transfer->Executed = false;
                transfer->Endpoint = (uint)(In ? _EndPointIn : _EndPointOut);
                transfer->Data = buffer;
                transfer->Type = (ushort)(In ? 0 : 1);
                transfer->Length = bufferLength;

                _Device.TransferOne(_Device, transfer);

                if (!transfer->Success)
                {
                    Console.WriteLine("[USB-MSC] Data transfer failed.");

                    Heap.Free(transfer);

                    return false;
                }

            }

            /**
             * Status transport
             */
            CommandStatusWrapper* statusWrapper = (CommandStatusWrapper*)Heap.Alloc(sizeof(CommandStatusWrapper));

            transfer->Executed = false;
            transfer->Endpoint = (uint)_EndPointIn;
            transfer->Type = 0;
            transfer->Data = (byte*)statusWrapper;
            transfer->Length = (uint)sizeof(CommandStatusWrapper);
            
            _Device.TransferOne(_Device, transfer);

            if(statusWrapper->Status != 0x00)
            {
                Heap.Free(statusWrapper);
                Heap.Free(transfer);

                return false;
            }

            Heap.Free(statusWrapper);
            Heap.Free(transfer);

            return true;
        }

        private unsafe byte GetMaxLun()
        {
            byte* data = (byte*)Heap.Alloc(1);
            
            if (!_Device.Request(USBDevice.TYPE_DEVICETOHOST | USBDevice.TYPE_CLASS | USBDevice.TYPE_INTF, 0xFE, 0, 0, 1, data))
                return 0xFF;
            

            byte ret = *data;
            
            Heap.Free(data);

            return ret;
        }

        public void Close(USBDevice device)
        {

        }

        public unsafe IUSBDriver Load(USBDevice device)
        {
            if (device.InterfaceDesc->Class == (byte)USBClassCodes.MASS_STORAGE && device.InterfaceDesc->SubClass == 0x06)
            {
                USBMSC hardDisk = new USBMSC();
                hardDisk.Init(device);


                device.Classifier = USBDeviceClassifier.FUNCTION;

                return hardDisk;
            }

            return null;
        }

        public void Poll(USBDevice device)
        {

        }
    }
}
