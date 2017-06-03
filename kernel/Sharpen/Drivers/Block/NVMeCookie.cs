using Sharpen.FileSystem.Cookie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Block
{
    class NVMeCookie : ICookie
    {
        public NVMe NVMe { get; set; }


        public void Dispose()
        {

        }
    }
}
