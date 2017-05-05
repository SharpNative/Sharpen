using Sharpen.Arch;

namespace Sharpen.FileSystem.Cookie
{
    public class PciDeviceCookie : ICookie
    {
        public PciDevice Device { get; private set; }

        /// <summary>
        /// Creates a new PciDeviceCookie
        /// </summary>
        /// <param name="dev">The device</param>
        public PciDeviceCookie(PciDevice dev)
        {
            Device = dev;
        }

        /// <summary>
        /// Cleans up
        /// </summary>
        public void Dispose()
        {
        }
    }
}
