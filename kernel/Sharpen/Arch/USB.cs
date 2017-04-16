using Sharpen.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Arch
{
    class USB
    {
        private static List Controllers;

        public static void Init()
        {
            Controllers = new List();
        }

        public static void RegisterController(IUSBController controller)
        {
            Controllers.Add(controller);
        }

        public static void Poll()
        {
            for (int i = 0; i < Controllers.Count; i++)
            {
                IUSBController controller = (IUSBController)Controllers.Item[i];

                controller?.Poll(controller);
            }
        }
    }
}
