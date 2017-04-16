using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Arch
{
    public class USBHelpers
    {

        public unsafe delegate void ControllerPoll(IUSBController controller);
    }

    public enum USBControllerType
    {
        OHCI,
        UHCI
    }

    public interface IUSBController
    {

        USBControllerType Type { get;  }

        USBHelpers.ControllerPoll Poll { get; }
    }

    public class USBDevice
    {

        public USBDevice Parent { get; set; }

        public IUSBController Controller { get; set; }
    }

}
