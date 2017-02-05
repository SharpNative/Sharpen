using static Sharpen.Net.UDPFS;

namespace Sharpen.FileSystem
{
    public class UDPFSCookie : ICookie
    {
        public OPT Opt;

        /// <summary>
        /// Creates a new UDPFS cookie
        /// </summary>
        /// <param name="opt">The option</param>
        public UDPFSCookie(OPT opt)
        {
            Opt = opt;
        }

        /// <summary>
        /// Cleans up
        /// </summary>
        public void Dispose()
        {
        }
    }
}
