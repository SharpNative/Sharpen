using Sharpen.Collections;

namespace Sharpen.Drivers.Char
{
    struct SerialPortComport
    {
        public string Name { get; set; }
        public ushort Address { get; set; }
        public Fifo Buffer { get; set; }
    }
}