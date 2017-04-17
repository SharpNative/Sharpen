using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.USB
{
    public enum USBControllerType
    {
        OHCI,
        UHCI
    }

    public enum USBDeviceState
    {
        ATTACHED,
        POWERED,
        DEFAULT,
        CONFIGURED,
        SUSPENDED
    }

    public enum USBDeviceSpeed
    {
        LOW_SPEED,
        HIGH_SPEED
    }

    public enum USBClassCodes: byte
    {
        DEVICE = 0x00,
        INTERFACE = 0x01,
        BOTH =0x02,
        HID = 0x03,
        PHYSICAL = 0x05,
        IMAGE = 0x06,
        PRINTER = 0x07,
        MASS_STORAGE = 0x08,
        HUB = 0x09,
        CDC_DATA = 0x0A,
        SMARTCARD = 0x0B,
        SECURITY = 0x0D,
        VIDEO = 0x0E
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct USBDeviceDescriptor
    {
        public byte Length;
        public byte Type;
        public ushort USBVersion;
        public byte DeviceClass;
        public byte DeviceSubClass;
        public byte DeviceProtocol;
        public byte MaxpacketSize0;
        public ushort VendorID;
        public ushort ProductID;
        public ushort BCDDevice;
        public byte ManufacturerIndex;
        public byte ProductIndex;
        public byte SerialNumberIndex;
        public byte NumConfigurations;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct USBConfigurationDescriptor
    {
        public byte Length;
        public byte Type;
        public ushort TotalLength;
        public byte NumberOfInterfaces;
        public byte ConfigurationValue;
        public byte ConfigurationIndex;
        public byte Attributes;
        public byte MaxPower;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct USBEndpointDescriptor
    {
        public byte Length;
        public byte DescriptorType;
        public byte Address;
        public byte Attributes;
        public ushort MaxPacketSize;
        public byte Interval;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct USBInterfaceDescriptor
    {
        public byte Length;
        public byte DescriptorType;
        public byte eNumber;
        public byte AlternateSetting;
        public byte NumEndpoints;
        public byte Class;
        public byte SubClass;
        public byte Protocol;
        public byte Index;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct USBStringDescriptor
    {

        public byte Length;
        public byte Type;

        // Here follows data
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct USBDeviceRequest
    {
        public byte Type { get; set; }

        public byte Request { get; set; }

        public ushort Value { get; set; }

        public ushort Index { get; set; }

        public ushort Length { get; set; }
    }

    public unsafe struct USBTransfer
    {

        public USBDeviceRequest Request { get; set; }

        public byte *Data { get; set; }

        public uint Length { get; set; }

        public bool Success { get; set; }

        public bool Executed { get; set; }
    }

    public class USBHelpers
    {
        public const byte DRIVER_NO_SUBCLASS_CHECK = 0xFF;

        public unsafe delegate void ControllerPoll(IUSBController controller);

        public unsafe delegate void DeviceControl(USBDevice dev, USBTransfer *transfer);

        public unsafe delegate void PrepareDevice(USBDevice dev, USBTransfer* transfer);
    }
}
