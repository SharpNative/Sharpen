using Sharpen.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Other
{
    class VboxDevFSDriver
    {

        public static void Init()
        {
            Device device = new Device();
            device.Name = "VMMDEV";
            device.node = new Node();
            device.node.Flags = NodeFlags.DIRECTORY | NodeFlags.DEVICE;
            device.node.FindDir = findDirImpl;

            DevFS.RegisterDevice(device);

            Console.WriteLine("[VMMDev] FsDevice registered under VMMDEV");
        }
        

        private static unsafe Node findDirImpl(Node node, string name)
        {
            uint functionID = 0;
            if (String.Equals(name, "sessionid"))
            {
                functionID = (uint)VboxDevRequestTypes.VMMDevReq_GetSessionId;
            }
            else if(String.Equals(name, "powerstate"))
            {
                functionID = (uint)VboxDevRequestTypes.VMMDevReq_SetPowerStatus;
            }

            if (functionID == 0)
                return null;

            Node outNode = new Node();
            outNode.Cookie = functionID;
            outNode.Read = readImpl;
            outNode.Write = writeImpl;
            outNode.Flags = NodeFlags.FILE;

            return outNode;
        }

        private static unsafe uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            VboxDevRequestTypes function = (VboxDevRequestTypes)node.Cookie;

            switch (function)
            {
                case VboxDevRequestTypes.VMMDevReq_SetPowerStatus:
                    if (size < 4)
                        return 0;

                    int state = ByteUtil.ToInt(buffer);
                    VboxDevPowerState stateConverted = (VboxDevPowerState)state;

                    VboxDev.ChangePowerState(stateConverted);

                    return 4;
            }

            return 0;
        }

        private static unsafe uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            VboxDevRequestTypes function = (VboxDevRequestTypes)node.Cookie;

            switch (function)
            {
                case VboxDevRequestTypes.VMMDevReq_GetSessionId:
                    if (size != 8)
                        return 0;

                    ulong sessionID = VboxDev.GetSessionID();

                    ByteUtil.toBytes((long)sessionID, buffer);

                    return 8;
            }

            return 0;
        }
    }
}
