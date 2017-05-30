using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.FileSystem.Cookie
{
    class PacketFSCookie : ICookie
    {
        public string PacketFSEntry { get; set; }

        public PacketFSCookie(string packetFSName)
        {
            PacketFSEntry = packetFSName;
        }


        public void Dispose()
        {


        }
    }
}
