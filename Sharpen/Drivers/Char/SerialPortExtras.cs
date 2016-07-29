using Sharpen.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Char
{
    struct SerialPortComport
    {

        public string Name { get; set; }

        public ushort Address { get; set; }

        public Fifo Buffer { get; set; }
    }
}
