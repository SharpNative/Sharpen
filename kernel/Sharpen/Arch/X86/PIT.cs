namespace Sharpen.Arch
{
    public sealed class PIT
    {
        // In MHz
        public const uint PIT_FREQUENCY = 1193180;

        // Ports
        public const ushort PIT_DATA_0 = 0x40;
        public const ushort PIT_DATA_1 = 0x41;
        public const ushort PIT_DATA_2 = 0x42;
        public const ushort PIT_CMD = 0x43;

        public const byte PIT_MODE_IOTC = 0x0;
        public const byte PIT_MODE_ONESHOT = 0x2;
        public const byte PIT_MODE_RATE = 0x4;
        public const byte PIT_MODE_SQUARE = 0x6;
        public const byte PIT_MODE_SOFTSTROBE = 0x8;
        public const byte PIT_MODE_HARDSTROBE = 0xA;

        public const byte PIT_ACCESS_LATCHCOUNT = 0x0;
        public const byte PIT_ACCESS_LOBYTE = 0x10;
        public const byte PIT_ACCESS_HIBYTE = 0x20;
        public const byte PIT_ACCESS_LOHIBYTE = 0x30;
        
        /// <summary>
        /// Prepares the PIT to sleep a couple of microseconds
        /// </summary>
        /// <param name="us">The microseconds</param>
        /// <returns>The sleep divisor</returns>
        public static uint PrepareSleep(uint us)
        {
            // Initialize PIT
            PortIO.Out8(PIT_CMD, PIT_DATA_2 | PIT_MODE_IOTC | PIT_ACCESS_LOHIBYTE);
            uint sleepDivisor = PIT_FREQUENCY / (1000000 / us);
            return sleepDivisor;
        }

        /// <summary>
        /// Sleeps
        /// </summary>
        /// <param name="sleepDivisor">The sleep divisor</param>
        public static void Sleep(uint sleepDivisor)
        {
            // Write sleep divisor
            PortIO.Out8(PIT_DATA_2, (byte)(sleepDivisor & 0xFF));
            PortIO.Out8(PIT_DATA_2, (byte)(sleepDivisor >> 8));

            // Reset PIT counter and start
            byte controlByte = PortIO.In8(0x61);
            PortIO.Out8(0x61, (byte)(controlByte & ~1));
            PortIO.Out8(0x61, (byte)(controlByte | 1));

            // Wait until the PIT counter reaches zero
            while ((PortIO.In8(0x61) & 0x20) == 0) ;
        }
    }
}
