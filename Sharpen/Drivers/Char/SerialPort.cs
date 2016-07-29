using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Char
{
    class SerialPort
    {
        private static SerialPortComport[] comports = new SerialPortComport[4];

        /// <summary>
        /// Init device
        /// </summary>
        /// <param name="num"></param>
        private static void initDevice(int num)
        {

        }

        /// <summary>
        /// Read bios data area for addresses
        /// </summary>
        private static unsafe void readBda()
        {
            UInt16* bda = (UInt16*)0x00000400;

            comports[0].Address = *bda;          // COM1
            comports[1].Address = *(bda + 1);    // COM2
            comports[2].Address = *(bda + 2);    // COM3
            comports[3].Address = *(bda + 3);	// COM4

        }

        public static void Init()
        {
            comports[0] = new SerialPortComport();
            comports[0].Name = "COM1";
            comports[1] = new SerialPortComport();
            comports[1].Name = "COM2";
            comports[2] = new SerialPortComport();
            comports[2].Name = "COM3";
            comports[3] = new SerialPortComport();
            comports[3].Name = "COM4";

            readBda();
            Console.Write("COM1 - address 0x");
            Console.WriteHex(comports[0].Address);

            initDevice(0);
            initDevice(1);
            initDevice(2);
            initDevice(3);
        }
    }
}
