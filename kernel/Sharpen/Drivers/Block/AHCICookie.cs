using Sharpen.FileSystem.Cookie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Block
{
    class AHCICookie: ICookie
    {
        public AHCI AHCI { get; set; }

        public AHCIPortInfo PortInfo { get; set; }


        public void Dispose()
        {

        }
    }
}
