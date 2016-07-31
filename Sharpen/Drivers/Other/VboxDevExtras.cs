using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Other
{
   public enum VboxDevRequestTypes
   {
        VMMDevReq_InvalidRequest = 0,
        VMMDevReq_GetMouseStatus = 1,
        VMMDevReq_SetMouseStatus = 2,
        VMMDevReq_SetPointerShape = 3,
        VMMDevReq_GetHostVersion = 4,
        VMMDevReq_Idle = 5,
        VMMDevReq_GetHostTime = 10,
        VMMDevReq_GetHypervisorInfo = 20,
        VMMDevReq_SetHypervisorInfo = 21,
        VMMDevReq_RegisterPatchMemory = 22,
        VMMDevReq_DeregisterPatchMemory = 23,
        VMMDevReq_SetPowerStatus = 30,
        VMMDevReq_AcknowledgeEvents = 41,
        VMMDevReq_CtlGuestFilterMask = 42,
        VMMDevReq_ReportGuestInfo = 50,
        VMMDevReq_ReportGuestInfo2 = 58,
        VMMDevReq_ReportGuestStatus = 59,
        VMMDevReq_ReportGuestUserState = 74,
        VMMDevReq_GetDisplayChangeRequest = 51,
        VMMDevReq_VideoModeSupported = 52,
        VMMDevReq_GetHeightReduction = 53,
        VMMDevReq_GetDisplayChangeRequest2 = 54,
        VMMDevReq_ReportGuestCapabilities = 55,
        VMMDevReq_SetGuestCapabilities = 56,
        VMMDevReq_VideoModeSupported2 = 57,
        VMMDevReq_GetDisplayChangeRequestEx = 80,
        VMMDevReq_VideoAccelEnable = 70,
        VMMDevReq_VideoAccelFlush = 71,
        VMMDevReq_VideoSetVisibleRegion = 72,
        VMMDevReq_GetSeamlessChangeRequest = 73,
        VMMDevReq_QueryCredentials = 100,
        VMMDevReq_ReportCredentialsJudgement = 101,
        VMMDevReq_ReportGuestStats = 110,
        VMMDevReq_GetMemBalloonChangeRequest = 111,
        VMMDevReq_GetStatisticsChangeRequest = 112,
        VMMDevReq_ChangeMemBalloon = 113,
        VMMDevReq_GetVRDPChangeRequest = 150,
        VMMDevReq_LogString = 200,
        VMMDevReq_GetCpuHotPlugRequest = 210,
        VMMDevReq_SetCpuHotPlugStatus = 211,
        VMMDevReq_RegisterSharedModule = 212,
        VMMDevReq_UnregisterSharedModule = 213,
        VMMDevReq_CheckSharedModules = 214,
        VMMDevReq_GetPageSharingStatus = 215,
        VMMDevReq_DebugIsPageShared = 216,
        VMMDevReq_GetSessionId = 217,
        VMMDevReq_WriteCoreDump = 218,
        VMMDevReq_GuestHeartbeat = 219,
        VMMDevReq_HeartbeatConfigure = 220,
        VMMDevReq_SizeHack = 0x7fffffff
    }

    public enum VboxDevPowerState
    {
        Invalid = 0,
        Pause = 1,
        PowerOff = 2,
        SaveState = 3,
        SizeHack = 0x7FFFFFFF
    }
}
