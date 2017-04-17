using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.USB
{
    public interface IUSBDriver
    {

        IUSBDriver Load(USBDevice device);

        void Close(USBDevice device);

        void Poll(USBDevice device);
    }
}
