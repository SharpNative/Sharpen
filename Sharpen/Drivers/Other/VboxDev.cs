using Sharpen.Arch;
using Sharpen.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sharpen.FileSystem.DevFS;

namespace Sharpen.Drivers.Other
{
    class VboxDev
    {

        private static PCI.PciDevice m_dev;
        private static bool m_initalized;

        struct RequestHeader
        {
            public uint Size;
            public uint Version;
            public VboxDevRequestTypes requestType;
            public int rc;
            public uint reserved_1;
            public uint reserved_2;
        }

        struct RequestSessionID
        {
            public RequestHeader header;

            public ulong idSession;
        }

        struct RequestGuestInfo
        {
            public RequestHeader header;

            public uint interfaceVersion;

            public uint osType;
        }

        struct RequestPowerState
        {
            public RequestHeader header;

            public VboxDevPowerState PowerState;
        }
        
        struct RequestHostTime
        {
            public RequestHeader header;

            public ulong Time;
        }

        private unsafe static void GetGuestInfo()
        {
            RequestGuestInfo* req = (RequestGuestInfo*)Heap.Alloc(sizeof(RequestGuestInfo));
            req->header.Size = (uint)sizeof(RequestGuestInfo);
            req->header.Version = 0x10001;
            req->header.requestType = VboxDevRequestTypes.VMMDevReq_ReportGuestInfo;
            req->header.rc = 0xFFFFF;
            req->interfaceVersion = 0x10000;
            req->osType = 0x10000;

            
            PortIO.Out32(m_dev.Port1, (uint)Paging.GetPhysicalFromVirtual(req));

            if (req->header.rc == 0)
            {
                Console.WriteLine("[VMMDev] Initalized");

                m_initalized = true;
            }
            else
            {
                Console.WriteLine("[VMMDev] Initalization failed");
            }
        }


        private unsafe static void InitHandler(PCI.PciDevice dev)
        {
            m_dev = dev;

            GetGuestInfo();

            if(m_initalized)
                VboxDevFSDriver.Init();
        }

        private static void ExitHander(PCI.PciDevice dev)
        {

        }

        public static void Init()
        {
            PCI.PciDriver driver = new PCI.PciDriver();
            driver.Name = "VboxDev driver";
            driver.Exit = ExitHander;
            driver.Init = InitHandler;

            PCI.RegisterDriver(0x80EE, 0xCAFE, driver);

        }

        #region Functions


        /// <summary>
        /// Change power state
        /// </summary>
        /// <param name="state">Power state</param>
        public unsafe static void ChangePowerState(VboxDevPowerState state)
        {
            RequestPowerState* req = (RequestPowerState*)Heap.Alloc(sizeof(RequestPowerState));
            req->header.Size = (uint)sizeof(RequestPowerState);
            req->header.Version = 0x10001;
            req->header.requestType = VboxDevRequestTypes.VMMDevReq_SetPowerStatus;
            req->header.rc = 0xFFFFF;
            req->PowerState = state;

            PortIO.Out32(m_dev.Port1, (uint)Paging.GetPhysicalFromVirtual(req));
        }

        /// <summary>
        /// Get virtual session ID
        /// </summary>
        /// <returns>The sessionID</returns>
        public unsafe static ulong GetSessionID()
        {
            RequestSessionID* req = (RequestSessionID*)Heap.Alloc(sizeof(RequestSessionID));
            req->header.Size = (uint)sizeof(RequestSessionID);
            req->header.Version = 0x10001;
            req->header.requestType = VboxDevRequestTypes.VMMDevReq_GetSessionId;
            req->header.rc = 0xFFFFF;

            PortIO.Out32(m_dev.Port1, (uint)Paging.GetPhysicalFromVirtual(req));

            return req->idSession;
        }

        /// <summary>
        /// Get host time
        /// </summary>
        /// <returns>Time sinds linux epoch</returns>
        public unsafe static ulong GetHostTime()
        {
            RequestHostTime* req = (RequestHostTime*)Heap.Alloc(sizeof(RequestHostTime));
            req->header.Size = (uint)sizeof(RequestHostTime);
            req->header.Version = 0x10001;
            req->header.requestType = VboxDevRequestTypes.VMMDevReq_GetHostTime;
            req->header.rc = 0xFFFFF;

            PortIO.Out32(m_dev.Port1, (uint)Paging.GetPhysicalFromVirtual(req));

            return req->Time;
        }

        #endregion
    }
}
