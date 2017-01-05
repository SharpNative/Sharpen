namespace Sharpen.Arch
{
    public sealed class PortIO
    {
        /// <summary>
        /// Write 8-bit value to the given port
        /// </summary>
        /// <param name="port">The port</param>
        /// <param name="value">The value</param>
        public static extern void Out8(ushort port, byte value);

        /// <summary>
        /// Read 8-bit value from the given port
        /// </summary>
        /// <param name="port">The port</param>
        /// <returns>Value from the port</returns>
        public static extern byte In8(ushort port);

        /// <summary>
        /// Write 16-bit value to the given port
        /// </summary>
        /// <param name="port">The port</param>
        /// <param name="value">The value</param>
        public static extern void Out16(ushort port, ushort value);

        /// <summary>
        /// Read 16-bit value from the given port
        /// </summary>
        /// <param name="port">The port</param>
        /// <returns>Value from the port</returns>
        public static extern ushort In16(ushort port);

        /// <summary>
        /// Write 32-bit value to the given port
        /// </summary>
        /// <param name="port">The port</param>
        /// <param name="value">The value</param>
        public static extern void Out32(ushort port, uint value);

        /// <summary>
        /// Read 32-bit value from the given port
        /// </summary>
        /// <param name="port">The port</param>
        /// <returns>Value from the port</returns>
        public static extern uint In32(ushort port);
    }
}
