using static Sharpen.Net.NetworkInfoFS;

namespace Sharpen.FileSystem
{
    public class NetworkInfoFSCookie : ICookie
    {
        public InfoOPT InfoOPT;

        /// <summary>
        /// Creates a new NetworkInfoFS cookie
        /// </summary>
        /// <param name="opt">The option</param>
        public NetworkInfoFSCookie(InfoOPT opt)
        {
            InfoOPT = opt;
        }

        /// <summary>
        /// Cleans up
        /// </summary>
        public void Dispose()
        {
        }
    }
}
