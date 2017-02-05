using Sharpen.Drivers.Other;

namespace Sharpen.FileSystem
{
    public class VboxDevFSCookie : ICookie
    {
        public VboxDevRequestTypes Request;

        /// <summary>
        /// Creates a new VboxDevFS cookie
        /// </summary>
        /// <param name="request">The request</param>
        public VboxDevFSCookie(VboxDevRequestTypes request)
        {
            Request = request;
        }

        /// <summary>
        /// Cleans up
        /// </summary>
        public void Dispose()
        {
        }
    }
}
