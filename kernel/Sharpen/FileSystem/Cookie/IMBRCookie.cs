using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.FileSystem.Cookie
{
    class IMBRCookie:  ICookie
    {
        public int Offset { get; set; }

        public int Size { get; set; }

        public int MaxLBA { get; set; }

        public byte Type { get; set; }

        public Node Disk { get; set; }

        public void Dispose()
        {

        }
    }
}
