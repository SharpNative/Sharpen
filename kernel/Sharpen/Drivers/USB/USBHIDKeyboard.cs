using Sharpen.Drivers.Char;
using Sharpen.Mem;
using Sharpen.USB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.USB
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct KeyboardHIDStruct
    {
        /// <summary>
        /// Modifier keys
        /// </summary>
        public byte Modifier { get; set; }

        /// <summary>
        /// Reserved field
        /// </summary>
        public byte Reserved { get; set; }

        public fixed byte Keys[6];
    }

    unsafe class USBHIDKeyboard: IUSBDriver
    {
        public const byte LEFT_CTRL = (1 << 0);
        public const byte LEFT_SHIFT = (1 << 1);
        public const byte LEFT_ALT = (1 << 2);
        public const byte LEFT_GUI = (1 << 3);
        public const byte RIGHT_CTRL = (1 << 4);
        public const byte RIGHT_SHIFT = (1 << 5);
        public const byte RIGHT_ALT = (1 << 6);
        public const byte RIGHT_GUI = (1 << 7);

        private USBTransfer* mTransfer;

        private KeyboardHIDStruct* mData;

        private static bool mShift = false;

        public static void Init()
        {
            USBHIDKeyboard keyboard = new USBHIDKeyboard();
            USBDrivers.RegisterDriver(keyboard);
        }

        public void Close(USBDevice device)
        {

        }

        public unsafe IUSBDriver Load(USBDevice device)
        {

            USBInterfaceDescriptor* desc = device.InterfaceDesc;
            if (!(desc->Class == (byte)USBClassCodes.HID &&
                desc->SubClass == 0x01 &&
                desc->Protocol == 0x01))
                return null;

            device.Classifier = USBDeviceClassifier.FUNCTION;

            USBHIDKeyboard kb = new USBHIDKeyboard();
            kb.initDevice(device);
            return kb;
        }

        public unsafe void initDevice(USBDevice device)
        {
            mTransfer = (USBTransfer*)Heap.Alloc(sizeof(USBTransfer));
            mData = (KeyboardHIDStruct *)Heap.Alloc(sizeof(KeyboardHIDStruct));

            /**
             * Prepare poll transfer
             */
            mTransfer->Data = (byte*)mData;
            mTransfer->Length = 8;
            mTransfer->Executed = false;
            mTransfer->Success = false;
            mTransfer->ID = 1;

            //device.PrepareInterrupt(device, mTransfer);
        }

        public void HandleData()
        {
            mShift = ((mData->Modifier & LEFT_SHIFT) > 0) || ((mData->Modifier & RIGHT_SHIFT) > 0);

            for (int i = 0; i < 6; i++)
                if (mData->Keys[i] != 0x00)
                    HandleKey(mData->Keys[i]);
        }

        public void HandleKey(byte code)
        {


            // Wait for timeouts etc
        }

        public void Poll(USBDevice device)
        {
            if (mTransfer->Executed)
            {
                if (mTransfer->Success)
                {
                    HandleData();
                }

                /**
                 * And again
                 */
                mTransfer->Executed = false;
                device.PrepareInterrupt(device, mTransfer);
            }
        }
    }
}
