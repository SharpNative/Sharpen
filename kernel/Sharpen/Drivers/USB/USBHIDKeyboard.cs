using Sharpen.Mem;
using Sharpen.USB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.USB
{
    unsafe class USBHIDKeyboard: IUSBDriver
    {
        private USBTransfer* mTransfer;

        private byte* Data;

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
            Data = (byte*)Heap.Alloc(8);

            /**
             * Prepare poll transfer
             */
            mTransfer->Data = (byte*)Data;
            mTransfer->Length = 8;
            mTransfer->Executed = false;
            mTransfer->Success = false;

            //device.PrepareInterrupt(device, mTransfer);
        }

        public void Poll(USBDevice device)
        {
            if (mTransfer->Executed)
            {
                if (mTransfer->Success)
                {
                    /**
                     * Todo: Handle keyboard events here!
                     */
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
