using System;
using System.Collections.Generic;
using System.Linq;
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

    public struct USBDeviceRequest
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
        public unsafe delegate void ControllerPoll(IUSBController controller);

        public unsafe delegate void DeviceControl(USBDevice dev, USBTransfer transfer);
    }
}
