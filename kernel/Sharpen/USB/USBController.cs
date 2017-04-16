using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.USB
{
    public interface IUSBController
    {

        USBControllerType Type { get; }

        USBHelpers.ControllerPoll Poll { get; }
    }
}
