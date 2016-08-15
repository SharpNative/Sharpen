#define enum_Sharpen_Arch_IDT_Type_TASK32 (0x5)
#define enum_Sharpen_Arch_IDT_Type_INT16 (0x6)
#define enum_Sharpen_Arch_IDT_Type_TRAP16 (0x7)
#define enum_Sharpen_Arch_IDT_Type_INT32 (0xE)
#define enum_Sharpen_Arch_IDT_Type_TRAP32 (0xF)
#define enum_Sharpen_ErrorCode_SUCCESS (0)
#define enum_Sharpen_ErrorCode_EPERM (1)
#define enum_Sharpen_ErrorCode_ENOENT (2)
#define enum_Sharpen_ErrorCode_ESRCH (3)
#define enum_Sharpen_ErrorCode_EINTR (4)
#define enum_Sharpen_ErrorCode_EIO (5)
#define enum_Sharpen_ErrorCode_ENXIO (6)
#define enum_Sharpen_ErrorCode_E2BIG (7)
#define enum_Sharpen_ErrorCode_ENOEXEC (8)
#define enum_Sharpen_ErrorCode_EBADF (9)
#define enum_Sharpen_ErrorCode_ECHILD (10)
#define enum_Sharpen_ErrorCode_EAGAIN (11)
#define enum_Sharpen_ErrorCode_ENOMEM (12)
#define enum_Sharpen_ErrorCode_EACCES (13)
#define enum_Sharpen_ErrorCode_EFAULT (14)
#define enum_Sharpen_ErrorCode_ENOTBLK (15)
#define enum_Sharpen_ErrorCode_EBUSY (16)
#define enum_Sharpen_ErrorCode_EEXIST (17)
#define enum_Sharpen_ErrorCode_EXDEV (18)
#define enum_Sharpen_ErrorCode_ENODEV (19)
#define enum_Sharpen_ErrorCode_ENOTDIR (20)
#define enum_Sharpen_ErrorCode_EISDIR (21)
#define enum_Sharpen_ErrorCode_EINVAL (22)
#define enum_Sharpen_ErrorCode_ENFILE (23)
#define enum_Sharpen_ErrorCode_EMFILE (24)
#define enum_Sharpen_ErrorCode_ENOTTY (25)
#define enum_Sharpen_ErrorCode_ETXTBSY (26)
#define enum_Sharpen_ErrorCode_EFBIG (27)
#define enum_Sharpen_ErrorCode_ENOSPC (28)
#define enum_Sharpen_ErrorCode_ESPIPE (29)
#define enum_Sharpen_ErrorCode_EROFS (30)
#define enum_Sharpen_ErrorCode_EMLINK (31)
#define enum_Sharpen_ErrorCode_EPIPE (32)
#define enum_Sharpen_ErrorCode_EDOM (33)
#define enum_Sharpen_ErrorCode_ERANGE (34)
#define enum_Sharpen_Exec_ELFLoader_Ident_EI_MAG0 (0)
#define enum_Sharpen_Exec_ELFLoader_Ident_EI_MAG1 (1)
#define enum_Sharpen_Exec_ELFLoader_Ident_EI_MAG2 (2)
#define enum_Sharpen_Exec_ELFLoader_Ident_EI_MAG3 (3)
#define enum_Sharpen_Exec_ELFLoader_Ident_EI_CLASS (4)
#define enum_Sharpen_Exec_ELFLoader_Ident_EI_DATA (5)
#define enum_Sharpen_Exec_ELFLoader_Ident_EI_VERSION (6)
#define enum_Sharpen_Exec_ELFLoader_Ident_EI_OSABI (7)
#define enum_Sharpen_Exec_ELFLoader_Ident_EI_ABIVER (8)
#define enum_Sharpen_Exec_ELFLoader_Ident_EI_PAD (9)
#define enum_Sharpen_Exec_ELFLoader_ExecutableType_ET_NONE (0)
#define enum_Sharpen_Exec_ELFLoader_ExecutableType_ET_REL (1)
#define enum_Sharpen_Exec_ELFLoader_ExecutableType_ET_EXEC (2)
#define enum_Sharpen_Exec_ELFLoader_ExecutableType_ET_SHARE (3)
#define enum_Sharpen_Exec_ELFLoader_ExecutableType_ET_CORE (4)
#define enum_Sharpen_Exec_ELFLoader_MachineType_EM_SPARC (0x02)
#define enum_Sharpen_Exec_ELFLoader_MachineType_EM_X86 (0x03)
#define enum_Sharpen_Exec_ELFLoader_MachineType_EM_MIPS (0x08)
#define enum_Sharpen_Exec_ELFLoader_MachineType_EM_PPC (0x14)
#define enum_Sharpen_Exec_ELFLoader_MachineType_EM_ARM (0x28)
#define enum_Sharpen_Exec_ELFLoader_MachineType_EM_SUPERH (0x2A)
#define enum_Sharpen_Exec_ELFLoader_MachineType_EM_IA64 (0x32)
#define enum_Sharpen_Exec_ELFLoader_MachineType_EM_X86_64 (0x3E)
#define enum_Sharpen_Exec_ELFLoader_MachineType_EM_AARCH64 (0xB7)
#define enum_Sharpen_Exec_ELFLoader_SectionHeaderType_SHT_NULL (0)
#define enum_Sharpen_Exec_ELFLoader_SectionHeaderType_SHT_PROGBITS (1)
#define enum_Sharpen_Exec_ELFLoader_SectionHeaderType_SHT_SYMTAB (2)
#define enum_Sharpen_Exec_ELFLoader_SectionHeaderType_SHT_STRTAB (3)
#define enum_Sharpen_Exec_ELFLoader_SectionHeaderType_SHT_RELA (4)
#define enum_Sharpen_Exec_ELFLoader_SectionHeaderType_SHT_HASH (5)
#define enum_Sharpen_Exec_ELFLoader_SectionHeaderType_SHT_DYNAMIC (6)
#define enum_Sharpen_Exec_ELFLoader_SectionHeaderType_SHT_NOTE (7)
#define enum_Sharpen_Exec_ELFLoader_SectionHeaderType_SHT_NOBITS (8)
#define enum_Sharpen_Exec_ELFLoader_SectionHeaderType_SHT_REL (9)
#define enum_Sharpen_Exec_ELFLoader_SectionHeaderType_SHT_SHLIB (10)
#define enum_Sharpen_Exec_ELFLoader_SectionHeaderType_SHT_DYNSYM (11)
#define const_Sharpen_Exec_Syscalls_SYS_EXIT (0)
#define const_Sharpen_Exec_Syscalls_SYS_GETPID (1)
#define const_Sharpen_Exec_Syscalls_SYS_SBRK (2)
#define const_Sharpen_Exec_Syscalls_SYS_FORK (3)
#define const_Sharpen_Exec_Syscalls_SYS_WRITE (4)
#define const_Sharpen_Exec_Syscalls_SYS_READ (5)
#define const_Sharpen_Exec_Syscalls_SYS_OPEN (6)
#define const_Sharpen_Exec_Syscalls_SYS_CLOSE (7)
#define const_Sharpen_Exec_Syscalls_SYS_SEEK (8)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_InvalidRequest (0)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetMouseStatus (1)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_SetMouseStatus (2)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_SetPointerShape (3)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetHostVersion (4)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_Idle (5)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetHostTime (10)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetHypervisorInfo (20)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_SetHypervisorInfo (21)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_RegisterPatchMemory (22)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_DeregisterPatchMemory (23)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_SetPowerStatus (30)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_AcknowledgeEvents (41)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_CtlGuestFilterMask (42)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_ReportGuestInfo (50)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_ReportGuestInfo2 (58)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_ReportGuestStatus (59)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_ReportGuestUserState (74)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetDisplayChangeRequest (51)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_VideoModeSupported (52)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetHeightReduction (53)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetDisplayChangeRequest2 (54)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_ReportGuestCapabilities (55)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_SetGuestCapabilities (56)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_VideoModeSupported2 (57)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetDisplayChangeRequestEx (80)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_VideoAccelEnable (70)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_VideoAccelFlush (71)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_VideoSetVisibleRegion (72)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetSeamlessChangeRequest (73)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_QueryCredentials (100)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_ReportCredentialsJudgement (101)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_ReportGuestStats (110)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetMemBalloonChangeRequest (111)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetStatisticsChangeRequest (112)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_ChangeMemBalloon (113)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetVRDPChangeRequest (150)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_LogString (200)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetCpuHotPlugRequest (210)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_SetCpuHotPlugStatus (211)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_RegisterSharedModule (212)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_UnregisterSharedModule (213)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_CheckSharedModules (214)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetPageSharingStatus (215)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_DebugIsPageShared (216)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetSessionId (217)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_WriteCoreDump (218)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GuestHeartbeat (219)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_HeartbeatConfigure (220)
#define enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_SizeHack (0x7fffffff)
#define enum_Sharpen_Drivers_Other_VboxDevPowerState_Invalid (0)
#define enum_Sharpen_Drivers_Other_VboxDevPowerState_Pause (1)
#define enum_Sharpen_Drivers_Other_VboxDevPowerState_PowerOff (2)
#define enum_Sharpen_Drivers_Other_VboxDevPowerState_SaveState (3)
#define enum_Sharpen_Drivers_Other_VboxDevPowerState_SizeHack (0x7FFFFFFF)
#define enum_Sharpen_Arch_GDT_GDT_Data_R (0x00)
#define enum_Sharpen_Arch_GDT_GDT_Data_RA (0x01)
#define enum_Sharpen_Arch_GDT_GDT_Data_RW (0x02)
#define enum_Sharpen_Arch_GDT_GDT_Data_RWA (0x03)
#define enum_Sharpen_Arch_GDT_GDT_Data_RED (0x04)
#define enum_Sharpen_Arch_GDT_GDT_Data_REDA (0x05)
#define enum_Sharpen_Arch_GDT_GDT_Data_RWED (0x06)
#define enum_Sharpen_Arch_GDT_GDT_Data_RWEDA (0x07)
#define enum_Sharpen_Arch_GDT_GDT_Data_E (0x08)
#define enum_Sharpen_Arch_GDT_GDT_Data_EA (0x09)
#define enum_Sharpen_Arch_GDT_GDT_Data_ER (0x0A)
#define enum_Sharpen_Arch_GDT_GDT_Data_ERA (0x0B)
#define enum_Sharpen_Arch_GDT_GDT_Data_EC (0x0C)
#define enum_Sharpen_Arch_GDT_GDT_Data_ECA (0x0D)
#define enum_Sharpen_Arch_GDT_GDT_Data_ERC (0x0E)
#define enum_Sharpen_Arch_GDT_GDT_Data_ERCA (0x0F)
#define enum_Sharpen_Arch_GDT_GDTFlags_DescriptorCodeOrData ((1 << 0x4))
#define enum_Sharpen_Arch_GDT_GDTFlags_Size32 ((1 << 0x6))
#define enum_Sharpen_Arch_GDT_GDTFlags_Present ((1 << 0x7))
#define enum_Sharpen_Arch_GDT_GDTFlags_Available ((1 << 0xC))
#define enum_Sharpen_Arch_GDT_GDTFlags_Granularity ((1 << 0xF))
#define enum_Sharpen_FileSystem_NodeFlags_DIRECTORY ((1 << 0))
#define enum_Sharpen_FileSystem_NodeFlags_FILE ((1 << 1))
#define enum_Sharpen_FileSystem_NodeFlags_DEVICE ((1 << 2))
#define enum_Sharpen_FileSystem_FileMode_O_RDONLY (0)
#define enum_Sharpen_FileSystem_FileMode_O_WRONLY (1)
#define enum_Sharpen_FileSystem_FileMode_O_RDWR (2)
#define enum_Sharpen_FileSystem_FileMode_O_NONE (3)
#define enum_Sharpen_FileSystem_FileWhence_SEEK_SET (0)
#define enum_Sharpen_FileSystem_FileWhence_SEEK_CUR (1)
#define enum_Sharpen_FileSystem_FileWhence_SEEK_END (2)
#define enum_Sharpen_Lib_AudioActions_Master (0)
#define enum_Sharpen_Lib_AudioActions_PCM_OUT (1)
#define enum_Sharpen_Arch_Paging_PageFlags_Present (1)
#define enum_Sharpen_Arch_Paging_PageFlags_Writable ((1 << 0x1))
#define enum_Sharpen_Arch_Paging_PageFlags_UserMode ((1 << 0x2))
#define enum_Sharpen_Arch_Paging_PageFlags_NoCache ((1 << 0x4))
#define enum_Sharpen_Arch_Paging_TableFlags_Present (1)
#define enum_Sharpen_Arch_Paging_TableFlags_Writable ((1 << 0x1))
#define enum_Sharpen_Arch_Paging_TableFlags_UserMode ((1 << 0x2))
#define enum_Sharpen_Arch_Paging_TableFlags_WriteThrough ((1 << 0x3))
#define enum_Sharpen_Arch_Paging_TableFlags_NoCache ((1 << 0x4))
#define enum_Sharpen_Arch_Paging_TableFlags_SizeMegs ((1 << 0x7))
#define enum_Sharpen_Task_TaskPriority_VERYLOW (10)
#define enum_Sharpen_Task_TaskPriority_LOW (20)
#define enum_Sharpen_Task_TaskPriority_NORMAL (30)
#define enum_Sharpen_Task_TaskPriority_HIGH (40)
#define enum_Sharpen_Task_TaskPriority_VERYHIGH (50)

struct class_Sharpen_Arch_CMOS;
struct class_Sharpen_Arch_CPU;
struct class_Sharpen_Arch_FPU;
struct class_Sharpen_Arch_IDT;
struct struct_Sharpen_Arch_IDT_IDT_Entry;
struct struct_Sharpen_Arch_IDT_IDT_Pointer;
struct class_Sharpen_Arch_IRQ;
struct class_Sharpen_Arch_ISR;
struct class_Sharpen_Drivers_Net_rtl8139;
struct class_Sharpen_Exec_ELFLoader;
struct struct_Sharpen_Exec_ELFLoader_ELF32;
struct struct_Sharpen_Exec_ELFLoader_ProgramHeader;
struct struct_Sharpen_Exec_ELFLoader_SectionHeader;
struct class_Sharpen_Exec_Loader;
struct class_Sharpen_Exec_Syscalls;
struct struct_Sharpen_FileSystem_Fat16BPB;
struct struct_Sharpen_FileSystem_FatDirEntry;
struct class_Sharpen_FileSystem_SubDirectory;
struct class_Sharpen_FileSystem_STDOUT;
struct class_Sharpen_Net_DHCP;
struct struct_Sharpen_Net_DHCP_DHCPHeader;
struct class_Sharpen_Net_Network;
struct struct_Sharpen_Net_Network_NetDevice;
struct class_Sharpen_Net_NetworkTools;
struct struct_Sharpen_Task_FileDescriptors;
struct class_Sharpen_Task_Task;
struct class_Sharpen_Task_Tasking;
struct class_Sharpen_Arch_PCI;
struct struct_Sharpen_Arch_PCI_PciDriver;
struct struct_Sharpen_Arch_PCI_PciDevice;
struct class_Sharpen_Arch_PIC;
struct class_Sharpen_Arch_PIT;
struct struct_Sharpen_Arch_Regs;
struct class_Sharpen_Arch_Syscall;
struct class_Sharpen_Utilities_ByteUtil;
struct class_Sharpen_Drivers_Other_VboxDev;
struct struct_Sharpen_Drivers_Other_VboxDev_RequestHeader;
struct struct_Sharpen_Drivers_Other_VboxDev_RequestSessionID;
struct struct_Sharpen_Drivers_Other_VboxDev_RequestGuestInfo;
struct struct_Sharpen_Drivers_Other_VboxDev_RequestPowerState;
struct struct_Sharpen_Drivers_Other_VboxDev_RequestHostTime;
struct class_Sharpen_Drivers_Other_VboxDevFSDriver;
struct class_Sharpen_Drivers_Power_Acpi;
struct struct_Sharpen_Drivers_Power_RDSTH;
struct struct_Sharpen_Drivers_Power_RDSP;
struct struct_Sharpen_Drivers_Power_GenericAddressStructure;
struct struct_Sharpen_Drivers_Power_RSDT;
struct struct_Sharpen_Drivers_Power_FADT;
struct class_Sharpen_Drivers_Sound_IntelHD;
struct class_Sharpen_Drivers_Sound_AC97;
struct struct_Sharpen_Drivers_Sound_AC97_BDL_Entry;
struct class_Sharpen_FileSystem_DevFS;
struct class_Sharpen_FileSystem_Device;
struct struct_Sharpen_FileSystem_DirEntry;
struct class_Sharpen_Collections_Dictionary;
struct class_Sharpen_FileSystem_Fat16;
struct class_Sharpen_Collections_LongIndex;
struct class_Sharpen_Console;
struct class_Sharpen_Arch_GDT;
struct struct_Sharpen_Arch_GDT_GDT_Entry;
struct struct_Sharpen_Arch_GDT_GDT_Pointer;
struct struct_Sharpen_Arch_GDT_TSS;
struct class_Sharpen_Drivers_Block_ATA;
struct struct_Sharpen_Drivers_Block_IDE_Device;
struct class_Sharpen_Drivers_Char_SerialPort;
struct class_Sharpen_Drivers_Char_Keyboard;
struct class_Sharpen_Drivers_Char_KeyboardMap;
struct struct_Sharpen_Drivers_Char_SerialPortComport;
struct class_Sharpen_FileSystem_MountPoint;
struct class_Sharpen_FileSystem_Node;
struct class_Sharpen_FileSystem_VFS;
struct class_Sharpen_Heap;
struct struct_Sharpen_Heap_Block;
struct struct_Sharpen_Heap_BlockDescriptor;
struct class_Sharpen_Collections_Fifo;
struct class_Sharpen_Collections_List;
struct class_Sharpen_Collections_BitArray;
struct class_Sharpen_Lib_Audio;
struct struct_Sharpen_Lib_Audio_SoundDevice;
struct class_Sharpen_Memory;
struct class_Sharpen_Multiboot;
struct struct_Sharpen_Multiboot_Header;
struct struct_Sharpen_Multiboot_MMAP;
struct struct_Sharpen_Multiboot_Module;
struct class_Sharpen_Panic;
struct class_Sharpen_Arch_PortIO;
struct class_Sharpen_Arch_Paging;
struct struct_Sharpen_Arch_Paging_PageTable;
struct struct_Sharpen_Arch_Paging_PageDirectory;
struct class_Sharpen_Program;
struct class_Sharpen_Utilities_String;
struct class_Sharpen_Time;
struct class_Sharpen_Utilities_Util;

typedef void (*delegate_Sharpen_Arch_IRQ_IRQHandler) (struct struct_Sharpen_Arch_Regs* regsPtr);
typedef void (*delegate_Sharpen_Net_Network_TransmitAction) (uint8_t* bytes, uint32_t size);
typedef void (*delegate_Sharpen_Net_Network_GetMACAction) (uint8_t* mac);
typedef void (*delegate_Sharpen_Arch_PCI_PciDriverInit) (struct struct_Sharpen_Arch_PCI_PciDevice dev);
typedef void (*delegate_Sharpen_Arch_PCI_PciDriverExit) (struct struct_Sharpen_Arch_PCI_PciDevice dev);
typedef uint32_t (*delegate_Sharpen_FileSystem_Node_FSRead) (struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
typedef uint32_t (*delegate_Sharpen_FileSystem_Node_FSWrite) (struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
typedef void (*delegate_Sharpen_FileSystem_Node_FSOpen) (struct class_Sharpen_FileSystem_Node* node);
typedef void (*delegate_Sharpen_FileSystem_Node_FSClose) (struct class_Sharpen_FileSystem_Node* node);
typedef struct class_Sharpen_FileSystem_Node* (*delegate_Sharpen_FileSystem_Node_FSFindDir) (struct class_Sharpen_FileSystem_Node* node, char* name);
typedef struct struct_Sharpen_FileSystem_DirEntry* (*delegate_Sharpen_FileSystem_Node_FSReaddir) (struct class_Sharpen_FileSystem_Node* node, uint32_t index);
typedef void (*delegate_Sharpen_Lib_Audio_SoundDeviceWriter) (int32_t action, uint32_t value);
typedef uint32_t (*delegate_Sharpen_Lib_Audio_SoundDeviceReader) (int32_t action);

struct struct_Sharpen_Arch_IDT_IDT_Entry
{
	uint16_t field_AddressLow;
	uint16_t field_Selector;
	uint8_t field_Zero;
	uint8_t field_Flags;
	uint16_t field_AddressHigh;
} __attribute__((packed));
struct struct_Sharpen_Arch_IDT_IDT_Pointer
{
	uint16_t field_Limit;
	uint32_t field_BaseAddress;
} __attribute__((packed));
struct struct_Sharpen_Exec_ELFLoader_ELF32
{
	uint8_t field_Ident[16];
	uint16_t field_Type;
	uint16_t field_Machine;
	uint32_t field_Version;
	uint32_t field_Entry;
	uint32_t field_PhOff;
	uint32_t field_ShOff;
	uint32_t field_Flags;
	uint16_t field_EhSize;
	uint16_t field_PhEntSize;
	uint16_t field_PhNum;
	uint16_t field_ShEntSize;
	uint16_t field_ShNum;
	uint16_t field_ShnStrNdx;
};
struct struct_Sharpen_Exec_ELFLoader_ProgramHeader
{
	uint16_t field_Type;
	uint32_t field_Offset;
	uint32_t field_VirtAddress;
	uint32_t field_PhysAddress;
	uint16_t field_FileSize;
	uint16_t field_MemSize;
	uint16_t field_Flags;
	uint16_t field_Alignment;
};
struct struct_Sharpen_Exec_ELFLoader_SectionHeader
{
	uint32_t field_Name;
	uint32_t field_Type;
	uint32_t field_Flags;
	uint32_t field_Address;
	uint32_t field_Offset;
	uint32_t field_Size;
	uint32_t field_Link;
	uint32_t field_Info;
	uint32_t field_AddressAlignment;
	uint32_t field_EntrySize;
};
struct struct_Sharpen_FileSystem_Fat16BPB
{
	uint8_t field_Jump[3];
	char field_OemID[8];
	uint16_t field_BytesPerSector;
	uint8_t field_SectorsPerCluster;
	uint16_t field_ReservedSectors;
	uint8_t field_NumFats;
	uint16_t field_NumDirEntries;
	uint16_t field_TotalSectorsLogical;
	uint8_t field_MediaDescriptorType;
	uint16_t field_SectorsPerFat16;
	uint16_t field_SectorsPerTrack;
	uint16_t field_NumHeadsOrSides;
	uint32_t field_HiddenSectors;
	uint32_t field_LargeAmountOfSectors;
	uint8_t field_DriveNumber;
	uint8_t field_Flags;
	uint8_t field_Signature;
	char field_VolumeID[4];
	char field_Label[11];
	char field_SysIdentifier[8];
} __attribute__((packed));
struct struct_Sharpen_FileSystem_FatDirEntry
{
	char field_Name[11];
	uint8_t field_Attribs;
	uint8_t field_Reserved;
	uint8_t field_CreationTime;
	uint16_t field_TimeCreated;
	uint16_t field_DateCreated;
	uint16_t field_LastAccessedDate;
	uint16_t field_ClusterNumberHi;
	uint16_t field_LastModTime;
	uint16_t field_LastModDate;
	uint16_t field_ClusterNumberLo;
	uint16_t field_Size;
	char field_Bullshit[2];
} __attribute__((packed));
struct struct_Sharpen_Net_DHCP_DHCPHeader
{
	uint8_t field_Opcode;
	uint8_t field_Htype;
	uint8_t field_Hlen;
	uint8_t field_HopCount;
	uint32_t field_Xid;
	uint16_t field_SecCount;
	uint16_t field_Flags;
	uint32_t field_ClientIp;
	uint32_t field_YourIp;
	uint32_t field_ServerIP;
	uint32_t field_ClientEth;
	uint8_t field_Reserved[10];
	char field_ServerName[64];
	char field_bootFileName[128];
} __attribute__((packed));
struct struct_Sharpen_Net_Network_NetDevice
{
	uint32_t field_ID;
	delegate_Sharpen_Net_Network_TransmitAction field_Transmit;
	delegate_Sharpen_Net_Network_GetMACAction field_GetMac;
};
struct struct_Sharpen_Task_FileDescriptors
{
	int32_t field_Used;
	int32_t field_Capacity;
	struct class_Sharpen_FileSystem_Node** field_Nodes;
	uint32_t* field_Offsets;
};
struct struct_Sharpen_Arch_PCI_PciDriver
{
	char* field_Name;
	delegate_Sharpen_Arch_PCI_PciDriverInit field_Init;
	delegate_Sharpen_Arch_PCI_PciDriverExit field_Exit;
};
struct struct_Sharpen_Arch_PCI_PciDevice
{
	uint16_t field_Bus;
	uint16_t field_Slot;
	uint16_t field_Function;
	uint16_t field_Vendor;
	uint16_t field_Device;
	struct struct_Sharpen_Arch_PCI_PciDriver field_Driver;
	uint16_t field_Port1;
	uint16_t field_Port2;
};
struct struct_Sharpen_Arch_Regs
{
	int32_t field_GS;
	int32_t field_FD;
	int32_t field_ES;
	int32_t field_DS;
	int32_t field_EDI;
	int32_t field_ESI;
	int32_t field_EBP;
	int32_t field_Unused;
	int32_t field_EBX;
	int32_t field_EDX;
	int32_t field_ECX;
	int32_t field_EAX;
	int32_t field_IntNum;
	int32_t field_Error;
	int32_t field_EIP;
	int32_t field_CS;
	int32_t field_EFlags;
	int32_t field_ESP;
	int32_t field_SS;
} __attribute__((packed));
struct struct_Sharpen_Drivers_Other_VboxDev_RequestHeader
{
	uint32_t field_Size;
	uint32_t field_Version;
	int32_t field_requestType;
	int32_t field_rc;
	uint32_t field_reserved_1;
	uint32_t field_reserved_2;
};
struct struct_Sharpen_Drivers_Other_VboxDev_RequestSessionID
{
	struct struct_Sharpen_Drivers_Other_VboxDev_RequestHeader field_header;
	uint64_t field_idSession;
};
struct struct_Sharpen_Drivers_Other_VboxDev_RequestGuestInfo
{
	struct struct_Sharpen_Drivers_Other_VboxDev_RequestHeader field_header;
	uint32_t field_interfaceVersion;
	uint32_t field_osType;
};
struct struct_Sharpen_Drivers_Other_VboxDev_RequestPowerState
{
	struct struct_Sharpen_Drivers_Other_VboxDev_RequestHeader field_header;
	int32_t field_PowerState;
};
struct struct_Sharpen_Drivers_Other_VboxDev_RequestHostTime
{
	struct struct_Sharpen_Drivers_Other_VboxDev_RequestHeader field_header;
	uint64_t field_Time;
};
struct struct_Sharpen_Drivers_Power_RDSTH
{
	char field_Signature[4];
	uint32_t field_Length;
	uint8_t field_Revision;
	uint8_t field_Checksum;
	char field_OEMID[6];
	char field_OEMTableID[8];
	uint32_t field_OEMRevision;
	uint32_t field_CreatorID;
	uint32_t field_CreatorRevision;
} __attribute__((packed));
struct struct_Sharpen_Drivers_Power_RDSP
{
	char field_Signature[8];
	uint8_t field_Checksum;
	char field_OEMID[6];
	uint8_t field_Revision;
	uint32_t field_RsdtAddress;
} __attribute__((packed));
struct struct_Sharpen_Drivers_Power_GenericAddressStructure
{
	uint8_t field_AddressSpace;
	uint8_t field_BitWidth;
	uint8_t field_BitOffset;
	uint8_t field_AccessSize;
	uint64_t field_Address;
} __attribute__((packed));
struct struct_Sharpen_Drivers_Power_RSDT
{
	struct struct_Sharpen_Drivers_Power_RDSTH field_header;
	uint32_t field_firstSDT;
} __attribute__((packed));
struct struct_Sharpen_Drivers_Power_FADT
{
	struct struct_Sharpen_Drivers_Power_RDSTH field_Header;
	uint32_t field_FirmwareCtrl;
	uint32_t field_Dsdt;
	uint8_t field_Reserved;
	uint8_t field_PreferredPowerManagementProfile;
	uint16_t field_SCI_Interrupt;
	uint32_t field_SMI_CommandPort;
	uint8_t field_AcpiEnable;
	uint8_t field_AcpiDisable;
	uint8_t field_S4BIOS_REQ;
	uint8_t field_PSTATE_Control;
	uint32_t field_PM1aEventBlock;
	uint32_t field_PM1bEventBlock;
	uint32_t field_PM1aControlBlock;
	uint32_t field_PM1bControlBlock;
	uint32_t field_PM2ControlBlock;
	uint32_t field_PMTimerBlock;
	uint32_t field_GPE0Block;
	uint32_t field_GPE1Block;
	uint8_t field_PM1EventLength;
	uint8_t field_PM1ControlLength;
	uint8_t field_PM2ControlLength;
	uint8_t field_PMTimerLength;
	uint8_t field_GPE0Length;
	uint8_t field_GPE1Length;
	uint8_t field_GPE1Base;
	uint8_t field_CStateControl;
	uint16_t field_WorstC2Latency;
	uint16_t field_WorstC3Latency;
	uint16_t field_FlushSize;
	uint16_t field_FlushStride;
	uint8_t field_DutyOffset;
	uint8_t field_DutyWidth;
	uint8_t field_DayAlarm;
	uint8_t field_MonthAlarm;
	uint8_t field_Century;
	uint16_t field_BootArchitectureFlags;
	uint8_t field_Reserved2;
	uint32_t field_Flags;
	struct struct_Sharpen_Drivers_Power_GenericAddressStructure field_ResetReg;
	uint8_t field_ResetValue;
	uint8_t field_Reserved3[3];
	uint64_t field_X_FirmwareControl;
	uint64_t field_X_Dsdt;
	struct struct_Sharpen_Drivers_Power_GenericAddressStructure field_X_PM1aEventBlock;
	struct struct_Sharpen_Drivers_Power_GenericAddressStructure field_X_PM1bEventBlock;
	struct struct_Sharpen_Drivers_Power_GenericAddressStructure field_X_PM1aControlBlock;
	struct struct_Sharpen_Drivers_Power_GenericAddressStructure field_X_PM1bControlBlock;
	struct struct_Sharpen_Drivers_Power_GenericAddressStructure field_X_PM2ControlBlock;
	struct struct_Sharpen_Drivers_Power_GenericAddressStructure field_X_PMTimerBlock;
	struct struct_Sharpen_Drivers_Power_GenericAddressStructure field_X_GPE0Block;
	struct struct_Sharpen_Drivers_Power_GenericAddressStructure field_X_GPE1Block;
} __attribute__((packed));
struct struct_Sharpen_Drivers_Sound_AC97_BDL_Entry
{
	void* field_pointer;
	int32_t field_cl;
};
struct struct_Sharpen_FileSystem_DirEntry
{
	uint32_t field_Ino;
	int32_t field_Offset;
	uint16_t field_Reclen;
	uint8_t field_Type;
	char field_Name[256];
};
struct struct_Sharpen_Arch_GDT_GDT_Entry
{
	uint16_t field_LimitLow;
	uint16_t field_BaseLow;
	uint8_t field_BaseMid;
	uint8_t field_Access;
	uint8_t field_Granularity;
	uint8_t field_BaseHigh;
} __attribute__((packed));
struct struct_Sharpen_Arch_GDT_GDT_Pointer
{
	uint16_t field_Limit;
	uint32_t field_BaseAddress;
} __attribute__((packed));
struct struct_Sharpen_Arch_GDT_TSS
{
	uint32_t field_PreviousTSS;
	uint32_t field_ESP0;
	uint32_t field_SS0;
	uint32_t field_ESP1;
	uint32_t field_SS1;
	uint32_t field_ESP2;
	uint32_t field_SS2;
	uint32_t field_CR3;
	uint32_t field_EIP;
	uint32_t field_EFlags;
	uint32_t field_EAX;
	uint32_t field_ECX;
	uint32_t field_EDX;
	uint32_t field_EBX;
	uint32_t field_ESP;
	uint32_t field_EBP;
	uint32_t field_ESI;
	uint32_t field_EDI;
	uint32_t field_ES;
	uint32_t field_CS;
	uint32_t field_SS;
	uint32_t field_DS;
	uint32_t field_FS;
	uint32_t field_GS;
	uint32_t field_LDT;
	uint16_t field_Trap;
	uint16_t field_IOMap;
} __attribute__((packed));
struct struct_Sharpen_Drivers_Block_IDE_Device
{
	int32_t field_Exists;
	uint8_t field_Channel;
	uint16_t field_BasePort;
	uint8_t field_Drive;
	uint64_t field_Size;
	uint32_t field_CmdSet;
	uint16_t field_Type;
	uint16_t field_Capabilities;
	uint16_t field_Cylinders;
	uint16_t field_Heads;
	uint16_t field_Sectorspt;
	char* field_Name;
};
struct struct_Sharpen_Drivers_Char_SerialPortComport
{
	char* prop_Name;
	uint16_t prop_Address;
	struct class_Sharpen_Collections_Fifo* prop_Buffer;
};
struct struct_Sharpen_Heap_Block
{
	int32_t field_Size;
	int32_t field_Used;
	struct struct_Sharpen_Heap_Block* field_Prev;
	struct struct_Sharpen_Heap_Block* field_Next;
	struct struct_Sharpen_Heap_BlockDescriptor* field_Descriptor;
};
struct struct_Sharpen_Heap_BlockDescriptor
{
	int32_t field_FreeSpace;
	struct struct_Sharpen_Heap_BlockDescriptor* field_Next;
	struct struct_Sharpen_Heap_Block* field_First;
};
struct struct_Sharpen_Lib_Audio_SoundDevice
{
	char* field_Name;
	delegate_Sharpen_Lib_Audio_SoundDeviceWriter field_Writer;
	delegate_Sharpen_Lib_Audio_SoundDeviceReader field_Reader;
};
struct struct_Sharpen_Multiboot_Header
{
	uint32_t field_Flags;
	uint32_t field_MemLow;
	uint32_t field_MemHi;
	uint32_t field_BootDevice;
	void* field_CMDLine;
	uint32_t field_ModsCount;
	void* field_ModsAddr;
	uint32_t field_Num;
	uint32_t field_Size;
	uint32_t field_Addr;
	uint32_t field_Shndx;
	uint32_t field_MMAPLen;
	void* field_MMAPAddr;
	uint32_t field_DrivesLen;
	void* field_DriverAddr;
	void* field_ConfigTable;
	void* field_BootloaderName;
	void* field_ApmTable;
	void* field_VbeCTRLInfo;
	void* field_VbeModeInfo;
	uint32_t field_VbeMode;
	void* field_VbeInterfaceSeg;
	void* field_VbeInterfaceOff;
	uint32_t field_VbeInterfaceLen;
};
struct struct_Sharpen_Multiboot_MMAP
{
	uint32_t field_Size;
	uint64_t field_Addr;
	uint64_t field_Length;
	uint32_t field_Type;
};
struct struct_Sharpen_Multiboot_Module
{
	void* field_Start;
	void* field_End;
	void* field_CMDLine;
	uint32_t field_Padding;
};
struct struct_Sharpen_Arch_Paging_PageTable
{
	int32_t field_pages[1024];
};
struct struct_Sharpen_Arch_Paging_PageDirectory
{
	int32_t field_tables[1024];
};

struct class_Sharpen_Arch_CMOS
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	uint16_t CMOS_CMD;
	uint16_t CMOS_DATA;
	uint8_t CMOS_RTC_SECONDS;
	uint8_t CMOS_RTC_MINUTES;
	uint8_t CMOS_RTC_HOURS;
	uint8_t CMOS_RTC_WEEKDAY;
	uint8_t CMOS_RTC_MONTHDAY;
	uint8_t CMOS_RTC_MONTH;
	uint8_t CMOS_RTC_YEAR;
	uint8_t CMOS_STATUS_A;
	uint8_t CMOS_STATUS_B;
	int32_t CMOS_RTC_UPDATING;
	int32_t CMOS_RTC_24H;
	int32_t CMOS_RTC_BIN_MODE;
	int32_t CMOS_RTC_HOURS_PM;
} classStatics_Sharpen_Arch_CMOS = {
	0x70,
	0x71,
	0x00,
	0x02,
	0x04,
	0x06,
	0x07,
	0x08,
	0x09,
	0x0A,
	0x0B,
	0,
	0,
	0,
	0,
};
struct class_Sharpen_Arch_CPU
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Arch_CPU = {
};
struct class_Sharpen_Arch_FPU
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Arch_FPU = {
};
struct class_Sharpen_Arch_IDT
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	uint8_t FLAG_ISR;
	uint8_t FLAG_IRQ;
	uint8_t FLAG_INT;
	uint8_t FLAG_TRAP;
	struct struct_Sharpen_Arch_IDT_IDT_Entry* m_entries;
	struct struct_Sharpen_Arch_IDT_IDT_Pointer m_ptr;
} classStatics_Sharpen_Arch_IDT = {
	0,
	0,
	0,
	0,
	0,
	{0},
};
struct class_Sharpen_Arch_IRQ
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	uint8_t MASTER_OFFSET;
	uint8_t SLAVE_OFFSET;
	delegate_Sharpen_Arch_IRQ_IRQHandler handlers[16];
} classStatics_Sharpen_Arch_IRQ = {
	32,
	40,
	{ null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null },
};
struct class_Sharpen_Arch_ISR
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	char* m_errorCodes[19];
} classStatics_Sharpen_Arch_ISR = {
	{
            "Divide by zero",
            "Debug",
            "Non-maskable interrupt",
            "Breakpoint",
            "Detected overflow",
            "Out-of-bounds",
            "Invalid opcode",
            "No FPU",
            "Double fault",
            "FPU segment overrun",
            "Bad TSS",
            "Segment not present",
            "Stack fault",
            "General protection fault",
            "Page fault",
            "?",
            "FPU exception",
            "Alignment check",
            "Machine check"
        },
};
struct class_Sharpen_Drivers_Net_rtl8139
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	uint16_t CONFIG_1;
	uint16_t REG_MAC;
	uint16_t REG_BUF;
	uint16_t REG_CMD;
	uint16_t REG_IM;
	uint16_t REG_IS;
	uint16_t REG_TC;
	uint16_t REG_RC;
	uint16_t REG_MS;
	uint16_t REG_TSAD0;
	uint16_t REG_TSAD1;
	uint16_t REG_TSAD2;
	uint16_t REG_TSAD3;
	uint16_t REG_TSD0;
	uint16_t REG_TSD1;
	uint16_t REG_TSD2;
	uint16_t REG_TSD3;
	uint16_t CMD_RXEMPTY;
	uint16_t CMD_TXE;
	uint16_t CMD_RXE;
	uint16_t CMD_RST;
	uint8_t MS_LINKB;
	uint8_t MS_SPEED_10;
	uint8_t* m_mac;
	uint16_t m_io_base;
	int32_t m_linkSpeed;
	int32_t m_linkFail;
	int32_t m_irqNum;
	int32_t m_curBuffer;
	uint8_t* m_buffer;
	uint8_t* m_transmit0;
	uint8_t* m_transmit1;
	uint8_t* m_transmit2;
	uint8_t* m_transmit3;
} classStatics_Sharpen_Drivers_Net_rtl8139 = {
	0x52,
	0x00,
	0x30,
	0x37,
	0x3C,
	0x3E,
	0x40,
	0x44,
	0x58,
	0x20,
	0x24,
	0x28,
	0x2C,
	0x10,
	0x14,
	0x18,
	0x1C,
	0x01,
	0x04,
	0x08,
	0x10,
	0x04,
	0x08,
	0,
	0,
	0,
	true,
	0,
	0,
	0,
	0,
	0,
	0,
	0,
};
struct class_Sharpen_Exec_ELFLoader
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Exec_ELFLoader = {
};
struct class_Sharpen_Exec_Loader
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Exec_Loader = {
};
struct class_Sharpen_Exec_Syscalls
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Exec_Syscalls = {
};
struct class_Sharpen_FileSystem_SubDirectory
{
	int32_t usage_count;
	void** lookup_table;
	uint32_t field_Length;
	struct struct_Sharpen_FileSystem_FatDirEntry* field_DirEntries;
};

struct
{
} classStatics_Sharpen_FileSystem_SubDirectory = {
};
struct class_Sharpen_FileSystem_STDOUT
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_FileSystem_STDOUT = {
};
struct class_Sharpen_Net_DHCP
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Net_DHCP = {
};
struct class_Sharpen_Net_Network
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	struct struct_Sharpen_Net_Network_NetDevice m_dev;
	struct struct_Sharpen_Net_Network_NetDevice prop_Device;
} classStatics_Sharpen_Net_Network = {
	{0},
	{0},
};
struct class_Sharpen_Net_NetworkTools
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Net_NetworkTools = {
};
struct class_Sharpen_Task_Task
{
	int32_t usage_count;
	void** lookup_table;
	int32_t field_PID;
	int32_t field_GID;
	int32_t field_UID;
	struct struct_Sharpen_Task_FileDescriptors field_FileDescriptors;
	struct struct_Sharpen_Arch_Paging_PageDirectory* field_PageDir;
	int32_t* field_Stack;
	int32_t* field_KernelStack;
	void* field_FPUContext;
	void* field_DataEnd;
	struct class_Sharpen_Task_Task* field_Next;
	int32_t field_TimeFull;
	int32_t field_TimeLeft;
};

struct
{
} classStatics_Sharpen_Task_Task = {
};
struct class_Sharpen_Task_Tasking
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	int32_t m_lastPid;
	int32_t m_taskingEnabled;
	struct class_Sharpen_Task_Task* prop_KernelTask;
	struct class_Sharpen_Task_Task* prop_CurrentTask;
} classStatics_Sharpen_Task_Tasking = {
	0,
	false,
	0,
	0,
};
struct class_Sharpen_Arch_PCI
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	uint16_t COMMAND;
	struct struct_Sharpen_Arch_PCI_PciDevice* m_devices;
	uint32_t m_currentdevice;
} classStatics_Sharpen_Arch_PCI = {
	0x04,
	0,
	0,
};
struct class_Sharpen_Arch_PIC
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	uint16_t MASTER_PIC_CMD;
	uint16_t MASTER_PIC_DATA;
	uint16_t SLAVE_PIC_CMD;
	uint16_t SLAVE_PIC_DATA;
	uint8_t PIC_INIT;
	uint8_t PIC_EOI;
	uint8_t PIC_8086;
	uint8_t PIC_CASCADE;
} classStatics_Sharpen_Arch_PIC = {
	0x20,
	0x21,
	0xA0,
	0xA1,
	0x11,
	0x20,
	0x01,
	0x04,
};
struct class_Sharpen_Arch_PIT
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	int32_t PIT_OSCILLATOR;
	uint16_t PIT_DATA;
	uint16_t PIT_CMD;
	int32_t m_frequency;
	int32_t prop_Frequency;
	int32_t prop_SubTicks;
	int32_t prop_FullTicks;
} classStatics_Sharpen_Arch_PIT = {
	1193182,
	0x40,
	0x43,
	0,
	0,
	0,
	0,
};
struct class_Sharpen_Arch_Syscall
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Arch_Syscall = {
};
struct class_Sharpen_Utilities_ByteUtil
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Utilities_ByteUtil = {
};
struct class_Sharpen_Drivers_Other_VboxDev
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	struct struct_Sharpen_Arch_PCI_PciDevice m_dev;
	int32_t m_initalized;
} classStatics_Sharpen_Drivers_Other_VboxDev = {
	{0},
	0,
};
struct class_Sharpen_Drivers_Other_VboxDevFSDriver
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	int32_t m_numCommands;
	char* m_commands[3];
} classStatics_Sharpen_Drivers_Other_VboxDevFSDriver = {
	3,
	{
            "sessionid",
            "powerstate",
            "hosttime"
        },
};
struct class_Sharpen_Drivers_Power_Acpi
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	struct struct_Sharpen_Drivers_Power_RDSP* rdsp;
	struct struct_Sharpen_Drivers_Power_RSDT* rsdt;
	struct struct_Sharpen_Drivers_Power_FADT* fadt;
	uint16_t SLP_TYPa;
	uint16_t SLP_TYPb;
	uint32_t SLP_EN;
} classStatics_Sharpen_Drivers_Power_Acpi = {
	0,
	0,
	0,
	0,
	0,
	0,
};
struct class_Sharpen_Drivers_Sound_IntelHD
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Drivers_Sound_IntelHD = {
};
struct class_Sharpen_Drivers_Sound_AC97
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	int32_t BDBAR;
	int32_t CIV;
	int32_t LVI;
	int32_t m_sr;
	int32_t PICB;
	int32_t CR;
	int32_t CR_RPBM;
	int32_t CR_PR;
	int32_t CR_LVBIE;
	int32_t CR_FEIE;
	int32_t CR_IOCE;
	int32_t RESET;
	int32_t MASTER_VOLUME;
	int32_t AUX_OUT_VALUE;
	int32_t MONO_VOLUME;
	int32_t PCM_OUT_VOLUME;
	int32_t CL_BUP;
	int32_t CL_IOC;
	uint16_t SR_DCH;
	uint16_t SR_CELV;
	uint16_t SR_LVBCI;
	uint16_t SR_BCIS;
	uint16_t SR_FIFOE;
	struct struct_Sharpen_Arch_PCI_PciDevice m_dev;
	uint16_t m_nambar;
	uint16_t m_nabmbar;
	uint16_t m_lvi;
	uint16_t** m_bufs;
	struct struct_Sharpen_Drivers_Sound_AC97_BDL_Entry* m_bdls;
} classStatics_Sharpen_Drivers_Sound_AC97 = {
	0x10,
	0x14,
	0x15,
	0x16,
	0x18,
	0x1B,
	0,
	0,
	0,
	0,
	0,
	0x00,
	0x02,
	0x04,
	0x06,
	0x18,
	0,
	0,
	0,
	0,
	0,
	0,
	0,
	{0},
	0,
	0,
	0,
	0,
	0,
};
struct class_Sharpen_FileSystem_DevFS
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	struct class_Sharpen_Collections_Dictionary* m_devices;
	struct class_Sharpen_FileSystem_Node* m_currentNode;
	int32_t prop_Count;
} classStatics_Sharpen_FileSystem_DevFS = {
	0,
	0,
	0,
};
struct class_Sharpen_FileSystem_Device
{
	int32_t usage_count;
	void** lookup_table;
	char* field_Name;
	struct class_Sharpen_FileSystem_Node* field_node;
};

struct
{
} classStatics_Sharpen_FileSystem_Device = {
};
struct class_Sharpen_Collections_Dictionary
{
	int32_t usage_count;
	void** lookup_table;
	struct class_Sharpen_Collections_LongIndex* field_m_index;
	struct class_Sharpen_Collections_List* field_m_values;
};

struct
{
} classStatics_Sharpen_Collections_Dictionary = {
};
struct class_Sharpen_FileSystem_Fat16
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	int32_t FirstPartitonEntry;
	int32_t ENTRYACTIVE;
	int32_t ENTRYBEGINHEAD;
	int32_t ENTRYBEGINCYLSEC;
	int32_t ENTRYTYPE;
	int32_t ENTRYENDHEAD;
	int32_t ENTRYENDCYLSEC;
	int32_t ENTRYNUMSECTORSBETWEEN;
	int32_t ENTRYNUMSECTORS;
	int32_t FAT_FREE;
	int32_t FAT_EOF;
	struct class_Sharpen_FileSystem_Node* m_dev;
	int32_t m_bytespersector;
	int32_t m_beginLBA;
	int32_t m_clusterBeginLBA;
	int32_t m_beginDataLBA;
	struct struct_Sharpen_FileSystem_Fat16BPB* m_bpb;
	struct struct_Sharpen_FileSystem_FatDirEntry* m_dirEntries;
	uint32_t m_numDirEntries;
	uint32_t m_sectorOffset;
	uint8_t LFN;
} classStatics_Sharpen_FileSystem_Fat16 = {
	0x1BE,
	0,
	0x01,
	0x02,
	0x04,
	0x05,
	0x06,
	0x08,
	0x0C,
	0x00,
	0xFFF8,
	0,
	0,
	0,
	0,
	0,
	0,
	0,
	0,
	0,
	0x0F,
};
struct class_Sharpen_Collections_LongIndex
{
	int32_t usage_count;
	void** lookup_table;
	int32_t field_m_currentCap;
	int64_t* prop_Item;
	int32_t prop_Count;
	int32_t prop_Capacity;
};

struct
{
} classStatics_Sharpen_Collections_LongIndex = {
};
struct class_Sharpen_Console
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	uint8_t* vidmem;
	int32_t prop_X;
	int32_t prop_Y;
	uint8_t prop_Attribute;
} classStatics_Sharpen_Console = {
	0,
	0,
	0,
	0x07,
};
struct class_Sharpen_Arch_GDT
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	struct struct_Sharpen_Arch_GDT_GDT_Entry* m_entries;
	struct struct_Sharpen_Arch_GDT_GDT_Pointer m_ptr;
	struct struct_Sharpen_Arch_GDT_TSS* prop_TSS_Entry;
} classStatics_Sharpen_Arch_GDT = {
	0,
	{0},
	0,
};
struct class_Sharpen_Drivers_Block_ATA
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	uint16_t ATA_PRIMARY_IO;
	uint16_t ATA_SECONDARY_IO;
	uint8_t ATA_PRIMARY;
	uint8_t ATA_SECONDARY;
	uint8_t ATA_MASTER;
	uint8_t ATA_SLAVE;
	uint8_t ATA_REG_DATA;
	uint8_t ATA_REG_ERROR;
	uint8_t ATA_REG_FEATURE;
	uint8_t ATA_REG_SECCNT;
	uint8_t ATA_REG_LBALO;
	uint8_t ATA_REG_LBAMID;
	uint8_t ATA_REG_LBAHI;
	uint8_t ATA_REG_DRIVE;
	uint8_t ATA_REG_CMD;
	uint8_t ATA_REG_STATUS;
	uint8_t ATA_REG_SECCOUNT1;
	uint8_t ATA_REG_LBA3;
	uint8_t ATA_REG_LBA4;
	uint8_t ATA_REG_LBA5;
	uint8_t ATA_REG_CONTROL;
	uint8_t ATA_REG_ALTSTATUS;
	uint8_t ATA_REG_DEVADDRESS;
	uint8_t ATA_CMD_IDENTIFY;
	uint8_t ATA_CMD_PIO_READ;
	uint8_t ATA_CMD_PIO_WRITE;
	uint8_t ATA_CMD_FLUSH;
	uint8_t ATA_NO_DISK;
	uint8_t ATA_STATUS_BSY;
	uint8_t ATA_STATUS_ERR;
	uint8_t ATA_STATUS_RDY;
	uint8_t ATA_STATUS_SRV;
	uint8_t ATA_STATUS_DRQ;
	uint8_t ATA_STATUS_DF;
	uint8_t ATA_FEATURE_PIO;
	uint8_t ATA_FEATURE_DMA;
	uint8_t ATA_IDENT_DEVICETYPE;
	uint8_t ATA_IDENT_CYLINDERS;
	uint8_t ATA_IDENT_HEADS;
	uint8_t ATA_IDENT_SECTORSPT;
	uint8_t ATA_IDENT_SERIAL;
	uint8_t ATA_IDENT_MODEL;
	uint8_t ATA_IDENT_CAPABILITIES;
	uint8_t ATA_IDENT_FIELDVALID;
	uint8_t ATA_IDENT_MAX_LBA;
	uint8_t ATA_IDENT_COMMANDSETS;
	uint8_t ATA_IDENT_MAX_LBA_EXT;
	struct struct_Sharpen_Drivers_Block_IDE_Device* prop_Devices;
} classStatics_Sharpen_Drivers_Block_ATA = {
	0x1F0,
	0x170,
	0x00,
	0x01,
	0x00,
	0x01,
	0x00,
	0x01,
	0x01,
	0x02,
	0x03,
	0x04,
	0x05,
	0x06,
	0x07,
	0x07,
	0x08,
	0x09,
	0x0A,
	0x0B,
	0x0C,
	0x0C,
	0x0D,
	0xEC,
	0x20,
	0x30,
	0xE7,
	0x00,
	0x80,
	0x01,
	0,
	0x10,
	0x08,
	0x20,
	0x00,
	0x01,
	0x00,
	0x6C,
	0x6E,
	0x70,
	0x14,
	0x36,
	0x62,
	0x6A,
	0x78,
	0xA4,
	0xC8,
	0,
};
struct class_Sharpen_Drivers_Char_SerialPort
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	struct struct_Sharpen_Drivers_Char_SerialPortComport* comports;
} classStatics_Sharpen_Drivers_Char_SerialPort = {
	0,
};
struct class_Sharpen_Drivers_Char_Keyboard
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	uint8_t m_leds;
	int32_t m_capslock;
	uint8_t m_shift;
	char readchar;
	int32_t readingchar;
	struct class_Sharpen_Collections_Fifo* m_fifo;
	int32_t prop_Capslock;
	uint8_t prop_Shift;
	uint8_t prop_Leds;
} classStatics_Sharpen_Drivers_Char_Keyboard = {
	0x00,
	0,
	0x00,
	0,
	0,
	0,
	0,
	0,
	0,
};
struct class_Sharpen_Drivers_Char_KeyboardMap
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	char Normal[128];
	char Shifted[128];
} classStatics_Sharpen_Drivers_Char_KeyboardMap = {
	{
            (char) 0, (char)27, '1', '2', '3', '4', '5', '6', '7', '8',	/* 9 */
            '9', '0', '-', '=', '\b',	/* Backspace */
            '\t',			/* Tab */
            'q', 'w', 'e', 'r',	/* 19 */
            't', 'y', 'u', 'i', 'o', 'p', '[', ']', '\n',	/* Enter key */
            (char)0,			/* 29   - Control */
            'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', ';',	/* 39 */
            '\'', '`',   (char)0,		/* Left shift */
            '\\', 'z', 'x', 'c', 'v', 'b', 'n',			/* 49 */
            'm', ',', '.', '/',   (char)0,				/* Right shift */
            '*',
            (char)0,	/* Alt */
            ' ',	/* Space bar */
            (char)0,	/* Caps lock */
            (char)0,	/* 59 - F1 key ... > */
            (char)0,   (char)0,   (char)0,   (char)0,   (char)0,   (char)0,   (char)0,   (char)0,
            (char)0,	/* < ... F10 */
            (char)0,	/* 69 - Num lock*/
            (char)0,	/* Scroll Lock */
            (char)0,	/* Home key */
            (char)0,	/* Up Arrow */
            (char)0,	/* Page Up */
            '-',
            (char)0,	/* Left Arrow */
            (char)0,
            (char)0,	/* Right Arrow */
            '+',
            (char)0,	/* 79 - End key*/
            (char)0,	/* Down Arrow */
            (char)0,	/* Page Down */
            (char)0,	/* Insert Key */
            (char)0,	/* Delete Key */
            (char)0,   (char)0,   (char)0,
            (char)0,	/* F11 Key */
            (char)0,	/* F12 Key */
            (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0,
            (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0,
            (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0,/* All other keys are undefined */
        },
	{
            (char) 0, (char)27, '!', '@', '#', '$', '%', '^', '&', '*',	/* 9 */
            '(', ')', '_', '+', '\b',	/* Backspace */
            '\t',			/* Tab */
            'Q', 'W', 'E', 'R',	/* 19 */
            'T', 'Y', 'U', 'I', 'O', 'P', '[', ']', '\n',	/* Enter key */
            (char)0,			/* 29   - Control */
            'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', ':',	/* 39 */
            '\"', '~',   (char)0,		/* Left shift */
            '|', 'Z', 'X', 'C', 'V', 'B', 'N',			/* 49 */
            'M', '<', '>', '?',   (char)0,				/* Right shift */
            '*',
            (char)0,	/* Alt */
            ' ',	/* Space bar */
            (char)0,	/* Caps lock */
            (char)0,	/* 59 - F1 key ... > */
            (char)0,   (char)0,   (char)0,   (char)0,   (char)0,   (char)0,   (char)0,   (char)0,
            (char)0,	/* < ... F10 */
            (char)0,	/* 69 - Num lock*/
            (char)0,	/* Scroll Lock */
            (char)0,	/* Home key */
            (char)0,	/* Up Arrow */
            (char)0,	/* Page Up */
            '-',
            (char)0,	/* Left Arrow */
            (char)0,
            (char)0,	/* Right Arrow */
            '+',
            (char)0,	/* 79 - End key*/
            (char)0,	/* Down Arrow */
            (char)0,	/* Page Down */
            (char)0,	/* Insert Key */
            (char)0,	/* Delete Key */
            (char)0,   (char)0,   (char)0,
            (char)0,	/* F11 Key */
            (char)0,	/* F12 Key */
            (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0,
            (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0,
            (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0,/* All other keys are undefined */
        },
};
struct class_Sharpen_FileSystem_MountPoint
{
	int32_t usage_count;
	void** lookup_table;
	char* field_Name;
	struct class_Sharpen_FileSystem_Node* field_Node;
};

struct
{
} classStatics_Sharpen_FileSystem_MountPoint = {
};
struct class_Sharpen_FileSystem_Node
{
	int32_t usage_count;
	void** lookup_table;
	uint32_t field_Size;
	uint32_t field_Cookie;
	int32_t field_FileMode;
	int32_t field_Flags;
	delegate_Sharpen_FileSystem_Node_FSRead field_Read;
	delegate_Sharpen_FileSystem_Node_FSWrite field_Write;
	delegate_Sharpen_FileSystem_Node_FSOpen field_Open;
	delegate_Sharpen_FileSystem_Node_FSClose field_Close;
	delegate_Sharpen_FileSystem_Node_FSFindDir field_FindDir;
	delegate_Sharpen_FileSystem_Node_FSReaddir field_ReadDir;
};

struct
{
} classStatics_Sharpen_FileSystem_Node = {
};
struct class_Sharpen_FileSystem_VFS
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	struct class_Sharpen_Collections_Dictionary* m_dictionary;
} classStatics_Sharpen_FileSystem_VFS = {
	0,
};
struct class_Sharpen_Heap
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	int32_t m_realHeap;
	struct struct_Sharpen_Heap_BlockDescriptor* firstDescriptor;
	int32_t MINIMALPAGES;
	void* prop_CurrentEnd;
} classStatics_Sharpen_Heap = {
	false,
	0,
	32,
	0,
};
struct class_Sharpen_Collections_Fifo
{
	int32_t usage_count;
	void** lookup_table;
	uint8_t* field_m_buffer;
	int32_t field_m_wait;
	int32_t field_m_head;
	int32_t field_m_tail;
	int32_t field_m_size;
};

struct
{
} classStatics_Sharpen_Collections_Fifo = {
};
struct class_Sharpen_Collections_List
{
	int32_t usage_count;
	void** lookup_table;
	int32_t field_m_currentCap;
	void** prop_Item;
	int32_t prop_Count;
	int32_t prop_Capacity;
};

struct
{
} classStatics_Sharpen_Collections_List = {
};
struct class_Sharpen_Collections_BitArray
{
	int32_t usage_count;
	void** lookup_table;
	int32_t* field_m_bitmap;
	int32_t field_m_N;
};

struct
{
} classStatics_Sharpen_Collections_BitArray = {
};
struct class_Sharpen_Lib_Audio
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	struct struct_Sharpen_Lib_Audio_SoundDevice m_device;
} classStatics_Sharpen_Lib_Audio = {
	{0},
};
struct class_Sharpen_Memory
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Memory = {
};
struct class_Sharpen_Multiboot
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	int32_t HeaderMagic;
	int32_t Magic;
	int32_t FlagMem;
	int32_t FlagDevice;
	int32_t FlagCMDLine;
	int32_t FlagMods;
	int32_t FlagAOUT;
	int32_t FlagELF;
	int32_t FlagMMAP;
	int32_t FlagConfig;
	int32_t FlagLoader;
	int32_t FlagAPM;
	int32_t FlagVBE;
} classStatics_Sharpen_Multiboot = {
	0x1BADB002,
	0x2BADB002,
	0x001,
	0x002,
	0x004,
	0x008,
	0x010,
	0x020,
	0x040,
	0x080,
	0x100,
	0x200,
	0x400,
};
struct class_Sharpen_Panic
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Panic = {
};
struct class_Sharpen_Arch_PortIO
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Arch_PortIO = {
};
struct class_Sharpen_Arch_Paging
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	struct class_Sharpen_Collections_BitArray* m_bitmap;
	struct struct_Sharpen_Arch_Paging_PageDirectory* m_currentDirectory;
	struct struct_Sharpen_Arch_Paging_PageDirectory* prop_KernelDirectory;
	struct struct_Sharpen_Arch_Paging_PageDirectory* prop_CurrentDirectory;
} classStatics_Sharpen_Arch_Paging = {
	0,
	0,
	0,
	0,
};
struct class_Sharpen_Program
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	struct struct_Sharpen_Multiboot_Header m_mbootHeader;
	int32_t m_isMultiboot;
} classStatics_Sharpen_Program = {
	{0},
	false,
};
struct class_Sharpen_Utilities_String
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Utilities_String = {
};
struct class_Sharpen_Time
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	int32_t prop_Seconds;
	int32_t prop_Minutes;
	int32_t prop_Hours;
	int32_t prop_Day;
	int32_t prop_Month;
	int32_t prop_Year;
} classStatics_Sharpen_Time = {
	0,
	0,
	0,
	0,
	0,
	0,
};
struct class_Sharpen_Utilities_Util
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Utilities_Util = {
};

struct base_class
{
	int32_t usage_count;
	void** lookup_table;
};

struct class_Sharpen_Arch_CMOS* classInit_Sharpen_Arch_CMOS(void);
inline void classCctor_Sharpen_Arch_CMOS(void);
uint8_t Sharpen_Arch_CMOS_GetData_1uint8_t_(uint8_t reg);
typedef uint8_t (*fp_Sharpen_Arch_CMOS_GetData_1uint8_t_)(uint8_t reg);
int32_t Sharpen_Arch_CMOS_BCD_TO_BIN_1int32_t_(int32_t x);
typedef int32_t (*fp_Sharpen_Arch_CMOS_BCD_TO_BIN_1int32_t_)(int32_t x);
void Sharpen_Arch_CMOS_UpdateTime_0(void);
typedef void (*fp_Sharpen_Arch_CMOS_UpdateTime_0)(void);
struct class_Sharpen_Arch_CPU* classInit_Sharpen_Arch_CPU(void);
void Sharpen_Arch_CPU_STI_0(void);
typedef void (*fp_Sharpen_Arch_CPU_STI_0)(void);
void Sharpen_Arch_CPU_CLI_0(void);
typedef void (*fp_Sharpen_Arch_CPU_CLI_0)(void);
void Sharpen_Arch_CPU_HLT_0(void);
typedef void (*fp_Sharpen_Arch_CPU_HLT_0)(void);
struct class_Sharpen_Arch_FPU* classInit_Sharpen_Arch_FPU(void);
void Sharpen_Arch_FPU_Init_0(void);
typedef void (*fp_Sharpen_Arch_FPU_Init_0)(void);
void Sharpen_Arch_FPU_StoreContext_1void__(void* context);
typedef void (*fp_Sharpen_Arch_FPU_StoreContext_1void__)(void* context);
void Sharpen_Arch_FPU_RestoreContext_1void__(void* context);
typedef void (*fp_Sharpen_Arch_FPU_RestoreContext_1void__)(void* context);
struct class_Sharpen_Arch_IDT* classInit_Sharpen_Arch_IDT(void);
inline void classCctor_Sharpen_Arch_IDT(void);
inline struct struct_Sharpen_Arch_IDT_IDT_Entry structInit_Sharpen_Arch_IDT_IDT_Entry(void);
inline struct struct_Sharpen_Arch_IDT_IDT_Pointer structInit_Sharpen_Arch_IDT_IDT_Pointer(void);
int32_t Sharpen_Arch_IDT_Privilege_1int32_t_(int32_t a);
typedef int32_t (*fp_Sharpen_Arch_IDT_Privilege_1int32_t_)(int32_t a);
int32_t Sharpen_Arch_IDT_Present_1int32_t_(int32_t a);
typedef int32_t (*fp_Sharpen_Arch_IDT_Present_1int32_t_)(int32_t a);
void Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(int32_t num, void* address, uint16_t selector, uint8_t flags);
typedef void (*fp_Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_)(int32_t num, void* address, uint16_t selector, uint8_t flags);
void Sharpen_Arch_IDT_Init_0(void);
typedef void (*fp_Sharpen_Arch_IDT_Init_0)(void);
void Sharpen_Arch_IDT_flushIDT_1struct_struct_Sharpen_Arch_IDT_IDT_Pointer__(struct struct_Sharpen_Arch_IDT_IDT_Pointer* ptr);
typedef void (*fp_Sharpen_Arch_IDT_flushIDT_1struct_struct_Sharpen_Arch_IDT_IDT_Pointer__)(struct struct_Sharpen_Arch_IDT_IDT_Pointer* ptr);
void Sharpen_Arch_IDT_INTIgnore_0(void);
typedef void (*fp_Sharpen_Arch_IDT_INTIgnore_0)(void);
void Sharpen_Arch_IDT_ISR0_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR0_0)(void);
void Sharpen_Arch_IDT_ISR1_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR1_0)(void);
void Sharpen_Arch_IDT_ISR2_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR2_0)(void);
void Sharpen_Arch_IDT_ISR3_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR3_0)(void);
void Sharpen_Arch_IDT_ISR4_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR4_0)(void);
void Sharpen_Arch_IDT_ISR5_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR5_0)(void);
void Sharpen_Arch_IDT_ISR6_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR6_0)(void);
void Sharpen_Arch_IDT_ISR7_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR7_0)(void);
void Sharpen_Arch_IDT_ISR8_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR8_0)(void);
void Sharpen_Arch_IDT_ISR9_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR9_0)(void);
void Sharpen_Arch_IDT_ISR10_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR10_0)(void);
void Sharpen_Arch_IDT_ISR11_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR11_0)(void);
void Sharpen_Arch_IDT_ISR12_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR12_0)(void);
void Sharpen_Arch_IDT_ISR13_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR13_0)(void);
void Sharpen_Arch_IDT_ISR14_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR14_0)(void);
void Sharpen_Arch_IDT_ISR15_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR15_0)(void);
void Sharpen_Arch_IDT_ISR16_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR16_0)(void);
void Sharpen_Arch_IDT_ISR17_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR17_0)(void);
void Sharpen_Arch_IDT_ISR18_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR18_0)(void);
void Sharpen_Arch_IDT_ISR19_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR19_0)(void);
void Sharpen_Arch_IDT_ISR20_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR20_0)(void);
void Sharpen_Arch_IDT_ISR21_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR21_0)(void);
void Sharpen_Arch_IDT_ISR22_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR22_0)(void);
void Sharpen_Arch_IDT_ISR23_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR23_0)(void);
void Sharpen_Arch_IDT_ISR24_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR24_0)(void);
void Sharpen_Arch_IDT_ISR25_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR25_0)(void);
void Sharpen_Arch_IDT_ISR26_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR26_0)(void);
void Sharpen_Arch_IDT_ISR27_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR27_0)(void);
void Sharpen_Arch_IDT_ISR28_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR28_0)(void);
void Sharpen_Arch_IDT_ISR29_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR29_0)(void);
void Sharpen_Arch_IDT_ISR30_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR30_0)(void);
void Sharpen_Arch_IDT_ISR31_0(void);
typedef void (*fp_Sharpen_Arch_IDT_ISR31_0)(void);
void Sharpen_Arch_IDT_IRQ0_0(void);
typedef void (*fp_Sharpen_Arch_IDT_IRQ0_0)(void);
void Sharpen_Arch_IDT_IRQ1_0(void);
typedef void (*fp_Sharpen_Arch_IDT_IRQ1_0)(void);
void Sharpen_Arch_IDT_IRQ2_0(void);
typedef void (*fp_Sharpen_Arch_IDT_IRQ2_0)(void);
void Sharpen_Arch_IDT_IRQ3_0(void);
typedef void (*fp_Sharpen_Arch_IDT_IRQ3_0)(void);
void Sharpen_Arch_IDT_IRQ4_0(void);
typedef void (*fp_Sharpen_Arch_IDT_IRQ4_0)(void);
void Sharpen_Arch_IDT_IRQ5_0(void);
typedef void (*fp_Sharpen_Arch_IDT_IRQ5_0)(void);
void Sharpen_Arch_IDT_IRQ6_0(void);
typedef void (*fp_Sharpen_Arch_IDT_IRQ6_0)(void);
void Sharpen_Arch_IDT_IRQ7_0(void);
typedef void (*fp_Sharpen_Arch_IDT_IRQ7_0)(void);
void Sharpen_Arch_IDT_IRQ8_0(void);
typedef void (*fp_Sharpen_Arch_IDT_IRQ8_0)(void);
void Sharpen_Arch_IDT_IRQ9_0(void);
typedef void (*fp_Sharpen_Arch_IDT_IRQ9_0)(void);
void Sharpen_Arch_IDT_IRQ10_0(void);
typedef void (*fp_Sharpen_Arch_IDT_IRQ10_0)(void);
void Sharpen_Arch_IDT_IRQ11_0(void);
typedef void (*fp_Sharpen_Arch_IDT_IRQ11_0)(void);
void Sharpen_Arch_IDT_IRQ12_0(void);
typedef void (*fp_Sharpen_Arch_IDT_IRQ12_0)(void);
void Sharpen_Arch_IDT_IRQ13_0(void);
typedef void (*fp_Sharpen_Arch_IDT_IRQ13_0)(void);
void Sharpen_Arch_IDT_IRQ14_0(void);
typedef void (*fp_Sharpen_Arch_IDT_IRQ14_0)(void);
void Sharpen_Arch_IDT_IRQ15_0(void);
typedef void (*fp_Sharpen_Arch_IDT_IRQ15_0)(void);
void Sharpen_Arch_IDT_Syscall_0(void);
typedef void (*fp_Sharpen_Arch_IDT_Syscall_0)(void);
struct class_Sharpen_Arch_IRQ* classInit_Sharpen_Arch_IRQ(void);
inline void classCctor_Sharpen_Arch_IRQ(void);
void Sharpen_Arch_IRQ_SetHandler_2int32_t_delegate_Sharpen_Arch_IRQ_IRQHandler_(int32_t num, delegate_Sharpen_Arch_IRQ_IRQHandler handler);
typedef void (*fp_Sharpen_Arch_IRQ_SetHandler_2int32_t_delegate_Sharpen_Arch_IRQ_IRQHandler_)(int32_t num, delegate_Sharpen_Arch_IRQ_IRQHandler handler);
void Sharpen_Arch_IRQ_RemoveHandler_1int32_t_(int32_t num);
typedef void (*fp_Sharpen_Arch_IRQ_RemoveHandler_1int32_t_)(int32_t num);
void Sharpen_Arch_IRQ_Handler_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr);
typedef void (*fp_Sharpen_Arch_IRQ_Handler_1struct_struct_Sharpen_Arch_Regs__)(struct struct_Sharpen_Arch_Regs* regsPtr);
struct class_Sharpen_Arch_ISR* classInit_Sharpen_Arch_ISR(void);
inline void classCctor_Sharpen_Arch_ISR(void);
void Sharpen_Arch_ISR_Handler_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr);
typedef void (*fp_Sharpen_Arch_ISR_Handler_1struct_struct_Sharpen_Arch_Regs__)(struct struct_Sharpen_Arch_Regs* regsPtr);
struct class_Sharpen_Drivers_Net_rtl8139* classInit_Sharpen_Drivers_Net_rtl8139(void);
inline void classCctor_Sharpen_Drivers_Net_rtl8139(void);
void Sharpen_Drivers_Net_rtl8139_initHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_(struct struct_Sharpen_Arch_PCI_PciDevice dev);
typedef void (*fp_Sharpen_Drivers_Net_rtl8139_initHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_)(struct struct_Sharpen_Arch_PCI_PciDevice dev);
void Sharpen_Drivers_Net_rtl8139_GetMac_1uint8_t__(uint8_t* mac);
typedef void (*fp_Sharpen_Drivers_Net_rtl8139_GetMac_1uint8_t__)(uint8_t* mac);
void Sharpen_Drivers_Net_rtl8139_Transmit_2uint8_t__uint32_t_(uint8_t* bytes, uint32_t size);
typedef void (*fp_Sharpen_Drivers_Net_rtl8139_Transmit_2uint8_t__uint32_t_)(uint8_t* bytes, uint32_t size);
void Sharpen_Drivers_Net_rtl8139_PrintRes_0(void);
typedef void (*fp_Sharpen_Drivers_Net_rtl8139_PrintRes_0)(void);
void Sharpen_Drivers_Net_rtl8139_handler_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr);
typedef void (*fp_Sharpen_Drivers_Net_rtl8139_handler_1struct_struct_Sharpen_Arch_Regs__)(struct struct_Sharpen_Arch_Regs* regsPtr);
void Sharpen_Drivers_Net_rtl8139_turnOn_0(void);
typedef void (*fp_Sharpen_Drivers_Net_rtl8139_turnOn_0)(void);
void Sharpen_Drivers_Net_rtl8139_readMac_0(void);
typedef void (*fp_Sharpen_Drivers_Net_rtl8139_readMac_0)(void);
void Sharpen_Drivers_Net_rtl8139_setInterruptMask_1uint16_t_(uint16_t mask);
typedef void (*fp_Sharpen_Drivers_Net_rtl8139_setInterruptMask_1uint16_t_)(uint16_t mask);
void Sharpen_Drivers_Net_rtl8139_ackowledgeInterrupts_0(void);
typedef void (*fp_Sharpen_Drivers_Net_rtl8139_ackowledgeInterrupts_0)(void);
void Sharpen_Drivers_Net_rtl8139_updateLinkStatus_0(void);
typedef void (*fp_Sharpen_Drivers_Net_rtl8139_updateLinkStatus_0)(void);
void Sharpen_Drivers_Net_rtl8139_exitHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_(struct struct_Sharpen_Arch_PCI_PciDevice dev);
typedef void (*fp_Sharpen_Drivers_Net_rtl8139_exitHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_)(struct struct_Sharpen_Arch_PCI_PciDevice dev);
void Sharpen_Drivers_Net_rtl8139_Init_0(void);
typedef void (*fp_Sharpen_Drivers_Net_rtl8139_Init_0)(void);
struct class_Sharpen_Exec_ELFLoader* classInit_Sharpen_Exec_ELFLoader(void);
inline struct struct_Sharpen_Exec_ELFLoader_ELF32 structInit_Sharpen_Exec_ELFLoader_ELF32(void);
inline struct struct_Sharpen_Exec_ELFLoader_ProgramHeader structInit_Sharpen_Exec_ELFLoader_ProgramHeader(void);
inline struct struct_Sharpen_Exec_ELFLoader_SectionHeader structInit_Sharpen_Exec_ELFLoader_SectionHeader(void);
int32_t Sharpen_Exec_ELFLoader_isValidELF_1struct_struct_Sharpen_Exec_ELFLoader_ELF32__(struct struct_Sharpen_Exec_ELFLoader_ELF32* elf);
typedef int32_t (*fp_Sharpen_Exec_ELFLoader_isValidELF_1struct_struct_Sharpen_Exec_ELFLoader_ELF32__)(struct struct_Sharpen_Exec_ELFLoader_ELF32* elf);
struct struct_Sharpen_Exec_ELFLoader_SectionHeader* Sharpen_Exec_ELFLoader_getSection_2struct_struct_Sharpen_Exec_ELFLoader_ELF32__uint32_t_(struct struct_Sharpen_Exec_ELFLoader_ELF32* elf, uint32_t index);
typedef struct struct_Sharpen_Exec_ELFLoader_SectionHeader* (*fp_Sharpen_Exec_ELFLoader_getSection_2struct_struct_Sharpen_Exec_ELFLoader_ELF32__uint32_t_)(struct struct_Sharpen_Exec_ELFLoader_ELF32* elf, uint32_t index);
char* Sharpen_Exec_ELFLoader_getString_2struct_struct_Sharpen_Exec_ELFLoader_ELF32__uint32_t_(struct struct_Sharpen_Exec_ELFLoader_ELF32* elf, uint32_t offset);
typedef char* (*fp_Sharpen_Exec_ELFLoader_getString_2struct_struct_Sharpen_Exec_ELFLoader_ELF32__uint32_t_)(struct struct_Sharpen_Exec_ELFLoader_ELF32* elf, uint32_t offset);
int32_t Sharpen_Exec_ELFLoader_Execute_3uint8_t__uint32_t_char___(uint8_t* buffer, uint32_t size, char** argv);
typedef int32_t (*fp_Sharpen_Exec_ELFLoader_Execute_3uint8_t__uint32_t_char___)(uint8_t* buffer, uint32_t size, char** argv);
struct class_Sharpen_Exec_Loader* classInit_Sharpen_Exec_Loader(void);
int32_t Sharpen_Exec_Loader_StartProcess_2char__char___(char* path, char** argv);
typedef int32_t (*fp_Sharpen_Exec_Loader_StartProcess_2char__char___)(char* path, char** argv);
struct class_Sharpen_Exec_Syscalls* classInit_Sharpen_Exec_Syscalls(void);
int32_t Sharpen_Exec_Syscalls_Exit_1int32_t_(int32_t status);
typedef int32_t (*fp_Sharpen_Exec_Syscalls_Exit_1int32_t_)(int32_t status);
int32_t Sharpen_Exec_Syscalls_GetPID_0(void);
typedef int32_t (*fp_Sharpen_Exec_Syscalls_GetPID_0)(void);
int32_t Sharpen_Exec_Syscalls_Sbrk_1int32_t_(int32_t increase);
typedef int32_t (*fp_Sharpen_Exec_Syscalls_Sbrk_1int32_t_)(int32_t increase);
int32_t Sharpen_Exec_Syscalls_Fork_0(void);
typedef int32_t (*fp_Sharpen_Exec_Syscalls_Fork_0)(void);
int32_t Sharpen_Exec_Syscalls_Write_3int32_t_uint8_t__uint32_t_(int32_t descriptor, uint8_t* buffer, uint32_t size);
typedef int32_t (*fp_Sharpen_Exec_Syscalls_Write_3int32_t_uint8_t__uint32_t_)(int32_t descriptor, uint8_t* buffer, uint32_t size);
int32_t Sharpen_Exec_Syscalls_Read_3int32_t_uint8_t__uint32_t_(int32_t descriptor, uint8_t* buffer, uint32_t size);
typedef int32_t (*fp_Sharpen_Exec_Syscalls_Read_3int32_t_uint8_t__uint32_t_)(int32_t descriptor, uint8_t* buffer, uint32_t size);
int32_t Sharpen_Exec_Syscalls_Open_2char__int32_t_(char* path, int32_t flags);
typedef int32_t (*fp_Sharpen_Exec_Syscalls_Open_2char__int32_t_)(char* path, int32_t flags);
int32_t Sharpen_Exec_Syscalls_Close_1int32_t_(int32_t descriptor);
typedef int32_t (*fp_Sharpen_Exec_Syscalls_Close_1int32_t_)(int32_t descriptor);
int32_t Sharpen_Exec_Syscalls_Seek_3int32_t_uint32_t_int32_t_(int32_t descriptor, uint32_t offset, int32_t whence);
typedef int32_t (*fp_Sharpen_Exec_Syscalls_Seek_3int32_t_uint32_t_int32_t_)(int32_t descriptor, uint32_t offset, int32_t whence);
inline struct struct_Sharpen_FileSystem_Fat16BPB structInit_Sharpen_FileSystem_Fat16BPB(void);
inline struct struct_Sharpen_FileSystem_FatDirEntry structInit_Sharpen_FileSystem_FatDirEntry(void);
struct class_Sharpen_FileSystem_SubDirectory* classInit_Sharpen_FileSystem_SubDirectory(void);
struct class_Sharpen_FileSystem_STDOUT* classInit_Sharpen_FileSystem_STDOUT(void);
void Sharpen_FileSystem_STDOUT_Init_0(void);
typedef void (*fp_Sharpen_FileSystem_STDOUT_Init_0)(void);
uint32_t Sharpen_FileSystem_STDOUT_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
typedef uint32_t (*fp_Sharpen_FileSystem_STDOUT_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__)(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
struct class_Sharpen_Net_DHCP* classInit_Sharpen_Net_DHCP(void);
inline struct struct_Sharpen_Net_DHCP_DHCPHeader structInit_Sharpen_Net_DHCP_DHCPHeader(void);
struct struct_Sharpen_Net_DHCP_DHCPHeader Sharpen_Net_DHCP_makeHeader_3uint32_t_uint32_t_uint8_t_(uint32_t xid, uint32_t clientAddr, uint8_t type);
typedef struct struct_Sharpen_Net_DHCP_DHCPHeader (*fp_Sharpen_Net_DHCP_makeHeader_3uint32_t_uint32_t_uint8_t_)(uint32_t xid, uint32_t clientAddr, uint8_t type);
void Sharpen_Net_DHCP_Sample_0(void);
typedef void (*fp_Sharpen_Net_DHCP_Sample_0)(void);
struct class_Sharpen_Net_Network* classInit_Sharpen_Net_Network(void);
inline struct struct_Sharpen_Net_Network_NetDevice structInit_Sharpen_Net_Network_NetDevice(void);
struct struct_Sharpen_Net_Network_NetDevice Sharpen_Net_Network_Device_getter(void);
void Sharpen_Net_Network_Set_1struct_struct_Sharpen_Net_Network_NetDevice_(struct struct_Sharpen_Net_Network_NetDevice device);
typedef void (*fp_Sharpen_Net_Network_Set_1struct_struct_Sharpen_Net_Network_NetDevice_)(struct struct_Sharpen_Net_Network_NetDevice device);
void Sharpen_Net_Network_Transmit_2uint8_t__uint32_t_(uint8_t* bytes, uint32_t size);
typedef void (*fp_Sharpen_Net_Network_Transmit_2uint8_t__uint32_t_)(uint8_t* bytes, uint32_t size);
void Sharpen_Net_Network_GetMac_1uint8_t__(uint8_t* mac);
typedef void (*fp_Sharpen_Net_Network_GetMac_1uint8_t__)(uint8_t* mac);
struct class_Sharpen_Net_NetworkTools* classInit_Sharpen_Net_NetworkTools(void);
void Sharpen_Net_NetworkTools_WakeOnLan_1uint8_t__(uint8_t* mac);
typedef void (*fp_Sharpen_Net_NetworkTools_WakeOnLan_1uint8_t__)(uint8_t* mac);
inline struct struct_Sharpen_Task_FileDescriptors structInit_Sharpen_Task_FileDescriptors(void);
struct class_Sharpen_Task_Task* classInit_Sharpen_Task_Task(void);
struct class_Sharpen_Task_Tasking* classInit_Sharpen_Task_Tasking(void);
inline void classCctor_Sharpen_Task_Tasking(void);
struct class_Sharpen_Task_Task* Sharpen_Task_Tasking_KernelTask_getter(void);
struct class_Sharpen_Task_Task* Sharpen_Task_Tasking_KernelTask_setter(struct class_Sharpen_Task_Task* value);
struct class_Sharpen_Task_Task* Sharpen_Task_Tasking_CurrentTask_getter(void);
struct class_Sharpen_Task_Task* Sharpen_Task_Tasking_CurrentTask_setter(struct class_Sharpen_Task_Task* value);
void Sharpen_Task_Tasking_Init_0(void);
typedef void (*fp_Sharpen_Task_Tasking_Init_0)(void);
struct class_Sharpen_Task_Task* Sharpen_Task_Tasking_GetTaskByPID_1int32_t_(int32_t pid);
typedef struct class_Sharpen_Task_Task* (*fp_Sharpen_Task_Tasking_GetTaskByPID_1int32_t_)(int32_t pid);
void Sharpen_Task_Tasking_RemoveTaskByPID_1int32_t_(int32_t pid);
typedef void (*fp_Sharpen_Task_Tasking_RemoveTaskByPID_1int32_t_)(int32_t pid);
void Sharpen_Task_Tasking_ScheduleTask_1struct_class_Sharpen_Task_Task__(struct class_Sharpen_Task_Task* task);
typedef void (*fp_Sharpen_Task_Tasking_ScheduleTask_1struct_class_Sharpen_Task_Task__)(struct class_Sharpen_Task_Task* task);
void Sharpen_Task_Tasking_AddTask_2void__int32_t_(void* eip, int32_t priority);
typedef void (*fp_Sharpen_Task_Tasking_AddTask_2void__int32_t_)(void* eip, int32_t priority);
struct class_Sharpen_Task_Task* Sharpen_Task_Tasking_FindNextTask_0(void);
typedef struct class_Sharpen_Task_Task* (*fp_Sharpen_Task_Tasking_FindNextTask_0)(void);
struct struct_Sharpen_Arch_Regs* Sharpen_Task_Tasking_scheduler_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr);
typedef struct struct_Sharpen_Arch_Regs* (*fp_Sharpen_Task_Tasking_scheduler_1struct_struct_Sharpen_Arch_Regs__)(struct struct_Sharpen_Arch_Regs* regsPtr);
int32_t* Sharpen_Task_Tasking_writeSchedulerStack_4int32_t__int32_t_int32_t_void__(int32_t* ptr, int32_t cs, int32_t ds, void* eip);
typedef int32_t* (*fp_Sharpen_Task_Tasking_writeSchedulerStack_4int32_t__int32_t_int32_t_void__)(int32_t* ptr, int32_t cs, int32_t ds, void* eip);
struct class_Sharpen_FileSystem_Node* Sharpen_Task_Tasking_GetNodeFromDescriptor_1int32_t_(int32_t descriptor);
typedef struct class_Sharpen_FileSystem_Node* (*fp_Sharpen_Task_Tasking_GetNodeFromDescriptor_1int32_t_)(int32_t descriptor);
uint32_t Sharpen_Task_Tasking_GetOffsetFromDescriptor_1int32_t_(int32_t descriptor);
typedef uint32_t (*fp_Sharpen_Task_Tasking_GetOffsetFromDescriptor_1int32_t_)(int32_t descriptor);
int32_t Sharpen_Task_Tasking_AddNodeToDescriptor_1struct_class_Sharpen_FileSystem_Node__(struct class_Sharpen_FileSystem_Node* node);
typedef int32_t (*fp_Sharpen_Task_Tasking_AddNodeToDescriptor_1struct_class_Sharpen_FileSystem_Node__)(struct class_Sharpen_FileSystem_Node* node);
void Sharpen_Task_Tasking_ManualSchedule_0(void);
typedef void (*fp_Sharpen_Task_Tasking_ManualSchedule_0)(void);
struct class_Sharpen_Arch_PCI* classInit_Sharpen_Arch_PCI(void);
inline void classCctor_Sharpen_Arch_PCI(void);
inline struct struct_Sharpen_Arch_PCI_PciDriver structInit_Sharpen_Arch_PCI_PciDriver(void);
inline struct struct_Sharpen_Arch_PCI_PciDevice structInit_Sharpen_Arch_PCI_PciDevice(void);
uint32_t Sharpen_Arch_PCI_generateAddress_4uint32_t_uint32_t_uint32_t_uint32_t_(uint32_t lbus, uint32_t lslot, uint32_t lfun, uint32_t offset);
typedef uint32_t (*fp_Sharpen_Arch_PCI_generateAddress_4uint32_t_uint32_t_uint32_t_uint32_t_)(uint32_t lbus, uint32_t lslot, uint32_t lfun, uint32_t offset);
uint16_t Sharpen_Arch_PCI_readWord_4uint16_t_uint16_t_uint16_t_uint16_t_(uint16_t bus, uint16_t slot, uint16_t function, uint16_t offset);
typedef uint16_t (*fp_Sharpen_Arch_PCI_readWord_4uint16_t_uint16_t_uint16_t_uint16_t_)(uint16_t bus, uint16_t slot, uint16_t function, uint16_t offset);
uint32_t Sharpen_Arch_PCI_PCIRead_5uint16_t_uint16_t_uint16_t_uint16_t_uint32_t_(uint16_t bus, uint16_t slot, uint16_t function, uint16_t offset, uint32_t size);
typedef uint32_t (*fp_Sharpen_Arch_PCI_PCIRead_5uint16_t_uint16_t_uint16_t_uint16_t_uint32_t_)(uint16_t bus, uint16_t slot, uint16_t function, uint16_t offset, uint32_t size);
void Sharpen_Arch_PCI_PCIWrite_5uint16_t_uint16_t_uint16_t_uint16_t_uint32_t_(uint16_t bus, uint16_t slot, uint16_t function, uint16_t offset, uint32_t value);
typedef void (*fp_Sharpen_Arch_PCI_PCIWrite_5uint16_t_uint16_t_uint16_t_uint16_t_uint32_t_)(uint16_t bus, uint16_t slot, uint16_t function, uint16_t offset, uint32_t value);
void Sharpen_Arch_PCI_PCIWrite_3struct_struct_Sharpen_Arch_PCI_PciDevice_uint16_t_uint32_t_(struct struct_Sharpen_Arch_PCI_PciDevice dev, uint16_t offset, uint32_t value);
typedef void (*fp_Sharpen_Arch_PCI_PCIWrite_3struct_struct_Sharpen_Arch_PCI_PciDevice_uint16_t_uint32_t_)(struct struct_Sharpen_Arch_PCI_PciDevice dev, uint16_t offset, uint32_t value);
uint16_t Sharpen_Arch_PCI_PCIReadWord_2struct_struct_Sharpen_Arch_PCI_PciDevice_uint16_t_(struct struct_Sharpen_Arch_PCI_PciDevice dev, uint16_t offset);
typedef uint16_t (*fp_Sharpen_Arch_PCI_PCIReadWord_2struct_struct_Sharpen_Arch_PCI_PciDevice_uint16_t_)(struct struct_Sharpen_Arch_PCI_PciDevice dev, uint16_t offset);
uint16_t Sharpen_Arch_PCI_getDeviceID_3uint16_t_uint16_t_uint16_t_(uint16_t bus, uint16_t device, uint16_t function);
typedef uint16_t (*fp_Sharpen_Arch_PCI_getDeviceID_3uint16_t_uint16_t_uint16_t_)(uint16_t bus, uint16_t device, uint16_t function);
uint16_t Sharpen_Arch_PCI_getHeaderType_3uint16_t_uint16_t_uint16_t_(uint16_t bus, uint16_t device, uint16_t function);
typedef uint16_t (*fp_Sharpen_Arch_PCI_getHeaderType_3uint16_t_uint16_t_uint16_t_)(uint16_t bus, uint16_t device, uint16_t function);
uint16_t Sharpen_Arch_PCI_GetVendorID_3uint16_t_uint16_t_uint16_t_(uint16_t bus, uint16_t device, uint16_t function);
typedef uint16_t (*fp_Sharpen_Arch_PCI_GetVendorID_3uint16_t_uint16_t_uint16_t_)(uint16_t bus, uint16_t device, uint16_t function);
uint16_t Sharpen_Arch_PCI_GetClassID_3uint16_t_uint16_t_uint16_t_(uint16_t bus, uint16_t device, uint16_t function);
typedef uint16_t (*fp_Sharpen_Arch_PCI_GetClassID_3uint16_t_uint16_t_uint16_t_)(uint16_t bus, uint16_t device, uint16_t function);
uint8_t Sharpen_Arch_PCI_GetSubClassID_3uint16_t_uint16_t_uint16_t_(uint16_t bus, uint16_t device, uint16_t function);
typedef uint8_t (*fp_Sharpen_Arch_PCI_GetSubClassID_3uint16_t_uint16_t_uint16_t_)(uint16_t bus, uint16_t device, uint16_t function);
void Sharpen_Arch_PCI_checkBus_1uint8_t_(uint8_t bus);
typedef void (*fp_Sharpen_Arch_PCI_checkBus_1uint8_t_)(uint8_t bus);
void Sharpen_Arch_PCI_checkDevice_2uint8_t_uint8_t_(uint8_t bus, uint8_t device);
typedef void (*fp_Sharpen_Arch_PCI_checkDevice_2uint8_t_uint8_t_)(uint8_t bus, uint8_t device);
void Sharpen_Arch_PCI_RegisterDriver_3uint16_t_uint16_t_struct_struct_Sharpen_Arch_PCI_PciDriver_(uint16_t vendorID, uint16_t deviceID, struct struct_Sharpen_Arch_PCI_PciDriver driver);
typedef void (*fp_Sharpen_Arch_PCI_RegisterDriver_3uint16_t_uint16_t_struct_struct_Sharpen_Arch_PCI_PciDriver_)(uint16_t vendorID, uint16_t deviceID, struct struct_Sharpen_Arch_PCI_PciDriver driver);
void Sharpen_Arch_PCI_PrintDevices_0(void);
typedef void (*fp_Sharpen_Arch_PCI_PrintDevices_0)(void);
void Sharpen_Arch_PCI_Probe_0(void);
typedef void (*fp_Sharpen_Arch_PCI_Probe_0)(void);
struct class_Sharpen_FileSystem_Node* Sharpen_Arch_PCI_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__(struct class_Sharpen_FileSystem_Node* node, char* name);
typedef struct class_Sharpen_FileSystem_Node* (*fp_Sharpen_Arch_PCI_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__)(struct class_Sharpen_FileSystem_Node* node, char* name);
void Sharpen_Arch_PCI_Init_0(void);
typedef void (*fp_Sharpen_Arch_PCI_Init_0)(void);
struct class_Sharpen_Arch_PIC* classInit_Sharpen_Arch_PIC(void);
inline void classCctor_Sharpen_Arch_PIC(void);
void Sharpen_Arch_PIC_SendEOI_1uint8_t_(uint8_t irq);
typedef void (*fp_Sharpen_Arch_PIC_SendEOI_1uint8_t_)(uint8_t irq);
void Sharpen_Arch_PIC_Remap_0(void);
typedef void (*fp_Sharpen_Arch_PIC_Remap_0)(void);
struct class_Sharpen_Arch_PIT* classInit_Sharpen_Arch_PIT(void);
inline void classCctor_Sharpen_Arch_PIT(void);
int32_t Sharpen_Arch_PIT_Frequency_getter(void);
int32_t Sharpen_Arch_PIT_Frequency_setter(int32_t value);
int32_t Sharpen_Arch_PIT_SubTicks_getter(void);
int32_t Sharpen_Arch_PIT_SubTicks_setter(int32_t value);
int32_t Sharpen_Arch_PIT_FullTicks_getter(void);
int32_t Sharpen_Arch_PIT_FullTicks_setter(int32_t value);
int32_t Sharpen_Arch_PIT_Channel_1int32_t_(int32_t a);
typedef int32_t (*fp_Sharpen_Arch_PIT_Channel_1int32_t_)(int32_t a);
int32_t Sharpen_Arch_PIT_Access_1int32_t_(int32_t a);
typedef int32_t (*fp_Sharpen_Arch_PIT_Access_1int32_t_)(int32_t a);
int32_t Sharpen_Arch_PIT_Operating_1int32_t_(int32_t a);
typedef int32_t (*fp_Sharpen_Arch_PIT_Operating_1int32_t_)(int32_t a);
int32_t Sharpen_Arch_PIT_Mode_1int32_t_(int32_t a);
typedef int32_t (*fp_Sharpen_Arch_PIT_Mode_1int32_t_)(int32_t a);
void Sharpen_Arch_PIT_Init_0(void);
typedef void (*fp_Sharpen_Arch_PIT_Init_0)(void);
void Sharpen_Arch_PIT_Handler_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr);
typedef void (*fp_Sharpen_Arch_PIT_Handler_1struct_struct_Sharpen_Arch_Regs__)(struct struct_Sharpen_Arch_Regs* regsPtr);
inline struct struct_Sharpen_Arch_Regs structInit_Sharpen_Arch_Regs(void);
struct class_Sharpen_Arch_Syscall* classInit_Sharpen_Arch_Syscall(void);
void Sharpen_Arch_Syscall_Handler_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr);
typedef void (*fp_Sharpen_Arch_Syscall_Handler_1struct_struct_Sharpen_Arch_Regs__)(struct struct_Sharpen_Arch_Regs* regsPtr);
struct class_Sharpen_Utilities_ByteUtil* classInit_Sharpen_Utilities_ByteUtil(void);
uint8_t* Sharpen_Utilities_ByteUtil_ToBytes_1int32_t_(int32_t inValue);
typedef uint8_t* (*fp_Sharpen_Utilities_ByteUtil_ToBytes_1int32_t_)(int32_t inValue);
uint8_t* Sharpen_Utilities_ByteUtil_ToBytes_1int64_t_(int64_t inValue);
typedef uint8_t* (*fp_Sharpen_Utilities_ByteUtil_ToBytes_1int64_t_)(int64_t inValue);
uint8_t* Sharpen_Utilities_ByteUtil_ToBytes_2int64_t_uint8_t__(int64_t inValue, uint8_t* result);
typedef uint8_t* (*fp_Sharpen_Utilities_ByteUtil_ToBytes_2int64_t_uint8_t__)(int64_t inValue, uint8_t* result);
int64_t Sharpen_Utilities_ByteUtil_ToLong_1uint8_t__(uint8_t* b);
typedef int64_t (*fp_Sharpen_Utilities_ByteUtil_ToLong_1uint8_t__)(uint8_t* b);
int16_t Sharpen_Utilities_ByteUtil_ToShort_2uint8_t__int32_t_(uint8_t* b, int32_t offset);
typedef int16_t (*fp_Sharpen_Utilities_ByteUtil_ToShort_2uint8_t__int32_t_)(uint8_t* b, int32_t offset);
int32_t Sharpen_Utilities_ByteUtil_ToInt_1uint8_t__(uint8_t* b);
typedef int32_t (*fp_Sharpen_Utilities_ByteUtil_ToInt_1uint8_t__)(uint8_t* b);
struct class_Sharpen_Drivers_Other_VboxDev* classInit_Sharpen_Drivers_Other_VboxDev(void);
inline struct struct_Sharpen_Drivers_Other_VboxDev_RequestHeader structInit_Sharpen_Drivers_Other_VboxDev_RequestHeader(void);
inline struct struct_Sharpen_Drivers_Other_VboxDev_RequestSessionID structInit_Sharpen_Drivers_Other_VboxDev_RequestSessionID(void);
inline struct struct_Sharpen_Drivers_Other_VboxDev_RequestGuestInfo structInit_Sharpen_Drivers_Other_VboxDev_RequestGuestInfo(void);
inline struct struct_Sharpen_Drivers_Other_VboxDev_RequestPowerState structInit_Sharpen_Drivers_Other_VboxDev_RequestPowerState(void);
inline struct struct_Sharpen_Drivers_Other_VboxDev_RequestHostTime structInit_Sharpen_Drivers_Other_VboxDev_RequestHostTime(void);
void Sharpen_Drivers_Other_VboxDev_getGuestInfo_0(void);
typedef void (*fp_Sharpen_Drivers_Other_VboxDev_getGuestInfo_0)(void);
void Sharpen_Drivers_Other_VboxDev_initHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_(struct struct_Sharpen_Arch_PCI_PciDevice dev);
typedef void (*fp_Sharpen_Drivers_Other_VboxDev_initHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_)(struct struct_Sharpen_Arch_PCI_PciDevice dev);
void Sharpen_Drivers_Other_VboxDev_exitHander_1struct_struct_Sharpen_Arch_PCI_PciDevice_(struct struct_Sharpen_Arch_PCI_PciDevice dev);
typedef void (*fp_Sharpen_Drivers_Other_VboxDev_exitHander_1struct_struct_Sharpen_Arch_PCI_PciDevice_)(struct struct_Sharpen_Arch_PCI_PciDevice dev);
void Sharpen_Drivers_Other_VboxDev_Init_0(void);
typedef void (*fp_Sharpen_Drivers_Other_VboxDev_Init_0)(void);
void Sharpen_Drivers_Other_VboxDev_ChangePowerState_1int32_t_(int32_t state);
typedef void (*fp_Sharpen_Drivers_Other_VboxDev_ChangePowerState_1int32_t_)(int32_t state);
uint64_t Sharpen_Drivers_Other_VboxDev_GetSessionID_0(void);
typedef uint64_t (*fp_Sharpen_Drivers_Other_VboxDev_GetSessionID_0)(void);
uint64_t Sharpen_Drivers_Other_VboxDev_GetHostTime_0(void);
typedef uint64_t (*fp_Sharpen_Drivers_Other_VboxDev_GetHostTime_0)(void);
struct class_Sharpen_Drivers_Other_VboxDevFSDriver* classInit_Sharpen_Drivers_Other_VboxDevFSDriver(void);
inline void classCctor_Sharpen_Drivers_Other_VboxDevFSDriver(void);
void Sharpen_Drivers_Other_VboxDevFSDriver_Init_0(void);
typedef void (*fp_Sharpen_Drivers_Other_VboxDevFSDriver_Init_0)(void);
struct struct_Sharpen_FileSystem_DirEntry* Sharpen_Drivers_Other_VboxDevFSDriver_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_(struct class_Sharpen_FileSystem_Node* node, uint32_t index);
typedef struct struct_Sharpen_FileSystem_DirEntry* (*fp_Sharpen_Drivers_Other_VboxDevFSDriver_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_)(struct class_Sharpen_FileSystem_Node* node, uint32_t index);
struct class_Sharpen_FileSystem_Node* Sharpen_Drivers_Other_VboxDevFSDriver_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__(struct class_Sharpen_FileSystem_Node* node, char* name);
typedef struct class_Sharpen_FileSystem_Node* (*fp_Sharpen_Drivers_Other_VboxDevFSDriver_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__)(struct class_Sharpen_FileSystem_Node* node, char* name);
uint32_t Sharpen_Drivers_Other_VboxDevFSDriver_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
typedef uint32_t (*fp_Sharpen_Drivers_Other_VboxDevFSDriver_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__)(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
uint32_t Sharpen_Drivers_Other_VboxDevFSDriver_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
typedef uint32_t (*fp_Sharpen_Drivers_Other_VboxDevFSDriver_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__)(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
struct class_Sharpen_Drivers_Power_Acpi* classInit_Sharpen_Drivers_Power_Acpi(void);
inline void classCctor_Sharpen_Drivers_Power_Acpi(void);
void Sharpen_Drivers_Power_Acpi_Find_0(void);
typedef void (*fp_Sharpen_Drivers_Power_Acpi_Find_0)(void);
int32_t Sharpen_Drivers_Power_Acpi_CheckSum_2void__uint32_t_(void* address, uint32_t length);
typedef int32_t (*fp_Sharpen_Drivers_Power_Acpi_CheckSum_2void__uint32_t_)(void* address, uint32_t length);
void Sharpen_Drivers_Power_Acpi_parseS5Object_0(void);
typedef void (*fp_Sharpen_Drivers_Power_Acpi_parseS5Object_0)(void);
void* Sharpen_Drivers_Power_Acpi_getEntry_1char__(char* signature);
typedef void* (*fp_Sharpen_Drivers_Power_Acpi_getEntry_1char__)(char* signature);
void Sharpen_Drivers_Power_Acpi_Init_0(void);
typedef void (*fp_Sharpen_Drivers_Power_Acpi_Init_0)(void);
void Sharpen_Drivers_Power_Acpi_Enable_0(void);
typedef void (*fp_Sharpen_Drivers_Power_Acpi_Enable_0)(void);
void Sharpen_Drivers_Power_Acpi_Disable_0(void);
typedef void (*fp_Sharpen_Drivers_Power_Acpi_Disable_0)(void);
void Sharpen_Drivers_Power_Acpi_Reset_0(void);
typedef void (*fp_Sharpen_Drivers_Power_Acpi_Reset_0)(void);
void Sharpen_Drivers_Power_Acpi_Shutdown_0(void);
typedef void (*fp_Sharpen_Drivers_Power_Acpi_Shutdown_0)(void);
inline struct struct_Sharpen_Drivers_Power_RDSTH structInit_Sharpen_Drivers_Power_RDSTH(void);
inline struct struct_Sharpen_Drivers_Power_RDSP structInit_Sharpen_Drivers_Power_RDSP(void);
inline struct struct_Sharpen_Drivers_Power_GenericAddressStructure structInit_Sharpen_Drivers_Power_GenericAddressStructure(void);
inline struct struct_Sharpen_Drivers_Power_RSDT structInit_Sharpen_Drivers_Power_RSDT(void);
inline struct struct_Sharpen_Drivers_Power_FADT structInit_Sharpen_Drivers_Power_FADT(void);
struct class_Sharpen_Drivers_Sound_IntelHD* classInit_Sharpen_Drivers_Sound_IntelHD(void);
void Sharpen_Drivers_Sound_IntelHD_initHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_(struct struct_Sharpen_Arch_PCI_PciDevice dev);
typedef void (*fp_Sharpen_Drivers_Sound_IntelHD_initHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_)(struct struct_Sharpen_Arch_PCI_PciDevice dev);
void Sharpen_Drivers_Sound_IntelHD_exitHander_1struct_struct_Sharpen_Arch_PCI_PciDevice_(struct struct_Sharpen_Arch_PCI_PciDevice dev);
typedef void (*fp_Sharpen_Drivers_Sound_IntelHD_exitHander_1struct_struct_Sharpen_Arch_PCI_PciDevice_)(struct struct_Sharpen_Arch_PCI_PciDevice dev);
void Sharpen_Drivers_Sound_IntelHD_Init_0(void);
typedef void (*fp_Sharpen_Drivers_Sound_IntelHD_Init_0)(void);
struct class_Sharpen_Drivers_Sound_AC97* classInit_Sharpen_Drivers_Sound_AC97(void);
inline void classCctor_Sharpen_Drivers_Sound_AC97(void);
inline struct struct_Sharpen_Drivers_Sound_AC97_BDL_Entry structInit_Sharpen_Drivers_Sound_AC97_BDL_Entry(void);
void Sharpen_Drivers_Sound_AC97_InitHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_(struct struct_Sharpen_Arch_PCI_PciDevice dev);
typedef void (*fp_Sharpen_Drivers_Sound_AC97_InitHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_)(struct struct_Sharpen_Arch_PCI_PciDevice dev);
void Sharpen_Drivers_Sound_AC97_IRQHandler_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr);
typedef void (*fp_Sharpen_Drivers_Sound_AC97_IRQHandler_1struct_struct_Sharpen_Arch_Regs__)(struct struct_Sharpen_Arch_Regs* regsPtr);
void Sharpen_Drivers_Sound_AC97_ExitHander_1struct_struct_Sharpen_Arch_PCI_PciDevice_(struct struct_Sharpen_Arch_PCI_PciDevice dev);
typedef void (*fp_Sharpen_Drivers_Sound_AC97_ExitHander_1struct_struct_Sharpen_Arch_PCI_PciDevice_)(struct struct_Sharpen_Arch_PCI_PciDevice dev);
void Sharpen_Drivers_Sound_AC97_Init_0(void);
typedef void (*fp_Sharpen_Drivers_Sound_AC97_Init_0)(void);
uint32_t Sharpen_Drivers_Sound_AC97_Reader_1int32_t_(int32_t action);
typedef uint32_t (*fp_Sharpen_Drivers_Sound_AC97_Reader_1int32_t_)(int32_t action);
void Sharpen_Drivers_Sound_AC97_Writer_2int32_t_uint32_t_(int32_t action, uint32_t value);
typedef void (*fp_Sharpen_Drivers_Sound_AC97_Writer_2int32_t_uint32_t_)(int32_t action, uint32_t value);
struct class_Sharpen_FileSystem_DevFS* classInit_Sharpen_FileSystem_DevFS(void);
inline void classCctor_Sharpen_FileSystem_DevFS(void);
int32_t Sharpen_FileSystem_DevFS_Count_getter(void);
void Sharpen_FileSystem_DevFS_Init_0(void);
typedef void (*fp_Sharpen_FileSystem_DevFS_Init_0)(void);
int64_t Sharpen_FileSystem_DevFS_GenerateHash_1char__(char* inVal);
typedef int64_t (*fp_Sharpen_FileSystem_DevFS_GenerateHash_1char__)(char* inVal);
void Sharpen_FileSystem_DevFS_RegisterDevice_1struct_class_Sharpen_FileSystem_Device__(struct class_Sharpen_FileSystem_Device* dev);
typedef void (*fp_Sharpen_FileSystem_DevFS_RegisterDevice_1struct_class_Sharpen_FileSystem_Device__)(struct class_Sharpen_FileSystem_Device* dev);
struct class_Sharpen_FileSystem_Node* Sharpen_FileSystem_DevFS_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__(struct class_Sharpen_FileSystem_Node* node, char* name);
typedef struct class_Sharpen_FileSystem_Node* (*fp_Sharpen_FileSystem_DevFS_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__)(struct class_Sharpen_FileSystem_Node* node, char* name);
struct struct_Sharpen_FileSystem_DirEntry* Sharpen_FileSystem_DevFS_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_(struct class_Sharpen_FileSystem_Node* node, uint32_t index);
typedef struct struct_Sharpen_FileSystem_DirEntry* (*fp_Sharpen_FileSystem_DevFS_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_)(struct class_Sharpen_FileSystem_Node* node, uint32_t index);
struct class_Sharpen_FileSystem_Device* classInit_Sharpen_FileSystem_Device(void);
inline struct struct_Sharpen_FileSystem_DirEntry structInit_Sharpen_FileSystem_DirEntry(void);
struct class_Sharpen_Collections_Dictionary* classInit_Sharpen_Collections_Dictionary(void);
void Sharpen_Collections_Dictionary_Clear_1class_(struct class_Sharpen_Collections_Dictionary* obj);
typedef void (*fp_Sharpen_Collections_Dictionary_Clear_1class_)(void* obj);
int32_t Sharpen_Collections_Dictionary_Count_1class_(struct class_Sharpen_Collections_Dictionary* obj);
typedef int32_t (*fp_Sharpen_Collections_Dictionary_Count_1class_)(void* obj);
void* Sharpen_Collections_Dictionary_GetAt_2class_int32_t_(struct class_Sharpen_Collections_Dictionary* obj, int32_t index);
typedef void* (*fp_Sharpen_Collections_Dictionary_GetAt_2class_int32_t_)(void* obj, int32_t index);
void Sharpen_Collections_Dictionary_Add_3class_int64_t_void__(struct class_Sharpen_Collections_Dictionary* obj, int64_t key, void* val);
typedef void (*fp_Sharpen_Collections_Dictionary_Add_3class_int64_t_void__)(void* obj, int64_t key, void* val);
void* Sharpen_Collections_Dictionary_GetByKey_2class_int64_t_(struct class_Sharpen_Collections_Dictionary* obj, int64_t key);
typedef void* (*fp_Sharpen_Collections_Dictionary_GetByKey_2class_int64_t_)(void* obj, int64_t key);
struct class_Sharpen_FileSystem_Fat16* classInit_Sharpen_FileSystem_Fat16(void);
inline void classCctor_Sharpen_FileSystem_Fat16(void);
void Sharpen_FileSystem_Fat16_initFAT_1struct_class_Sharpen_FileSystem_Node__(struct class_Sharpen_FileSystem_Node* dev);
typedef void (*fp_Sharpen_FileSystem_Fat16_initFAT_1struct_class_Sharpen_FileSystem_Node__)(struct class_Sharpen_FileSystem_Node* dev);
void Sharpen_FileSystem_Fat16_parseBoot_0(void);
typedef void (*fp_Sharpen_FileSystem_Fat16_parseBoot_0)(void);
uint32_t Sharpen_FileSystem_Fat16_Data_clust_to_lba_1uint32_t_(uint32_t cluster);
typedef uint32_t (*fp_Sharpen_FileSystem_Fat16_Data_clust_to_lba_1uint32_t_)(uint32_t cluster);
void Sharpen_FileSystem_Fat16_Init_2struct_class_Sharpen_FileSystem_Node__char__(struct class_Sharpen_FileSystem_Node* dev, char* name);
typedef void (*fp_Sharpen_FileSystem_Fat16_Init_2struct_class_Sharpen_FileSystem_Node__char__)(struct class_Sharpen_FileSystem_Node* dev, char* name);
struct class_Sharpen_FileSystem_Node* Sharpen_FileSystem_Fat16_CreateNode_1struct_struct_Sharpen_FileSystem_FatDirEntry__(struct struct_Sharpen_FileSystem_FatDirEntry* dirEntry);
typedef struct class_Sharpen_FileSystem_Node* (*fp_Sharpen_FileSystem_Fat16_CreateNode_1struct_struct_Sharpen_FileSystem_FatDirEntry__)(struct struct_Sharpen_FileSystem_FatDirEntry* dirEntry);
struct class_Sharpen_FileSystem_Node* Sharpen_FileSystem_Fat16_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__(struct class_Sharpen_FileSystem_Node* node, char* name);
typedef struct class_Sharpen_FileSystem_Node* (*fp_Sharpen_FileSystem_Fat16_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__)(struct class_Sharpen_FileSystem_Node* node, char* name);
struct class_Sharpen_FileSystem_Node* Sharpen_FileSystem_Fat16_FindFileInDirectory_2struct_class_Sharpen_FileSystem_SubDirectory__char__(struct class_Sharpen_FileSystem_SubDirectory* dir, char* testFor);
typedef struct class_Sharpen_FileSystem_Node* (*fp_Sharpen_FileSystem_Fat16_FindFileInDirectory_2struct_class_Sharpen_FileSystem_SubDirectory__char__)(struct class_Sharpen_FileSystem_SubDirectory* dir, char* testFor);
struct struct_Sharpen_FileSystem_DirEntry* Sharpen_FileSystem_Fat16_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_(struct class_Sharpen_FileSystem_Node* node, uint32_t index);
typedef struct struct_Sharpen_FileSystem_DirEntry* (*fp_Sharpen_FileSystem_Fat16_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_)(struct class_Sharpen_FileSystem_Node* node, uint32_t index);
uint32_t Sharpen_FileSystem_Fat16_FindNextCluster_1uint32_t_(uint32_t cluster);
typedef uint32_t (*fp_Sharpen_FileSystem_Fat16_FindNextCluster_1uint32_t_)(uint32_t cluster);
uint32_t Sharpen_FileSystem_Fat16_FirstFirstFreeSector_0(void);
typedef uint32_t (*fp_Sharpen_FileSystem_Fat16_FirstFirstFreeSector_0)(void);
uint32_t Sharpen_FileSystem_Fat16_readFile_4uint32_t_uint32_t_uint32_t_uint8_t__(uint32_t startCluster, uint32_t offset, uint32_t size, uint8_t* buffer);
typedef uint32_t (*fp_Sharpen_FileSystem_Fat16_readFile_4uint32_t_uint32_t_uint32_t_uint8_t__)(uint32_t startCluster, uint32_t offset, uint32_t size, uint8_t* buffer);
struct class_Sharpen_FileSystem_SubDirectory* Sharpen_FileSystem_Fat16_readDirectory_1uint32_t_(uint32_t cluster);
typedef struct class_Sharpen_FileSystem_SubDirectory* (*fp_Sharpen_FileSystem_Fat16_readDirectory_1uint32_t_)(uint32_t cluster);
uint32_t Sharpen_FileSystem_Fat16_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
typedef uint32_t (*fp_Sharpen_FileSystem_Fat16_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__)(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
uint32_t Sharpen_FileSystem_Fat16_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
typedef uint32_t (*fp_Sharpen_FileSystem_Fat16_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__)(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
struct class_Sharpen_Collections_LongIndex* classInit_Sharpen_Collections_LongIndex(void);
int64_t* Sharpen_Collections_LongIndex_Item_getter(struct class_Sharpen_Collections_LongIndex* obj);
int64_t* Sharpen_Collections_LongIndex_Item_setter(struct class_Sharpen_Collections_LongIndex* obj, int64_t* value);
int32_t Sharpen_Collections_LongIndex_Count_getter(struct class_Sharpen_Collections_LongIndex* obj);
int32_t Sharpen_Collections_LongIndex_Count_setter(struct class_Sharpen_Collections_LongIndex* obj, int32_t value);
int32_t Sharpen_Collections_LongIndex_Capacity_getter(struct class_Sharpen_Collections_LongIndex* obj);
int32_t Sharpen_Collections_LongIndex_Capacity_setter(struct class_Sharpen_Collections_LongIndex* obj, int32_t value);
struct class_Sharpen_Collections_LongIndex* Sharpen_Collections_LongIndex_LongIndex_1class_(struct class_Sharpen_Collections_LongIndex* obj);
;
void Sharpen_Collections_LongIndex_EnsureCapacity_2class_int32_t_(struct class_Sharpen_Collections_LongIndex* obj, int32_t required);
typedef void (*fp_Sharpen_Collections_LongIndex_EnsureCapacity_2class_int32_t_)(void* obj, int32_t required);
void Sharpen_Collections_LongIndex_Add_2class_int64_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t o);
typedef void (*fp_Sharpen_Collections_LongIndex_Add_2class_int64_t_)(void* obj, int64_t o);
void Sharpen_Collections_LongIndex_RemoveAt_2class_int32_t_(struct class_Sharpen_Collections_LongIndex* obj, int32_t index);
typedef void (*fp_Sharpen_Collections_LongIndex_RemoveAt_2class_int32_t_)(void* obj, int32_t index);
void Sharpen_Collections_LongIndex_Clear_1class_(struct class_Sharpen_Collections_LongIndex* obj);
typedef void (*fp_Sharpen_Collections_LongIndex_Clear_1class_)(void* obj);
int32_t Sharpen_Collections_LongIndex_Contains_2class_int64_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t item);
typedef int32_t (*fp_Sharpen_Collections_LongIndex_Contains_2class_int64_t_)(void* obj, int64_t item);
void Sharpen_Collections_LongIndex_CopyTo_2class_int64_t__(struct class_Sharpen_Collections_LongIndex* obj, int64_t* array);
typedef void (*fp_Sharpen_Collections_LongIndex_CopyTo_2class_int64_t__)(void* obj, int64_t* array);
void Sharpen_Collections_LongIndex_CopyTo_3class_int64_t__int32_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t* array, int32_t arrayIndex);
typedef void (*fp_Sharpen_Collections_LongIndex_CopyTo_3class_int64_t__int32_t_)(void* obj, int64_t* array, int32_t arrayIndex);
void Sharpen_Collections_LongIndex_CopyTo_5class_int32_t_int64_t__int32_t_int32_t_(struct class_Sharpen_Collections_LongIndex* obj, int32_t index, int64_t* array, int32_t arrayIndex, int32_t count);
typedef void (*fp_Sharpen_Collections_LongIndex_CopyTo_5class_int32_t_int64_t__int32_t_int32_t_)(void* obj, int32_t index, int64_t* array, int32_t arrayIndex, int32_t count);
int32_t Sharpen_Collections_LongIndex_IndexOf_2class_int64_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t item);
typedef int32_t (*fp_Sharpen_Collections_LongIndex_IndexOf_2class_int64_t_)(void* obj, int64_t item);
int32_t Sharpen_Collections_LongIndex_IndexOf_3class_int64_t_int32_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t item, int32_t index);
typedef int32_t (*fp_Sharpen_Collections_LongIndex_IndexOf_3class_int64_t_int32_t_)(void* obj, int64_t item, int32_t index);
int32_t Sharpen_Collections_LongIndex_IndexOf_4class_int64_t_int32_t_int32_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t item, int32_t index, int32_t count);
typedef int32_t (*fp_Sharpen_Collections_LongIndex_IndexOf_4class_int64_t_int32_t_int32_t_)(void* obj, int64_t item, int32_t index, int32_t count);
int32_t Sharpen_Collections_LongIndex_LastIndexOf_2class_int64_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t item);
typedef int32_t (*fp_Sharpen_Collections_LongIndex_LastIndexOf_2class_int64_t_)(void* obj, int64_t item);
int32_t Sharpen_Collections_LongIndex_LastIndexOf_3class_int64_t_int32_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t item, int32_t index);
typedef int32_t (*fp_Sharpen_Collections_LongIndex_LastIndexOf_3class_int64_t_int32_t_)(void* obj, int64_t item, int32_t index);
int32_t Sharpen_Collections_LongIndex_LastIndexOf_4class_int64_t_int32_t_int32_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t item, int32_t index, int32_t count);
typedef int32_t (*fp_Sharpen_Collections_LongIndex_LastIndexOf_4class_int64_t_int32_t_int32_t_)(void* obj, int64_t item, int32_t index, int32_t count);
struct class_Sharpen_Console* classInit_Sharpen_Console(void);
inline void classCctor_Sharpen_Console(void);
int32_t Sharpen_Console_X_getter(void);
int32_t Sharpen_Console_X_setter(int32_t value);
int32_t Sharpen_Console_Y_getter(void);
int32_t Sharpen_Console_Y_setter(int32_t value);
uint8_t Sharpen_Console_Attribute_getter(void);
uint8_t Sharpen_Console_Attribute_setter(uint8_t value);
void Sharpen_Console_PutChar_1char_(char ch);
typedef void (*fp_Sharpen_Console_PutChar_1char_)(char ch);
void Sharpen_Console_Clear_0(void);
typedef void (*fp_Sharpen_Console_Clear_0)(void);
void Sharpen_Console_Write_1char__(char* text);
typedef void (*fp_Sharpen_Console_Write_1char__)(char* text);
void Sharpen_Console_WriteLine_1char__(char* text);
typedef void (*fp_Sharpen_Console_WriteLine_1char__)(char* text);
void Sharpen_Console_WriteHex_1int64_t_(int64_t num);
typedef void (*fp_Sharpen_Console_WriteHex_1int64_t_)(int64_t num);
void Sharpen_Console_WriteNum_1int32_t_(int32_t num);
typedef void (*fp_Sharpen_Console_WriteNum_1int32_t_)(int32_t num);
void Sharpen_Console_MoveCursor_0(void);
typedef void (*fp_Sharpen_Console_MoveCursor_0)(void);
struct class_Sharpen_Arch_GDT* classInit_Sharpen_Arch_GDT(void);
inline struct struct_Sharpen_Arch_GDT_GDT_Entry structInit_Sharpen_Arch_GDT_GDT_Entry(void);
inline struct struct_Sharpen_Arch_GDT_GDT_Pointer structInit_Sharpen_Arch_GDT_GDT_Pointer(void);
inline struct struct_Sharpen_Arch_GDT_TSS structInit_Sharpen_Arch_GDT_TSS(void);
struct struct_Sharpen_Arch_GDT_TSS* Sharpen_Arch_GDT_TSS_Entry_getter(void);
struct struct_Sharpen_Arch_GDT_TSS* Sharpen_Arch_GDT_TSS_Entry_setter(struct struct_Sharpen_Arch_GDT_TSS* value);
int32_t Sharpen_Arch_GDT_Privilege_1int32_t_(int32_t a);
typedef int32_t (*fp_Sharpen_Arch_GDT_Privilege_1int32_t_)(int32_t a);
void Sharpen_Arch_GDT_setEntry_5int32_t_uint64_t_uint64_t_int32_t_int32_t_(int32_t num, uint64_t base_address, uint64_t limit, int32_t access, int32_t granularity);
typedef void (*fp_Sharpen_Arch_GDT_setEntry_5int32_t_uint64_t_uint64_t_int32_t_int32_t_)(int32_t num, uint64_t base_address, uint64_t limit, int32_t access, int32_t granularity);
void Sharpen_Arch_GDT_setTSS_2int32_t_struct_struct_Sharpen_Arch_GDT_TSS__(int32_t num, struct struct_Sharpen_Arch_GDT_TSS* tss);
typedef void (*fp_Sharpen_Arch_GDT_setTSS_2int32_t_struct_struct_Sharpen_Arch_GDT_TSS__)(int32_t num, struct struct_Sharpen_Arch_GDT_TSS* tss);
void Sharpen_Arch_GDT_Init_0(void);
typedef void (*fp_Sharpen_Arch_GDT_Init_0)(void);
void Sharpen_Arch_GDT_flushGDT_1struct_struct_Sharpen_Arch_GDT_GDT_Pointer__(struct struct_Sharpen_Arch_GDT_GDT_Pointer* ptr);
typedef void (*fp_Sharpen_Arch_GDT_flushGDT_1struct_struct_Sharpen_Arch_GDT_GDT_Pointer__)(struct struct_Sharpen_Arch_GDT_GDT_Pointer* ptr);
void Sharpen_Arch_GDT_flushTSS_0(void);
typedef void (*fp_Sharpen_Arch_GDT_flushTSS_0)(void);
struct class_Sharpen_Drivers_Block_ATA* classInit_Sharpen_Drivers_Block_ATA(void);
inline void classCctor_Sharpen_Drivers_Block_ATA(void);
struct struct_Sharpen_Drivers_Block_IDE_Device* Sharpen_Drivers_Block_ATA_Devices_getter(void);
struct struct_Sharpen_Drivers_Block_IDE_Device* Sharpen_Drivers_Block_ATA_Devices_setter(struct struct_Sharpen_Drivers_Block_IDE_Device* value);
void Sharpen_Drivers_Block_ATA_wait400ns_1uint32_t_(uint32_t port);
typedef void (*fp_Sharpen_Drivers_Block_ATA_wait400ns_1uint32_t_)(uint32_t port);
void Sharpen_Drivers_Block_ATA_selectDrive_2uint8_t_uint8_t_(uint8_t channel, uint8_t drive);
typedef void (*fp_Sharpen_Drivers_Block_ATA_selectDrive_2uint8_t_uint8_t_)(uint8_t channel, uint8_t drive);
uint8_t* Sharpen_Drivers_Block_ATA_identify_2uint8_t_uint8_t_(uint8_t channel, uint8_t drive);
typedef uint8_t* (*fp_Sharpen_Drivers_Block_ATA_identify_2uint8_t_uint8_t_)(uint8_t channel, uint8_t drive);
void Sharpen_Drivers_Block_ATA_poll_1uint32_t_(uint32_t port);
typedef void (*fp_Sharpen_Drivers_Block_ATA_poll_1uint32_t_)(uint32_t port);
int32_t Sharpen_Drivers_Block_ATA_ReadSector_4uint32_t_uint32_t_uint8_t_uint8_t__(uint32_t device_num, uint32_t lba, uint8_t size, uint8_t* buffer);
typedef int32_t (*fp_Sharpen_Drivers_Block_ATA_ReadSector_4uint32_t_uint32_t_uint8_t_uint8_t__)(uint32_t device_num, uint32_t lba, uint8_t size, uint8_t* buffer);
int32_t Sharpen_Drivers_Block_ATA_WriteSector_4uint32_t_uint32_t_uint8_t_uint8_t__(uint32_t device_num, uint32_t lba, uint8_t size, uint8_t* buffer);
typedef int32_t (*fp_Sharpen_Drivers_Block_ATA_WriteSector_4uint32_t_uint32_t_uint8_t_uint8_t__)(uint32_t device_num, uint32_t lba, uint8_t size, uint8_t* buffer);
void Sharpen_Drivers_Block_ATA_probe_0(void);
typedef void (*fp_Sharpen_Drivers_Block_ATA_probe_0)(void);
void Sharpen_Drivers_Block_ATA_Init_0(void);
typedef void (*fp_Sharpen_Drivers_Block_ATA_Init_0)(void);
uint32_t Sharpen_Drivers_Block_ATA_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
typedef uint32_t (*fp_Sharpen_Drivers_Block_ATA_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__)(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
uint32_t Sharpen_Drivers_Block_ATA_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
typedef uint32_t (*fp_Sharpen_Drivers_Block_ATA_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__)(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
inline struct struct_Sharpen_Drivers_Block_IDE_Device structInit_Sharpen_Drivers_Block_IDE_Device(void);
struct class_Sharpen_Drivers_Char_SerialPort* classInit_Sharpen_Drivers_Char_SerialPort(void);
inline void classCctor_Sharpen_Drivers_Char_SerialPort(void);
void Sharpen_Drivers_Char_SerialPort_initDevice_1int32_t_(int32_t num);
typedef void (*fp_Sharpen_Drivers_Char_SerialPort_initDevice_1int32_t_)(int32_t num);
uint32_t Sharpen_Drivers_Char_SerialPort_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
typedef uint32_t (*fp_Sharpen_Drivers_Char_SerialPort_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__)(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
uint32_t Sharpen_Drivers_Char_SerialPort_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
typedef uint32_t (*fp_Sharpen_Drivers_Char_SerialPort_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__)(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
int32_t Sharpen_Drivers_Char_SerialPort_isTransmitEmpty_1int32_t_(int32_t port);
typedef int32_t (*fp_Sharpen_Drivers_Char_SerialPort_isTransmitEmpty_1int32_t_)(int32_t port);
int32_t Sharpen_Drivers_Char_SerialPort_hasReceived_1int32_t_(int32_t port);
typedef int32_t (*fp_Sharpen_Drivers_Char_SerialPort_hasReceived_1int32_t_)(int32_t port);
uint8_t Sharpen_Drivers_Char_SerialPort_read_1uint16_t_(uint16_t port);
typedef uint8_t (*fp_Sharpen_Drivers_Char_SerialPort_read_1uint16_t_)(uint16_t port);
void Sharpen_Drivers_Char_SerialPort_write_2uint8_t_uint16_t_(uint8_t d, uint16_t port);
typedef void (*fp_Sharpen_Drivers_Char_SerialPort_write_2uint8_t_uint16_t_)(uint8_t d, uint16_t port);
void Sharpen_Drivers_Char_SerialPort_readBda_0(void);
typedef void (*fp_Sharpen_Drivers_Char_SerialPort_readBda_0)(void);
void Sharpen_Drivers_Char_SerialPort_Handler13_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr);
typedef void (*fp_Sharpen_Drivers_Char_SerialPort_Handler13_1struct_struct_Sharpen_Arch_Regs__)(struct struct_Sharpen_Arch_Regs* regsPtr);
void Sharpen_Drivers_Char_SerialPort_Handler24_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr);
typedef void (*fp_Sharpen_Drivers_Char_SerialPort_Handler24_1struct_struct_Sharpen_Arch_Regs__)(struct struct_Sharpen_Arch_Regs* regsPtr);
void Sharpen_Drivers_Char_SerialPort_Init_0(void);
typedef void (*fp_Sharpen_Drivers_Char_SerialPort_Init_0)(void);
struct class_Sharpen_Drivers_Char_Keyboard* classInit_Sharpen_Drivers_Char_Keyboard(void);
inline void classCctor_Sharpen_Drivers_Char_Keyboard(void);
int32_t Sharpen_Drivers_Char_Keyboard_Capslock_getter(void);
uint8_t Sharpen_Drivers_Char_Keyboard_Shift_getter(void);
uint8_t Sharpen_Drivers_Char_Keyboard_Leds_getter(void);
uint8_t Sharpen_Drivers_Char_Keyboard_Leds_setter(uint8_t value);
char Sharpen_Drivers_Char_Keyboard_transformKey_1uint8_t_(uint8_t scancode);
typedef char (*fp_Sharpen_Drivers_Char_Keyboard_transformKey_1uint8_t_)(uint8_t scancode);
void Sharpen_Drivers_Char_Keyboard_updateLED_0(void);
typedef void (*fp_Sharpen_Drivers_Char_Keyboard_updateLED_0)(void);
void Sharpen_Drivers_Char_Keyboard_Init_0(void);
typedef void (*fp_Sharpen_Drivers_Char_Keyboard_Init_0)(void);
uint32_t Sharpen_Drivers_Char_Keyboard_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
typedef uint32_t (*fp_Sharpen_Drivers_Char_Keyboard_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__)(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
char Sharpen_Drivers_Char_Keyboard_Getch_0(void);
typedef char (*fp_Sharpen_Drivers_Char_Keyboard_Getch_0)(void);
void Sharpen_Drivers_Char_Keyboard_handler_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr);
typedef void (*fp_Sharpen_Drivers_Char_Keyboard_handler_1struct_struct_Sharpen_Arch_Regs__)(struct struct_Sharpen_Arch_Regs* regsPtr);
struct class_Sharpen_Drivers_Char_KeyboardMap* classInit_Sharpen_Drivers_Char_KeyboardMap(void);
inline void classCctor_Sharpen_Drivers_Char_KeyboardMap(void);
inline struct struct_Sharpen_Drivers_Char_SerialPortComport structInit_Sharpen_Drivers_Char_SerialPortComport(void);
char* Sharpen_Drivers_Char_SerialPortComport_Name_getter(struct struct_Sharpen_Drivers_Char_SerialPortComport* obj);
char* Sharpen_Drivers_Char_SerialPortComport_Name_setter(struct struct_Sharpen_Drivers_Char_SerialPortComport* obj, char* value);
uint16_t Sharpen_Drivers_Char_SerialPortComport_Address_getter(struct struct_Sharpen_Drivers_Char_SerialPortComport* obj);
uint16_t Sharpen_Drivers_Char_SerialPortComport_Address_setter(struct struct_Sharpen_Drivers_Char_SerialPortComport* obj, uint16_t value);
struct class_Sharpen_Collections_Fifo* Sharpen_Drivers_Char_SerialPortComport_Buffer_getter(struct struct_Sharpen_Drivers_Char_SerialPortComport* obj);
struct class_Sharpen_Collections_Fifo* Sharpen_Drivers_Char_SerialPortComport_Buffer_setter(struct struct_Sharpen_Drivers_Char_SerialPortComport* obj, struct class_Sharpen_Collections_Fifo* value);
struct class_Sharpen_FileSystem_MountPoint* classInit_Sharpen_FileSystem_MountPoint(void);
struct class_Sharpen_FileSystem_Node* classInit_Sharpen_FileSystem_Node(void);
struct class_Sharpen_FileSystem_VFS* classInit_Sharpen_FileSystem_VFS(void);
inline void classCctor_Sharpen_FileSystem_VFS(void);
void Sharpen_FileSystem_VFS_Init_0(void);
typedef void (*fp_Sharpen_FileSystem_VFS_Init_0)(void);
struct struct_Sharpen_FileSystem_DirEntry* Sharpen_FileSystem_VFS_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_(struct class_Sharpen_FileSystem_Node* node, uint32_t index);
typedef struct struct_Sharpen_FileSystem_DirEntry* (*fp_Sharpen_FileSystem_VFS_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_)(struct class_Sharpen_FileSystem_Node* node, uint32_t index);
int64_t Sharpen_FileSystem_VFS_generateHash_1char__(char* inVal);
typedef int64_t (*fp_Sharpen_FileSystem_VFS_generateHash_1char__)(char* inVal);
void Sharpen_FileSystem_VFS_AddMountPoint_1struct_class_Sharpen_FileSystem_MountPoint__(struct class_Sharpen_FileSystem_MountPoint* mountPoint);
typedef void (*fp_Sharpen_FileSystem_VFS_AddMountPoint_1struct_class_Sharpen_FileSystem_MountPoint__)(struct class_Sharpen_FileSystem_MountPoint* mountPoint);
struct class_Sharpen_FileSystem_MountPoint* Sharpen_FileSystem_VFS_FindMountByName_1char__(char* name);
typedef struct class_Sharpen_FileSystem_MountPoint* (*fp_Sharpen_FileSystem_VFS_FindMountByName_1char__)(char* name);
struct class_Sharpen_FileSystem_Node* Sharpen_FileSystem_VFS_GetByPath_1char__(char* path);
typedef struct class_Sharpen_FileSystem_Node* (*fp_Sharpen_FileSystem_VFS_GetByPath_1char__)(char* path);
void Sharpen_FileSystem_VFS_Open_2struct_class_Sharpen_FileSystem_Node__int32_t_(struct class_Sharpen_FileSystem_Node* node, int32_t fileMode);
typedef void (*fp_Sharpen_FileSystem_VFS_Open_2struct_class_Sharpen_FileSystem_Node__int32_t_)(struct class_Sharpen_FileSystem_Node* node, int32_t fileMode);
void Sharpen_FileSystem_VFS_Close_1struct_class_Sharpen_FileSystem_Node__(struct class_Sharpen_FileSystem_Node* node);
typedef void (*fp_Sharpen_FileSystem_VFS_Close_1struct_class_Sharpen_FileSystem_Node__)(struct class_Sharpen_FileSystem_Node* node);
uint32_t Sharpen_FileSystem_VFS_Read_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
typedef uint32_t (*fp_Sharpen_FileSystem_VFS_Read_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__)(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
uint32_t Sharpen_FileSystem_VFS_Write_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
typedef uint32_t (*fp_Sharpen_FileSystem_VFS_Write_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__)(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
struct class_Sharpen_FileSystem_Node* Sharpen_FileSystem_VFS_FindDir_2struct_class_Sharpen_FileSystem_Node__char__(struct class_Sharpen_FileSystem_Node* node, char* name);
typedef struct class_Sharpen_FileSystem_Node* (*fp_Sharpen_FileSystem_VFS_FindDir_2struct_class_Sharpen_FileSystem_Node__char__)(struct class_Sharpen_FileSystem_Node* node, char* name);
struct struct_Sharpen_FileSystem_DirEntry* Sharpen_FileSystem_VFS_ReadDir_2struct_class_Sharpen_FileSystem_Node__uint32_t_(struct class_Sharpen_FileSystem_Node* node, uint32_t index);
typedef struct struct_Sharpen_FileSystem_DirEntry* (*fp_Sharpen_FileSystem_VFS_ReadDir_2struct_class_Sharpen_FileSystem_Node__uint32_t_)(struct class_Sharpen_FileSystem_Node* node, uint32_t index);
struct class_Sharpen_Heap* classInit_Sharpen_Heap(void);
inline void classCctor_Sharpen_Heap(void);
inline struct struct_Sharpen_Heap_Block structInit_Sharpen_Heap_Block(void);
inline struct struct_Sharpen_Heap_BlockDescriptor structInit_Sharpen_Heap_BlockDescriptor(void);
void* Sharpen_Heap_CurrentEnd_getter(void);
void* Sharpen_Heap_CurrentEnd_setter(void* value);
void Sharpen_Heap_Init_1void__(void* start);
typedef void (*fp_Sharpen_Heap_Init_1void__)(void* start);
int32_t Sharpen_Heap_getRequiredPageCount_1int32_t_(int32_t size);
typedef int32_t (*fp_Sharpen_Heap_getRequiredPageCount_1int32_t_)(int32_t size);
struct struct_Sharpen_Heap_BlockDescriptor* Sharpen_Heap_createBlockDescriptor_1int32_t_(int32_t size);
typedef struct struct_Sharpen_Heap_BlockDescriptor* (*fp_Sharpen_Heap_createBlockDescriptor_1int32_t_)(int32_t size);
struct struct_Sharpen_Heap_BlockDescriptor* Sharpen_Heap_getSufficientDescriptor_1int32_t_(int32_t size);
typedef struct struct_Sharpen_Heap_BlockDescriptor* (*fp_Sharpen_Heap_getSufficientDescriptor_1int32_t_)(int32_t size);
void Sharpen_Heap_SetupRealHeap_0(void);
typedef void (*fp_Sharpen_Heap_SetupRealHeap_0)(void);
void* Sharpen_Heap_AlignedAlloc_2int32_t_int32_t_(int32_t alignment, int32_t size);
typedef void* (*fp_Sharpen_Heap_AlignedAlloc_2int32_t_int32_t_)(int32_t alignment, int32_t size);
void* Sharpen_Heap_Alloc_1int32_t_(int32_t size);
typedef void* (*fp_Sharpen_Heap_Alloc_1int32_t_)(int32_t size);
struct struct_Sharpen_Heap_Block* Sharpen_Heap_getBlockFromPtr_1void__(void* ptr);
typedef struct struct_Sharpen_Heap_Block* (*fp_Sharpen_Heap_getBlockFromPtr_1void__)(void* ptr);
void* Sharpen_Heap_Expand_2void__int32_t_(void* ptr, int32_t newSize);
typedef void* (*fp_Sharpen_Heap_Expand_2void__int32_t_)(void* ptr, int32_t newSize);
void Sharpen_Heap_Free_1void__(void* ptr);
typedef void (*fp_Sharpen_Heap_Free_1void__)(void* ptr);
void Sharpen_Heap_dumpBlock_1struct_struct_Sharpen_Heap_Block__(struct struct_Sharpen_Heap_Block* currentBlock);
typedef void (*fp_Sharpen_Heap_dumpBlock_1struct_struct_Sharpen_Heap_Block__)(struct struct_Sharpen_Heap_Block* currentBlock);
void Sharpen_Heap_Dump_0(void);
typedef void (*fp_Sharpen_Heap_Dump_0)(void);
void* Sharpen_Heap_KAlloc_2int32_t_int32_t_(int32_t size, int32_t align);
typedef void* (*fp_Sharpen_Heap_KAlloc_2int32_t_int32_t_)(int32_t size, int32_t align);
struct class_Sharpen_Collections_Fifo* classInit_Sharpen_Collections_Fifo(void);
struct class_Sharpen_Collections_Fifo* Sharpen_Collections_Fifo_Fifo_2class_int32_t_(struct class_Sharpen_Collections_Fifo* obj, int32_t size);
;
uint32_t Sharpen_Collections_Fifo_ReadWait_3class_uint8_t__uint32_t_(struct class_Sharpen_Collections_Fifo* obj, uint8_t* buffer, uint32_t size);
typedef uint32_t (*fp_Sharpen_Collections_Fifo_ReadWait_3class_uint8_t__uint32_t_)(void* obj, uint8_t* buffer, uint32_t size);
uint32_t Sharpen_Collections_Fifo_Read_3class_uint8_t__uint32_t_(struct class_Sharpen_Collections_Fifo* obj, uint8_t* buffer, uint32_t size);
typedef uint32_t (*fp_Sharpen_Collections_Fifo_Read_3class_uint8_t__uint32_t_)(void* obj, uint8_t* buffer, uint32_t size);
uint32_t Sharpen_Collections_Fifo_Read_4class_uint8_t__uint32_t_uint32_t_(struct class_Sharpen_Collections_Fifo* obj, uint8_t* buffer, uint32_t size, uint32_t offset);
typedef uint32_t (*fp_Sharpen_Collections_Fifo_Read_4class_uint8_t__uint32_t_uint32_t_)(void* obj, uint8_t* buffer, uint32_t size, uint32_t offset);
uint32_t Sharpen_Collections_Fifo_Write_3class_uint8_t__uint32_t_(struct class_Sharpen_Collections_Fifo* obj, uint8_t* buffer, uint32_t size);
typedef uint32_t (*fp_Sharpen_Collections_Fifo_Write_3class_uint8_t__uint32_t_)(void* obj, uint8_t* buffer, uint32_t size);
int32_t Sharpen_Collections_Fifo_WriteByte_2class_uint8_t_(struct class_Sharpen_Collections_Fifo* obj, uint8_t byt);
typedef int32_t (*fp_Sharpen_Collections_Fifo_WriteByte_2class_uint8_t_)(void* obj, uint8_t byt);
struct class_Sharpen_Collections_List* classInit_Sharpen_Collections_List(void);
void** Sharpen_Collections_List_Item_getter(struct class_Sharpen_Collections_List* obj);
void** Sharpen_Collections_List_Item_setter(struct class_Sharpen_Collections_List* obj, void** value);
int32_t Sharpen_Collections_List_Count_getter(struct class_Sharpen_Collections_List* obj);
int32_t Sharpen_Collections_List_Count_setter(struct class_Sharpen_Collections_List* obj, int32_t value);
int32_t Sharpen_Collections_List_Capacity_getter(struct class_Sharpen_Collections_List* obj);
int32_t Sharpen_Collections_List_Capacity_setter(struct class_Sharpen_Collections_List* obj, int32_t value);
struct class_Sharpen_Collections_List* Sharpen_Collections_List_List_1class_(struct class_Sharpen_Collections_List* obj);
;
void Sharpen_Collections_List_EnsureCapacity_2class_int32_t_(struct class_Sharpen_Collections_List* obj, int32_t required);
typedef void (*fp_Sharpen_Collections_List_EnsureCapacity_2class_int32_t_)(void* obj, int32_t required);
void Sharpen_Collections_List_Add_2class_void__(struct class_Sharpen_Collections_List* obj, void* o);
typedef void (*fp_Sharpen_Collections_List_Add_2class_void__)(void* obj, void* o);
void Sharpen_Collections_List_RemoveAt_2class_int32_t_(struct class_Sharpen_Collections_List* obj, int32_t index);
typedef void (*fp_Sharpen_Collections_List_RemoveAt_2class_int32_t_)(void* obj, int32_t index);
void Sharpen_Collections_List_Clear_1class_(struct class_Sharpen_Collections_List* obj);
typedef void (*fp_Sharpen_Collections_List_Clear_1class_)(void* obj);
int32_t Sharpen_Collections_List_Contains_2class_void__(struct class_Sharpen_Collections_List* obj, void* item);
typedef int32_t (*fp_Sharpen_Collections_List_Contains_2class_void__)(void* obj, void* item);
void Sharpen_Collections_List_CopyTo_2class_void___(struct class_Sharpen_Collections_List* obj, void** array);
typedef void (*fp_Sharpen_Collections_List_CopyTo_2class_void___)(void* obj, void** array);
void Sharpen_Collections_List_CopyTo_3class_void___int32_t_(struct class_Sharpen_Collections_List* obj, void** array, int32_t arrayIndex);
typedef void (*fp_Sharpen_Collections_List_CopyTo_3class_void___int32_t_)(void* obj, void** array, int32_t arrayIndex);
void Sharpen_Collections_List_CopyTo_5class_int32_t_void___int32_t_int32_t_(struct class_Sharpen_Collections_List* obj, int32_t index, void** array, int32_t arrayIndex, int32_t count);
typedef void (*fp_Sharpen_Collections_List_CopyTo_5class_int32_t_void___int32_t_int32_t_)(void* obj, int32_t index, void** array, int32_t arrayIndex, int32_t count);
int32_t Sharpen_Collections_List_IndexOf_2class_void__(struct class_Sharpen_Collections_List* obj, void* item);
typedef int32_t (*fp_Sharpen_Collections_List_IndexOf_2class_void__)(void* obj, void* item);
int32_t Sharpen_Collections_List_IndexOf_3class_void__int32_t_(struct class_Sharpen_Collections_List* obj, void* item, int32_t index);
typedef int32_t (*fp_Sharpen_Collections_List_IndexOf_3class_void__int32_t_)(void* obj, void* item, int32_t index);
int32_t Sharpen_Collections_List_IndexOf_4class_void__int32_t_int32_t_(struct class_Sharpen_Collections_List* obj, void* item, int32_t index, int32_t count);
typedef int32_t (*fp_Sharpen_Collections_List_IndexOf_4class_void__int32_t_int32_t_)(void* obj, void* item, int32_t index, int32_t count);
int32_t Sharpen_Collections_List_LastIndexOf_2class_void__(struct class_Sharpen_Collections_List* obj, void* item);
typedef int32_t (*fp_Sharpen_Collections_List_LastIndexOf_2class_void__)(void* obj, void* item);
int32_t Sharpen_Collections_List_LastIndexOf_3class_void__int32_t_(struct class_Sharpen_Collections_List* obj, void* item, int32_t index);
typedef int32_t (*fp_Sharpen_Collections_List_LastIndexOf_3class_void__int32_t_)(void* obj, void* item, int32_t index);
int32_t Sharpen_Collections_List_LastIndexOf_4class_void__int32_t_int32_t_(struct class_Sharpen_Collections_List* obj, void* item, int32_t index, int32_t count);
typedef int32_t (*fp_Sharpen_Collections_List_LastIndexOf_4class_void__int32_t_int32_t_)(void* obj, void* item, int32_t index, int32_t count);
struct class_Sharpen_Collections_BitArray* classInit_Sharpen_Collections_BitArray(void);
struct class_Sharpen_Collections_BitArray* Sharpen_Collections_BitArray_BitArray_2class_int32_t_(struct class_Sharpen_Collections_BitArray* obj, int32_t N);
;
void Sharpen_Collections_BitArray_SetBit_2class_int32_t_(struct class_Sharpen_Collections_BitArray* obj, int32_t k);
typedef void (*fp_Sharpen_Collections_BitArray_SetBit_2class_int32_t_)(void* obj, int32_t k);
void Sharpen_Collections_BitArray_ClearBit_2class_int32_t_(struct class_Sharpen_Collections_BitArray* obj, int32_t k);
typedef void (*fp_Sharpen_Collections_BitArray_ClearBit_2class_int32_t_)(void* obj, int32_t k);
int32_t Sharpen_Collections_BitArray_TestBit_2class_int32_t_(struct class_Sharpen_Collections_BitArray* obj, int32_t k);
typedef int32_t (*fp_Sharpen_Collections_BitArray_TestBit_2class_int32_t_)(void* obj, int32_t k);
int32_t Sharpen_Collections_BitArray_FindFirstFree_1class_(struct class_Sharpen_Collections_BitArray* obj);
typedef int32_t (*fp_Sharpen_Collections_BitArray_FindFirstFree_1class_)(void* obj);
struct class_Sharpen_Lib_Audio* classInit_Sharpen_Lib_Audio(void);
inline struct struct_Sharpen_Lib_Audio_SoundDevice structInit_Sharpen_Lib_Audio_SoundDevice(void);
void Sharpen_Lib_Audio_SetDevice_1struct_struct_Sharpen_Lib_Audio_SoundDevice_(struct struct_Sharpen_Lib_Audio_SoundDevice device);
typedef void (*fp_Sharpen_Lib_Audio_SetDevice_1struct_struct_Sharpen_Lib_Audio_SoundDevice_)(struct struct_Sharpen_Lib_Audio_SoundDevice device);
void Sharpen_Lib_Audio_RequestBuffer_2uint32_t_uint16_t__(uint32_t size, uint16_t* buffer);
typedef void (*fp_Sharpen_Lib_Audio_RequestBuffer_2uint32_t_uint16_t__)(uint32_t size, uint16_t* buffer);
void Sharpen_Lib_Audio_Init_0(void);
typedef void (*fp_Sharpen_Lib_Audio_Init_0)(void);
uint32_t Sharpen_Lib_Audio_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
typedef uint32_t (*fp_Sharpen_Lib_Audio_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__)(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer);
struct class_Sharpen_Memory* classInit_Sharpen_Memory(void);
void Sharpen_Memory_Memcpy_3void__void__int32_t_(void* destination, void* source, int32_t num);
typedef void (*fp_Sharpen_Memory_Memcpy_3void__void__int32_t_)(void* destination, void* source, int32_t num);
int32_t Sharpen_Memory_Compare_3char__char__int32_t_(char* s1, char* s2, int32_t n);
typedef int32_t (*fp_Sharpen_Memory_Compare_3char__char__int32_t_)(char* s1, char* s2, int32_t n);
void Sharpen_Memory_Memset_3void__int32_t_int32_t_(void* ptr, int32_t value, int32_t num);
typedef void (*fp_Sharpen_Memory_Memset_3void__int32_t_int32_t_)(void* ptr, int32_t value, int32_t num);
struct class_Sharpen_Multiboot* classInit_Sharpen_Multiboot(void);
inline void classCctor_Sharpen_Multiboot(void);
inline struct struct_Sharpen_Multiboot_Header structInit_Sharpen_Multiboot_Header(void);
inline struct struct_Sharpen_Multiboot_MMAP structInit_Sharpen_Multiboot_MMAP(void);
inline struct struct_Sharpen_Multiboot_Module structInit_Sharpen_Multiboot_Module(void);
struct class_Sharpen_Panic* classInit_Sharpen_Panic(void);
void Sharpen_Panic_DoPanic_1char__(char* str);
typedef void (*fp_Sharpen_Panic_DoPanic_1char__)(char* str);
struct class_Sharpen_Arch_PortIO* classInit_Sharpen_Arch_PortIO(void);
void Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(uint16_t port, uint8_t value);
typedef void (*fp_Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_)(uint16_t port, uint8_t value);
uint8_t Sharpen_Arch_PortIO_In8_1uint16_t_(uint16_t port);
typedef uint8_t (*fp_Sharpen_Arch_PortIO_In8_1uint16_t_)(uint16_t port);
void Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_(uint16_t port, uint16_t value);
typedef void (*fp_Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_)(uint16_t port, uint16_t value);
uint16_t Sharpen_Arch_PortIO_In16_1uint16_t_(uint16_t port);
typedef uint16_t (*fp_Sharpen_Arch_PortIO_In16_1uint16_t_)(uint16_t port);
void Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_(uint16_t port, uint32_t value);
typedef void (*fp_Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_)(uint16_t port, uint32_t value);
uint32_t Sharpen_Arch_PortIO_In32_1uint16_t_(uint16_t port);
typedef uint32_t (*fp_Sharpen_Arch_PortIO_In32_1uint16_t_)(uint16_t port);
struct class_Sharpen_Arch_Paging* classInit_Sharpen_Arch_Paging(void);
inline struct struct_Sharpen_Arch_Paging_PageTable structInit_Sharpen_Arch_Paging_PageTable(void);
inline struct struct_Sharpen_Arch_Paging_PageDirectory structInit_Sharpen_Arch_Paging_PageDirectory(void);
struct struct_Sharpen_Arch_Paging_PageDirectory* Sharpen_Arch_Paging_KernelDirectory_getter(void);
struct struct_Sharpen_Arch_Paging_PageDirectory* Sharpen_Arch_Paging_KernelDirectory_setter(struct struct_Sharpen_Arch_Paging_PageDirectory* value);
struct struct_Sharpen_Arch_Paging_PageDirectory* Sharpen_Arch_Paging_CurrentDirectory_getter(void);
struct struct_Sharpen_Arch_Paging_PageDirectory* Sharpen_Arch_Paging_CurrentDirectory_setter(struct struct_Sharpen_Arch_Paging_PageDirectory* value);
int32_t Sharpen_Arch_Paging_FrameAddress_1int32_t_(int32_t a);
typedef int32_t (*fp_Sharpen_Arch_Paging_FrameAddress_1int32_t_)(int32_t a);
int32_t Sharpen_Arch_Paging_GetFrameAddress_1int32_t_(int32_t a);
typedef int32_t (*fp_Sharpen_Arch_Paging_GetFrameAddress_1int32_t_)(int32_t a);
void Sharpen_Arch_Paging_Init_1uint32_t_(uint32_t memSize);
typedef void (*fp_Sharpen_Arch_Paging_Init_1uint32_t_)(uint32_t memSize);
void Sharpen_Arch_Paging_MapPage_4struct_struct_Sharpen_Arch_Paging_PageDirectory__int32_t_int32_t_int32_t_(struct struct_Sharpen_Arch_Paging_PageDirectory* directory, int32_t phys, int32_t virt, int32_t flags);
typedef void (*fp_Sharpen_Arch_Paging_MapPage_4struct_struct_Sharpen_Arch_Paging_PageDirectory__int32_t_int32_t_int32_t_)(struct struct_Sharpen_Arch_Paging_PageDirectory* directory, int32_t phys, int32_t virt, int32_t flags);
void* Sharpen_Arch_Paging_GetPhysicalFromVirtual_1void__(void* virt);
typedef void* (*fp_Sharpen_Arch_Paging_GetPhysicalFromVirtual_1void__)(void* virt);
int32_t Sharpen_Arch_Paging_GetPage_2struct_struct_Sharpen_Arch_Paging_PageDirectory__int32_t_(struct struct_Sharpen_Arch_Paging_PageDirectory* directory, int32_t address);
typedef int32_t (*fp_Sharpen_Arch_Paging_GetPage_2struct_struct_Sharpen_Arch_Paging_PageDirectory__int32_t_)(struct struct_Sharpen_Arch_Paging_PageDirectory* directory, int32_t address);
void Sharpen_Arch_Paging_SetFrame_1int32_t_(int32_t frame);
typedef void (*fp_Sharpen_Arch_Paging_SetFrame_1int32_t_)(int32_t frame);
void Sharpen_Arch_Paging_ClearFrame_1int32_t_(int32_t frame);
typedef void (*fp_Sharpen_Arch_Paging_ClearFrame_1int32_t_)(int32_t frame);
int32_t Sharpen_Arch_Paging_AllocateFrame_0(void);
typedef int32_t (*fp_Sharpen_Arch_Paging_AllocateFrame_0)(void);
void Sharpen_Arch_Paging_FreeFrame_1int32_t_(int32_t page);
typedef void (*fp_Sharpen_Arch_Paging_FreeFrame_1int32_t_)(int32_t page);
void* Sharpen_Arch_Paging_AllocatePhysical_1int32_t_(int32_t size);
typedef void* (*fp_Sharpen_Arch_Paging_AllocatePhysical_1int32_t_)(int32_t size);
struct struct_Sharpen_Arch_Paging_PageDirectory* Sharpen_Arch_Paging_CloneDirectory_1struct_struct_Sharpen_Arch_Paging_PageDirectory__(struct struct_Sharpen_Arch_Paging_PageDirectory* source);
typedef struct struct_Sharpen_Arch_Paging_PageDirectory* (*fp_Sharpen_Arch_Paging_CloneDirectory_1struct_struct_Sharpen_Arch_Paging_PageDirectory__)(struct struct_Sharpen_Arch_Paging_PageDirectory* source);
void Sharpen_Arch_Paging_FreeDirectory_1struct_struct_Sharpen_Arch_Paging_PageDirectory__(struct struct_Sharpen_Arch_Paging_PageDirectory* directory);
typedef void (*fp_Sharpen_Arch_Paging_FreeDirectory_1struct_struct_Sharpen_Arch_Paging_PageDirectory__)(struct struct_Sharpen_Arch_Paging_PageDirectory* directory);
void Sharpen_Arch_Paging_Enable_0(void);
typedef void (*fp_Sharpen_Arch_Paging_Enable_0)(void);
void Sharpen_Arch_Paging_Disable_0(void);
typedef void (*fp_Sharpen_Arch_Paging_Disable_0)(void);
void Sharpen_Arch_Paging_setDirectoryInternal_1struct_struct_Sharpen_Arch_Paging_PageDirectory__(struct struct_Sharpen_Arch_Paging_PageDirectory* directory);
typedef void (*fp_Sharpen_Arch_Paging_setDirectoryInternal_1struct_struct_Sharpen_Arch_Paging_PageDirectory__)(struct struct_Sharpen_Arch_Paging_PageDirectory* directory);
int32_t Sharpen_Arch_Paging_ReadCR2_0(void);
typedef int32_t (*fp_Sharpen_Arch_Paging_ReadCR2_0)(void);
struct class_Sharpen_Program* classInit_Sharpen_Program(void);
inline void classCctor_Sharpen_Program(void);
void Sharpen_Program_KernelMain_3struct_struct_Sharpen_Multiboot_Header__uint32_t_uint32_t_(struct struct_Sharpen_Multiboot_Header* header, uint32_t magic, uint32_t end);
typedef void (*fp_Sharpen_Program_KernelMain_3struct_struct_Sharpen_Multiboot_Header__uint32_t_uint32_t_)(struct struct_Sharpen_Multiboot_Header* header, uint32_t magic, uint32_t end);
struct class_Sharpen_Utilities_String* classInit_Sharpen_Utilities_String(void);
int32_t Sharpen_Utilities_String_Length_1char__(char* text);
typedef int32_t (*fp_Sharpen_Utilities_String_Length_1char__)(char* text);
int32_t Sharpen_Utilities_String_IndexOf_2char__char__(char* text, char* occurence);
typedef int32_t (*fp_Sharpen_Utilities_String_IndexOf_2char__char__)(char* text, char* occurence);
int32_t Sharpen_Utilities_String_Count_2char__char_(char* str, char occurence);
typedef int32_t (*fp_Sharpen_Utilities_String_Count_2char__char_)(char* str, char occurence);
char* Sharpen_Utilities_String_SubString_3char__int32_t_int32_t_(char* str, int32_t start, int32_t count);
typedef char* (*fp_Sharpen_Utilities_String_SubString_3char__int32_t_int32_t_)(char* str, int32_t start, int32_t count);
int64_t Sharpen_Utilities_String_toLong_2class_char__(struct class_Sharpen_Utilities_String* obj, char* str);
typedef int64_t (*fp_Sharpen_Utilities_String_toLong_2class_char__)(void* obj, char* str);
char* Sharpen_Utilities_String_Merge_2char__char__(char* first, char* second);
typedef char* (*fp_Sharpen_Utilities_String_Merge_2char__char__)(char* first, char* second);
int32_t Sharpen_Utilities_String_Equals_2char__char__(char* one, char* two);
typedef int32_t (*fp_Sharpen_Utilities_String_Equals_2char__char__)(char* one, char* two);
char Sharpen_Utilities_String_ToUpper_1char_(char c);
typedef char (*fp_Sharpen_Utilities_String_ToUpper_1char_)(char c);
char Sharpen_Utilities_String_ToLower_1char_(char c);
typedef char (*fp_Sharpen_Utilities_String_ToLower_1char_)(char c);
struct class_Sharpen_Time* classInit_Sharpen_Time(void);
int32_t Sharpen_Time_Seconds_getter(void);
int32_t Sharpen_Time_Seconds_setter(int32_t value);
int32_t Sharpen_Time_Minutes_getter(void);
int32_t Sharpen_Time_Minutes_setter(int32_t value);
int32_t Sharpen_Time_Hours_getter(void);
int32_t Sharpen_Time_Hours_setter(int32_t value);
int32_t Sharpen_Time_Day_getter(void);
int32_t Sharpen_Time_Day_setter(int32_t value);
int32_t Sharpen_Time_Month_getter(void);
int32_t Sharpen_Time_Month_setter(int32_t value);
int32_t Sharpen_Time_Year_getter(void);
int32_t Sharpen_Time_Year_setter(int32_t value);
struct class_Sharpen_Utilities_Util* classInit_Sharpen_Utilities_Util(void);
char* Sharpen_Utilities_Util_CharPtrToString_1char__(char* ptr);
typedef char* (*fp_Sharpen_Utilities_Util_CharPtrToString_1char__)(char* ptr);
uint8_t* Sharpen_Utilities_Util_BytePtrToByteArray_1uint8_t__(uint8_t* ptr);
typedef uint8_t* (*fp_Sharpen_Utilities_Util_BytePtrToByteArray_1uint8_t__)(uint8_t* ptr);
void* Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(void* obj);
typedef void* (*fp_Sharpen_Utilities_Util_ObjectToVoidPtr_1void__)(void* obj);
void* Sharpen_Utilities_Util_MethodToPtr_1void__(void* method);
typedef void* (*fp_Sharpen_Utilities_Util_MethodToPtr_1void__)(void* method);

static void* methods_Sharpen_Arch_CMOS[];
static void* methods_Sharpen_Arch_CPU[];
static void* methods_Sharpen_Arch_FPU[];
static void* methods_Sharpen_Arch_IDT[];
static void* methods_Sharpen_Arch_IRQ[];
static void* methods_Sharpen_Arch_ISR[];
static void* methods_Sharpen_Drivers_Net_rtl8139[];
static void* methods_Sharpen_Exec_ELFLoader[];
static void* methods_Sharpen_Exec_Loader[];
static void* methods_Sharpen_Exec_Syscalls[];
static void* methods_Sharpen_FileSystem_SubDirectory[];
static void* methods_Sharpen_FileSystem_STDOUT[];
static void* methods_Sharpen_Net_DHCP[];
static void* methods_Sharpen_Net_Network[];
static void* methods_Sharpen_Net_NetworkTools[];
static void* methods_Sharpen_Task_Task[];
static void* methods_Sharpen_Task_Tasking[];
static void* methods_Sharpen_Arch_PCI[];
static void* methods_Sharpen_Arch_PIC[];
static void* methods_Sharpen_Arch_PIT[];
static void* methods_Sharpen_Arch_Syscall[];
static void* methods_Sharpen_Utilities_ByteUtil[];
static void* methods_Sharpen_Drivers_Other_VboxDev[];
static void* methods_Sharpen_Drivers_Other_VboxDevFSDriver[];
static void* methods_Sharpen_Drivers_Power_Acpi[];
static void* methods_Sharpen_Drivers_Sound_IntelHD[];
static void* methods_Sharpen_Drivers_Sound_AC97[];
static void* methods_Sharpen_FileSystem_DevFS[];
static void* methods_Sharpen_FileSystem_Device[];
static void* methods_Sharpen_Collections_Dictionary[];
static void* methods_Sharpen_FileSystem_Fat16[];
static void* methods_Sharpen_Collections_LongIndex[];
static void* methods_Sharpen_Console[];
static void* methods_Sharpen_Arch_GDT[];
static void* methods_Sharpen_Drivers_Block_ATA[];
static void* methods_Sharpen_Drivers_Char_SerialPort[];
static void* methods_Sharpen_Drivers_Char_Keyboard[];
static void* methods_Sharpen_Drivers_Char_KeyboardMap[];
static void* methods_Sharpen_FileSystem_MountPoint[];
static void* methods_Sharpen_FileSystem_Node[];
static void* methods_Sharpen_FileSystem_VFS[];
static void* methods_Sharpen_Heap[];
static void* methods_Sharpen_Collections_Fifo[];
static void* methods_Sharpen_Collections_List[];
static void* methods_Sharpen_Collections_BitArray[];
static void* methods_Sharpen_Lib_Audio[];
static void* methods_Sharpen_Memory[];
static void* methods_Sharpen_Multiboot[];
static void* methods_Sharpen_Panic[];
static void* methods_Sharpen_Arch_PortIO[];
static void* methods_Sharpen_Arch_Paging[];
static void* methods_Sharpen_Program[];
static void* methods_Sharpen_Utilities_String[];
static void* methods_Sharpen_Time[];
static void* methods_Sharpen_Utilities_Util[];

struct class_Sharpen_Arch_CMOS* classInit_Sharpen_Arch_CMOS(void)
{
	struct class_Sharpen_Arch_CMOS* object = calloc(1, sizeof(struct class_Sharpen_Arch_CMOS));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Arch_CMOS;
	return object;
}

inline void classCctor_Sharpen_Arch_CMOS(void)
{
	classStatics_Sharpen_Arch_CMOS.CMOS_RTC_UPDATING =  ( 1 << 7 ) ;
	classStatics_Sharpen_Arch_CMOS.CMOS_RTC_24H =  ( 1 << 1 ) ;
	classStatics_Sharpen_Arch_CMOS.CMOS_RTC_BIN_MODE =  ( 1 << 2 ) ;
	classStatics_Sharpen_Arch_CMOS.CMOS_RTC_HOURS_PM =  ( 1 << 7 ) ;
}
uint8_t Sharpen_Arch_CMOS_GetData_1uint8_t_(uint8_t reg)
{
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(classStatics_Sharpen_Arch_CMOS.CMOS_CMD, reg);
	return Sharpen_Arch_PortIO_In8_1uint16_t_(classStatics_Sharpen_Arch_CMOS.CMOS_DATA);
}
int32_t Sharpen_Arch_CMOS_BCD_TO_BIN_1int32_t_(int32_t x)
{
	return  (  (  ( x >> 4 )  * 10 )  +  ( x & 0x0F )  ) ;
}
void Sharpen_Arch_CMOS_UpdateTime_0(void)
{
	uint8_t statusB = Sharpen_Arch_CMOS_GetData_1uint8_t_(classStatics_Sharpen_Arch_CMOS.CMOS_STATUS_B);
	int32_t is24h =  (  ( statusB & classStatics_Sharpen_Arch_CMOS.CMOS_RTC_24H )  > 0 ) ;
	int32_t isBin =  (  ( statusB & classStatics_Sharpen_Arch_CMOS.CMOS_RTC_BIN_MODE )  > 0 ) ;
	int32_t oldSeconds, oldMinutes, oldHours, oldDay, oldMonth, oldYear;
	while( ( Sharpen_Arch_CMOS_GetData_1uint8_t_(classStatics_Sharpen_Arch_CMOS.CMOS_STATUS_A) & classStatics_Sharpen_Arch_CMOS.CMOS_RTC_UPDATING )  > 0)
	{
	}
	;
	do
	{
		{
			oldSeconds = Sharpen_Time_Seconds_getter();
			oldMinutes = Sharpen_Time_Minutes_getter();
			oldHours = Sharpen_Time_Hours_getter();
			oldDay = Sharpen_Time_Day_getter();
			oldMonth = Sharpen_Time_Month_getter();
			oldYear = Sharpen_Time_Year_getter();
			while( ( Sharpen_Arch_CMOS_GetData_1uint8_t_(classStatics_Sharpen_Arch_CMOS.CMOS_STATUS_A) & classStatics_Sharpen_Arch_CMOS.CMOS_RTC_UPDATING )  > 0)
			{
			}
			;
			Sharpen_Time_Seconds_setter(Sharpen_Arch_CMOS_GetData_1uint8_t_(classStatics_Sharpen_Arch_CMOS.CMOS_RTC_SECONDS));
			Sharpen_Time_Minutes_setter(Sharpen_Arch_CMOS_GetData_1uint8_t_(classStatics_Sharpen_Arch_CMOS.CMOS_RTC_MINUTES));
			Sharpen_Time_Hours_setter(Sharpen_Arch_CMOS_GetData_1uint8_t_(classStatics_Sharpen_Arch_CMOS.CMOS_RTC_HOURS));
			Sharpen_Time_Day_setter(Sharpen_Arch_CMOS_GetData_1uint8_t_(classStatics_Sharpen_Arch_CMOS.CMOS_RTC_MONTHDAY));
			Sharpen_Time_Month_setter(Sharpen_Arch_CMOS_GetData_1uint8_t_(classStatics_Sharpen_Arch_CMOS.CMOS_RTC_MONTH));
			Sharpen_Time_Year_setter(Sharpen_Arch_CMOS_GetData_1uint8_t_(classStatics_Sharpen_Arch_CMOS.CMOS_RTC_YEAR));
			if( ! is24h &&  (  ( Sharpen_Time_Hours_getter() & classStatics_Sharpen_Arch_CMOS.CMOS_RTC_HOURS_PM )  > 0 ) ){
				Sharpen_Time_Hours_setter(Sharpen_Time_Hours_getter() &  ~ classStatics_Sharpen_Arch_CMOS.CMOS_RTC_HOURS_PM);
				Sharpen_Time_Hours_setter(Sharpen_Time_Hours_getter() + 12);
				if(Sharpen_Time_Hours_getter() == 24)
								Sharpen_Time_Hours_setter(0);
			}
			if( ! isBin){
				Sharpen_Time_Seconds_setter(Sharpen_Arch_CMOS_BCD_TO_BIN_1int32_t_(Sharpen_Time_Seconds_getter()));
				Sharpen_Time_Minutes_setter(Sharpen_Arch_CMOS_BCD_TO_BIN_1int32_t_(Sharpen_Time_Minutes_getter()));
				Sharpen_Time_Hours_setter(Sharpen_Arch_CMOS_BCD_TO_BIN_1int32_t_(Sharpen_Time_Hours_getter()));
				Sharpen_Time_Day_setter(Sharpen_Arch_CMOS_BCD_TO_BIN_1int32_t_(Sharpen_Time_Day_getter()));
				Sharpen_Time_Month_setter(Sharpen_Arch_CMOS_BCD_TO_BIN_1int32_t_(Sharpen_Time_Month_getter()));
				Sharpen_Time_Year_setter(Sharpen_Arch_CMOS_BCD_TO_BIN_1int32_t_(Sharpen_Time_Year_getter()));
			}
			Sharpen_Time_Year_setter(Sharpen_Time_Year_getter() + 2000);
		}
	}
	while(oldSeconds != Sharpen_Time_Seconds_getter() || oldMinutes != Sharpen_Time_Minutes_getter() || oldHours != Sharpen_Time_Hours_getter() || oldDay != Sharpen_Time_Day_getter() || oldMonth != Sharpen_Time_Month_getter() || oldYear != Sharpen_Time_Year_getter());
}
struct class_Sharpen_Arch_CPU* classInit_Sharpen_Arch_CPU(void)
{
	struct class_Sharpen_Arch_CPU* object = calloc(1, sizeof(struct class_Sharpen_Arch_CPU));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Arch_CPU;
	return object;
}

struct class_Sharpen_Arch_FPU* classInit_Sharpen_Arch_FPU(void)
{
	struct class_Sharpen_Arch_FPU* object = calloc(1, sizeof(struct class_Sharpen_Arch_FPU));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Arch_FPU;
	return object;
}

struct class_Sharpen_Arch_IDT* classInit_Sharpen_Arch_IDT(void)
{
	struct class_Sharpen_Arch_IDT* object = calloc(1, sizeof(struct class_Sharpen_Arch_IDT));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Arch_IDT;
	return object;
}

inline void classCctor_Sharpen_Arch_IDT(void)
{
	classStatics_Sharpen_Arch_IDT.FLAG_ISR = (uint8_t) ( Sharpen_Arch_IDT_Present_1int32_t_(1) | Sharpen_Arch_IDT_Privilege_1int32_t_(0) | (int32_t)enum_Sharpen_Arch_IDT_Type_INT32 ) ;
	classStatics_Sharpen_Arch_IDT.FLAG_IRQ = (uint8_t) ( Sharpen_Arch_IDT_Present_1int32_t_(1) | Sharpen_Arch_IDT_Privilege_1int32_t_(0) | (int32_t)enum_Sharpen_Arch_IDT_Type_TRAP32 ) ;
	classStatics_Sharpen_Arch_IDT.FLAG_INT = (uint8_t) ( Sharpen_Arch_IDT_Present_1int32_t_(1) | Sharpen_Arch_IDT_Privilege_1int32_t_(3) | (int32_t)enum_Sharpen_Arch_IDT_Type_INT32 ) ;
	classStatics_Sharpen_Arch_IDT.FLAG_TRAP = (uint8_t) ( Sharpen_Arch_IDT_Present_1int32_t_(1) | Sharpen_Arch_IDT_Privilege_1int32_t_(3) | (int32_t)enum_Sharpen_Arch_IDT_Type_TRAP32 ) ;
}
inline struct struct_Sharpen_Arch_IDT_IDT_Entry structInit_Sharpen_Arch_IDT_IDT_Entry(void)
{
	struct struct_Sharpen_Arch_IDT_IDT_Entry object;
	return object;
}
inline struct struct_Sharpen_Arch_IDT_IDT_Pointer structInit_Sharpen_Arch_IDT_IDT_Pointer(void)
{
	struct struct_Sharpen_Arch_IDT_IDT_Pointer object;
	return object;
}
int32_t Sharpen_Arch_IDT_Privilege_1int32_t_(int32_t a)
{
	return  ( a << 0x05 ) ;
}
int32_t Sharpen_Arch_IDT_Present_1int32_t_(int32_t a)
{
	return  ( a << 0x07 ) ;
}
void Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(int32_t num, void* address, uint16_t selector, uint8_t flags)
{
	uint32_t addr = (uint32_t)address;
	classStatics_Sharpen_Arch_IDT.m_entries[num].field_AddressLow = (uint16_t) ( addr & 0xFFFF ) ;
	classStatics_Sharpen_Arch_IDT.m_entries[num].field_AddressHigh = (uint16_t) (  ( addr >> 16 )  & 0xFFFF ) ;
	classStatics_Sharpen_Arch_IDT.m_entries[num].field_Selector = selector;
	classStatics_Sharpen_Arch_IDT.m_entries[num].field_Zero = 0;
	classStatics_Sharpen_Arch_IDT.m_entries[num].field_Flags = flags;
}
void Sharpen_Arch_IDT_Init_0(void)
{
	classStatics_Sharpen_Arch_IDT.m_entries = calloc((256), sizeof(struct struct_Sharpen_Arch_IDT_IDT_Entry));
	classStatics_Sharpen_Arch_IDT.m_ptr = structInit_Sharpen_Arch_IDT_IDT_Pointer();
	classStatics_Sharpen_Arch_IDT.m_ptr.field_Limit = (uint16_t) (  ( 256 * sizeof(struct struct_Sharpen_Arch_IDT_IDT_Entry) )  - 1 ) ;
	{
		struct struct_Sharpen_Arch_IDT_IDT_Entry* ptr = classStatics_Sharpen_Arch_IDT.m_entries;
		{
			classStatics_Sharpen_Arch_IDT.m_ptr.field_BaseAddress = (uint32_t)ptr;
		}
	}
	void* ignore = Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_INTIgnore_0);
	for(int32_t i = 0;i < 256;i = i + 1
	)
	{
		{
			Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(i, ignore, 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
		}
	}
	;
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(0, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR0_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(1, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR1_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(2, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR2_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(3, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR3_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(4, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR4_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(5, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR5_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(6, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR6_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(7, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR7_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(8, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR8_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(9, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR9_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(10, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR10_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(11, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR11_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(12, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR12_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(13, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR13_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(14, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR14_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(15, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR15_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(16, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR16_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(17, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR17_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(18, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR18_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(19, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR19_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(20, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR20_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(21, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR21_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(22, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR22_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(23, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR23_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(24, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR24_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(25, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR25_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(26, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR26_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(27, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR27_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(28, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR28_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(29, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR29_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(30, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR30_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(31, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_ISR31_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_ISR);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(classStatics_Sharpen_Arch_IRQ.MASTER_OFFSET + 0, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_IRQ0_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_IRQ);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(classStatics_Sharpen_Arch_IRQ.MASTER_OFFSET + 1, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_IRQ1_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_IRQ);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(classStatics_Sharpen_Arch_IRQ.MASTER_OFFSET + 2, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_IRQ2_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_IRQ);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(classStatics_Sharpen_Arch_IRQ.MASTER_OFFSET + 3, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_IRQ3_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_IRQ);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(classStatics_Sharpen_Arch_IRQ.MASTER_OFFSET + 4, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_IRQ4_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_IRQ);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(classStatics_Sharpen_Arch_IRQ.MASTER_OFFSET + 5, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_IRQ5_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_IRQ);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(classStatics_Sharpen_Arch_IRQ.MASTER_OFFSET + 6, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_IRQ6_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_IRQ);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(classStatics_Sharpen_Arch_IRQ.MASTER_OFFSET + 7, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_IRQ7_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_IRQ);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(classStatics_Sharpen_Arch_IRQ.SLAVE_OFFSET + 0, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_IRQ8_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_IRQ);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(classStatics_Sharpen_Arch_IRQ.SLAVE_OFFSET + 1, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_IRQ9_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_IRQ);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(classStatics_Sharpen_Arch_IRQ.SLAVE_OFFSET + 2, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_IRQ10_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_IRQ);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(classStatics_Sharpen_Arch_IRQ.SLAVE_OFFSET + 3, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_IRQ11_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_IRQ);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(classStatics_Sharpen_Arch_IRQ.SLAVE_OFFSET + 4, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_IRQ12_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_IRQ);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(classStatics_Sharpen_Arch_IRQ.SLAVE_OFFSET + 5, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_IRQ13_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_IRQ);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(classStatics_Sharpen_Arch_IRQ.SLAVE_OFFSET + 6, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_IRQ14_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_IRQ);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(classStatics_Sharpen_Arch_IRQ.SLAVE_OFFSET + 7, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_IRQ15_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_IRQ);
	Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_(0x80, Sharpen_Utilities_Util_MethodToPtr_1void__(Sharpen_Arch_IDT_Syscall_0), 0x08, classStatics_Sharpen_Arch_IDT.FLAG_INT);
	{
		struct struct_Sharpen_Arch_IDT_IDT_Pointer* ptr = &classStatics_Sharpen_Arch_IDT.m_ptr;
		{
			Sharpen_Arch_IDT_flushIDT_1struct_struct_Sharpen_Arch_IDT_IDT_Pointer__(ptr);
		}
	}
	Sharpen_Arch_CPU_STI_0();
}
struct class_Sharpen_Arch_IRQ* classInit_Sharpen_Arch_IRQ(void)
{
	struct class_Sharpen_Arch_IRQ* object = calloc(1, sizeof(struct class_Sharpen_Arch_IRQ));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Arch_IRQ;
	return object;
}

inline void classCctor_Sharpen_Arch_IRQ(void)
{
}
void Sharpen_Arch_IRQ_SetHandler_2int32_t_delegate_Sharpen_Arch_IRQ_IRQHandler_(int32_t num, delegate_Sharpen_Arch_IRQ_IRQHandler handler)
{
	classStatics_Sharpen_Arch_IRQ.handlers[num] = handler;
}
void Sharpen_Arch_IRQ_RemoveHandler_1int32_t_(int32_t num)
{
	classStatics_Sharpen_Arch_IRQ.handlers[num] = null;
}
void Sharpen_Arch_IRQ_Handler_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr)
{
	int32_t irqNum =  ( *regsPtr ) .field_IntNum - classStatics_Sharpen_Arch_IRQ.MASTER_OFFSET;
	if((classStatics_Sharpen_Arch_IRQ.handlers[irqNum]) != null){
		classStatics_Sharpen_Arch_IRQ.handlers[irqNum](regsPtr);
	}
	;
	Sharpen_Arch_PIC_SendEOI_1uint8_t_((uint8_t)irqNum);
}
struct class_Sharpen_Arch_ISR* classInit_Sharpen_Arch_ISR(void)
{
	struct class_Sharpen_Arch_ISR* object = calloc(1, sizeof(struct class_Sharpen_Arch_ISR));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Arch_ISR;
	return object;
}

inline void classCctor_Sharpen_Arch_ISR(void)
{
}
void Sharpen_Arch_ISR_Handler_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr)
{
	int32_t isrNum =  ( *regsPtr ) .field_IntNum;
	Sharpen_Console_WriteLine_1char__(classStatics_Sharpen_Arch_ISR.m_errorCodes[isrNum]);
	Sharpen_Console_WriteHex_1int64_t_(Sharpen_Arch_Paging_ReadCR2_0());
	Sharpen_Arch_CPU_CLI_0();
	Sharpen_Arch_CPU_HLT_0();
}
struct class_Sharpen_Drivers_Net_rtl8139* classInit_Sharpen_Drivers_Net_rtl8139(void)
{
	struct class_Sharpen_Drivers_Net_rtl8139* object = calloc(1, sizeof(struct class_Sharpen_Drivers_Net_rtl8139));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Drivers_Net_rtl8139;
	return object;
}

inline void classCctor_Sharpen_Drivers_Net_rtl8139(void)
{
}
void Sharpen_Drivers_Net_rtl8139_initHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_(struct struct_Sharpen_Arch_PCI_PciDevice dev)
{
	classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base = dev.field_Port1;
	classStatics_Sharpen_Drivers_Net_rtl8139.m_buffer = calloc((8192 + 16), sizeof(uint8_t));
	classStatics_Sharpen_Drivers_Net_rtl8139.m_transmit0 = calloc((8192 + 16), sizeof(uint8_t));
	classStatics_Sharpen_Drivers_Net_rtl8139.m_transmit1 = calloc((8192 + 16), sizeof(uint8_t));
	classStatics_Sharpen_Drivers_Net_rtl8139.m_transmit2 = calloc((8192 + 16), sizeof(uint8_t));
	classStatics_Sharpen_Drivers_Net_rtl8139.m_transmit3 = calloc((8192 + 16), sizeof(uint8_t));
	classStatics_Sharpen_Drivers_Net_rtl8139.m_mac = calloc((6), sizeof(uint8_t));
	uint32_t outVal = Sharpen_Arch_PCI_PCIReadWord_2struct_struct_Sharpen_Arch_PCI_PciDevice_uint16_t_(dev, 0x3C);
	outVal &= 0x00;
	outVal |= 10;
	Sharpen_Arch_PCI_PCIWrite_5uint16_t_uint16_t_uint16_t_uint16_t_uint32_t_(dev.field_Bus, dev.field_Slot, dev.field_Function, 0x3C, outVal);
	classStatics_Sharpen_Drivers_Net_rtl8139.m_irqNum = Sharpen_Arch_PCI_PCIReadWord_2struct_struct_Sharpen_Arch_PCI_PciDevice_uint16_t_(dev, 0x3C) & 0xFF;
	Sharpen_Arch_PCI_PCIWrite_5uint16_t_uint16_t_uint16_t_uint16_t_uint32_t_(dev.field_Bus, dev.field_Slot, dev.field_Function, classStatics_Sharpen_Arch_PCI.COMMAND, 0x05);
	Sharpen_Drivers_Net_rtl8139_turnOn_0();
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_CMD ) , (uint8_t)classStatics_Sharpen_Drivers_Net_rtl8139.CMD_RST);
	while( ( Sharpen_Arch_PortIO_In8_1uint16_t_((uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_CMD ) ) & classStatics_Sharpen_Drivers_Net_rtl8139.CMD_RST )  != 0)
	{
		{
		}
	}
	;
	void* inAdr = Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(classStatics_Sharpen_Drivers_Net_rtl8139.m_buffer);
	uint32_t adr = (uint32_t)Sharpen_Arch_Paging_GetPhysicalFromVirtual_1void__(inAdr);
	Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_((uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_BUF ) , 0xFFFF);
	Sharpen_Drivers_Net_rtl8139_setInterruptMask_1uint16_t_(0x0000);
	Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_((uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_RC ) , 0xf |  ( 1 << 7 ) );
	Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_((uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_TC ) , 0x700);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_CMD ) , (uint8_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.CMD_TXE | classStatics_Sharpen_Drivers_Net_rtl8139.CMD_RXE ) );
	Sharpen_Drivers_Net_rtl8139_updateLinkStatus_0();
	Sharpen_Arch_IRQ_SetHandler_2int32_t_delegate_Sharpen_Arch_IRQ_IRQHandler_(classStatics_Sharpen_Drivers_Net_rtl8139.m_irqNum, Sharpen_Drivers_Net_rtl8139_handler_1struct_struct_Sharpen_Arch_Regs__);
	inAdr = Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(classStatics_Sharpen_Drivers_Net_rtl8139.m_transmit0);
	adr = (uint32_t)Sharpen_Arch_Paging_GetPhysicalFromVirtual_1void__(inAdr);
	Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_((uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_TSAD0 ) , adr);
	inAdr = Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(classStatics_Sharpen_Drivers_Net_rtl8139.m_transmit1);
	adr = (uint32_t)Sharpen_Arch_Paging_GetPhysicalFromVirtual_1void__(inAdr);
	Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_((uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_TSAD1 ) , adr);
	inAdr = Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(classStatics_Sharpen_Drivers_Net_rtl8139.m_transmit2);
	adr = (uint32_t)Sharpen_Arch_Paging_GetPhysicalFromVirtual_1void__(inAdr);
	Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_((uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_TSAD2 ) , adr);
	inAdr = Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(classStatics_Sharpen_Drivers_Net_rtl8139.m_transmit3);
	adr = (uint32_t)Sharpen_Arch_Paging_GetPhysicalFromVirtual_1void__(inAdr);
	Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_((uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_TSAD3 ) , adr);
	Sharpen_Drivers_Net_rtl8139_readMac_0();
	struct struct_Sharpen_Net_Network_NetDevice netDev = structInit_Sharpen_Net_Network_NetDevice();
	netDev.field_ID = dev.field_Device;
	netDev.field_Transmit = Sharpen_Drivers_Net_rtl8139_Transmit_2uint8_t__uint32_t_;
	netDev.field_GetMac = Sharpen_Drivers_Net_rtl8139_GetMac_1uint8_t__;
	Sharpen_Net_Network_Set_1struct_struct_Sharpen_Net_Network_NetDevice_(netDev);
}
void Sharpen_Drivers_Net_rtl8139_GetMac_1uint8_t__(uint8_t* mac)
{
	for(int32_t i = 0;i < 6;i = i + 1
	)
	{
		mac[i] = classStatics_Sharpen_Drivers_Net_rtl8139.m_mac[i];
	}
	;
}
void Sharpen_Drivers_Net_rtl8139_Transmit_2uint8_t__uint32_t_(uint8_t* bytes, uint32_t size)
{
	uint8_t* inAdr = (uint8_t*)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(classStatics_Sharpen_Drivers_Net_rtl8139.m_transmit0);
	uint16_t adr = (uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_TSD0 ) ;
	if(classStatics_Sharpen_Drivers_Net_rtl8139.m_curBuffer == 0){
		classStatics_Sharpen_Drivers_Net_rtl8139.m_curBuffer = classStatics_Sharpen_Drivers_Net_rtl8139.m_curBuffer + 1
		;
	}
	else if(classStatics_Sharpen_Drivers_Net_rtl8139.m_curBuffer == 1){
		inAdr = (uint8_t*)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(classStatics_Sharpen_Drivers_Net_rtl8139.m_transmit1);
		adr = (uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_TSD1 ) ;
		classStatics_Sharpen_Drivers_Net_rtl8139.m_curBuffer = classStatics_Sharpen_Drivers_Net_rtl8139.m_curBuffer + 1
		;
	}
	else if(classStatics_Sharpen_Drivers_Net_rtl8139.m_curBuffer == 2){
		inAdr = (uint8_t*)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(classStatics_Sharpen_Drivers_Net_rtl8139.m_transmit2);
		adr = (uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_TSD2 ) ;
		classStatics_Sharpen_Drivers_Net_rtl8139.m_curBuffer = classStatics_Sharpen_Drivers_Net_rtl8139.m_curBuffer + 1
		;
	}
	else if(classStatics_Sharpen_Drivers_Net_rtl8139.m_curBuffer == 3){
		inAdr = (uint8_t*)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(classStatics_Sharpen_Drivers_Net_rtl8139.m_transmit3);
		adr = (uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_TSD3 ) ;
		classStatics_Sharpen_Drivers_Net_rtl8139.m_curBuffer = 0;
	}
	Sharpen_Memory_Memset_3void__int32_t_int32_t_(inAdr, 0x00, 8192 + 16);
	Sharpen_Memory_Memcpy_3void__void__int32_t_(inAdr, bytes, (int32_t)size);
	Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_(adr, size);
}
void Sharpen_Drivers_Net_rtl8139_PrintRes_0(void)
{
	Sharpen_Console_WriteLine_1char__("");
	Sharpen_Console_WriteLine_1char__("-----------------------------------");
	Sharpen_Console_WriteLine_1char__("RTL8139 registers");
	Sharpen_Console_WriteLine_1char__("");
	Sharpen_Console_Write_1char__("MAC: ");
	for(int32_t i = 0;i < 6;i = i + 1
	)
	{
		{
			Sharpen_Console_WriteHex_1int64_t_(classStatics_Sharpen_Drivers_Net_rtl8139.m_mac[i]);
			Sharpen_Console_PutChar_1char_(' ');
		}
	}
	;
	Sharpen_Console_WriteLine_1char__("");
	Sharpen_Console_Write_1char__("Linkspeed: ");
	Sharpen_Console_WriteNum_1int32_t_(classStatics_Sharpen_Drivers_Net_rtl8139.m_linkSpeed);
	Sharpen_Console_WriteLine_1char__("");
	Sharpen_Console_Write_1char__("Linkstate: ");
	Sharpen_Console_WriteLine_1char__( ( classStatics_Sharpen_Drivers_Net_rtl8139.m_linkFail )  ? "FAIL" : "OK");
	Sharpen_Console_Write_1char__("IRQ number: ");
	Sharpen_Console_WriteNum_1int32_t_(classStatics_Sharpen_Drivers_Net_rtl8139.m_irqNum);
	Sharpen_Console_WriteLine_1char__("");
}
void Sharpen_Drivers_Net_rtl8139_handler_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr)
{
	Sharpen_Console_WriteLine_1char__("-------------------------");
	Sharpen_Console_WriteLine_1char__("RTL8139 ");
	Sharpen_Console_WriteLine_1char__("-------------------------");
}
void Sharpen_Drivers_Net_rtl8139_turnOn_0(void)
{
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.CONFIG_1 ) , 0x00);
}
void Sharpen_Drivers_Net_rtl8139_readMac_0(void)
{
	for(int32_t i = 0;i < 6;i = i + 1
	)
	{
		classStatics_Sharpen_Drivers_Net_rtl8139.m_mac[i] = Sharpen_Arch_PortIO_In8_1uint16_t_((uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_MAC + i ) );
	}
	;
}
void Sharpen_Drivers_Net_rtl8139_setInterruptMask_1uint16_t_(uint16_t mask)
{
	Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_((uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_IM ) , mask);
}
void Sharpen_Drivers_Net_rtl8139_ackowledgeInterrupts_0(void)
{
	Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_((uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_IS ) , 0xFF);
}
void Sharpen_Drivers_Net_rtl8139_updateLinkStatus_0(void)
{
	uint8_t mediaState = Sharpen_Arch_PortIO_In8_1uint16_t_((uint16_t) ( classStatics_Sharpen_Drivers_Net_rtl8139.m_io_base + classStatics_Sharpen_Drivers_Net_rtl8139.REG_MS ) );
	classStatics_Sharpen_Drivers_Net_rtl8139.m_linkSpeed =  (  ( mediaState & classStatics_Sharpen_Drivers_Net_rtl8139.MS_SPEED_10 )  > 0 )  ? 10 : 100;
	classStatics_Sharpen_Drivers_Net_rtl8139.m_linkFail =  (  ( mediaState & classStatics_Sharpen_Drivers_Net_rtl8139.MS_LINKB )  > 0 ) ;
}
void Sharpen_Drivers_Net_rtl8139_exitHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_(struct struct_Sharpen_Arch_PCI_PciDevice dev)
{
}
void Sharpen_Drivers_Net_rtl8139_Init_0(void)
{
	struct struct_Sharpen_Arch_PCI_PciDriver driver = structInit_Sharpen_Arch_PCI_PciDriver();
	driver.field_Name = "RTL8139 Driver";
	driver.field_Exit = Sharpen_Drivers_Net_rtl8139_exitHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_;
	driver.field_Init = Sharpen_Drivers_Net_rtl8139_initHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_;
	Sharpen_Arch_PCI_RegisterDriver_3uint16_t_uint16_t_struct_struct_Sharpen_Arch_PCI_PciDriver_(0x10EC, 0x8139, driver);
}
struct class_Sharpen_Exec_ELFLoader* classInit_Sharpen_Exec_ELFLoader(void)
{
	struct class_Sharpen_Exec_ELFLoader* object = calloc(1, sizeof(struct class_Sharpen_Exec_ELFLoader));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Exec_ELFLoader;
	return object;
}

inline struct struct_Sharpen_Exec_ELFLoader_ELF32 structInit_Sharpen_Exec_ELFLoader_ELF32(void)
{
	struct struct_Sharpen_Exec_ELFLoader_ELF32 object;
	return object;
}
inline struct struct_Sharpen_Exec_ELFLoader_ProgramHeader structInit_Sharpen_Exec_ELFLoader_ProgramHeader(void)
{
	struct struct_Sharpen_Exec_ELFLoader_ProgramHeader object;
	return object;
}
inline struct struct_Sharpen_Exec_ELFLoader_SectionHeader structInit_Sharpen_Exec_ELFLoader_SectionHeader(void)
{
	struct struct_Sharpen_Exec_ELFLoader_SectionHeader object;
	return object;
}
int32_t Sharpen_Exec_ELFLoader_isValidELF_1struct_struct_Sharpen_Exec_ELFLoader_ELF32__(struct struct_Sharpen_Exec_ELFLoader_ELF32* elf)
{
	if(elf->field_Ident[(int32_t)enum_Sharpen_Exec_ELFLoader_Ident_EI_MAG0] != 0x7F || elf->field_Ident[(int32_t)enum_Sharpen_Exec_ELFLoader_Ident_EI_MAG1] != 'E' || elf->field_Ident[(int32_t)enum_Sharpen_Exec_ELFLoader_Ident_EI_MAG2] != 'L' || elf->field_Ident[(int32_t)enum_Sharpen_Exec_ELFLoader_Ident_EI_MAG3] != 'F')
		return false;
	if(elf->field_Type != (int32_t)enum_Sharpen_Exec_ELFLoader_ExecutableType_ET_EXEC)
		return false;
	if(elf->field_Version != 1)
		return false;
	if(elf->field_Machine != (int32_t)enum_Sharpen_Exec_ELFLoader_MachineType_EM_X86)
		return false;
	return true;
}
struct struct_Sharpen_Exec_ELFLoader_SectionHeader* Sharpen_Exec_ELFLoader_getSection_2struct_struct_Sharpen_Exec_ELFLoader_ELF32__uint32_t_(struct struct_Sharpen_Exec_ELFLoader_ELF32* elf, uint32_t index)
{
	return (struct struct_Sharpen_Exec_ELFLoader_SectionHeader*) ( (int32_t)elf + elf->field_ShOff +  ( index * elf->field_ShEntSize )  ) ;
}
char* Sharpen_Exec_ELFLoader_getString_2struct_struct_Sharpen_Exec_ELFLoader_ELF32__uint32_t_(struct struct_Sharpen_Exec_ELFLoader_ELF32* elf, uint32_t offset)
{
	struct struct_Sharpen_Exec_ELFLoader_SectionHeader* section = Sharpen_Exec_ELFLoader_getSection_2struct_struct_Sharpen_Exec_ELFLoader_ELF32__uint32_t_(elf, elf->field_ShnStrNdx);
	uint32_t strtab = (uint32_t)elf + section->field_Offset;
	return Sharpen_Utilities_Util_CharPtrToString_1char__((char*) ( strtab + offset ) );
}
int32_t Sharpen_Exec_ELFLoader_Execute_3uint8_t__uint32_t_char___(uint8_t* buffer, uint32_t size, char** argv)
{
	struct struct_Sharpen_Exec_ELFLoader_ELF32* elf;
	{
		uint8_t* ptr = buffer;
		{
			elf = (struct struct_Sharpen_Exec_ELFLoader_ELF32*)ptr;
		}
	}
	if( ! Sharpen_Exec_ELFLoader_isValidELF_1struct_struct_Sharpen_Exec_ELFLoader_ELF32__(elf))
		return enum_Sharpen_ErrorCode_EINVAL;
	struct struct_Sharpen_Exec_ELFLoader_ProgramHeader* programHeader = (struct struct_Sharpen_Exec_ELFLoader_ProgramHeader*) ( (int32_t)elf + elf->field_PhOff ) ;
	uint32_t virtAddress = programHeader->field_VirtAddress;
	void* allocated = Sharpen_Heap_AlignedAlloc_2int32_t_int32_t_(0x1000, (int32_t)size);
	for(uint32_t i = 0;i < elf->field_ShNum;i = i + 1
	)
	{
		{
			struct struct_Sharpen_Exec_ELFLoader_SectionHeader* section = Sharpen_Exec_ELFLoader_getSection_2struct_struct_Sharpen_Exec_ELFLoader_ELF32__uint32_t_(elf, i);
			if(section->field_Address == 0)
						continue;
			uint32_t offset = section->field_Address - virtAddress;
			if(section->field_Type == (uint32_t)enum_Sharpen_Exec_ELFLoader_SectionHeaderType_SHT_NOBITS){
				Sharpen_Memory_Memset_3void__int32_t_int32_t_((void*) ( (uint32_t)allocated + offset ) , 0, (int32_t)section->field_Size);
			}
			else
			{
				Sharpen_Memory_Memcpy_3void__void__int32_t_((void*) ( (uint32_t)allocated + offset ) , (void*) ( (uint32_t)elf + section->field_Offset ) , (int32_t)section->field_Size);
			}
		}
	}
	;
	for(uint32_t j = 0;j < size + 0x1000;j += 0x1000)
	{
		{
			Sharpen_Arch_Paging_MapPage_4struct_struct_Sharpen_Arch_Paging_PageDirectory__int32_t_int32_t_int32_t_(Sharpen_Arch_Paging_CurrentDirectory_getter(), (int32_t) ( (uint32_t)allocated + j ) , (int32_t) ( virtAddress + j ) , (int32_t)enum_Sharpen_Arch_Paging_PageFlags_Present | (int32_t)enum_Sharpen_Arch_Paging_PageFlags_Writable | (int32_t)enum_Sharpen_Arch_Paging_PageFlags_UserMode);
		}
	}
	;
	Sharpen_Task_Tasking_AddTask_2void__int32_t_((void*)elf->field_Entry, enum_Sharpen_Task_TaskPriority_NORMAL);
	return enum_Sharpen_ErrorCode_SUCCESS;
}
struct class_Sharpen_Exec_Loader* classInit_Sharpen_Exec_Loader(void)
{
	struct class_Sharpen_Exec_Loader* object = calloc(1, sizeof(struct class_Sharpen_Exec_Loader));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Exec_Loader;
	return object;
}

int32_t Sharpen_Exec_Loader_StartProcess_2char__char___(char* path, char** argv)
{
	struct class_Sharpen_FileSystem_Node* node = Sharpen_FileSystem_VFS_GetByPath_1char__(path);
	if(node == null)
		return enum_Sharpen_ErrorCode_ENOENT;
	Sharpen_FileSystem_VFS_Open_2struct_class_Sharpen_FileSystem_Node__int32_t_(node, enum_Sharpen_FileSystem_FileMode_O_RDONLY);
	uint8_t* buffer = calloc((node->field_Size), sizeof(uint8_t));
	if(buffer == null){
		Sharpen_FileSystem_VFS_Close_1struct_class_Sharpen_FileSystem_Node__(node);
		return enum_Sharpen_ErrorCode_ENOMEM;
	}
	Sharpen_FileSystem_VFS_Read_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(node, 0, node->field_Size, buffer);
	Sharpen_FileSystem_VFS_Close_1struct_class_Sharpen_FileSystem_Node__(node);
	return Sharpen_Exec_ELFLoader_Execute_3uint8_t__uint32_t_char___(buffer, node->field_Size, argv);
}
struct class_Sharpen_Exec_Syscalls* classInit_Sharpen_Exec_Syscalls(void)
{
	struct class_Sharpen_Exec_Syscalls* object = calloc(1, sizeof(struct class_Sharpen_Exec_Syscalls));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Exec_Syscalls;
	return object;
}

int32_t Sharpen_Exec_Syscalls_Exit_1int32_t_(int32_t status)
{
	Sharpen_Task_Tasking_RemoveTaskByPID_1int32_t_(Sharpen_Task_Tasking_CurrentTask_getter()->field_PID);
	return 0;
}
int32_t Sharpen_Exec_Syscalls_GetPID_0(void)
{
	return Sharpen_Task_Tasking_CurrentTask_getter()->field_PID;
}
int32_t Sharpen_Exec_Syscalls_Sbrk_1int32_t_(int32_t increase)
{
	return (int32_t)Sharpen_Arch_Paging_AllocatePhysical_1int32_t_(increase);
}
int32_t Sharpen_Exec_Syscalls_Fork_0(void)
{
	return  - (int32_t)enum_Sharpen_ErrorCode_EAGAIN;
}
int32_t Sharpen_Exec_Syscalls_Write_3int32_t_uint8_t__uint32_t_(int32_t descriptor, uint8_t* buffer, uint32_t size)
{
	struct class_Sharpen_FileSystem_Node* node = Sharpen_Task_Tasking_GetNodeFromDescriptor_1int32_t_(descriptor);
	if(node == null)
		return  - (int32_t)enum_Sharpen_ErrorCode_EBADF;
	uint32_t offset = Sharpen_Task_Tasking_GetOffsetFromDescriptor_1int32_t_(descriptor);
	Sharpen_Task_Tasking_CurrentTask_getter()->field_FileDescriptors.field_Offsets[descriptor] += size;
	return (int32_t)Sharpen_FileSystem_VFS_Write_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(node, offset, size, buffer);
}
int32_t Sharpen_Exec_Syscalls_Read_3int32_t_uint8_t__uint32_t_(int32_t descriptor, uint8_t* buffer, uint32_t size)
{
	struct class_Sharpen_FileSystem_Node* node = Sharpen_Task_Tasking_GetNodeFromDescriptor_1int32_t_(descriptor);
	if(node == null)
		return  - (int32_t)enum_Sharpen_ErrorCode_EBADF;
	uint32_t offset = Sharpen_Task_Tasking_GetOffsetFromDescriptor_1int32_t_(descriptor);
	Sharpen_Task_Tasking_CurrentTask_getter()->field_FileDescriptors.field_Offsets[descriptor] += size;
	return (int32_t)Sharpen_FileSystem_VFS_Read_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(node, offset, size, buffer);
}
int32_t Sharpen_Exec_Syscalls_Open_2char__int32_t_(char* path, int32_t flags)
{
	struct class_Sharpen_FileSystem_Node* node = Sharpen_FileSystem_VFS_GetByPath_1char__(path);
	if(node == null)
		return  - (int32_t)enum_Sharpen_ErrorCode_ENOENT;
	Sharpen_FileSystem_VFS_Open_2struct_class_Sharpen_FileSystem_Node__int32_t_(node, enum_Sharpen_FileSystem_FileMode_O_RDWR);
	return Sharpen_Task_Tasking_AddNodeToDescriptor_1struct_class_Sharpen_FileSystem_Node__(node);
}
int32_t Sharpen_Exec_Syscalls_Close_1int32_t_(int32_t descriptor)
{
	struct class_Sharpen_FileSystem_Node* node = Sharpen_Task_Tasking_GetNodeFromDescriptor_1int32_t_(descriptor);
	if(node == null)
		return  - (int32_t)enum_Sharpen_ErrorCode_EBADF;
	Sharpen_FileSystem_VFS_Close_1struct_class_Sharpen_FileSystem_Node__(node);
	Sharpen_Task_Tasking_CurrentTask_getter()->field_FileDescriptors.field_Nodes[descriptor] = null;
	Sharpen_Task_Tasking_CurrentTask_getter()->field_FileDescriptors.field_Used = Sharpen_Task_Tasking_CurrentTask_getter()->field_FileDescriptors.field_Used - 1
	;
	return 0;
}
int32_t Sharpen_Exec_Syscalls_Seek_3int32_t_uint32_t_int32_t_(int32_t descriptor, uint32_t offset, int32_t whence)
{
	struct class_Sharpen_FileSystem_Node* node = Sharpen_Task_Tasking_GetNodeFromDescriptor_1int32_t_(descriptor);
	if(node == null)
		return  - (int32_t)enum_Sharpen_ErrorCode_EBADF;
	if(whence == enum_Sharpen_FileSystem_FileWhence_SEEK_CUR)
		Sharpen_Task_Tasking_CurrentTask_getter()->field_FileDescriptors.field_Offsets[descriptor] += offset;
	else if(whence == enum_Sharpen_FileSystem_FileWhence_SEEK_SET)
		Sharpen_Task_Tasking_CurrentTask_getter()->field_FileDescriptors.field_Offsets[descriptor] = offset;
	else
	{
		Sharpen_Task_Tasking_CurrentTask_getter()->field_FileDescriptors.field_Offsets[descriptor] = node->field_Size - offset;
	}
	return (int32_t)Sharpen_Task_Tasking_CurrentTask_getter()->field_FileDescriptors.field_Offsets[descriptor];
}
inline struct struct_Sharpen_FileSystem_Fat16BPB structInit_Sharpen_FileSystem_Fat16BPB(void)
{
	struct struct_Sharpen_FileSystem_Fat16BPB object;
	return object;
}
inline struct struct_Sharpen_FileSystem_FatDirEntry structInit_Sharpen_FileSystem_FatDirEntry(void)
{
	struct struct_Sharpen_FileSystem_FatDirEntry object;
	return object;
}
struct class_Sharpen_FileSystem_SubDirectory* classInit_Sharpen_FileSystem_SubDirectory(void)
{
	struct class_Sharpen_FileSystem_SubDirectory* object = calloc(1, sizeof(struct class_Sharpen_FileSystem_SubDirectory));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_FileSystem_SubDirectory;
	return object;
}

struct class_Sharpen_FileSystem_STDOUT* classInit_Sharpen_FileSystem_STDOUT(void)
{
	struct class_Sharpen_FileSystem_STDOUT* object = calloc(1, sizeof(struct class_Sharpen_FileSystem_STDOUT));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_FileSystem_STDOUT;
	return object;
}

void Sharpen_FileSystem_STDOUT_Init_0(void)
{
	struct class_Sharpen_FileSystem_Device* device = classInit_Sharpen_FileSystem_Device();
	device->field_Name = "stdout";
	device->field_node = classInit_Sharpen_FileSystem_Node();
	device->field_node->field_Write = Sharpen_FileSystem_STDOUT_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__;
	Sharpen_FileSystem_DevFS_RegisterDevice_1struct_class_Sharpen_FileSystem_Device__(device);
}
uint32_t Sharpen_FileSystem_STDOUT_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer)
{
	for(int32_t i = 0;i < size;i = i + 1
	)
	{
		Sharpen_Console_PutChar_1char_((char)buffer[i]);
	}
	;
	return size;
}
struct class_Sharpen_Net_DHCP* classInit_Sharpen_Net_DHCP(void)
{
	struct class_Sharpen_Net_DHCP* object = calloc(1, sizeof(struct class_Sharpen_Net_DHCP));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Net_DHCP;
	return object;
}

inline struct struct_Sharpen_Net_DHCP_DHCPHeader structInit_Sharpen_Net_DHCP_DHCPHeader(void)
{
	struct struct_Sharpen_Net_DHCP_DHCPHeader object;
	return object;
}
struct struct_Sharpen_Net_DHCP_DHCPHeader Sharpen_Net_DHCP_makeHeader_3uint32_t_uint32_t_uint8_t_(uint32_t xid, uint32_t clientAddr, uint8_t type)
{
	return structInit_Sharpen_Net_DHCP_DHCPHeader();
}
void Sharpen_Net_DHCP_Sample_0(void)
{
}
struct class_Sharpen_Net_Network* classInit_Sharpen_Net_Network(void)
{
	struct class_Sharpen_Net_Network* object = calloc(1, sizeof(struct class_Sharpen_Net_Network));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Net_Network;
	return object;
}

inline struct struct_Sharpen_Net_Network_NetDevice structInit_Sharpen_Net_Network_NetDevice(void)
{
	struct struct_Sharpen_Net_Network_NetDevice object;
	return object;
}
struct struct_Sharpen_Net_Network_NetDevice Sharpen_Net_Network_Device_getter(void)
{
	return classStatics_Sharpen_Net_Network.m_dev;
}
void Sharpen_Net_Network_Set_1struct_struct_Sharpen_Net_Network_NetDevice_(struct struct_Sharpen_Net_Network_NetDevice device)
{
	Sharpen_Console_Write_1char__("[NET] Primary network device set with ID ");
	Sharpen_Console_WriteHex_1int64_t_((int32_t)device.field_ID);
	Sharpen_Console_WriteLine_1char__("");
	classStatics_Sharpen_Net_Network.m_dev = device;
}
void Sharpen_Net_Network_Transmit_2uint8_t__uint32_t_(uint8_t* bytes, uint32_t size)
{
	Sharpen_Console_Write_1char__("[NET] Transmit packet with ");
	Sharpen_Console_WriteNum_1int32_t_((int32_t)size);
	Sharpen_Console_WriteLine_1char__(" bytes");
	if(classStatics_Sharpen_Net_Network.m_dev.field_ID != 0)
		classStatics_Sharpen_Net_Network.m_dev.field_Transmit(bytes, size);
}
void Sharpen_Net_Network_GetMac_1uint8_t__(uint8_t* mac)
{
	if(classStatics_Sharpen_Net_Network.m_dev.field_ID != 0)
		classStatics_Sharpen_Net_Network.m_dev.field_GetMac(mac);
}
struct class_Sharpen_Net_NetworkTools* classInit_Sharpen_Net_NetworkTools(void)
{
	struct class_Sharpen_Net_NetworkTools* object = calloc(1, sizeof(struct class_Sharpen_Net_NetworkTools));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Net_NetworkTools;
	return object;
}

void Sharpen_Net_NetworkTools_WakeOnLan_1uint8_t__(uint8_t* mac)
{
	Sharpen_Console_Write_1char__("Waking up by LAN, MAC: ");
	for(int32_t i = 0;i < 6;i = i + 1
	)
	{
		{
			Sharpen_Console_WriteHex_1int64_t_(mac[i]);
			if(i != 5)
						Sharpen_Console_Write_1char__(":");
		}
	}
	;
	Sharpen_Console_WriteLine_1char__("");
	uint8_t* buffer = (uint8_t*)Sharpen_Heap_Alloc_1int32_t_(108);
	for(int32_t i = 0;i < 6;i = i + 1
	)
	{
		buffer[i] = 0xFF;
	}
	;
	for(int32_t i = 0;i < 16;i = i + 1
	)
	{
		for(int32_t j = 0;j < 6;j = j + 1
		)
		{
			buffer[6 +  ( i * 6 )  + j] = mac[j];
		}
		;
	}
	;
	Sharpen_Net_Network_Transmit_2uint8_t__uint32_t_(buffer, 108);
	Sharpen_Heap_Free_1void__(buffer);
}
inline struct struct_Sharpen_Task_FileDescriptors structInit_Sharpen_Task_FileDescriptors(void)
{
	struct struct_Sharpen_Task_FileDescriptors object;
	return object;
}
struct class_Sharpen_Task_Task* classInit_Sharpen_Task_Task(void)
{
	struct class_Sharpen_Task_Task* object = calloc(1, sizeof(struct class_Sharpen_Task_Task));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Task_Task;
	return object;
}

struct class_Sharpen_Task_Tasking* classInit_Sharpen_Task_Tasking(void)
{
	struct class_Sharpen_Task_Tasking* object = calloc(1, sizeof(struct class_Sharpen_Task_Tasking));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Task_Tasking;
	return object;
}

inline void classCctor_Sharpen_Task_Tasking(void)
{
}
struct class_Sharpen_Task_Task* Sharpen_Task_Tasking_KernelTask_getter(void)
{
	return classStatics_Sharpen_Task_Tasking.prop_KernelTask;
}
struct class_Sharpen_Task_Task* Sharpen_Task_Tasking_KernelTask_setter(struct class_Sharpen_Task_Task* value)
{
	classStatics_Sharpen_Task_Tasking.prop_KernelTask = value;
	return value;
}
struct class_Sharpen_Task_Task* Sharpen_Task_Tasking_CurrentTask_getter(void)
{
	return classStatics_Sharpen_Task_Tasking.prop_CurrentTask;
}
struct class_Sharpen_Task_Task* Sharpen_Task_Tasking_CurrentTask_setter(struct class_Sharpen_Task_Task* value)
{
	classStatics_Sharpen_Task_Tasking.prop_CurrentTask = value;
	return value;
}
void Sharpen_Task_Tasking_Init_0(void)
{
	Sharpen_Arch_CPU_CLI_0();
	struct class_Sharpen_Task_Task* kernel = classInit_Sharpen_Task_Task();
	kernel->field_PID = classStatics_Sharpen_Task_Tasking.m_lastPid;
	classStatics_Sharpen_Task_Tasking.m_lastPid = classStatics_Sharpen_Task_Tasking.m_lastPid + 1;
	kernel->field_GID = 0;
	kernel->field_UID = 0;
	kernel->field_PageDir = Sharpen_Arch_Paging_KernelDirectory_getter();
	kernel->field_Next = null;
	kernel->field_FPUContext = Sharpen_Heap_AlignedAlloc_2int32_t_int32_t_(16, 512);
	Sharpen_Task_Tasking_KernelTask_setter(kernel);
	Sharpen_Task_Tasking_CurrentTask_setter(kernel);
	classStatics_Sharpen_Task_Tasking.m_taskingEnabled = true;
	Sharpen_Arch_CPU_STI_0();
	Sharpen_Task_Tasking_ManualSchedule_0();
}
struct class_Sharpen_Task_Task* Sharpen_Task_Tasking_GetTaskByPID_1int32_t_(int32_t pid)
{
	struct class_Sharpen_Task_Task* current = Sharpen_Task_Tasking_KernelTask_getter();
	while(true)
	{
		{
			if(current->field_PID == pid)
						return current;
			current = current->field_Next;
			if(current == null)
						return null;
		}
	}
	;
}
void Sharpen_Task_Tasking_RemoveTaskByPID_1int32_t_(int32_t pid)
{
	struct class_Sharpen_Task_Task* current = Sharpen_Task_Tasking_KernelTask_getter();
	struct class_Sharpen_Task_Task* previous = null;
	while(true)
	{
		{
			if(current->field_PID == pid)
						break;
			previous = current;
			current = current->field_Next;
			if(current == null)
						return;
		}
	}
	;
	if(current == Sharpen_Task_Tasking_CurrentTask_getter())
		Sharpen_Arch_CPU_HLT_0();
	Sharpen_Arch_CPU_CLI_0();
	previous->field_Next = current->field_Next;
	Sharpen_Heap_Free_1void__(current->field_FPUContext);
	Sharpen_Heap_Free_1void__(current->field_Stack);
	Sharpen_Arch_Paging_FreeDirectory_1struct_struct_Sharpen_Arch_Paging_PageDirectory__(current->field_PageDir);
	Sharpen_Arch_CPU_STI_0();
}
void Sharpen_Task_Tasking_ScheduleTask_1struct_class_Sharpen_Task_Task__(struct class_Sharpen_Task_Task* task)
{
	struct class_Sharpen_Task_Task* current = Sharpen_Task_Tasking_CurrentTask_getter();
	while(true)
	{
		{
			if(current->field_Next == null)
						break;
			current = current->field_Next;
		}
	}
	;
	Sharpen_Arch_CPU_CLI_0();
	current->field_Next = task;
	Sharpen_Arch_CPU_STI_0();
}
void Sharpen_Task_Tasking_AddTask_2void__int32_t_(void* eip, int32_t priority)
{
	struct class_Sharpen_Task_Task* newTask = classInit_Sharpen_Task_Task();
	newTask->field_PID = classStatics_Sharpen_Task_Tasking.m_lastPid;
	classStatics_Sharpen_Task_Tasking.m_lastPid = classStatics_Sharpen_Task_Tasking.m_lastPid + 1;
	newTask->field_GID = 0;
	newTask->field_UID = 0;
	newTask->field_PageDir =  ( Sharpen_Arch_Paging_CurrentDirectory_getter() ) ;
	newTask->field_TimeFull = (int32_t)priority;
	newTask->field_Stack = (int32_t*) ( (int32_t)Sharpen_Heap_AlignedAlloc_2int32_t_int32_t_(16, 8192) + 8192 ) ;
	newTask->field_Stack = Sharpen_Task_Tasking_writeSchedulerStack_4int32_t__int32_t_int32_t_void__(newTask->field_Stack, 0x1B, 0x23, eip);
	newTask->field_KernelStack = (int32_t*) ( (int32_t)Sharpen_Heap_AlignedAlloc_2int32_t_int32_t_(16, 4096) + 4096 ) ;
	newTask->field_DataEnd = null;
	newTask->field_FileDescriptors.field_Capacity = 16;
	newTask->field_FileDescriptors.field_Used = 0;
	newTask->field_FileDescriptors.field_Nodes = calloc((newTask->field_FileDescriptors.field_Capacity), sizeof(struct class_Sharpen_FileSystem_Node*));
	newTask->field_FileDescriptors.field_Offsets = calloc((newTask->field_FileDescriptors.field_Capacity), sizeof(uint32_t));
	newTask->field_FPUContext = Sharpen_Heap_AlignedAlloc_2int32_t_int32_t_(16, 512);
	Sharpen_Arch_FPU_StoreContext_1void__(newTask->field_FPUContext);
	Sharpen_Task_Tasking_ScheduleTask_1struct_class_Sharpen_Task_Task__(newTask);
}
struct class_Sharpen_Task_Task* Sharpen_Task_Tasking_FindNextTask_0(void)
{
	struct class_Sharpen_Task_Task* current = Sharpen_Task_Tasking_CurrentTask_getter();
	current->field_TimeLeft = current->field_TimeLeft - 1
	;
	if(current->field_TimeLeft > 0)
		return current;
	current->field_TimeLeft = current->field_TimeFull;
	struct class_Sharpen_Task_Task* next = Sharpen_Task_Tasking_CurrentTask_getter()->field_Next;
	if(next == null)
		return Sharpen_Task_Tasking_KernelTask_getter();
	return next;
}
struct struct_Sharpen_Arch_Regs* Sharpen_Task_Tasking_scheduler_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr)
{
	if( ! classStatics_Sharpen_Task_Tasking.m_taskingEnabled)
		return regsPtr;
	struct class_Sharpen_Task_Task* oldTask = Sharpen_Task_Tasking_CurrentTask_getter();
	oldTask->field_Stack = (int32_t*)regsPtr;
	oldTask->field_KernelStack = (int32_t*)Sharpen_Arch_GDT_TSS_Entry_getter()->field_ESP0;
	Sharpen_Arch_FPU_StoreContext_1void__(oldTask->field_FPUContext);
	struct class_Sharpen_Task_Task* current = Sharpen_Task_Tasking_FindNextTask_0();
	Sharpen_Arch_Paging_CurrentDirectory_setter(current->field_PageDir);
	Sharpen_Arch_FPU_RestoreContext_1void__(current->field_FPUContext);
	Sharpen_Arch_GDT_TSS_Entry_getter()->field_ESP0 = (uint32_t)current->field_KernelStack;
	Sharpen_Task_Tasking_CurrentTask_setter(current);
	return (struct struct_Sharpen_Arch_Regs*)current->field_Stack;
}
int32_t* Sharpen_Task_Tasking_writeSchedulerStack_4int32_t__int32_t_int32_t_void__(int32_t* ptr, int32_t cs, int32_t ds, void* eip)
{
	int32_t esp = (int32_t)ptr;
	*--ptr = ds;
	*--ptr = esp;
	*--ptr = 0x200;
	*--ptr = cs;
	*--ptr = (int32_t)eip;
	*--ptr = 0;
	*--ptr = 0;
	*--ptr = 0;
	*--ptr = 0;
	ptr = ptr - 1
	;
	*--ptr = 0;
	*--ptr = 0;
	*--ptr = 0;
	*--ptr = ds;
	*--ptr = ds;
	*--ptr = ds;
	*--ptr = ds;
	return ptr;
}
struct class_Sharpen_FileSystem_Node* Sharpen_Task_Tasking_GetNodeFromDescriptor_1int32_t_(int32_t descriptor)
{
	struct class_Sharpen_Task_Task* current = Sharpen_Task_Tasking_CurrentTask_getter();
	if(descriptor >= current->field_FileDescriptors.field_Capacity)
		return null;
	return current->field_FileDescriptors.field_Nodes[descriptor];
}
uint32_t Sharpen_Task_Tasking_GetOffsetFromDescriptor_1int32_t_(int32_t descriptor)
{
	struct class_Sharpen_Task_Task* current = Sharpen_Task_Tasking_CurrentTask_getter();
	if(descriptor >= current->field_FileDescriptors.field_Capacity)
		return 0;
	return current->field_FileDescriptors.field_Offsets[descriptor];
}
int32_t Sharpen_Task_Tasking_AddNodeToDescriptor_1struct_class_Sharpen_FileSystem_Node__(struct class_Sharpen_FileSystem_Node* node)
{
	struct class_Sharpen_Task_Task* current = Sharpen_Task_Tasking_CurrentTask_getter();
	if(current->field_FileDescriptors.field_Used == current->field_FileDescriptors.field_Capacity){
		int32_t oldCap = current->field_FileDescriptors.field_Capacity;
		current->field_FileDescriptors.field_Capacity += 8;
		struct class_Sharpen_FileSystem_Node** newNodeArray = calloc((current->field_FileDescriptors.field_Capacity), sizeof(struct class_Sharpen_FileSystem_Node*));
		uint32_t* newOffsetArray = calloc((current->field_FileDescriptors.field_Capacity), sizeof(uint32_t));
		Sharpen_Memory_Memcpy_3void__void__int32_t_(Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(newNodeArray), Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(current->field_FileDescriptors.field_Nodes), oldCap * sizeof(void*));
		Sharpen_Memory_Memcpy_3void__void__int32_t_(Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(newOffsetArray), Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(current->field_FileDescriptors.field_Offsets), oldCap * sizeof(uint32_t));
		current->field_FileDescriptors.field_Nodes = newNodeArray;
		current->field_FileDescriptors.field_Offsets = newOffsetArray;
	}
	int32_t i = 0;
	for(;i < current->field_FileDescriptors.field_Capacity;i = i + 1
	)
	{
		{
			if(current->field_FileDescriptors.field_Nodes[i] == null){
				current->field_FileDescriptors.field_Nodes[i] = node;
				current->field_FileDescriptors.field_Offsets[i] = 0;
				break;
			}
		}
	}
	;
	current->field_FileDescriptors.field_Used = current->field_FileDescriptors.field_Used + 1
	;
	return i;
}
struct class_Sharpen_Arch_PCI* classInit_Sharpen_Arch_PCI(void)
{
	struct class_Sharpen_Arch_PCI* object = calloc(1, sizeof(struct class_Sharpen_Arch_PCI));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Arch_PCI;
	return object;
}

inline void classCctor_Sharpen_Arch_PCI(void)
{
}
inline struct struct_Sharpen_Arch_PCI_PciDriver structInit_Sharpen_Arch_PCI_PciDriver(void)
{
	struct struct_Sharpen_Arch_PCI_PciDriver object;
	return object;
}
inline struct struct_Sharpen_Arch_PCI_PciDevice structInit_Sharpen_Arch_PCI_PciDevice(void)
{
	struct struct_Sharpen_Arch_PCI_PciDevice object;
	return object;
}
uint32_t Sharpen_Arch_PCI_generateAddress_4uint32_t_uint32_t_uint32_t_uint32_t_(uint32_t lbus, uint32_t lslot, uint32_t lfun, uint32_t offset)
{
	return lbus << 16 | lslot << 11 | lfun << 8 |  ( offset & 0xFC )  | 0x80000000;
}
uint16_t Sharpen_Arch_PCI_readWord_4uint16_t_uint16_t_uint16_t_uint16_t_(uint16_t bus, uint16_t slot, uint16_t function, uint16_t offset)
{
	return (uint16_t)Sharpen_Arch_PCI_PCIRead_5uint16_t_uint16_t_uint16_t_uint16_t_uint32_t_(bus, slot, function, offset, 2);
}
uint32_t Sharpen_Arch_PCI_PCIRead_5uint16_t_uint16_t_uint16_t_uint16_t_uint32_t_(uint16_t bus, uint16_t slot, uint16_t function, uint16_t offset, uint32_t size)
{
	uint32_t address = Sharpen_Arch_PCI_generateAddress_4uint32_t_uint32_t_uint32_t_uint32_t_(bus, slot, function, offset);
	Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_(0xCF8, address);
	if(size == 4)
		return Sharpen_Arch_PortIO_In32_1uint16_t_(0xCFC);
	else if(size == 2)
		return  (  ( Sharpen_Arch_PortIO_In32_1uint16_t_(0xCFC) >>  (  ( offset & 2 )  * 8 )  )  & 0xFFFF ) ;
	else if(size == 1)
		return  (  ( Sharpen_Arch_PortIO_In32_1uint16_t_(0xCFC) >>  (  ( offset & 4 )  * 8 )  )  & 0xFF ) ;
	return 0xFFFFFFFF;
}
void Sharpen_Arch_PCI_PCIWrite_5uint16_t_uint16_t_uint16_t_uint16_t_uint32_t_(uint16_t bus, uint16_t slot, uint16_t function, uint16_t offset, uint32_t value)
{
	uint32_t address = Sharpen_Arch_PCI_generateAddress_4uint32_t_uint32_t_uint32_t_uint32_t_(bus, slot, function, offset);
	Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_(0xCF8, address);
	Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_(0xCFC, value);
}
void Sharpen_Arch_PCI_PCIWrite_3struct_struct_Sharpen_Arch_PCI_PciDevice_uint16_t_uint32_t_(struct struct_Sharpen_Arch_PCI_PciDevice dev, uint16_t offset, uint32_t value)
{
	Sharpen_Arch_PCI_PCIWrite_5uint16_t_uint16_t_uint16_t_uint16_t_uint32_t_(dev.field_Bus, dev.field_Slot, dev.field_Function, offset, value);
}
uint16_t Sharpen_Arch_PCI_PCIReadWord_2struct_struct_Sharpen_Arch_PCI_PciDevice_uint16_t_(struct struct_Sharpen_Arch_PCI_PciDevice dev, uint16_t offset)
{
	return Sharpen_Arch_PCI_readWord_4uint16_t_uint16_t_uint16_t_uint16_t_(dev.field_Bus, dev.field_Slot, dev.field_Function, offset);
}
uint16_t Sharpen_Arch_PCI_getDeviceID_3uint16_t_uint16_t_uint16_t_(uint16_t bus, uint16_t device, uint16_t function)
{
	return Sharpen_Arch_PCI_readWord_4uint16_t_uint16_t_uint16_t_uint16_t_(bus, device, function, 0x2);
}
uint16_t Sharpen_Arch_PCI_getHeaderType_3uint16_t_uint16_t_uint16_t_(uint16_t bus, uint16_t device, uint16_t function)
{
	return (uint16_t) ( Sharpen_Arch_PCI_readWord_4uint16_t_uint16_t_uint16_t_uint16_t_(bus, device, function, 0xE) & 0xFF ) ;
}
uint16_t Sharpen_Arch_PCI_GetVendorID_3uint16_t_uint16_t_uint16_t_(uint16_t bus, uint16_t device, uint16_t function)
{
	return Sharpen_Arch_PCI_readWord_4uint16_t_uint16_t_uint16_t_uint16_t_(bus, device, function, 0);
}
uint16_t Sharpen_Arch_PCI_GetClassID_3uint16_t_uint16_t_uint16_t_(uint16_t bus, uint16_t device, uint16_t function)
{
	return (uint8_t) ( Sharpen_Arch_PCI_readWord_4uint16_t_uint16_t_uint16_t_uint16_t_(bus, device, function, 0XA) & 0xFF ) ;
}
uint8_t Sharpen_Arch_PCI_GetSubClassID_3uint16_t_uint16_t_uint16_t_(uint16_t bus, uint16_t device, uint16_t function)
{
	return (uint8_t) (  ( Sharpen_Arch_PCI_readWord_4uint16_t_uint16_t_uint16_t_uint16_t_(bus, device, function, 0XA) >> 8 )  & 0xFF ) ;
}
void Sharpen_Arch_PCI_checkBus_1uint8_t_(uint8_t bus)
{
	for(uint8_t device = 0;device < 32;device = device + 1
	)
	{
		Sharpen_Arch_PCI_checkDevice_2uint8_t_uint8_t_(bus, device);
	}
	;
}
void Sharpen_Arch_PCI_checkDevice_2uint8_t_uint8_t_(uint8_t bus, uint8_t device)
{
	uint16_t vendorID = Sharpen_Arch_PCI_GetVendorID_3uint16_t_uint16_t_uint16_t_(bus, device, 0);
	if(vendorID == 0xFFFF)
		return;
	uint16_t deviceID = Sharpen_Arch_PCI_getDeviceID_3uint16_t_uint16_t_uint16_t_(bus, device, 0);
	if(deviceID == 0xFFFF)
		return;
	struct struct_Sharpen_Arch_PCI_PciDevice dev = structInit_Sharpen_Arch_PCI_PciDevice();
	dev.field_Device = deviceID;
	dev.field_Function = 0;
	dev.field_Bus = bus;
	dev.field_Slot = device;
	dev.field_Vendor = vendorID;
	dev.field_Port1 = (uint16_t) ( Sharpen_Arch_PCI_readWord_4uint16_t_uint16_t_uint16_t_uint16_t_(bus, device, 0, 0x10) &  - 1 << 1 ) ;
	dev.field_Port2 = (uint16_t) ( Sharpen_Arch_PCI_readWord_4uint16_t_uint16_t_uint16_t_uint16_t_(bus, device, 0, 0x14) &  - 1 << 1 ) ;
	classStatics_Sharpen_Arch_PCI.m_devices[classStatics_Sharpen_Arch_PCI.m_currentdevice] = dev;
	classStatics_Sharpen_Arch_PCI.m_currentdevice = classStatics_Sharpen_Arch_PCI.m_currentdevice + 1;
}
void Sharpen_Arch_PCI_RegisterDriver_3uint16_t_uint16_t_struct_struct_Sharpen_Arch_PCI_PciDriver_(uint16_t vendorID, uint16_t deviceID, struct struct_Sharpen_Arch_PCI_PciDriver driver)
{
	int32_t foundIndex =  - 1;
	for(int32_t i = 0;i < classStatics_Sharpen_Arch_PCI.m_currentdevice;i = i + 1
	)
	{
		{
			if(classStatics_Sharpen_Arch_PCI.m_devices[i].field_Vendor == vendorID && classStatics_Sharpen_Arch_PCI.m_devices[i].field_Device == deviceID){
				foundIndex = i;
				break;
			}
		}
	}
	;
	if(foundIndex ==  - 1)
		return;
	Sharpen_Console_Write_1char__("[PCI] Registered driver for ");
	Sharpen_Console_WriteHex_1int64_t_(vendorID);
	Sharpen_Console_Write_1char__(":");
	Sharpen_Console_WriteHex_1int64_t_(deviceID);
	Sharpen_Console_Write_1char__(" Name: ");
	Sharpen_Console_WriteLine_1char__(driver.field_Name);
	classStatics_Sharpen_Arch_PCI.m_devices[foundIndex].field_Driver = driver;
	driver.field_Init(classStatics_Sharpen_Arch_PCI.m_devices[foundIndex]);
}
void Sharpen_Arch_PCI_PrintDevices_0(void)
{
	for(int32_t i = 0;i < classStatics_Sharpen_Arch_PCI.m_currentdevice;i = i + 1
	)
	{
		{
			Sharpen_Console_Write_1char__("Device ");
			Sharpen_Console_WriteHex_1int64_t_(classStatics_Sharpen_Arch_PCI.m_devices[i].field_Vendor);
			Sharpen_Console_Write_1char__(":");
			Sharpen_Console_WriteHex_1int64_t_(classStatics_Sharpen_Arch_PCI.m_devices[i].field_Device);
			Sharpen_Console_WriteLine_1char__("");
		}
	}
	;
}
void Sharpen_Arch_PCI_Probe_0(void)
{
	Sharpen_Arch_PCI_checkBus_1uint8_t_(0);
	Sharpen_Console_Write_1char__("[PCI] ");
	Sharpen_Console_WriteNum_1int32_t_((int32_t)classStatics_Sharpen_Arch_PCI.m_currentdevice - 1);
	Sharpen_Console_WriteLine_1char__(" devices detected");
}
struct class_Sharpen_FileSystem_Node* Sharpen_Arch_PCI_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__(struct class_Sharpen_FileSystem_Node* node, char* name)
{
	return classInit_Sharpen_FileSystem_Node();
}
void Sharpen_Arch_PCI_Init_0(void)
{
	classStatics_Sharpen_Arch_PCI.m_devices = calloc((300), sizeof(struct struct_Sharpen_Arch_PCI_PciDevice));
	Sharpen_Arch_PCI_Probe_0();
}
struct class_Sharpen_Arch_PIC* classInit_Sharpen_Arch_PIC(void)
{
	struct class_Sharpen_Arch_PIC* object = calloc(1, sizeof(struct class_Sharpen_Arch_PIC));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Arch_PIC;
	return object;
}

inline void classCctor_Sharpen_Arch_PIC(void)
{
}
void Sharpen_Arch_PIC_SendEOI_1uint8_t_(uint8_t irq)
{
	if(irq >= 8)
		Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(classStatics_Sharpen_Arch_PIC.SLAVE_PIC_CMD, classStatics_Sharpen_Arch_PIC.PIC_EOI);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(classStatics_Sharpen_Arch_PIC.MASTER_PIC_CMD, classStatics_Sharpen_Arch_PIC.PIC_EOI);
}
void Sharpen_Arch_PIC_Remap_0(void)
{
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(classStatics_Sharpen_Arch_PIC.MASTER_PIC_CMD, classStatics_Sharpen_Arch_PIC.PIC_INIT);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(classStatics_Sharpen_Arch_PIC.SLAVE_PIC_CMD, classStatics_Sharpen_Arch_PIC.PIC_INIT);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(classStatics_Sharpen_Arch_PIC.MASTER_PIC_DATA, classStatics_Sharpen_Arch_IRQ.MASTER_OFFSET);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(classStatics_Sharpen_Arch_PIC.SLAVE_PIC_DATA, classStatics_Sharpen_Arch_IRQ.SLAVE_OFFSET);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(classStatics_Sharpen_Arch_PIC.MASTER_PIC_DATA, 4);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(classStatics_Sharpen_Arch_PIC.SLAVE_PIC_DATA, classStatics_Sharpen_Arch_PIC.PIC_CASCADE);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(classStatics_Sharpen_Arch_PIC.MASTER_PIC_DATA, classStatics_Sharpen_Arch_PIC.PIC_8086);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(classStatics_Sharpen_Arch_PIC.SLAVE_PIC_DATA, classStatics_Sharpen_Arch_PIC.PIC_8086);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(classStatics_Sharpen_Arch_PIC.MASTER_PIC_DATA, 0);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(classStatics_Sharpen_Arch_PIC.SLAVE_PIC_DATA, 0);
}
struct class_Sharpen_Arch_PIT* classInit_Sharpen_Arch_PIT(void)
{
	struct class_Sharpen_Arch_PIT* object = calloc(1, sizeof(struct class_Sharpen_Arch_PIT));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Arch_PIT;
	return object;
}

inline void classCctor_Sharpen_Arch_PIT(void)
{
}
int32_t Sharpen_Arch_PIT_Frequency_getter(void)
{
	return classStatics_Sharpen_Arch_PIT.m_frequency;
}
int32_t Sharpen_Arch_PIT_Frequency_setter(int32_t value)
{
	classStatics_Sharpen_Arch_PIT.m_frequency = value;
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(classStatics_Sharpen_Arch_PIT.PIT_CMD, (uint8_t) ( Sharpen_Arch_PIT_Channel_1int32_t_(0) | Sharpen_Arch_PIT_Access_1int32_t_(3) | Sharpen_Arch_PIT_Operating_1int32_t_(3) | Sharpen_Arch_PIT_Mode_1int32_t_(0) ) );
	uint16_t divisor = (uint16_t) ( classStatics_Sharpen_Arch_PIT.PIT_OSCILLATOR / value ) ;
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(classStatics_Sharpen_Arch_PIT.PIT_DATA, (uint8_t) ( divisor & 0xFF ) );
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(classStatics_Sharpen_Arch_PIT.PIT_DATA, (uint8_t) (  ( divisor >> 8 )  & 0xFF ) );
	return value;
}
int32_t Sharpen_Arch_PIT_SubTicks_getter(void)
{
	return classStatics_Sharpen_Arch_PIT.prop_SubTicks;
}
int32_t Sharpen_Arch_PIT_SubTicks_setter(int32_t value)
{
	classStatics_Sharpen_Arch_PIT.prop_SubTicks = value;
	return value;
}
int32_t Sharpen_Arch_PIT_FullTicks_getter(void)
{
	return classStatics_Sharpen_Arch_PIT.prop_FullTicks;
}
int32_t Sharpen_Arch_PIT_FullTicks_setter(int32_t value)
{
	classStatics_Sharpen_Arch_PIT.prop_FullTicks = value;
	return value;
}
int32_t Sharpen_Arch_PIT_Channel_1int32_t_(int32_t a)
{
	return  ( a << 0x6 ) ;
}
int32_t Sharpen_Arch_PIT_Access_1int32_t_(int32_t a)
{
	return  ( a << 0x4 ) ;
}
int32_t Sharpen_Arch_PIT_Operating_1int32_t_(int32_t a)
{
	return  ( a << 0x1 ) ;
}
int32_t Sharpen_Arch_PIT_Mode_1int32_t_(int32_t a)
{
	return a;
}
void Sharpen_Arch_PIT_Init_0(void)
{
	Sharpen_Arch_PIT_Frequency_setter(200);
	Sharpen_Arch_IRQ_SetHandler_2int32_t_delegate_Sharpen_Arch_IRQ_IRQHandler_(0, Sharpen_Arch_PIT_Handler_1struct_struct_Sharpen_Arch_Regs__);
}
void Sharpen_Arch_PIT_Handler_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr)
{
	Sharpen_Arch_PIT_SubTicks_setter(Sharpen_Arch_PIT_SubTicks_getter() + 1)
	;
	if(Sharpen_Arch_PIT_SubTicks_getter() == classStatics_Sharpen_Arch_PIT.m_frequency){
		Sharpen_Arch_PIT_SubTicks_setter(0);
		Sharpen_Arch_PIT_FullTicks_setter(Sharpen_Arch_PIT_FullTicks_getter() + 1)
		;
		Sharpen_Time_Seconds_setter(Sharpen_Time_Seconds_getter() + 1)
		;
		if(Sharpen_Time_Seconds_getter() == 60){
			Sharpen_Time_Seconds_setter(0);
			Sharpen_Time_Minutes_setter(Sharpen_Time_Minutes_getter() + 1)
			;
			if(Sharpen_Time_Minutes_getter() == 60)
						Sharpen_Arch_CMOS_UpdateTime_0();
		}
	}
}
inline struct struct_Sharpen_Arch_Regs structInit_Sharpen_Arch_Regs(void)
{
	struct struct_Sharpen_Arch_Regs object;
	return object;
}
struct class_Sharpen_Arch_Syscall* classInit_Sharpen_Arch_Syscall(void)
{
	struct class_Sharpen_Arch_Syscall* object = calloc(1, sizeof(struct class_Sharpen_Arch_Syscall));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Arch_Syscall;
	return object;
}

void Sharpen_Arch_Syscall_Handler_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr)
{
	int32_t function = regsPtr->field_EAX;
	if(function > 8)
		return;
	int32_t ret = 0;
	switch(function)
	{
		case (const_Sharpen_Exec_Syscalls_SYS_EXIT):
			ret = Sharpen_Exec_Syscalls_Exit_1int32_t_(regsPtr->field_EBX);
			break;
		case (const_Sharpen_Exec_Syscalls_SYS_GETPID):
			ret = Sharpen_Exec_Syscalls_GetPID_0();
			break;
		case (const_Sharpen_Exec_Syscalls_SYS_SBRK):
			ret = Sharpen_Exec_Syscalls_Sbrk_1int32_t_(regsPtr->field_EBX);
			break;
		case (const_Sharpen_Exec_Syscalls_SYS_FORK):
			ret = Sharpen_Exec_Syscalls_Fork_0();
			break;
		case (const_Sharpen_Exec_Syscalls_SYS_WRITE):
			ret = Sharpen_Exec_Syscalls_Write_3int32_t_uint8_t__uint32_t_(regsPtr->field_EBX, Sharpen_Utilities_Util_BytePtrToByteArray_1uint8_t__((uint8_t*)regsPtr->field_ECX), (uint32_t)regsPtr->field_EDX);
			break;
		case (const_Sharpen_Exec_Syscalls_SYS_READ):
			ret = Sharpen_Exec_Syscalls_Read_3int32_t_uint8_t__uint32_t_(regsPtr->field_EBX, Sharpen_Utilities_Util_BytePtrToByteArray_1uint8_t__((uint8_t*)regsPtr->field_ECX), (uint32_t)regsPtr->field_EDX);
			break;
		case (const_Sharpen_Exec_Syscalls_SYS_OPEN):
			ret = Sharpen_Exec_Syscalls_Open_2char__int32_t_(Sharpen_Utilities_Util_CharPtrToString_1char__((char*)regsPtr->field_EBX), regsPtr->field_ECX);
			break;
		case (const_Sharpen_Exec_Syscalls_SYS_CLOSE):
			ret = Sharpen_Exec_Syscalls_Close_1int32_t_(regsPtr->field_EBX);
			break;
		case (const_Sharpen_Exec_Syscalls_SYS_SEEK):
			ret = Sharpen_Exec_Syscalls_Seek_3int32_t_uint32_t_int32_t_(regsPtr->field_EBX, (uint32_t)regsPtr->field_ECX, (int32_t)regsPtr->field_EDX);
			break;
		default:
			Sharpen_Console_Write_1char__("Unhandled syscall ");
			Sharpen_Console_WriteHex_1int64_t_(function);
			Sharpen_Console_WriteLine_1char__("");
			break;
	}
	regsPtr->field_EAX = ret;
}
struct class_Sharpen_Utilities_ByteUtil* classInit_Sharpen_Utilities_ByteUtil(void)
{
	struct class_Sharpen_Utilities_ByteUtil* object = calloc(1, sizeof(struct class_Sharpen_Utilities_ByteUtil));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Utilities_ByteUtil;
	return object;
}

uint8_t* Sharpen_Utilities_ByteUtil_ToBytes_1int32_t_(int32_t inValue)
{
	uint8_t* result = calloc((8), sizeof(uint8_t));
	for(int32_t i = 3;i >= 0;i = i - 1
	)
	{
		{
			result[i] = (uint8_t) ( inValue & 0xFF ) ;
			inValue >>= 8;
		}
	}
	;
	return result;
}
uint8_t* Sharpen_Utilities_ByteUtil_ToBytes_1int64_t_(int64_t inValue)
{
	uint8_t* result = calloc((8), sizeof(uint8_t));
	for(int32_t i = 7;i >= 0;i = i - 1
	)
	{
		{
			result[i] = (uint8_t) ( inValue & 0xFF ) ;
			inValue >>= 8;
		}
	}
	;
	return result;
}
uint8_t* Sharpen_Utilities_ByteUtil_ToBytes_2int64_t_uint8_t__(int64_t inValue, uint8_t* result)
{
	for(int32_t i = 7;i >= 0;i = i - 1
	)
	{
		{
			result[i] = (uint8_t) ( inValue & 0xFF ) ;
			inValue >>= 8;
		}
	}
	;
	return result;
}
int64_t Sharpen_Utilities_ByteUtil_ToLong_1uint8_t__(uint8_t* b)
{
	int64_t result = 0;
	for(int32_t i = 0;i < 8;i = i + 1
	)
	{
		{
			result <<= 8;
			result |= (uint8_t) ( b[i] & 0xFF ) ;
		}
	}
	;
	return result;
}
int16_t Sharpen_Utilities_ByteUtil_ToShort_2uint8_t__int32_t_(uint8_t* b, int32_t offset)
{
	int16_t result = 0;
	for(int32_t i = 0;i < 2;i = i + 1
	)
	{
		{
			result <<= 8;
			result |= (uint8_t) ( b[offset] & 0xFF ) ;
			offset = offset + 1
			;
		}
	}
	;
	return result;
}
int32_t Sharpen_Utilities_ByteUtil_ToInt_1uint8_t__(uint8_t* b)
{
	int32_t result = 0;
	for(int32_t i = 0;i < 4;i = i + 1
	)
	{
		{
			result <<= 8;
			result |=  ( b[i] & 0xFF ) ;
		}
	}
	;
	return result;
}
struct class_Sharpen_Drivers_Other_VboxDev* classInit_Sharpen_Drivers_Other_VboxDev(void)
{
	struct class_Sharpen_Drivers_Other_VboxDev* object = calloc(1, sizeof(struct class_Sharpen_Drivers_Other_VboxDev));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Drivers_Other_VboxDev;
	return object;
}

inline struct struct_Sharpen_Drivers_Other_VboxDev_RequestHeader structInit_Sharpen_Drivers_Other_VboxDev_RequestHeader(void)
{
	struct struct_Sharpen_Drivers_Other_VboxDev_RequestHeader object;
	return object;
}
inline struct struct_Sharpen_Drivers_Other_VboxDev_RequestSessionID structInit_Sharpen_Drivers_Other_VboxDev_RequestSessionID(void)
{
	struct struct_Sharpen_Drivers_Other_VboxDev_RequestSessionID object;
	return object;
}
inline struct struct_Sharpen_Drivers_Other_VboxDev_RequestGuestInfo structInit_Sharpen_Drivers_Other_VboxDev_RequestGuestInfo(void)
{
	struct struct_Sharpen_Drivers_Other_VboxDev_RequestGuestInfo object;
	return object;
}
inline struct struct_Sharpen_Drivers_Other_VboxDev_RequestPowerState structInit_Sharpen_Drivers_Other_VboxDev_RequestPowerState(void)
{
	struct struct_Sharpen_Drivers_Other_VboxDev_RequestPowerState object;
	return object;
}
inline struct struct_Sharpen_Drivers_Other_VboxDev_RequestHostTime structInit_Sharpen_Drivers_Other_VboxDev_RequestHostTime(void)
{
	struct struct_Sharpen_Drivers_Other_VboxDev_RequestHostTime object;
	return object;
}
void Sharpen_Drivers_Other_VboxDev_getGuestInfo_0(void)
{
	struct struct_Sharpen_Drivers_Other_VboxDev_RequestGuestInfo* req = (struct struct_Sharpen_Drivers_Other_VboxDev_RequestGuestInfo*)Sharpen_Heap_Alloc_1int32_t_(sizeof(struct struct_Sharpen_Drivers_Other_VboxDev_RequestGuestInfo));
	req->field_header.field_Size = (uint32_t)sizeof(struct struct_Sharpen_Drivers_Other_VboxDev_RequestGuestInfo);
	req->field_header.field_Version = 0x10001;
	req->field_header.field_requestType = enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_ReportGuestInfo;
	req->field_header.field_rc = 0xFFFFF;
	req->field_interfaceVersion = 0x10000;
	req->field_osType = 0x10000;
	Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_(classStatics_Sharpen_Drivers_Other_VboxDev.m_dev.field_Port1, (uint32_t)Sharpen_Arch_Paging_GetPhysicalFromVirtual_1void__(req));
	if(req->field_header.field_rc == 0){
		Sharpen_Console_WriteLine_1char__("[VMMDev] Initalized");
		classStatics_Sharpen_Drivers_Other_VboxDev.m_initalized = true;
	}
	else
	{
		Sharpen_Console_WriteLine_1char__("[VMMDev] Initalization failed");
	}
}
void Sharpen_Drivers_Other_VboxDev_initHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_(struct struct_Sharpen_Arch_PCI_PciDevice dev)
{
	classStatics_Sharpen_Drivers_Other_VboxDev.m_dev = dev;
	Sharpen_Drivers_Other_VboxDev_getGuestInfo_0();
	if(
		classStatics_Sharpen_Drivers_Other_VboxDev.m_initalized)
		Sharpen_Drivers_Other_VboxDevFSDriver_Init_0();
}
void Sharpen_Drivers_Other_VboxDev_exitHander_1struct_struct_Sharpen_Arch_PCI_PciDevice_(struct struct_Sharpen_Arch_PCI_PciDevice dev)
{
}
void Sharpen_Drivers_Other_VboxDev_Init_0(void)
{
	struct struct_Sharpen_Arch_PCI_PciDriver driver = structInit_Sharpen_Arch_PCI_PciDriver();
	driver.field_Name = "VboxDev driver";
	driver.field_Exit = Sharpen_Drivers_Other_VboxDev_exitHander_1struct_struct_Sharpen_Arch_PCI_PciDevice_;
	driver.field_Init = Sharpen_Drivers_Other_VboxDev_initHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_;
	Sharpen_Arch_PCI_RegisterDriver_3uint16_t_uint16_t_struct_struct_Sharpen_Arch_PCI_PciDriver_(0x80EE, 0xCAFE, driver);
}
void Sharpen_Drivers_Other_VboxDev_ChangePowerState_1int32_t_(int32_t state)
{
	struct struct_Sharpen_Drivers_Other_VboxDev_RequestPowerState* req = (struct struct_Sharpen_Drivers_Other_VboxDev_RequestPowerState*)Sharpen_Heap_Alloc_1int32_t_(sizeof(struct struct_Sharpen_Drivers_Other_VboxDev_RequestPowerState));
	req->field_header.field_Size = (uint32_t)sizeof(struct struct_Sharpen_Drivers_Other_VboxDev_RequestPowerState);
	req->field_header.field_Version = 0x10001;
	req->field_header.field_requestType = enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_SetPowerStatus;
	req->field_header.field_rc = 0xFFFFF;
	req->field_PowerState = state;
	Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_(classStatics_Sharpen_Drivers_Other_VboxDev.m_dev.field_Port1, (uint32_t)Sharpen_Arch_Paging_GetPhysicalFromVirtual_1void__(req));
}
uint64_t Sharpen_Drivers_Other_VboxDev_GetSessionID_0(void)
{
	struct struct_Sharpen_Drivers_Other_VboxDev_RequestSessionID* req = (struct struct_Sharpen_Drivers_Other_VboxDev_RequestSessionID*)Sharpen_Heap_Alloc_1int32_t_(sizeof(struct struct_Sharpen_Drivers_Other_VboxDev_RequestSessionID));
	req->field_header.field_Size = (uint32_t)sizeof(struct struct_Sharpen_Drivers_Other_VboxDev_RequestSessionID);
	req->field_header.field_Version = 0x10001;
	req->field_header.field_requestType = enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetSessionId;
	req->field_header.field_rc = 0xFFFFF;
	Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_(classStatics_Sharpen_Drivers_Other_VboxDev.m_dev.field_Port1, (uint32_t)Sharpen_Arch_Paging_GetPhysicalFromVirtual_1void__(req));
	return req->field_idSession;
}
uint64_t Sharpen_Drivers_Other_VboxDev_GetHostTime_0(void)
{
	struct struct_Sharpen_Drivers_Other_VboxDev_RequestHostTime* req = (struct struct_Sharpen_Drivers_Other_VboxDev_RequestHostTime*)Sharpen_Heap_Alloc_1int32_t_(sizeof(struct struct_Sharpen_Drivers_Other_VboxDev_RequestHostTime));
	req->field_header.field_Size = (uint32_t)sizeof(struct struct_Sharpen_Drivers_Other_VboxDev_RequestHostTime);
	req->field_header.field_Version = 0x10001;
	req->field_header.field_requestType = enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetHostTime;
	req->field_header.field_rc = 0xFFFFF;
	Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_(classStatics_Sharpen_Drivers_Other_VboxDev.m_dev.field_Port1, (uint32_t)Sharpen_Arch_Paging_GetPhysicalFromVirtual_1void__(req));
	return req->field_Time;
}
struct class_Sharpen_Drivers_Other_VboxDevFSDriver* classInit_Sharpen_Drivers_Other_VboxDevFSDriver(void)
{
	struct class_Sharpen_Drivers_Other_VboxDevFSDriver* object = calloc(1, sizeof(struct class_Sharpen_Drivers_Other_VboxDevFSDriver));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Drivers_Other_VboxDevFSDriver;
	return object;
}

inline void classCctor_Sharpen_Drivers_Other_VboxDevFSDriver(void)
{
}
void Sharpen_Drivers_Other_VboxDevFSDriver_Init_0(void)
{
	struct class_Sharpen_FileSystem_Device* device = classInit_Sharpen_FileSystem_Device();
	device->field_Name = "VMMDEV";
	device->field_node = classInit_Sharpen_FileSystem_Node();
	device->field_node->field_Flags = enum_Sharpen_FileSystem_NodeFlags_DIRECTORY | enum_Sharpen_FileSystem_NodeFlags_DEVICE;
	device->field_node->field_FindDir = Sharpen_Drivers_Other_VboxDevFSDriver_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__;
	device->field_node->field_ReadDir = Sharpen_Drivers_Other_VboxDevFSDriver_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_;
	Sharpen_FileSystem_DevFS_RegisterDevice_1struct_class_Sharpen_FileSystem_Device__(device);
	Sharpen_Console_WriteLine_1char__("[VMMDev] FsDevice registered under VMMDEV");
}
struct struct_Sharpen_FileSystem_DirEntry* Sharpen_Drivers_Other_VboxDevFSDriver_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_(struct class_Sharpen_FileSystem_Node* node, uint32_t index)
{
	if(index >= classStatics_Sharpen_Drivers_Other_VboxDevFSDriver.m_numCommands)
		return null;
	struct struct_Sharpen_FileSystem_DirEntry* entry = (struct struct_Sharpen_FileSystem_DirEntry*)Sharpen_Heap_Alloc_1int32_t_(sizeof(struct struct_Sharpen_FileSystem_DirEntry));
	int32_t i = 0;
	for(;classStatics_Sharpen_Drivers_Other_VboxDevFSDriver.m_commands[index][i] != '\0';i = i + 1
	)
	{
		entry->field_Name[i] = classStatics_Sharpen_Drivers_Other_VboxDevFSDriver.m_commands[index][i];
	}
	;
	entry->field_Name[i] = '\0';
	return entry;
}
struct class_Sharpen_FileSystem_Node* Sharpen_Drivers_Other_VboxDevFSDriver_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__(struct class_Sharpen_FileSystem_Node* node, char* name)
{
	uint32_t functionID = 0;
	if(Sharpen_Utilities_String_Equals_2char__char__(name, "sessionid")){
		functionID = (uint32_t)enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetSessionId;
	}
	else if(Sharpen_Utilities_String_Equals_2char__char__(name, "powerstate")){
		functionID = (uint32_t)enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_SetPowerStatus;
	}
	else if(Sharpen_Utilities_String_Equals_2char__char__(name, "hosttime")){
		functionID = (uint32_t)enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetHostTime;
	}
	if(functionID == 0)
		return null;
	struct class_Sharpen_FileSystem_Node* outNode = classInit_Sharpen_FileSystem_Node();
	outNode->field_Cookie = functionID;
	outNode->field_Read = Sharpen_Drivers_Other_VboxDevFSDriver_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__;
	outNode->field_Write = Sharpen_Drivers_Other_VboxDevFSDriver_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__;
	outNode->field_Flags = enum_Sharpen_FileSystem_NodeFlags_FILE;
	return outNode;
}
uint32_t Sharpen_Drivers_Other_VboxDevFSDriver_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer)
{
	int32_t function = (int32_t)node->field_Cookie;
	switch(function)
	{
		case (enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_SetPowerStatus):
			if(size < 4)
						return 0;
			int32_t state = Sharpen_Utilities_ByteUtil_ToInt_1uint8_t__(buffer);
			int32_t stateConverted = (int32_t)state;
			Sharpen_Drivers_Other_VboxDev_ChangePowerState_1int32_t_(stateConverted);
			return 4;
	}
	return 0;
}
uint32_t Sharpen_Drivers_Other_VboxDevFSDriver_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer)
{
	int32_t function = (int32_t)node->field_Cookie;
	switch(function)
	{
		case (enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetSessionId):
			if(size != 8)
						return 0;
			uint64_t sessionID = Sharpen_Drivers_Other_VboxDev_GetSessionID_0();
			Sharpen_Utilities_ByteUtil_ToBytes_2int64_t_uint8_t__((int64_t)sessionID, buffer);
			return 8;
		case (enum_Sharpen_Drivers_Other_VboxDevRequestTypes_VMMDevReq_GetHostTime):
			if(size != 8)
						return 0;
			uint64_t time = Sharpen_Drivers_Other_VboxDev_GetHostTime_0();
			Sharpen_Utilities_ByteUtil_ToBytes_2int64_t_uint8_t__((int64_t)time, buffer);
			return 8;
	}
	return 0;
}
struct class_Sharpen_Drivers_Power_Acpi* classInit_Sharpen_Drivers_Power_Acpi(void)
{
	struct class_Sharpen_Drivers_Power_Acpi* object = calloc(1, sizeof(struct class_Sharpen_Drivers_Power_Acpi));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Drivers_Power_Acpi;
	return object;
}

inline void classCctor_Sharpen_Drivers_Power_Acpi(void)
{
	classStatics_Sharpen_Drivers_Power_Acpi.SLP_EN = 1 << 13;
}
void Sharpen_Drivers_Power_Acpi_Find_0(void)
{
	uint8_t* biosp = (uint8_t*)0x000E0000;
	while((uint32_t)biosp < 0x000FFFFF)
	{
		{
			if(Sharpen_Memory_Compare_3char__char__int32_t_((char*)biosp, (char*)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__("RSD PTR "), 8) && Sharpen_Drivers_Power_Acpi_CheckSum_2void__uint32_t_((uint32_t*)biosp, (uint32_t)sizeof(struct struct_Sharpen_Drivers_Power_RSDT))){
				classStatics_Sharpen_Drivers_Power_Acpi.rdsp = (struct struct_Sharpen_Drivers_Power_RDSP*)biosp;
				break;
			}
			biosp += 16;
		}
	}
	;
	if(classStatics_Sharpen_Drivers_Power_Acpi.rdsp == null){
		uint16_t* adr = (uint16_t*)0x040E;
		uint8_t* ebdap = (uint8_t*) (  ( *adr )  << 4 ) ;
		while((int32_t)ebdap < 0x000A0000)
		{
			{
				if(Sharpen_Memory_Compare_3char__char__int32_t_((char*)ebdap, (char*)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__("RSD PTR "), 8) && Sharpen_Drivers_Power_Acpi_CheckSum_2void__uint32_t_((uint32_t*)ebdap, (uint32_t)sizeof(struct struct_Sharpen_Drivers_Power_RSDT))){
					classStatics_Sharpen_Drivers_Power_Acpi.rdsp = (struct struct_Sharpen_Drivers_Power_RDSP*)biosp;
					break;
				}
				ebdap += 16;
			}
		}
		;
		if(classStatics_Sharpen_Drivers_Power_Acpi.rdsp == null)
				Sharpen_Panic_DoPanic_1char__("RDSP not found!");
	}
	classStatics_Sharpen_Drivers_Power_Acpi.rsdt = (struct struct_Sharpen_Drivers_Power_RSDT*)classStatics_Sharpen_Drivers_Power_Acpi.rdsp->field_RsdtAddress;
	if(classStatics_Sharpen_Drivers_Power_Acpi.rsdt == null)
		Sharpen_Panic_DoPanic_1char__("RDST not found!");
	classStatics_Sharpen_Drivers_Power_Acpi.fadt = (struct struct_Sharpen_Drivers_Power_FADT*)Sharpen_Drivers_Power_Acpi_getEntry_1char__("FACP");
	if(classStatics_Sharpen_Drivers_Power_Acpi.fadt == null)
		Sharpen_Panic_DoPanic_1char__("FACP not found!");
}
int32_t Sharpen_Drivers_Power_Acpi_CheckSum_2void__uint32_t_(void* address, uint32_t length)
{
	char* bptr = (char*)address;
	uint8_t check = 0;
	for(int32_t i = 0;i < length;i = i + 1
	)
	{
		{
			check += (uint8_t)*bptr;
			bptr = bptr + 1
			;
		}
	}
	;
	if(check == 0)
		return true;
	return false;
}
void Sharpen_Drivers_Power_Acpi_parseS5Object_0(void)
{
	int32_t s5Found = false;
	uint32_t dsdtLength =  ( classStatics_Sharpen_Drivers_Power_Acpi.fadt->field_Dsdt + 1 )  - 36;
	uint8_t* s5Address = (uint8_t*)classStatics_Sharpen_Drivers_Power_Acpi.fadt->field_Dsdt + 36;
	if(dsdtLength > 0){
		while(dsdtLength > 0)
		{
			{
				if(Sharpen_Memory_Compare_3char__char__int32_t_((char*)s5Address, (char*)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__("_S5_"), 4)){
					s5Found = true;
					break;
				}
				s5Address = s5Address + 1
				;
			}
		}
		;
		if( ! s5Found)
				Sharpen_Panic_DoPanic_1char__("Could not find S5 object!");
		if( ( *(s5Address - 1) == 0x08 ||  ( *(s5Address - 2) == 0x08 && *(s5Address - 1) == '\\' )  )  && *(s5Address + 4) == 0x12){
			s5Address += 5;
			s5Address +=  (  ( *s5Address & 0xC0 )  >> 6 )  + 2;
			if(*s5Address == 0x0A)
						s5Address = s5Address + 1
			;
			classStatics_Sharpen_Drivers_Power_Acpi.SLP_TYPa = (uint16_t) ( *(s5Address) << 10 ) ;
			s5Address = s5Address + 1
			;
			if(*s5Address == 0x0A)
						s5Address = s5Address + 1
			;
			classStatics_Sharpen_Drivers_Power_Acpi.SLP_TYPb = (uint16_t) ( *(s5Address) << 10 ) ;
		}
		else
		{
			Sharpen_Panic_DoPanic_1char__("S5 object has not the right structure");
		}
	}
}
void* Sharpen_Drivers_Power_Acpi_getEntry_1char__(char* signature)
{
	if(classStatics_Sharpen_Drivers_Power_Acpi.rsdt == null)
		return null;
	uint32_t n = (uint32_t) ( classStatics_Sharpen_Drivers_Power_Acpi.rsdt->field_header.field_Length - sizeof(struct struct_Sharpen_Drivers_Power_RDSTH) )  / 4;
	for(uint32_t i = 0;i < n;i = i + 1
	)
	{
		{
			struct struct_Sharpen_Drivers_Power_RDSTH* header = (struct struct_Sharpen_Drivers_Power_RDSTH*) ( classStatics_Sharpen_Drivers_Power_Acpi.rsdt->field_firstSDT + i ) ;
			if(Sharpen_Memory_Compare_3char__char__int32_t_((char*)header, (char*)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(signature), 4))
						if(Sharpen_Drivers_Power_Acpi_CheckSum_2void__uint32_t_(header, header->field_Length))
						return (void*)header;
		}
	}
	;
	return null;
}
void Sharpen_Drivers_Power_Acpi_Init_0(void)
{
	Sharpen_Drivers_Power_Acpi_Find_0();
	Sharpen_Drivers_Power_Acpi_parseS5Object_0();
	Sharpen_Drivers_Power_Acpi_Enable_0();
}
void Sharpen_Drivers_Power_Acpi_Enable_0(void)
{
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t)classStatics_Sharpen_Drivers_Power_Acpi.fadt->field_SMI_CommandPort, classStatics_Sharpen_Drivers_Power_Acpi.fadt->field_AcpiEnable);
}
void Sharpen_Drivers_Power_Acpi_Disable_0(void)
{
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t)classStatics_Sharpen_Drivers_Power_Acpi.fadt->field_SMI_CommandPort, classStatics_Sharpen_Drivers_Power_Acpi.fadt->field_AcpiDisable);
}
void Sharpen_Drivers_Power_Acpi_Reset_0(void)
{
	uint8_t good = 0x02;
	while( ( good & 0x02 )  > 0)
	{
		good = Sharpen_Arch_PortIO_In8_1uint16_t_(0x64);
	}
	;
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(0x64, 0xFE);
}
void Sharpen_Drivers_Power_Acpi_Shutdown_0(void)
{
	Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_((uint16_t)classStatics_Sharpen_Drivers_Power_Acpi.fadt->field_PM1aControlBlock, (uint16_t) ( classStatics_Sharpen_Drivers_Power_Acpi.SLP_TYPa | classStatics_Sharpen_Drivers_Power_Acpi.SLP_EN ) );
	Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_((uint16_t)classStatics_Sharpen_Drivers_Power_Acpi.fadt->field_PM1aControlBlock, (uint16_t) ( classStatics_Sharpen_Drivers_Power_Acpi.SLP_TYPb | classStatics_Sharpen_Drivers_Power_Acpi.SLP_EN ) );
}
inline struct struct_Sharpen_Drivers_Power_RDSTH structInit_Sharpen_Drivers_Power_RDSTH(void)
{
	struct struct_Sharpen_Drivers_Power_RDSTH object;
	return object;
}
inline struct struct_Sharpen_Drivers_Power_RDSP structInit_Sharpen_Drivers_Power_RDSP(void)
{
	struct struct_Sharpen_Drivers_Power_RDSP object;
	return object;
}
inline struct struct_Sharpen_Drivers_Power_GenericAddressStructure structInit_Sharpen_Drivers_Power_GenericAddressStructure(void)
{
	struct struct_Sharpen_Drivers_Power_GenericAddressStructure object;
	return object;
}
inline struct struct_Sharpen_Drivers_Power_RSDT structInit_Sharpen_Drivers_Power_RSDT(void)
{
	struct struct_Sharpen_Drivers_Power_RSDT object;
	return object;
}
inline struct struct_Sharpen_Drivers_Power_FADT structInit_Sharpen_Drivers_Power_FADT(void)
{
	struct struct_Sharpen_Drivers_Power_FADT object;
	return object;
}
struct class_Sharpen_Drivers_Sound_IntelHD* classInit_Sharpen_Drivers_Sound_IntelHD(void)
{
	struct class_Sharpen_Drivers_Sound_IntelHD* object = calloc(1, sizeof(struct class_Sharpen_Drivers_Sound_IntelHD));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Drivers_Sound_IntelHD;
	return object;
}

void Sharpen_Drivers_Sound_IntelHD_initHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_(struct struct_Sharpen_Arch_PCI_PciDevice dev)
{
}
void Sharpen_Drivers_Sound_IntelHD_exitHander_1struct_struct_Sharpen_Arch_PCI_PciDevice_(struct struct_Sharpen_Arch_PCI_PciDevice dev)
{
}
void Sharpen_Drivers_Sound_IntelHD_Init_0(void)
{
	struct struct_Sharpen_Arch_PCI_PciDriver driver = structInit_Sharpen_Arch_PCI_PciDriver();
	driver.field_Name = "Intel HD Driver";
	driver.field_Exit = Sharpen_Drivers_Sound_IntelHD_exitHander_1struct_struct_Sharpen_Arch_PCI_PciDevice_;
	driver.field_Init = Sharpen_Drivers_Sound_IntelHD_initHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_;
	Sharpen_Arch_PCI_RegisterDriver_3uint16_t_uint16_t_struct_struct_Sharpen_Arch_PCI_PciDriver_(0x8086, 0x2668, driver);
}
struct class_Sharpen_Drivers_Sound_AC97* classInit_Sharpen_Drivers_Sound_AC97(void)
{
	struct class_Sharpen_Drivers_Sound_AC97* object = calloc(1, sizeof(struct class_Sharpen_Drivers_Sound_AC97));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Drivers_Sound_AC97;
	return object;
}

inline void classCctor_Sharpen_Drivers_Sound_AC97(void)
{
	classStatics_Sharpen_Drivers_Sound_AC97.CR_RPBM =  ( 1 << 0 ) ;
	classStatics_Sharpen_Drivers_Sound_AC97.CR_PR =  ( 1 << 1 ) ;
	classStatics_Sharpen_Drivers_Sound_AC97.CR_LVBIE =  ( 1 << 2 ) ;
	classStatics_Sharpen_Drivers_Sound_AC97.CR_FEIE =  ( 1 << 3 ) ;
	classStatics_Sharpen_Drivers_Sound_AC97.CR_IOCE =  ( 1 << 4 ) ;
	classStatics_Sharpen_Drivers_Sound_AC97.CL_BUP =  ( 1 << 30 ) ;
	classStatics_Sharpen_Drivers_Sound_AC97.CL_IOC =  ( 1 << 31 ) ;
	classStatics_Sharpen_Drivers_Sound_AC97.SR_DCH =  ( 1 << 0 ) ;
	classStatics_Sharpen_Drivers_Sound_AC97.SR_CELV =  ( 1 << 1 ) ;
	classStatics_Sharpen_Drivers_Sound_AC97.SR_LVBCI =  ( 1 << 2 ) ;
	classStatics_Sharpen_Drivers_Sound_AC97.SR_BCIS =  ( 1 << 3 ) ;
	classStatics_Sharpen_Drivers_Sound_AC97.SR_FIFOE =  ( 1 << 4 ) ;
	classStatics_Sharpen_Drivers_Sound_AC97.m_bdls = calloc((32), sizeof(struct struct_Sharpen_Drivers_Sound_AC97_BDL_Entry));
}
inline struct struct_Sharpen_Drivers_Sound_AC97_BDL_Entry structInit_Sharpen_Drivers_Sound_AC97_BDL_Entry(void)
{
	struct struct_Sharpen_Drivers_Sound_AC97_BDL_Entry object;
	return object;
}
void Sharpen_Drivers_Sound_AC97_InitHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_(struct struct_Sharpen_Arch_PCI_PciDevice dev)
{
	classStatics_Sharpen_Drivers_Sound_AC97.m_dev = dev;
	classStatics_Sharpen_Drivers_Sound_AC97.m_nambar = dev.field_Port1;
	classStatics_Sharpen_Drivers_Sound_AC97.m_nabmbar = dev.field_Port2;
	uint32_t irqNum = Sharpen_Arch_PCI_PCIRead_5uint16_t_uint16_t_uint16_t_uint16_t_uint32_t_(dev.field_Bus, dev.field_Slot, dev.field_Function, 0x3C, 1);
	Sharpen_Arch_IRQ_SetHandler_2int32_t_delegate_Sharpen_Arch_IRQ_IRQHandler_((int32_t)irqNum, Sharpen_Drivers_Sound_AC97_IRQHandler_1struct_struct_Sharpen_Arch_Regs__);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( classStatics_Sharpen_Drivers_Sound_AC97.m_nabmbar + classStatics_Sharpen_Drivers_Sound_AC97.CR ) , (uint8_t) ( classStatics_Sharpen_Drivers_Sound_AC97.CR_FEIE | classStatics_Sharpen_Drivers_Sound_AC97.CR_IOCE | classStatics_Sharpen_Drivers_Sound_AC97.CR_LVBIE ) );
	Sharpen_Arch_PCI_PCIWrite_5uint16_t_uint16_t_uint16_t_uint16_t_uint32_t_(dev.field_Bus, dev.field_Slot, dev.field_Function, classStatics_Sharpen_Arch_PCI.COMMAND, 0x05);
	uint16_t volume = 0x03 |  ( 0x03 << 8 ) ;
	Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_((uint16_t) ( classStatics_Sharpen_Drivers_Sound_AC97.m_nambar + classStatics_Sharpen_Drivers_Sound_AC97.MASTER_VOLUME ) , volume);
	Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_((uint16_t) ( classStatics_Sharpen_Drivers_Sound_AC97.m_nambar + classStatics_Sharpen_Drivers_Sound_AC97.PCM_OUT_VOLUME ) , volume);
	classStatics_Sharpen_Drivers_Sound_AC97.m_bufs = calloc((32), sizeof(uint16_t*));
	for(int32_t i = 0;i < 32;i = i + 1
	)
	{
		{
			classStatics_Sharpen_Drivers_Sound_AC97.m_bufs[i] = calloc((0x1000), sizeof(uint16_t));
			{
				void* ptr = classStatics_Sharpen_Drivers_Sound_AC97.m_bufs[i];
				{
					classStatics_Sharpen_Drivers_Sound_AC97.m_bdls[i].field_pointer = Sharpen_Arch_Paging_GetPhysicalFromVirtual_1void__(ptr);
				}
			}
			classStatics_Sharpen_Drivers_Sound_AC97.m_bdls[i].field_cl = 0x1000 & 0xFFFF;
			classStatics_Sharpen_Drivers_Sound_AC97.m_bdls[i].field_cl |= classStatics_Sharpen_Drivers_Sound_AC97.CL_IOC;
		}
	}
	;
	{
		void* ptr = classStatics_Sharpen_Drivers_Sound_AC97.m_bdls;
		{
			Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_((uint16_t) ( classStatics_Sharpen_Drivers_Sound_AC97.m_nabmbar + classStatics_Sharpen_Drivers_Sound_AC97.BDBAR ) , (uint32_t)Sharpen_Arch_Paging_GetPhysicalFromVirtual_1void__(ptr));
		}
	}
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( classStatics_Sharpen_Drivers_Sound_AC97.m_nabmbar + classStatics_Sharpen_Drivers_Sound_AC97.LVI ) , 2);
	classStatics_Sharpen_Drivers_Sound_AC97.m_lvi = 2;
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( classStatics_Sharpen_Drivers_Sound_AC97.m_nabmbar + classStatics_Sharpen_Drivers_Sound_AC97.CR ) , (uint8_t) ( Sharpen_Arch_PortIO_In8_1uint16_t_((uint16_t) ( classStatics_Sharpen_Drivers_Sound_AC97.m_nabmbar + classStatics_Sharpen_Drivers_Sound_AC97.CR ) ) | classStatics_Sharpen_Drivers_Sound_AC97.CR_RPBM ) );
	Sharpen_Console_WriteLine_1char__("[AC97] Initalized");
}
void Sharpen_Drivers_Sound_AC97_IRQHandler_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr)
{
	uint16_t sr = Sharpen_Arch_PortIO_In16_1uint16_t_((uint16_t) ( classStatics_Sharpen_Drivers_Sound_AC97.m_nabmbar + classStatics_Sharpen_Drivers_Sound_AC97.m_sr ) );
	if( ( sr & classStatics_Sharpen_Drivers_Sound_AC97.SR_LVBCI )  > 0){
		Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_((uint16_t) ( classStatics_Sharpen_Drivers_Sound_AC97.m_nabmbar + classStatics_Sharpen_Drivers_Sound_AC97.m_sr ) , classStatics_Sharpen_Drivers_Sound_AC97.SR_LVBCI);
	}
	else if( ( sr & classStatics_Sharpen_Drivers_Sound_AC97.SR_BCIS )  > 0){
		int32_t tmp = classStatics_Sharpen_Drivers_Sound_AC97.m_lvi + 2;
		uint32_t start = (uint32_t) ( tmp &  ( 32 - 1 )  ) ;
		for(int32_t i = 0;i < 0x1000 * 4;i += 128)
		{
			{
				uint16_t* shr;
				{
					void* ptr = classStatics_Sharpen_Drivers_Sound_AC97.m_bufs[start];
					{
						shr = (uint16_t*) ( (int32_t)ptr + i ) ;
					}
				}
				Sharpen_Lib_Audio_RequestBuffer_2uint32_t_uint16_t__(128, shr);
			}
		}
		;
		tmp = classStatics_Sharpen_Drivers_Sound_AC97.m_lvi + 1;
		classStatics_Sharpen_Drivers_Sound_AC97.m_lvi = (uint16_t) ( (tmp) % 32 ) ;
		Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( classStatics_Sharpen_Drivers_Sound_AC97.m_nabmbar + classStatics_Sharpen_Drivers_Sound_AC97.LVI ) , (uint8_t)classStatics_Sharpen_Drivers_Sound_AC97.m_lvi);
		Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_((uint16_t) ( classStatics_Sharpen_Drivers_Sound_AC97.m_nabmbar + classStatics_Sharpen_Drivers_Sound_AC97.m_sr ) , classStatics_Sharpen_Drivers_Sound_AC97.SR_BCIS);
	}
	else if( ( sr & classStatics_Sharpen_Drivers_Sound_AC97.SR_FIFOE )  > 0){
		Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_((uint16_t) ( classStatics_Sharpen_Drivers_Sound_AC97.m_nabmbar + classStatics_Sharpen_Drivers_Sound_AC97.m_sr ) , classStatics_Sharpen_Drivers_Sound_AC97.SR_FIFOE);
	}
}
void Sharpen_Drivers_Sound_AC97_ExitHander_1struct_struct_Sharpen_Arch_PCI_PciDevice_(struct struct_Sharpen_Arch_PCI_PciDevice dev)
{
}
void Sharpen_Drivers_Sound_AC97_Init_0(void)
{
	struct struct_Sharpen_Arch_PCI_PciDriver driver = structInit_Sharpen_Arch_PCI_PciDriver();
	driver.field_Name = "AC97 Driver";
	driver.field_Exit = Sharpen_Drivers_Sound_AC97_ExitHander_1struct_struct_Sharpen_Arch_PCI_PciDevice_;
	driver.field_Init = Sharpen_Drivers_Sound_AC97_InitHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_;
	Sharpen_Arch_PCI_RegisterDriver_3uint16_t_uint16_t_struct_struct_Sharpen_Arch_PCI_PciDriver_(0x8086, 0x2415, driver);
	struct struct_Sharpen_Lib_Audio_SoundDevice device = structInit_Sharpen_Lib_Audio_SoundDevice();
	device.field_Name = "AC97 audio device";
	device.field_Writer = Sharpen_Drivers_Sound_AC97_Writer_2int32_t_uint32_t_;
	device.field_Reader = Sharpen_Drivers_Sound_AC97_Reader_1int32_t_;
	Sharpen_Lib_Audio_SetDevice_1struct_struct_Sharpen_Lib_Audio_SoundDevice_(device);
}
uint32_t Sharpen_Drivers_Sound_AC97_Reader_1int32_t_(int32_t action)
{
	return 0;
}
void Sharpen_Drivers_Sound_AC97_Writer_2int32_t_uint32_t_(int32_t action, uint32_t value)
{
	if(action == enum_Sharpen_Lib_AudioActions_Master){
		value =  ~ value;
		value >>= 26;
		uint16_t encoded = (uint16_t) ( value |  ( value << 8 )  ) ;
		Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_((uint16_t) ( classStatics_Sharpen_Drivers_Sound_AC97.m_nambar + classStatics_Sharpen_Drivers_Sound_AC97.MASTER_VOLUME ) , encoded);
	}
	else if(action == enum_Sharpen_Lib_AudioActions_PCM_OUT){
		value =  ~ value;
		value >>= 27;
		uint16_t encoded = (uint16_t) ( value |  ( value << 8 )  ) ;
		Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_((uint16_t) ( classStatics_Sharpen_Drivers_Sound_AC97.m_nambar + classStatics_Sharpen_Drivers_Sound_AC97.PCM_OUT_VOLUME ) , encoded);
	}
}
struct class_Sharpen_FileSystem_DevFS* classInit_Sharpen_FileSystem_DevFS(void)
{
	struct class_Sharpen_FileSystem_DevFS* object = calloc(1, sizeof(struct class_Sharpen_FileSystem_DevFS));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_FileSystem_DevFS;
	return object;
}

inline void classCctor_Sharpen_FileSystem_DevFS(void)
{
	classStatics_Sharpen_FileSystem_DevFS.m_devices = classInit_Sharpen_Collections_Dictionary();
}
int32_t Sharpen_FileSystem_DevFS_Count_getter(void)
{
	return Sharpen_Collections_Dictionary_Count_1class_(classStatics_Sharpen_FileSystem_DevFS.m_devices);
}
void Sharpen_FileSystem_DevFS_Init_0(void)
{
	struct class_Sharpen_FileSystem_MountPoint* mp = classInit_Sharpen_FileSystem_MountPoint();
	mp->field_Name = "devices";
	classStatics_Sharpen_FileSystem_DevFS.m_currentNode = classInit_Sharpen_FileSystem_Node();
	classStatics_Sharpen_FileSystem_DevFS.m_currentNode->field_FindDir = Sharpen_FileSystem_DevFS_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__;
	classStatics_Sharpen_FileSystem_DevFS.m_currentNode->field_ReadDir = Sharpen_FileSystem_DevFS_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_;
	classStatics_Sharpen_FileSystem_DevFS.m_currentNode->field_Flags = enum_Sharpen_FileSystem_NodeFlags_DIRECTORY;
	mp->field_Node = classStatics_Sharpen_FileSystem_DevFS.m_currentNode;
	Sharpen_FileSystem_VFS_AddMountPoint_1struct_class_Sharpen_FileSystem_MountPoint__(mp);
}
int64_t Sharpen_FileSystem_DevFS_GenerateHash_1char__(char* inVal)
{
	int64_t hash = 0;
	for(int32_t i = 0;i <= 8;i = i + 1
	)
	{
		{
			char c = inVal[i];
			if(c == '\0')
						break;
			hash <<= 3;
			hash |= c;
		}
	}
	;
	return hash;
}
void Sharpen_FileSystem_DevFS_RegisterDevice_1struct_class_Sharpen_FileSystem_Device__(struct class_Sharpen_FileSystem_Device* dev)
{
	Sharpen_Collections_Dictionary_Add_3class_int64_t_void__(classStatics_Sharpen_FileSystem_DevFS.m_devices, Sharpen_FileSystem_DevFS_GenerateHash_1char__(dev->field_Name), dev);
}
struct class_Sharpen_FileSystem_Node* Sharpen_FileSystem_DevFS_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__(struct class_Sharpen_FileSystem_Node* node, char* name)
{
	int64_t hash = Sharpen_FileSystem_DevFS_GenerateHash_1char__(name);
	struct class_Sharpen_FileSystem_Device* dev = (struct class_Sharpen_FileSystem_Device*)Sharpen_Collections_Dictionary_GetByKey_2class_int64_t_(classStatics_Sharpen_FileSystem_DevFS.m_devices, hash);
	if(dev == null)
		return null;
	return dev->field_node;
}
struct struct_Sharpen_FileSystem_DirEntry* Sharpen_FileSystem_DevFS_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_(struct class_Sharpen_FileSystem_Node* node, uint32_t index)
{
	if(index >= Sharpen_Collections_Dictionary_Count_1class_(classStatics_Sharpen_FileSystem_DevFS.m_devices))
		return null;
	struct class_Sharpen_FileSystem_Device* dev = (struct class_Sharpen_FileSystem_Device*)Sharpen_Collections_Dictionary_GetAt_2class_int32_t_(classStatics_Sharpen_FileSystem_DevFS.m_devices, (int32_t)index);
	if(dev == null)
		return null;
	struct struct_Sharpen_FileSystem_DirEntry* entry = (struct struct_Sharpen_FileSystem_DirEntry*)Sharpen_Heap_Alloc_1int32_t_(sizeof(struct struct_Sharpen_FileSystem_DirEntry));
	int32_t i = 0;
	for(;dev->field_Name[i] != '\0';i = i + 1
	)
	{
		entry->field_Name[i] = dev->field_Name[i];
	}
	;
	entry->field_Name[i] = '\0';
	return entry;
}
struct class_Sharpen_FileSystem_Device* classInit_Sharpen_FileSystem_Device(void)
{
	struct class_Sharpen_FileSystem_Device* object = calloc(1, sizeof(struct class_Sharpen_FileSystem_Device));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_FileSystem_Device;
	return object;
}

inline struct struct_Sharpen_FileSystem_DirEntry structInit_Sharpen_FileSystem_DirEntry(void)
{
	struct struct_Sharpen_FileSystem_DirEntry object;
	return object;
}
struct class_Sharpen_Collections_Dictionary* classInit_Sharpen_Collections_Dictionary(void)
{
	struct class_Sharpen_Collections_Dictionary* object = calloc(1, sizeof(struct class_Sharpen_Collections_Dictionary));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Collections_Dictionary;
	object->field_m_index = Sharpen_Collections_LongIndex_LongIndex_1class_(classInit_Sharpen_Collections_LongIndex());
	object->field_m_values = Sharpen_Collections_List_List_1class_(classInit_Sharpen_Collections_List());
	return object;
}

void Sharpen_Collections_Dictionary_Clear_1class_(struct class_Sharpen_Collections_Dictionary* obj)
{
	Sharpen_Collections_List_Clear_1class_(obj->field_m_values);
}
int32_t Sharpen_Collections_Dictionary_Count_1class_(struct class_Sharpen_Collections_Dictionary* obj)
{
	return Sharpen_Collections_List_Count_getter(obj->field_m_values);
}
void* Sharpen_Collections_Dictionary_GetAt_2class_int32_t_(struct class_Sharpen_Collections_Dictionary* obj, int32_t index)
{
	return  ( index !=  - 1 )  ? Sharpen_Collections_List_Item_getter(obj->field_m_values)[index] : null;
}
void Sharpen_Collections_Dictionary_Add_3class_int64_t_void__(struct class_Sharpen_Collections_Dictionary* obj, int64_t key, void* val)
{
	int32_t index = Sharpen_Collections_LongIndex_IndexOf_2class_int64_t_(obj->field_m_index, key);
	if(index ==  - 1){
		Sharpen_Collections_LongIndex_Add_2class_int64_t_(obj->field_m_index, key);
		Sharpen_Collections_List_Add_2class_void__(obj->field_m_values, val);
	}
}
void* Sharpen_Collections_Dictionary_GetByKey_2class_int64_t_(struct class_Sharpen_Collections_Dictionary* obj, int64_t key)
{
	int32_t index = Sharpen_Collections_LongIndex_IndexOf_2class_int64_t_(obj->field_m_index, key);
	return  ( index !=  - 1 )  ? Sharpen_Collections_List_Item_getter(obj->field_m_values)[index] : null;
}
struct class_Sharpen_FileSystem_Fat16* classInit_Sharpen_FileSystem_Fat16(void)
{
	struct class_Sharpen_FileSystem_Fat16* object = calloc(1, sizeof(struct class_Sharpen_FileSystem_Fat16));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_FileSystem_Fat16;
	return object;
}

inline void classCctor_Sharpen_FileSystem_Fat16(void)
{
}
void Sharpen_FileSystem_Fat16_initFAT_1struct_class_Sharpen_FileSystem_Node__(struct class_Sharpen_FileSystem_Node* dev)
{
	uint8_t* firstSector = calloc((512), sizeof(uint8_t));
	firstSector[0x00] = 0xFF;
	dev->field_Read(dev, 0, 512, firstSector);
	if(firstSector[classStatics_Sharpen_FileSystem_Fat16.FirstPartitonEntry + classStatics_Sharpen_FileSystem_Fat16.ENTRYTYPE] != 0x06)
		return;
	int32_t off = classStatics_Sharpen_FileSystem_Fat16.FirstPartitonEntry + classStatics_Sharpen_FileSystem_Fat16.ENTRYNUMSECTORSBETWEEN;
	classStatics_Sharpen_FileSystem_Fat16.m_beginLBA = firstSector[off + 3] << 24 | firstSector[off + 2] << 16 | firstSector[off + 1] << 8 | firstSector[off];
	uint8_t* bootSector = calloc((512), sizeof(uint8_t));
	dev->field_Read(dev, (uint32_t)classStatics_Sharpen_FileSystem_Fat16.m_beginLBA, 512, bootSector);
	uint8_t* bootPtr = (uint8_t*)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(bootSector);
	uint8_t* bpb = (uint8_t*)Sharpen_Heap_Alloc_1int32_t_(90);
	Sharpen_Memory_Memcpy_3void__void__int32_t_(bpb, bootPtr, 90);
	classStatics_Sharpen_FileSystem_Fat16.m_bpb = (struct struct_Sharpen_FileSystem_Fat16BPB*)bpb;
	Sharpen_FileSystem_Fat16_parseBoot_0();
}
void Sharpen_FileSystem_Fat16_parseBoot_0(void)
{
	classStatics_Sharpen_FileSystem_Fat16.m_clusterBeginLBA = classStatics_Sharpen_FileSystem_Fat16.m_beginLBA + classStatics_Sharpen_FileSystem_Fat16.m_bpb->field_ReservedSectors +  ( classStatics_Sharpen_FileSystem_Fat16.m_bpb->field_NumFats * (int32_t)classStatics_Sharpen_FileSystem_Fat16.m_bpb->field_SectorsPerFat16 ) ;
	uint8_t* buffer = calloc((512), sizeof(uint8_t));
	classStatics_Sharpen_FileSystem_Fat16.m_dev->field_Read(classStatics_Sharpen_FileSystem_Fat16.m_dev, (uint32_t) ( classStatics_Sharpen_FileSystem_Fat16.m_clusterBeginLBA ) , 512, buffer);
	classStatics_Sharpen_FileSystem_Fat16.m_dirEntries = (struct struct_Sharpen_FileSystem_FatDirEntry*)Sharpen_Heap_Alloc_1int32_t_(classStatics_Sharpen_FileSystem_Fat16.m_bpb->field_NumDirEntries * sizeof(struct struct_Sharpen_FileSystem_FatDirEntry));
	struct struct_Sharpen_FileSystem_FatDirEntry* curBufPtr = (struct struct_Sharpen_FileSystem_FatDirEntry*)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(buffer);
	int32_t sectorOffset = 0;
	int32_t offset = 0;
	for(int32_t i = 0;i < classStatics_Sharpen_FileSystem_Fat16.m_bpb->field_NumDirEntries;i = i + 1
	)
	{
		{
			if(offset == 16){
				sectorOffset = sectorOffset + 1
				;
				classStatics_Sharpen_FileSystem_Fat16.m_dev->field_Read(classStatics_Sharpen_FileSystem_Fat16.m_dev, (uint32_t) ( classStatics_Sharpen_FileSystem_Fat16.m_clusterBeginLBA + sectorOffset ) , 512, buffer);
				offset = 0;
			}
			classStatics_Sharpen_FileSystem_Fat16.m_dirEntries[i] = curBufPtr[offset];
			offset = offset + 1
			;
		}
	}
	;
	classStatics_Sharpen_FileSystem_Fat16.m_numDirEntries = classStatics_Sharpen_FileSystem_Fat16.m_bpb->field_NumDirEntries;
	classStatics_Sharpen_FileSystem_Fat16.m_beginDataLBA = classStatics_Sharpen_FileSystem_Fat16.m_clusterBeginLBA +  (  ( classStatics_Sharpen_FileSystem_Fat16.m_bpb->field_NumDirEntries * 32 )  / classStatics_Sharpen_FileSystem_Fat16.m_bpb->field_BytesPerSector ) ;
}
uint32_t Sharpen_FileSystem_Fat16_Data_clust_to_lba_1uint32_t_(uint32_t cluster)
{
	return (uint32_t) ( classStatics_Sharpen_FileSystem_Fat16.m_beginDataLBA +  ( cluster - 2 )  * classStatics_Sharpen_FileSystem_Fat16.m_bpb->field_SectorsPerCluster ) ;
}
void Sharpen_FileSystem_Fat16_Init_2struct_class_Sharpen_FileSystem_Node__char__(struct class_Sharpen_FileSystem_Node* dev, char* name)
{
	classStatics_Sharpen_FileSystem_Fat16.m_dev = dev;
	Sharpen_FileSystem_Fat16_initFAT_1struct_class_Sharpen_FileSystem_Node__(dev);
	struct class_Sharpen_FileSystem_MountPoint* p = classInit_Sharpen_FileSystem_MountPoint();
	p->field_Name = name;
	p->field_Node = classInit_Sharpen_FileSystem_Node();
	p->field_Node->field_ReadDir = Sharpen_FileSystem_Fat16_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_;
	p->field_Node->field_FindDir = Sharpen_FileSystem_Fat16_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__;
	p->field_Node->field_Cookie = 0xFFFFFFFF;
	Sharpen_FileSystem_VFS_AddMountPoint_1struct_class_Sharpen_FileSystem_MountPoint__(p);
}
struct class_Sharpen_FileSystem_Node* Sharpen_FileSystem_Fat16_CreateNode_1struct_struct_Sharpen_FileSystem_FatDirEntry__(struct struct_Sharpen_FileSystem_FatDirEntry* dirEntry)
{
	struct class_Sharpen_FileSystem_Node* node = classInit_Sharpen_FileSystem_Node();
	node->field_Size = dirEntry->field_Size;
	node->field_Cookie = (uint32_t)dirEntry;
	node->field_Read = Sharpen_FileSystem_Fat16_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__;
	node->field_Write = Sharpen_FileSystem_Fat16_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__;
	node->field_ReadDir = Sharpen_FileSystem_Fat16_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_;
	node->field_FindDir = Sharpen_FileSystem_Fat16_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__;
	return node;
}
struct class_Sharpen_FileSystem_Node* Sharpen_FileSystem_Fat16_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__(struct class_Sharpen_FileSystem_Node* node, char* name)
{
	int32_t length = Sharpen_Utilities_String_Length_1char__(name);
	if(length > 12)
		return null;
	int32_t dot = Sharpen_Utilities_String_IndexOf_2char__char__(name, ".");
	if(dot > 8)
		return null;
	char* testFor = (char*)Sharpen_Heap_Alloc_1int32_t_(11);
	Sharpen_Memory_Memset_3void__int32_t_int32_t_(testFor, ' ', 11);
	for(int32_t i = 0;i < 8;i = i + 1
	)
	{
		{
			if( ( dot > 0 && i < length && i < dot )  ||  ( dot ==  - 1 && i < length ) )
						testFor[i] = Sharpen_Utilities_String_ToUpper_1char_(name[i]);
			else
			{
				testFor[i] = ' ';
			}
		}
	}
	;
	if(dot !=  - 1){
		int32_t lengthExt = length - dot - 1;
		for(int32_t i = 0;i < 3;i = i + 1
		)
		{
			{
				if(i < lengthExt)
								testFor[i + 8] = Sharpen_Utilities_String_ToUpper_1char_(name[i + dot + 1]);
				else
				{
					testFor[i + 8] = ' ';
				}
			}
		}
		;
	}
	uint32_t cluster = 0xFFFFFFFF;
	if(node->field_Cookie != 0xFFFFFFFF){
		struct struct_Sharpen_FileSystem_FatDirEntry* entry = (struct struct_Sharpen_FileSystem_FatDirEntry*)node->field_Cookie;
		cluster = entry->field_ClusterNumberLo;
	}
	struct class_Sharpen_FileSystem_SubDirectory* dir = Sharpen_FileSystem_Fat16_readDirectory_1uint32_t_(cluster);
	struct class_Sharpen_FileSystem_Node* nd = Sharpen_FileSystem_Fat16_FindFileInDirectory_2struct_class_Sharpen_FileSystem_SubDirectory__char__(dir, testFor);
	Sharpen_Heap_Free_1void__(testFor);
	return nd;
}
struct class_Sharpen_FileSystem_Node* Sharpen_FileSystem_Fat16_FindFileInDirectory_2struct_class_Sharpen_FileSystem_SubDirectory__char__(struct class_Sharpen_FileSystem_SubDirectory* dir, char* testFor)
{
	for(int32_t i = 0;i < dir->field_Length;i = i + 1
	)
	{
		{
			struct struct_Sharpen_FileSystem_FatDirEntry entry = dir->field_DirEntries[i];
			if(entry.field_Name[0] == 0 || entry.field_Name[0] == 0xE5 || entry.field_Attribs == 0xF ||  ( entry.field_Attribs & 0x08 )  > 0)
						continue;
			struct struct_Sharpen_FileSystem_FatDirEntry* entr = (struct struct_Sharpen_FileSystem_FatDirEntry*)Sharpen_Heap_Alloc_1int32_t_(sizeof(struct struct_Sharpen_FileSystem_FatDirEntry));
			Sharpen_Memory_Memcpy_3void__void__int32_t_(entr, dir->field_DirEntries + i, sizeof(struct struct_Sharpen_FileSystem_FatDirEntry));
			if(Sharpen_Memory_Compare_3char__char__int32_t_(testFor, entry.field_Name, 11)){
				return Sharpen_FileSystem_Fat16_CreateNode_1struct_struct_Sharpen_FileSystem_FatDirEntry__(entr);
			}
		}
	}
	;
	return null;
}
struct struct_Sharpen_FileSystem_DirEntry* Sharpen_FileSystem_Fat16_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_(struct class_Sharpen_FileSystem_Node* node, uint32_t index)
{
	if(index > classStatics_Sharpen_FileSystem_Fat16.m_numDirEntries)
		return null;
	int32_t j = 0;
	uint32_t cluster = 0xFFFFFFFF;
	if(node->field_Cookie != 0xFFFFFFFF){
		struct struct_Sharpen_FileSystem_FatDirEntry* entry = (struct struct_Sharpen_FileSystem_FatDirEntry*)node->field_Cookie;
		cluster = entry->field_ClusterNumberLo;
	}
	struct class_Sharpen_FileSystem_SubDirectory* dir = Sharpen_FileSystem_Fat16_readDirectory_1uint32_t_(cluster);
	for(int32_t i = 0;i < dir->field_Length;i = i + 1
	)
	{
		{
			struct struct_Sharpen_FileSystem_FatDirEntry entry = dir->field_DirEntries[i];
			if(entry.field_Name[0] == 0 || entry.field_Name[0] == (char)0xE5 || entry.field_Attribs == 0xF ||  ( entry.field_Attribs & 0x08 )  > 0)
						continue;
			if(j >= index){
				struct struct_Sharpen_FileSystem_DirEntry* outDir = (struct struct_Sharpen_FileSystem_DirEntry*)Sharpen_Heap_Alloc_1int32_t_(sizeof(struct struct_Sharpen_FileSystem_DirEntry));
				int32_t fnLength = Sharpen_Utilities_String_IndexOf_2char__char__(Sharpen_Utilities_Util_CharPtrToString_1char__(entry.field_Name), " ");
				if(fnLength > 8 || fnLength ==  - 1)
								fnLength = 8;
				int32_t extLength = Sharpen_Utilities_String_IndexOf_2char__char__(Sharpen_Utilities_Util_CharPtrToString_1char__(entry.field_Name + 8), " ");
				if(extLength ==  - 1)
								extLength = 3;
				int32_t offset = 0;
				for(int32_t z = 0;z < fnLength;z = z + 1
				)
				{
					outDir->field_Name[offset] = entry.field_Name[z];
					offset = offset + 1;
				}
				;
				outDir->field_Name[offset] = '.';
				offset = offset + 1;
				for(int32_t z = 0;z < extLength;z = z + 1
				)
				{
					outDir->field_Name[offset] = entry.field_Name[z + 8];
					offset = offset + 1;
				}
				;
				outDir->field_Name[offset] = '\0';
				for(int32_t z = 0;z < offset;z = z + 1
				)
				{
					outDir->field_Name[z] = Sharpen_Utilities_String_ToLower_1char_(outDir->field_Name[z]);
				}
				;
				return outDir;
			}
			j = j + 1
			;
		}
	}
	;
	return null;
}
uint32_t Sharpen_FileSystem_Fat16_FindNextCluster_1uint32_t_(uint32_t cluster)
{
	int32_t beginFat = classStatics_Sharpen_FileSystem_Fat16.m_beginLBA + classStatics_Sharpen_FileSystem_Fat16.m_bpb->field_ReservedSectors;
	uint32_t clusters =  ( cluster / 256 ) ;
	uint32_t adr = (uint32_t) ( beginFat + clusters ) ;
	uint32_t offset =  ( cluster * 2 )  -  ( clusters * 512 ) ;
	uint8_t* fatBuffer = calloc((512), sizeof(uint8_t));
	classStatics_Sharpen_FileSystem_Fat16.m_dev->field_Read(classStatics_Sharpen_FileSystem_Fat16.m_dev, (uint32_t) ( adr ) , 512, fatBuffer);
	uint8_t* ptr = (uint8_t*)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(fatBuffer);
	uint16_t* pointer = (uint16_t*) ( ptr + offset ) ;
	uint16_t nextCluster = *pointer;
	if(nextCluster >= classStatics_Sharpen_FileSystem_Fat16.FAT_EOF)
		return 0xFFFF;
	return nextCluster;
}
uint32_t Sharpen_FileSystem_Fat16_FirstFirstFreeSector_0(void)
{
	return 0;
}
uint32_t Sharpen_FileSystem_Fat16_readFile_4uint32_t_uint32_t_uint32_t_uint8_t__(uint32_t startCluster, uint32_t offset, uint32_t size, uint8_t* buffer)
{
	uint32_t dataPerCluster = classStatics_Sharpen_FileSystem_Fat16.m_bpb->field_SectorsPerCluster;
	uint32_t sectorsOffset = (uint32_t) ( (int32_t)offset / 512 ) ;
	uint32_t clusterOffset = sectorsOffset / dataPerCluster;
	if(clusterOffset > 0){
		for(int32_t i = 0;i < clusterOffset;i = i + 1
		)
		{
			{
				startCluster = Sharpen_FileSystem_Fat16_FindNextCluster_1uint32_t_(startCluster);
				if(startCluster == 0xFFFF)
								return 0;
			}
		}
		;
	}
	sectorsOffset = sectorsOffset -  ( clusterOffset * classStatics_Sharpen_FileSystem_Fat16.m_bpb->field_SectorsPerCluster ) ;
	uint32_t StartOffset = offset -  ( sectorsOffset * 512 ) ;
	uint8_t* buf = calloc((512), sizeof(uint8_t));
	classStatics_Sharpen_FileSystem_Fat16.m_dev->field_Read(classStatics_Sharpen_FileSystem_Fat16.m_dev, Sharpen_FileSystem_Fat16_Data_clust_to_lba_1uint32_t_(startCluster), 512, buf);
	uint32_t sizeInSectors = size / 512;
	if(sizeInSectors == 0)
		sizeInSectors = sizeInSectors + 1
	;
	uint32_t offsetInCluster = sectorsOffset;
	uint32_t offsetInSector = StartOffset;
	uint32_t currentCluster = startCluster;
	uint32_t currentOffset = 0;
	int32_t sizeLeft = (int32_t)size;
	for(int32_t i = 0;i < sizeInSectors;i = i + 1
	)
	{
		{
			if(offsetInCluster == classStatics_Sharpen_FileSystem_Fat16.m_bpb->field_SectorsPerCluster){
				currentCluster = Sharpen_FileSystem_Fat16_FindNextCluster_1uint32_t_(currentCluster);
				if(currentCluster == 0xFFFF)
								return currentOffset;
				offsetInCluster = 0;
			}
			classStatics_Sharpen_FileSystem_Fat16.m_dev->field_Read(classStatics_Sharpen_FileSystem_Fat16.m_dev, Sharpen_FileSystem_Fat16_Data_clust_to_lba_1uint32_t_(currentCluster) + offsetInCluster, 512, buf);
			int32_t sizeTemp =  ( sizeLeft > 512 )  ? 512 : sizeLeft;
			int32_t sizeLeftinSector = 512;
			sizeLeftinSector -= (int32_t)offsetInSector;
			if(sizeLeft > sizeLeftinSector){
				sizeTemp = sizeLeftinSector;
				sizeInSectors = sizeInSectors + 1
				;
			}
			Sharpen_Memory_Memcpy_3void__void__int32_t_((void*) ( (int32_t)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(buffer) + currentOffset ) , (void*) ( (int32_t)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(buf) + offsetInSector ) , sizeTemp);
			currentOffset += (uint32_t)sizeTemp;
			sizeLeft -= sizeTemp;
			offsetInCluster = offsetInCluster + 1
			;
			offsetInSector = 0;
		}
	}
	;
	return size;
}
struct class_Sharpen_FileSystem_SubDirectory* Sharpen_FileSystem_Fat16_readDirectory_1uint32_t_(uint32_t cluster)
{
	struct class_Sharpen_FileSystem_SubDirectory* outDir = classInit_Sharpen_FileSystem_SubDirectory();
	if(cluster == 0xFFFFFFFF){
		outDir->field_Length = classStatics_Sharpen_FileSystem_Fat16.m_numDirEntries;
		outDir->field_DirEntries = classStatics_Sharpen_FileSystem_Fat16.m_dirEntries;
	}
	else
	{
		uint8_t* buffer = calloc((classStatics_Sharpen_FileSystem_Fat16.m_bpb->field_NumDirEntries * sizeof(struct struct_Sharpen_FileSystem_FatDirEntry)), sizeof(uint8_t));
		Sharpen_FileSystem_Fat16_readFile_4uint32_t_uint32_t_uint32_t_uint8_t__(cluster, 0, (uint32_t) ( classStatics_Sharpen_FileSystem_Fat16.m_bpb->field_NumDirEntries * sizeof(struct struct_Sharpen_FileSystem_FatDirEntry) ) , buffer);
		struct struct_Sharpen_FileSystem_FatDirEntry* entries = (struct struct_Sharpen_FileSystem_FatDirEntry*)Sharpen_Heap_Alloc_1int32_t_(classStatics_Sharpen_FileSystem_Fat16.m_bpb->field_NumDirEntries * sizeof(struct struct_Sharpen_FileSystem_FatDirEntry));
		struct struct_Sharpen_FileSystem_FatDirEntry* curBufPtr = (struct struct_Sharpen_FileSystem_FatDirEntry*)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(buffer);
		int32_t length = 0;
		for(int32_t i = 0;i < classStatics_Sharpen_FileSystem_Fat16.m_bpb->field_NumDirEntries;i = i + 1
		)
		{
			{
				entries[i] = curBufPtr[i];
				if(curBufPtr[i].field_Name[0] == 0x00){
					break;
				}
				length = length + 1
				;
			}
		}
		;
		outDir->field_DirEntries = entries;
		outDir->field_Length = (uint32_t)length;
	}
	return outDir;
}
uint32_t Sharpen_FileSystem_Fat16_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer)
{
	struct struct_Sharpen_FileSystem_FatDirEntry* entry = (struct struct_Sharpen_FileSystem_FatDirEntry*)node->field_Cookie;
	if(offset + size > entry->field_Size)
		size = entry->field_Size - offset;
	return Sharpen_FileSystem_Fat16_readFile_4uint32_t_uint32_t_uint32_t_uint8_t__(entry->field_ClusterNumberLo, offset, size, buffer);
}
uint32_t Sharpen_FileSystem_Fat16_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer)
{
	return 0;
}
struct class_Sharpen_Collections_LongIndex* classInit_Sharpen_Collections_LongIndex(void)
{
	struct class_Sharpen_Collections_LongIndex* object = calloc(1, sizeof(struct class_Sharpen_Collections_LongIndex));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Collections_LongIndex;
	object->field_m_currentCap = 4;
	object->prop_Count = 0;
	return object;
}

int64_t* Sharpen_Collections_LongIndex_Item_getter(struct class_Sharpen_Collections_LongIndex* obj)
{
	return obj->prop_Item;
}
int64_t* Sharpen_Collections_LongIndex_Item_setter(struct class_Sharpen_Collections_LongIndex* obj, int64_t* value)
{
	obj->prop_Item = value;
	return value;
}
int32_t Sharpen_Collections_LongIndex_Count_getter(struct class_Sharpen_Collections_LongIndex* obj)
{
	return obj->prop_Count;
}
int32_t Sharpen_Collections_LongIndex_Count_setter(struct class_Sharpen_Collections_LongIndex* obj, int32_t value)
{
	obj->prop_Count = value;
	return value;
}
int32_t Sharpen_Collections_LongIndex_Capacity_getter(struct class_Sharpen_Collections_LongIndex* obj)
{
	return obj->field_m_currentCap;
}
int32_t Sharpen_Collections_LongIndex_Capacity_setter(struct class_Sharpen_Collections_LongIndex* obj, int32_t value)
{
	int64_t* newArray = calloc((value), sizeof(int64_t));
	Sharpen_Memory_Memcpy_3void__void__int32_t_(Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(newArray), Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(Sharpen_Collections_LongIndex_Item_getter(obj)),  ( obj->field_m_currentCap + 1 )  * sizeof(int64_t));
	Sharpen_Collections_LongIndex_Item_setter(obj, newArray);
	obj->field_m_currentCap = value;
	return value;
}
struct class_Sharpen_Collections_LongIndex* Sharpen_Collections_LongIndex_LongIndex_1class_(struct class_Sharpen_Collections_LongIndex* obj)
{
	Sharpen_Collections_LongIndex_Item_setter(obj, calloc((obj->field_m_currentCap), sizeof(int64_t)));
	return obj;
}
void Sharpen_Collections_LongIndex_EnsureCapacity_2class_int32_t_(struct class_Sharpen_Collections_LongIndex* obj, int32_t required)
{
	if(required < obj->field_m_currentCap)
		return;
	Sharpen_Collections_LongIndex_Capacity_setter(obj, Sharpen_Collections_LongIndex_Capacity_getter(obj) * 2);
}
void Sharpen_Collections_LongIndex_Add_2class_int64_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t o)
{
	Sharpen_Collections_LongIndex_EnsureCapacity_2class_int32_t_(obj, Sharpen_Collections_LongIndex_Count_getter(obj) + 1);
	Sharpen_Collections_LongIndex_Item_getter(obj)[Sharpen_Collections_LongIndex_Count_getter(obj)] = o;
	Sharpen_Collections_LongIndex_Count_setter(obj, Sharpen_Collections_LongIndex_Count_getter(obj) + 1);
}
void Sharpen_Collections_LongIndex_RemoveAt_2class_int32_t_(struct class_Sharpen_Collections_LongIndex* obj, int32_t index)
{
	if(index >= Sharpen_Collections_LongIndex_Count_getter(obj))
		return;
	int32_t destination = (int32_t)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(Sharpen_Collections_LongIndex_Item_getter(obj)) +  ( index * sizeof(int64_t) ) ;
	int32_t source = (int32_t)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(Sharpen_Collections_LongIndex_Item_getter(obj)) +  (  ( index + 1 )  * sizeof(int64_t) ) ;
	Sharpen_Memory_Memcpy_3void__void__int32_t_((void*)destination, (void*)source,  ( Sharpen_Collections_LongIndex_Count_getter(obj) - index - 1 )  * sizeof(int64_t));
	Sharpen_Collections_LongIndex_Count_setter(obj, Sharpen_Collections_LongIndex_Count_getter(obj) - 1)
	;
	if(Sharpen_Collections_LongIndex_Count_getter(obj) * 2 < Sharpen_Collections_LongIndex_Capacity_getter(obj))
		Sharpen_Collections_LongIndex_Capacity_setter(obj, Sharpen_Collections_LongIndex_Capacity_getter(obj) / 2);
}
void Sharpen_Collections_LongIndex_Clear_1class_(struct class_Sharpen_Collections_LongIndex* obj)
{
	Sharpen_Memory_Memset_3void__int32_t_int32_t_(Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(Sharpen_Collections_LongIndex_Item_getter(obj)), 0, obj->field_m_currentCap * sizeof(void*));
	Sharpen_Collections_LongIndex_Count_setter(obj, 0);
}
int32_t Sharpen_Collections_LongIndex_Contains_2class_int64_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t item)
{
	for(int32_t i = 0;i < Sharpen_Collections_LongIndex_Count_getter(obj);i = i + 1
	)
	{
		{
			if(Sharpen_Collections_LongIndex_Item_getter(obj)[i] == item)
						return true;
		}
	}
	;
	return false;
}
void Sharpen_Collections_LongIndex_CopyTo_2class_int64_t__(struct class_Sharpen_Collections_LongIndex* obj, int64_t* array)
{
	Sharpen_Collections_LongIndex_CopyTo_5class_int32_t_int64_t__int32_t_int32_t_(obj, 0, array, 0, Sharpen_Collections_LongIndex_Count_getter(obj));
}
void Sharpen_Collections_LongIndex_CopyTo_3class_int64_t__int32_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t* array, int32_t arrayIndex)
{
	Sharpen_Collections_LongIndex_CopyTo_5class_int32_t_int64_t__int32_t_int32_t_(obj, 0, array, arrayIndex, Sharpen_Collections_LongIndex_Count_getter(obj));
}
void Sharpen_Collections_LongIndex_CopyTo_5class_int32_t_int64_t__int32_t_int32_t_(struct class_Sharpen_Collections_LongIndex* obj, int32_t index, int64_t* array, int32_t arrayIndex, int32_t count)
{
	int32_t destination = (int32_t)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(array) +  ( sizeof(void*) * arrayIndex ) ;
	int32_t source = (int32_t)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(Sharpen_Collections_LongIndex_Item_getter(obj)) +  ( sizeof(void*) * index ) ;
	Sharpen_Memory_Memcpy_3void__void__int32_t_((void*)destination, (void*)source, count);
}
int32_t Sharpen_Collections_LongIndex_IndexOf_2class_int64_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t item)
{
	return Sharpen_Collections_LongIndex_IndexOf_4class_int64_t_int32_t_int32_t_(obj, item, 0, Sharpen_Collections_LongIndex_Count_getter(obj));
}
int32_t Sharpen_Collections_LongIndex_IndexOf_3class_int64_t_int32_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t item, int32_t index)
{
	return Sharpen_Collections_LongIndex_IndexOf_4class_int64_t_int32_t_int32_t_(obj, item, index, Sharpen_Collections_LongIndex_Count_getter(obj) - index);
}
int32_t Sharpen_Collections_LongIndex_IndexOf_4class_int64_t_int32_t_int32_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t item, int32_t index, int32_t count)
{
	for(int32_t i = index;i < Sharpen_Collections_LongIndex_Count_getter(obj) && i < count + index;i = i + 1
	)
	{
		{
			if(Sharpen_Collections_LongIndex_Item_getter(obj)[i] == item){
				return i;
			}
		}
	}
	;
	return  - 1;
}
int32_t Sharpen_Collections_LongIndex_LastIndexOf_2class_int64_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t item)
{
	return Sharpen_Collections_LongIndex_LastIndexOf_4class_int64_t_int32_t_int32_t_(obj, item, Sharpen_Collections_LongIndex_Count_getter(obj) - 1, Sharpen_Collections_LongIndex_Count_getter(obj));
}
int32_t Sharpen_Collections_LongIndex_LastIndexOf_3class_int64_t_int32_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t item, int32_t index)
{
	return Sharpen_Collections_LongIndex_LastIndexOf_4class_int64_t_int32_t_int32_t_(obj, item, index, Sharpen_Collections_LongIndex_Count_getter(obj) - index);
}
int32_t Sharpen_Collections_LongIndex_LastIndexOf_4class_int64_t_int32_t_int32_t_(struct class_Sharpen_Collections_LongIndex* obj, int64_t item, int32_t index, int32_t count)
{
	for(int32_t i = index;i >= 0 && i - index < count;i = i - 1
	)
	{
		{
			if(Sharpen_Collections_LongIndex_Item_getter(obj)[i] == item)
						return i;
		}
	}
	;
	return  - 1;
}
struct class_Sharpen_Console* classInit_Sharpen_Console(void)
{
	struct class_Sharpen_Console* object = calloc(1, sizeof(struct class_Sharpen_Console));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Console;
	return object;
}

inline void classCctor_Sharpen_Console(void)
{
	classStatics_Sharpen_Console.vidmem = (uint8_t*)0xB8000;
}
int32_t Sharpen_Console_X_getter(void)
{
	return classStatics_Sharpen_Console.prop_X;
}
int32_t Sharpen_Console_X_setter(int32_t value)
{
	classStatics_Sharpen_Console.prop_X = value;
	return value;
}
int32_t Sharpen_Console_Y_getter(void)
{
	return classStatics_Sharpen_Console.prop_Y;
}
int32_t Sharpen_Console_Y_setter(int32_t value)
{
	classStatics_Sharpen_Console.prop_Y = value;
	return value;
}
uint8_t Sharpen_Console_Attribute_getter(void)
{
	return classStatics_Sharpen_Console.prop_Attribute;
}
uint8_t Sharpen_Console_Attribute_setter(uint8_t value)
{
	classStatics_Sharpen_Console.prop_Attribute = value;
	return value;
}
void Sharpen_Console_PutChar_1char_(char ch)
{
	if(ch == '\n'){
		Sharpen_Console_X_setter(0);
		Sharpen_Console_Y_setter(Sharpen_Console_Y_getter() + 1)
		;
	}
	else if(ch == '\r'){
		Sharpen_Console_X_setter(0);
	}
	else if(ch == '\t'){
		Sharpen_Console_X_setter( ( Sharpen_Console_X_getter() + 4 )  &  ~  ( 4 - 1 ) );
	}
	else
	{
		classStatics_Sharpen_Console.vidmem[ ( Sharpen_Console_Y_getter() * 80 + Sharpen_Console_X_getter() )  * 2 + 0] = (uint8_t)ch;
		classStatics_Sharpen_Console.vidmem[ ( Sharpen_Console_Y_getter() * 80 + Sharpen_Console_X_getter() )  * 2 + 1] = Sharpen_Console_Attribute_getter();
		Sharpen_Console_X_setter(Sharpen_Console_X_getter() + 1)
		;
	}
	if(Sharpen_Console_X_getter() == 80){
		Sharpen_Console_X_setter(0);
		Sharpen_Console_Y_setter(Sharpen_Console_Y_getter() + 1)
		;
	}
	if(Sharpen_Console_Y_getter() > 24){
		Sharpen_Console_Y_setter(24);
		Sharpen_Memory_Memcpy_3void__void__int32_t_(classStatics_Sharpen_Console.vidmem, &classStatics_Sharpen_Console.vidmem[1 * 80 * 2], 80 * 24 * 2);
		for(int32_t i = 0;i < 80;i = i + 1
		)
		{
			{
				classStatics_Sharpen_Console.vidmem[ (  ( 24 * 80 )  + i )  * 2 + 0] = (uint8_t)' ';
				classStatics_Sharpen_Console.vidmem[ (  ( 24 * 80 )  + i )  * 2 + 1] = Sharpen_Console_Attribute_getter();
			}
		}
		;
	}
	Sharpen_Console_MoveCursor_0();
}
void Sharpen_Console_Clear_0(void)
{
	Sharpen_Console_X_setter(0);
	Sharpen_Console_Y_setter(0);
	Sharpen_Console_MoveCursor_0();
	for(int32_t i = 0;i < 25 * 80;i = i + 1
	)
	{
		{
			classStatics_Sharpen_Console.vidmem[i * 2 + 0] = (uint8_t)' ';
			classStatics_Sharpen_Console.vidmem[i * 2 + 1] = Sharpen_Console_Attribute_getter();
		}
	}
	;
}
void Sharpen_Console_Write_1char__(char* text)
{
	for(int32_t i = 0;text[i] != '\0';i = i + 1
	)
	{
		{
			Sharpen_Console_PutChar_1char_(text[i]);
		}
	}
	;
}
void Sharpen_Console_WriteLine_1char__(char* text)
{
	Sharpen_Console_Write_1char__(text);
	Sharpen_Console_PutChar_1char_('\n');
}
void Sharpen_Console_WriteHex_1int64_t_(int64_t num)
{
	if(num == 0){
		Sharpen_Console_PutChar_1char_('0');
		return;
	}
	int32_t noZeroes = true;
	for(int32_t j = 60;j >= 0;j -= 4)
	{
		{
			int64_t tmp =  ( num >> j )  & 0x0F;
			if(tmp == 0 && noZeroes)
						continue;
			noZeroes = false;
			if(tmp >= 0x0A){
				Sharpen_Console_PutChar_1char_((char) ( tmp - 0x0A + 'A' ) );
			}
			else
			{
				Sharpen_Console_PutChar_1char_((char) ( tmp + '0' ) );
			}
		}
	}
	;
}
void Sharpen_Console_WriteNum_1int32_t_(int32_t num)
{
	if(num == 0){
		Sharpen_Console_PutChar_1char_('0');
		return;
	}
	if(num < 0){
		Sharpen_Console_PutChar_1char_('-');
		num =  - num;
	}
	int32_t a = num % 10;
	if(num >= 10)
		Sharpen_Console_WriteNum_1int32_t_(num / 10);
	Sharpen_Console_PutChar_1char_((char) ( '0' + a ) );
}
void Sharpen_Console_MoveCursor_0(void)
{
	int32_t index = Sharpen_Console_Y_getter() * 80 + Sharpen_Console_X_getter();
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(0x3D4, 14);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(0x3D5, (uint8_t) ( index >> 8 ) );
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(0x3D4, 15);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(0x3D5, (uint8_t) ( index & 0xFF ) );
}
struct class_Sharpen_Arch_GDT* classInit_Sharpen_Arch_GDT(void)
{
	struct class_Sharpen_Arch_GDT* object = calloc(1, sizeof(struct class_Sharpen_Arch_GDT));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Arch_GDT;
	return object;
}

inline struct struct_Sharpen_Arch_GDT_GDT_Entry structInit_Sharpen_Arch_GDT_GDT_Entry(void)
{
	struct struct_Sharpen_Arch_GDT_GDT_Entry object;
	return object;
}
inline struct struct_Sharpen_Arch_GDT_GDT_Pointer structInit_Sharpen_Arch_GDT_GDT_Pointer(void)
{
	struct struct_Sharpen_Arch_GDT_GDT_Pointer object;
	return object;
}
inline struct struct_Sharpen_Arch_GDT_TSS structInit_Sharpen_Arch_GDT_TSS(void)
{
	struct struct_Sharpen_Arch_GDT_TSS object;
	return object;
}
struct struct_Sharpen_Arch_GDT_TSS* Sharpen_Arch_GDT_TSS_Entry_getter(void)
{
	return classStatics_Sharpen_Arch_GDT.prop_TSS_Entry;
}
struct struct_Sharpen_Arch_GDT_TSS* Sharpen_Arch_GDT_TSS_Entry_setter(struct struct_Sharpen_Arch_GDT_TSS* value)
{
	classStatics_Sharpen_Arch_GDT.prop_TSS_Entry = value;
	return value;
}
int32_t Sharpen_Arch_GDT_Privilege_1int32_t_(int32_t a)
{
	return  ( a << 0x05 ) ;
}
void Sharpen_Arch_GDT_setEntry_5int32_t_uint64_t_uint64_t_int32_t_int32_t_(int32_t num, uint64_t base_address, uint64_t limit, int32_t access, int32_t granularity)
{
	classStatics_Sharpen_Arch_GDT.m_entries[num].field_BaseLow = (uint16_t) ( base_address & 0xFFFF ) ;
	classStatics_Sharpen_Arch_GDT.m_entries[num].field_BaseMid = (uint8_t) (  ( base_address >> 16 )  & 0xFF ) ;
	classStatics_Sharpen_Arch_GDT.m_entries[num].field_BaseHigh = (uint8_t) (  ( base_address >> 24 )  & 0xFF ) ;
	classStatics_Sharpen_Arch_GDT.m_entries[num].field_LimitLow = (uint16_t) ( limit & 0xFFFF ) ;
	classStatics_Sharpen_Arch_GDT.m_entries[num].field_Granularity = (uint8_t) (  ( limit >> 16 )  & 0xFF ) ;
	classStatics_Sharpen_Arch_GDT.m_entries[num].field_Granularity |= (uint8_t) ( granularity & 0xF0 ) ;
	classStatics_Sharpen_Arch_GDT.m_entries[num].field_Access = (uint8_t)access;
}
void Sharpen_Arch_GDT_setTSS_2int32_t_struct_struct_Sharpen_Arch_GDT_TSS__(int32_t num, struct struct_Sharpen_Arch_GDT_TSS* tss)
{
	uint32_t baseAddr = (uint32_t)tss;
	uint32_t limit = (uint32_t) ( baseAddr + sizeof(struct struct_Sharpen_Arch_GDT_TSS) ) ;
	Sharpen_Memory_Memset_3void__int32_t_int32_t_(tss, 0, sizeof(struct struct_Sharpen_Arch_GDT_TSS));
	tss->field_SS0 = 0x10;
	tss->field_IOMap = (uint16_t)sizeof(struct struct_Sharpen_Arch_GDT_TSS);
	tss->field_CS = 0x08;
	tss->field_DS = 0x10;
	tss->field_ES = 0x10;
	tss->field_FS = 0x10;
	tss->field_GS = 0x10;
	tss->field_SS = 0x10;
	Sharpen_Arch_GDT_setEntry_5int32_t_uint64_t_uint64_t_int32_t_int32_t_(num, baseAddr, limit, (int32_t)enum_Sharpen_Arch_GDT_GDT_Data_EA | Sharpen_Arch_GDT_Privilege_1int32_t_(3) | (int32_t)enum_Sharpen_Arch_GDT_GDTFlags_Present, 0);
}
void Sharpen_Arch_GDT_Init_0(void)
{
	classStatics_Sharpen_Arch_GDT.m_entries = calloc((6), sizeof(struct struct_Sharpen_Arch_GDT_GDT_Entry));
	classStatics_Sharpen_Arch_GDT.m_ptr = structInit_Sharpen_Arch_GDT_GDT_Pointer();
	classStatics_Sharpen_Arch_GDT.m_ptr.field_Limit = (uint16_t) (  ( 6 * sizeof(struct struct_Sharpen_Arch_GDT_GDT_Entry) )  - 1 ) ;
	{
		struct struct_Sharpen_Arch_GDT_GDT_Entry* ptr = classStatics_Sharpen_Arch_GDT.m_entries;
		{
			classStatics_Sharpen_Arch_GDT.m_ptr.field_BaseAddress = (uint32_t)ptr;
		}
	}
	Sharpen_Arch_GDT_setEntry_5int32_t_uint64_t_uint64_t_int32_t_int32_t_(0, 0, 0, 0, 0);
	Sharpen_Arch_GDT_setEntry_5int32_t_uint64_t_uint64_t_int32_t_int32_t_(1, 0, 0xFFFFFFFF, (int32_t)enum_Sharpen_Arch_GDT_GDT_Data_ER | (int32_t)enum_Sharpen_Arch_GDT_GDTFlags_DescriptorCodeOrData | Sharpen_Arch_GDT_Privilege_1int32_t_(0) | (int32_t)enum_Sharpen_Arch_GDT_GDTFlags_Present, (int32_t)enum_Sharpen_Arch_GDT_GDTFlags_Size32 | (int32_t)enum_Sharpen_Arch_GDT_GDTFlags_Granularity);
	Sharpen_Arch_GDT_setEntry_5int32_t_uint64_t_uint64_t_int32_t_int32_t_(2, 0, 0xFFFFFFFF, (int32_t)enum_Sharpen_Arch_GDT_GDT_Data_RW | (int32_t)enum_Sharpen_Arch_GDT_GDTFlags_DescriptorCodeOrData | Sharpen_Arch_GDT_Privilege_1int32_t_(0) | (int32_t)enum_Sharpen_Arch_GDT_GDTFlags_Present, (int32_t)enum_Sharpen_Arch_GDT_GDTFlags_Size32 | (int32_t)enum_Sharpen_Arch_GDT_GDTFlags_Granularity);
	Sharpen_Arch_GDT_setEntry_5int32_t_uint64_t_uint64_t_int32_t_int32_t_(3, 0, 0xFFFFFFFF, (int32_t)enum_Sharpen_Arch_GDT_GDT_Data_ER | (int32_t)enum_Sharpen_Arch_GDT_GDTFlags_DescriptorCodeOrData | Sharpen_Arch_GDT_Privilege_1int32_t_(3) | (int32_t)enum_Sharpen_Arch_GDT_GDTFlags_Present, (int32_t)enum_Sharpen_Arch_GDT_GDTFlags_Size32 | (int32_t)enum_Sharpen_Arch_GDT_GDTFlags_Granularity);
	Sharpen_Arch_GDT_setEntry_5int32_t_uint64_t_uint64_t_int32_t_int32_t_(4, 0, 0xFFFFFFFF, (int32_t)enum_Sharpen_Arch_GDT_GDT_Data_RW | (int32_t)enum_Sharpen_Arch_GDT_GDTFlags_DescriptorCodeOrData | Sharpen_Arch_GDT_Privilege_1int32_t_(3) | (int32_t)enum_Sharpen_Arch_GDT_GDTFlags_Present, (int32_t)enum_Sharpen_Arch_GDT_GDTFlags_Size32 | (int32_t)enum_Sharpen_Arch_GDT_GDTFlags_Granularity);
	Sharpen_Arch_GDT_TSS_Entry_setter((struct struct_Sharpen_Arch_GDT_TSS*)Sharpen_Heap_Alloc_1int32_t_(sizeof(struct struct_Sharpen_Arch_GDT_TSS)));
	Sharpen_Arch_GDT_setTSS_2int32_t_struct_struct_Sharpen_Arch_GDT_TSS__(5, Sharpen_Arch_GDT_TSS_Entry_getter());
	{
		struct struct_Sharpen_Arch_GDT_GDT_Pointer* ptr = &classStatics_Sharpen_Arch_GDT.m_ptr;
		{
			Sharpen_Arch_GDT_flushGDT_1struct_struct_Sharpen_Arch_GDT_GDT_Pointer__(ptr);
		}
	}
	Sharpen_Arch_GDT_flushTSS_0();
}
struct class_Sharpen_Drivers_Block_ATA* classInit_Sharpen_Drivers_Block_ATA(void)
{
	struct class_Sharpen_Drivers_Block_ATA* object = calloc(1, sizeof(struct class_Sharpen_Drivers_Block_ATA));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Drivers_Block_ATA;
	return object;
}

inline void classCctor_Sharpen_Drivers_Block_ATA(void)
{
	classStatics_Sharpen_Drivers_Block_ATA.ATA_STATUS_RDY =  ( 1 << 4 ) ;
	classStatics_Sharpen_Drivers_Block_ATA.prop_Devices = calloc((4), sizeof(struct struct_Sharpen_Drivers_Block_IDE_Device));
}
struct struct_Sharpen_Drivers_Block_IDE_Device* Sharpen_Drivers_Block_ATA_Devices_getter(void)
{
	return classStatics_Sharpen_Drivers_Block_ATA.prop_Devices;
}
struct struct_Sharpen_Drivers_Block_IDE_Device* Sharpen_Drivers_Block_ATA_Devices_setter(struct struct_Sharpen_Drivers_Block_IDE_Device* value)
{
	classStatics_Sharpen_Drivers_Block_ATA.prop_Devices = value;
	return value;
}
void Sharpen_Drivers_Block_ATA_wait400ns_1uint32_t_(uint32_t port)
{
	Sharpen_Arch_PortIO_In8_1uint16_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_ALTSTATUS ) );
	Sharpen_Arch_PortIO_In8_1uint16_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_ALTSTATUS ) );
	Sharpen_Arch_PortIO_In8_1uint16_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_ALTSTATUS ) );
	Sharpen_Arch_PortIO_In8_1uint16_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_ALTSTATUS ) );
}
void Sharpen_Drivers_Block_ATA_selectDrive_2uint8_t_uint8_t_(uint8_t channel, uint8_t drive)
{
	uint16_t io =  ( channel == classStatics_Sharpen_Drivers_Block_ATA.ATA_PRIMARY )  ? classStatics_Sharpen_Drivers_Block_ATA.ATA_PRIMARY_IO : classStatics_Sharpen_Drivers_Block_ATA.ATA_SECONDARY_IO;
	uint8_t data =  ( drive == classStatics_Sharpen_Drivers_Block_ATA.ATA_MASTER )  ? (uint8_t)0xA0 : (uint8_t)0xB0;
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( io + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_DRIVE ) , data);
}
uint8_t* Sharpen_Drivers_Block_ATA_identify_2uint8_t_uint8_t_(uint8_t channel, uint8_t drive)
{
	Sharpen_Drivers_Block_ATA_selectDrive_2uint8_t_uint8_t_(channel, drive);
	uint16_t port =  ( channel == classStatics_Sharpen_Drivers_Block_ATA.ATA_PRIMARY )  ? classStatics_Sharpen_Drivers_Block_ATA.ATA_PRIMARY_IO : classStatics_Sharpen_Drivers_Block_ATA.ATA_SECONDARY_IO;
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_SECCNT ) , 0x00);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_LBALO ) , 0x00);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_LBAMID ) , 0x00);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_LBAHI ) , 0x00);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_CMD ) , classStatics_Sharpen_Drivers_Block_ATA.ATA_CMD_IDENTIFY);
	uint8_t status = Sharpen_Arch_PortIO_In8_1uint16_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_STATUS ) );
	if(status == 0)
		return null;
	do
	{
		status = Sharpen_Arch_PortIO_In8_1uint16_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_STATUS ) );
	}
	while( ( status & classStatics_Sharpen_Drivers_Block_ATA.ATA_STATUS_BSY )  != 0);
	while(true)
	{
		{
			status = Sharpen_Arch_PortIO_In8_1uint16_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_STATUS ) );
			if( ( status & classStatics_Sharpen_Drivers_Block_ATA.ATA_STATUS_ERR )  != 0)
						return null;
			if( ( status & classStatics_Sharpen_Drivers_Block_ATA.ATA_STATUS_DRQ )  != 0)
						break;
		}
	}
	;
	uint8_t* buffer = calloc((256), sizeof(uint8_t));
	int32_t offset = 0;
	for(int32_t i = 0;i < 128;i = i + 1
	)
	{
		{
			uint16_t shrt = Sharpen_Arch_PortIO_In16_1uint16_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_DATA ) );
			buffer[offset + 0] = (uint8_t) ( shrt >> 8 ) ;
			buffer[offset + 1] = (uint8_t) ( shrt ) ;
			offset += 2;
		}
	}
	;
	return buffer;
}
void Sharpen_Drivers_Block_ATA_poll_1uint32_t_(uint32_t port)
{
	Sharpen_Drivers_Block_ATA_wait400ns_1uint32_t_(port);
	uint8_t status;
	do
	{
		status = Sharpen_Arch_PortIO_In8_1uint16_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_STATUS ) );
	}
	while( ( status & classStatics_Sharpen_Drivers_Block_ATA.ATA_STATUS_BSY )  > 0);
	while( ( status & classStatics_Sharpen_Drivers_Block_ATA.ATA_STATUS_DRQ )  == 0)
	{
		{
			status = Sharpen_Arch_PortIO_In8_1uint16_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_STATUS ) );
			if( ( status & classStatics_Sharpen_Drivers_Block_ATA.ATA_STATUS_DF )  > 0)
						Sharpen_Panic_DoPanic_1char__("Device fault!");
			if( ( status & classStatics_Sharpen_Drivers_Block_ATA.ATA_STATUS_ERR )  > 0)
						Sharpen_Panic_DoPanic_1char__("ERR IN ATA!!");
		}
	}
	;
}
int32_t Sharpen_Drivers_Block_ATA_ReadSector_4uint32_t_uint32_t_uint8_t_uint8_t__(uint32_t device_num, uint32_t lba, uint8_t size, uint8_t* buffer)
{
	if(device_num >= 4)
		return 0;
	struct struct_Sharpen_Drivers_Block_IDE_Device device = Sharpen_Drivers_Block_ATA_Devices_getter()[device_num];
	if( ! device.field_Exists)
		return 0;
	uint32_t port = device.field_BasePort;
	int32_t drive = device.field_Drive;
	int32_t cmd =  ( drive == classStatics_Sharpen_Drivers_Block_ATA.ATA_MASTER )  ? 0xE0 : 0xF0;
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_DRIVE ) , (uint8_t) ( cmd | (uint8_t) (  ( lba >> 24 )  & 0x0F )  ) );
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_FEATURE ) , classStatics_Sharpen_Drivers_Block_ATA.ATA_FEATURE_PIO);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_SECCNT ) , size);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_LBALO ) , (uint8_t)lba);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_LBAMID ) , (uint8_t) ( lba >> 8 ) );
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_LBAHI ) , (uint8_t) ( lba >> 16 ) );
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_CMD ) , classStatics_Sharpen_Drivers_Block_ATA.ATA_CMD_PIO_READ);
	Sharpen_Drivers_Block_ATA_poll_1uint32_t_(port);
	int32_t offset = 0;
	for(int32_t i = 0;i < size * 256;i = i + 1
	)
	{
		{
			uint16_t data = Sharpen_Arch_PortIO_In16_1uint16_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_DATA ) );
			buffer[offset + 0] = (uint8_t) ( data ) ;
			buffer[offset + 1] = (uint8_t) ( data >> 8 ) ;
			offset += 2;
		}
	}
	;
	return size * 512;
}
int32_t Sharpen_Drivers_Block_ATA_WriteSector_4uint32_t_uint32_t_uint8_t_uint8_t__(uint32_t device_num, uint32_t lba, uint8_t size, uint8_t* buffer)
{
	if(device_num >= 4)
		return 0;
	struct struct_Sharpen_Drivers_Block_IDE_Device device = Sharpen_Drivers_Block_ATA_Devices_getter()[device_num];
	if( ! device.field_Exists)
		return 0;
	uint32_t port = device.field_BasePort;
	int32_t drive = device.field_Drive;
	int32_t cmd =  ( drive == classStatics_Sharpen_Drivers_Block_ATA.ATA_MASTER )  ? 0xE0 : 0xF0;
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_DRIVE ) , (uint8_t) ( cmd | (uint8_t) (  ( lba >> 24 )  & 0x0F )  ) );
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_FEATURE ) , classStatics_Sharpen_Drivers_Block_ATA.ATA_FEATURE_PIO);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_SECCNT ) , size);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_LBALO ) , (uint8_t)lba);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_LBAMID ) , (uint8_t) ( lba >> 8 ) );
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_LBAHI ) , (uint8_t) ( lba >> 16 ) );
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_CMD ) , classStatics_Sharpen_Drivers_Block_ATA.ATA_CMD_PIO_WRITE);
	Sharpen_Drivers_Block_ATA_poll_1uint32_t_(port);
	Sharpen_Drivers_Block_ATA_wait400ns_1uint32_t_(port);
	for(int32_t i = 0;i < size * 256;i = i + 1
	)
	{
		{
			int32_t pos = i * 2;
			uint16_t shrt = (uint16_t) (  ( buffer[pos + 1] << 8 )  | buffer[pos] ) ;
			Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_DATA ) , shrt);
		}
	}
	;
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_CMD ) , classStatics_Sharpen_Drivers_Block_ATA.ATA_CMD_FLUSH);
	uint8_t status;
	do
	{
		status = Sharpen_Arch_PortIO_In8_1uint16_t_((uint16_t) ( port + classStatics_Sharpen_Drivers_Block_ATA.ATA_REG_STATUS ) );
	}
	while( ( status & classStatics_Sharpen_Drivers_Block_ATA.ATA_STATUS_BSY )  > 0);
	return size * 512;
}
void Sharpen_Drivers_Block_ATA_probe_0(void)
{
	int32_t num = 0;
	while(num < 4)
	{
		{
			uint16_t port;
			uint8_t channel;
			uint8_t drive;
			if(num <= 1){
				port = classStatics_Sharpen_Drivers_Block_ATA.ATA_PRIMARY_IO;
				channel = classStatics_Sharpen_Drivers_Block_ATA.ATA_PRIMARY;
			}
			else
			{
				port = classStatics_Sharpen_Drivers_Block_ATA.ATA_SECONDARY_IO;
				channel = classStatics_Sharpen_Drivers_Block_ATA.ATA_SECONDARY;
			}
			if( ( num % 2 )  != 0){
				drive = classStatics_Sharpen_Drivers_Block_ATA.ATA_SLAVE;
			}
			else
			{
				drive = classStatics_Sharpen_Drivers_Block_ATA.ATA_PRIMARY;
			}
			Sharpen_Drivers_Block_ATA_Devices_getter()[num] = structInit_Sharpen_Drivers_Block_IDE_Device();
			Sharpen_Drivers_Block_ATA_Devices_getter()[num].field_BasePort = port;
			Sharpen_Drivers_Block_ATA_Devices_getter()[num].field_Channel = channel;
			Sharpen_Drivers_Block_ATA_Devices_getter()[num].field_Drive = drive;
			uint8_t* result = Sharpen_Drivers_Block_ATA_identify_2uint8_t_uint8_t_(channel, drive);
			if(result == null){
				Sharpen_Drivers_Block_ATA_Devices_getter()[num].field_Exists = false;
				num = num + 1
				;
				continue;
			}
			Sharpen_Drivers_Block_ATA_Devices_getter()[num].field_Exists = true;
			int32_t pos = classStatics_Sharpen_Drivers_Block_ATA.ATA_IDENT_COMMANDSETS;
			Sharpen_Drivers_Block_ATA_Devices_getter()[num].field_CmdSet = (uint32_t) (  ( result[pos] << 24 )  |  ( result[pos + 1] << 16 )  |  ( result[pos + 2] << 8 )  | result[pos + 3] ) ;
			pos = classStatics_Sharpen_Drivers_Block_ATA.ATA_IDENT_DEVICETYPE;
			Sharpen_Drivers_Block_ATA_Devices_getter()[num].field_Type = (uint16_t) (  ( result[pos + 1] << 8 )  | result[pos] ) ;
			pos = classStatics_Sharpen_Drivers_Block_ATA.ATA_IDENT_CAPABILITIES;
			Sharpen_Drivers_Block_ATA_Devices_getter()[num].field_Capabilities = (uint16_t) (  ( result[pos + 1] << 8 )  | result[pos] ) ;
			pos = classStatics_Sharpen_Drivers_Block_ATA.ATA_IDENT_CYLINDERS;
			Sharpen_Drivers_Block_ATA_Devices_getter()[num].field_Cylinders = (uint16_t) (  ( result[pos + 1] << 8 )  | result[pos] ) ;
			pos = classStatics_Sharpen_Drivers_Block_ATA.ATA_IDENT_HEADS;
			Sharpen_Drivers_Block_ATA_Devices_getter()[num].field_Heads = (uint16_t) (  ( result[pos + 1] << 8 )  | result[pos] ) ;
			pos = classStatics_Sharpen_Drivers_Block_ATA.ATA_IDENT_SECTORSPT;
			Sharpen_Drivers_Block_ATA_Devices_getter()[num].field_Sectorspt = (uint16_t) (  ( result[pos + 1] << 8 )  | result[pos] ) ;
			pos = classStatics_Sharpen_Drivers_Block_ATA.ATA_IDENT_MAX_LBA;
			Sharpen_Drivers_Block_ATA_Devices_getter()[num].field_Size = (uint32_t) (  (  ( result[pos] << 24 )  |  ( result[pos + 1] << 16 )  |  ( result[pos + 2] << 8 )  | result[pos + 3] )  ) ;
			pos = classStatics_Sharpen_Drivers_Block_ATA.ATA_IDENT_MODEL;
			char* name = (char*)Sharpen_Heap_Alloc_1int32_t_(40 + 1);
			{
				void* source = &result[pos];
				{
					Sharpen_Memory_Memcpy_3void__void__int32_t_(name, source, 40);
				}
			}
			name[40] = '\0';
			Sharpen_Drivers_Block_ATA_Devices_getter()[num].field_Name = Sharpen_Utilities_Util_CharPtrToString_1char__(name);
			num = num + 1
			;
		}
	}
	;
}
void Sharpen_Drivers_Block_ATA_Init_0(void)
{
	Sharpen_Drivers_Block_ATA_probe_0();
	if(Sharpen_Drivers_Block_ATA_Devices_getter()[0].field_Exists){
		struct class_Sharpen_FileSystem_Device* dev = classInit_Sharpen_FileSystem_Device();
		dev->field_Name = "HDD0";
		dev->field_node->field_Cookie = 0;
		dev->field_node = classInit_Sharpen_FileSystem_Node();
		dev->field_node->field_Read = Sharpen_Drivers_Block_ATA_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__;
		dev->field_node->field_Write = Sharpen_Drivers_Block_ATA_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__;
		Sharpen_FileSystem_DevFS_RegisterDevice_1struct_class_Sharpen_FileSystem_Device__(dev);
	}
}
uint32_t Sharpen_Drivers_Block_ATA_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer)
{
	if(size % 512 != 0)
		return 0;
	uint32_t inSize = size / 512;
	return (uint32_t)Sharpen_Drivers_Block_ATA_WriteSector_4uint32_t_uint32_t_uint8_t_uint8_t__(node->field_Cookie, offset, (uint8_t)inSize, buffer);
}
uint32_t Sharpen_Drivers_Block_ATA_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer)
{
	if(size % 512 != 0)
		return 0;
	uint32_t inSize = size / 512;
	return (uint32_t)Sharpen_Drivers_Block_ATA_ReadSector_4uint32_t_uint32_t_uint8_t_uint8_t__(node->field_Cookie, offset, (uint8_t)inSize, buffer);
}
inline struct struct_Sharpen_Drivers_Block_IDE_Device structInit_Sharpen_Drivers_Block_IDE_Device(void)
{
	struct struct_Sharpen_Drivers_Block_IDE_Device object;
	return object;
}
struct class_Sharpen_Drivers_Char_SerialPort* classInit_Sharpen_Drivers_Char_SerialPort(void)
{
	struct class_Sharpen_Drivers_Char_SerialPort* object = calloc(1, sizeof(struct class_Sharpen_Drivers_Char_SerialPort));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Drivers_Char_SerialPort;
	return object;
}

inline void classCctor_Sharpen_Drivers_Char_SerialPort(void)
{
	classStatics_Sharpen_Drivers_Char_SerialPort.comports = calloc((4), sizeof(struct struct_Sharpen_Drivers_Char_SerialPortComport));
}
void Sharpen_Drivers_Char_SerialPort_initDevice_1int32_t_(int32_t num)
{
	if(Sharpen_Drivers_Char_SerialPortComport_Address_getter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[num]) == 0)
		return;
	uint16_t port = Sharpen_Drivers_Char_SerialPortComport_Address_getter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[num]);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + 1 ) , 0x00);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + 3 ) , 0x80);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + 0 ) , 0x01);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + 1 ) , 0x00);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + 3 ) , 0x03);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + 2 ) , 0xC7);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + 4 ) , 0x0B);
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_((uint16_t) ( port + 1 ) , 0x01);
	Sharpen_Drivers_Char_SerialPortComport_Buffer_setter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[num], Sharpen_Collections_Fifo_Fifo_2class_int32_t_(classInit_Sharpen_Collections_Fifo(), 256));
	struct class_Sharpen_FileSystem_Device* dev = classInit_Sharpen_FileSystem_Device();
	dev->field_Name = Sharpen_Drivers_Char_SerialPortComport_Name_getter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[num]);
	dev->field_node = classInit_Sharpen_FileSystem_Node();
	dev->field_node->field_Cookie = (uint32_t)num;
	dev->field_node->field_Flags = enum_Sharpen_FileSystem_NodeFlags_FILE;
	dev->field_node->field_Write = Sharpen_Drivers_Char_SerialPort_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__;
	dev->field_node->field_Read = Sharpen_Drivers_Char_SerialPort_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__;
	Sharpen_FileSystem_DevFS_RegisterDevice_1struct_class_Sharpen_FileSystem_Device__(dev);
}
uint32_t Sharpen_Drivers_Char_SerialPort_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer)
{
	uint32_t i = 0;
	if(Sharpen_Drivers_Char_SerialPortComport_Address_getter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[node->field_Cookie]) == 0)
		return 0;
	while(i < size)
	{
		{
			Sharpen_Drivers_Char_SerialPort_write_2uint8_t_uint16_t_(buffer[i], Sharpen_Drivers_Char_SerialPortComport_Address_getter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[node->field_Cookie]));
			i = i + 1
			;
		}
	}
	;
	return i;
}
uint32_t Sharpen_Drivers_Char_SerialPort_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer)
{
	if(Sharpen_Drivers_Char_SerialPortComport_Address_getter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[node->field_Cookie]) == 0)
		return 0;
	return Sharpen_Collections_Fifo_Read_3class_uint8_t__uint32_t_(Sharpen_Drivers_Char_SerialPortComport_Buffer_getter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[node->field_Cookie]), buffer, (uint16_t)size);
}
int32_t Sharpen_Drivers_Char_SerialPort_isTransmitEmpty_1int32_t_(int32_t port)
{
	return  ( Sharpen_Arch_PortIO_In8_1uint16_t_((uint16_t) ( port + 0x05 ) ) & 0x20 )  == 0;
}
int32_t Sharpen_Drivers_Char_SerialPort_hasReceived_1int32_t_(int32_t port)
{
	return  ( Sharpen_Arch_PortIO_In8_1uint16_t_((uint16_t) ( port + 0x05 ) ) & 1 )  > 0;
}
uint8_t Sharpen_Drivers_Char_SerialPort_read_1uint16_t_(uint16_t port)
{
	while( ! Sharpen_Drivers_Char_SerialPort_hasReceived_1int32_t_(port))
	{
		Sharpen_Arch_CPU_HLT_0();
	}
	;
	return Sharpen_Arch_PortIO_In8_1uint16_t_(port);
}
void Sharpen_Drivers_Char_SerialPort_write_2uint8_t_uint16_t_(uint8_t d, uint16_t port)
{
	while(Sharpen_Drivers_Char_SerialPort_isTransmitEmpty_1int32_t_(port))
	{
		Sharpen_Arch_CPU_HLT_0();
	}
	;
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(port, d);
}
void Sharpen_Drivers_Char_SerialPort_readBda_0(void)
{
	uint16_t* bda = (uint16_t*)0x00000400;
	Sharpen_Drivers_Char_SerialPortComport_Address_setter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[0], *(bda + 0));
	Sharpen_Drivers_Char_SerialPortComport_Address_setter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[1], *(bda + 1));
	Sharpen_Drivers_Char_SerialPortComport_Address_setter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[2], *(bda + 2));
	Sharpen_Drivers_Char_SerialPortComport_Address_setter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[3], *(bda + 3));
}
void Sharpen_Drivers_Char_SerialPort_Handler13_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr)
{
	struct struct_Sharpen_Drivers_Char_SerialPortComport port;
	if(Sharpen_Drivers_Char_SerialPortComport_Address_getter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[0]) != 0 && Sharpen_Drivers_Char_SerialPort_hasReceived_1int32_t_(Sharpen_Drivers_Char_SerialPortComport_Address_getter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[0])))
		port = classStatics_Sharpen_Drivers_Char_SerialPort.comports[0];
	else
	{
		port = classStatics_Sharpen_Drivers_Char_SerialPort.comports[2];
	}
	if(Sharpen_Drivers_Char_SerialPortComport_Address_getter(&port) == 0)
		return;
	while(Sharpen_Drivers_Char_SerialPort_hasReceived_1int32_t_(Sharpen_Drivers_Char_SerialPortComport_Address_getter(&port)))
	{
		Sharpen_Collections_Fifo_WriteByte_2class_uint8_t_(Sharpen_Drivers_Char_SerialPortComport_Buffer_getter(&port), Sharpen_Drivers_Char_SerialPort_read_1uint16_t_(Sharpen_Drivers_Char_SerialPortComport_Address_getter(&port)));
	}
	;
}
void Sharpen_Drivers_Char_SerialPort_Handler24_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr)
{
	struct struct_Sharpen_Drivers_Char_SerialPortComport port;
	if(Sharpen_Drivers_Char_SerialPortComport_Address_getter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[1]) != 0 && Sharpen_Drivers_Char_SerialPort_hasReceived_1int32_t_(Sharpen_Drivers_Char_SerialPortComport_Address_getter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[1])))
		port = classStatics_Sharpen_Drivers_Char_SerialPort.comports[1];
	else
	{
		port = classStatics_Sharpen_Drivers_Char_SerialPort.comports[3];
	}
	if(Sharpen_Drivers_Char_SerialPortComport_Address_getter(&port) == 0)
		return;
	while(Sharpen_Drivers_Char_SerialPort_hasReceived_1int32_t_(Sharpen_Drivers_Char_SerialPortComport_Address_getter(&port)))
	{
		Sharpen_Collections_Fifo_WriteByte_2class_uint8_t_(Sharpen_Drivers_Char_SerialPortComport_Buffer_getter(&port), Sharpen_Drivers_Char_SerialPort_read_1uint16_t_(Sharpen_Drivers_Char_SerialPortComport_Address_getter(&port)));
	}
	;
}
void Sharpen_Drivers_Char_SerialPort_Init_0(void)
{
	classStatics_Sharpen_Drivers_Char_SerialPort.comports[0] = structInit_Sharpen_Drivers_Char_SerialPortComport();
	Sharpen_Drivers_Char_SerialPortComport_Name_setter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[0], "COM1");
	classStatics_Sharpen_Drivers_Char_SerialPort.comports[1] = structInit_Sharpen_Drivers_Char_SerialPortComport();
	Sharpen_Drivers_Char_SerialPortComport_Name_setter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[1], "COM2");
	classStatics_Sharpen_Drivers_Char_SerialPort.comports[2] = structInit_Sharpen_Drivers_Char_SerialPortComport();
	Sharpen_Drivers_Char_SerialPortComport_Name_setter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[2], "COM3");
	classStatics_Sharpen_Drivers_Char_SerialPort.comports[3] = structInit_Sharpen_Drivers_Char_SerialPortComport();
	Sharpen_Drivers_Char_SerialPortComport_Name_setter(&classStatics_Sharpen_Drivers_Char_SerialPort.comports[3], "COM4");
	Sharpen_Drivers_Char_SerialPort_readBda_0();
	Sharpen_Drivers_Char_SerialPort_initDevice_1int32_t_(0);
	Sharpen_Drivers_Char_SerialPort_initDevice_1int32_t_(1);
	Sharpen_Drivers_Char_SerialPort_initDevice_1int32_t_(2);
	Sharpen_Drivers_Char_SerialPort_initDevice_1int32_t_(3);
	Sharpen_Arch_IRQ_SetHandler_2int32_t_delegate_Sharpen_Arch_IRQ_IRQHandler_(3, Sharpen_Drivers_Char_SerialPort_Handler24_1struct_struct_Sharpen_Arch_Regs__);
	Sharpen_Arch_IRQ_SetHandler_2int32_t_delegate_Sharpen_Arch_IRQ_IRQHandler_(4, Sharpen_Drivers_Char_SerialPort_Handler13_1struct_struct_Sharpen_Arch_Regs__);
}
struct class_Sharpen_Drivers_Char_Keyboard* classInit_Sharpen_Drivers_Char_Keyboard(void)
{
	struct class_Sharpen_Drivers_Char_Keyboard* object = calloc(1, sizeof(struct class_Sharpen_Drivers_Char_Keyboard));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Drivers_Char_Keyboard;
	return object;
}

inline void classCctor_Sharpen_Drivers_Char_Keyboard(void)
{
	classStatics_Sharpen_Drivers_Char_Keyboard.m_fifo = Sharpen_Collections_Fifo_Fifo_2class_int32_t_(classInit_Sharpen_Collections_Fifo(), 250);
}
int32_t Sharpen_Drivers_Char_Keyboard_Capslock_getter(void)
{
	return classStatics_Sharpen_Drivers_Char_Keyboard.m_capslock > 0;
}
uint8_t Sharpen_Drivers_Char_Keyboard_Shift_getter(void)
{
	return classStatics_Sharpen_Drivers_Char_Keyboard.m_shift;
}
uint8_t Sharpen_Drivers_Char_Keyboard_Leds_getter(void)
{
	return classStatics_Sharpen_Drivers_Char_Keyboard.m_leds;
}
uint8_t Sharpen_Drivers_Char_Keyboard_Leds_setter(uint8_t value)
{
	classStatics_Sharpen_Drivers_Char_Keyboard.m_leds = value;
	Sharpen_Drivers_Char_Keyboard_updateLED_0();
	return value;
}
char Sharpen_Drivers_Char_Keyboard_transformKey_1uint8_t_(uint8_t scancode)
{
	char outputChar;
	int32_t codeFixed = scancode;
	outputChar =  ( classStatics_Sharpen_Drivers_Char_Keyboard.m_shift > 0 ? classStatics_Sharpen_Drivers_Char_KeyboardMap.Shifted[codeFixed] : classStatics_Sharpen_Drivers_Char_KeyboardMap.Normal[codeFixed] ) ;
	if(classStatics_Sharpen_Drivers_Char_Keyboard.m_capslock > 0){
		if(outputChar >= 'a' && outputChar <= 'z'){
			outputChar = (char) ( 'A' + outputChar - 'a' ) ;
		}
		else if(outputChar >= 'A' && outputChar <= 'Z'){
			outputChar = (char) (  ( outputChar - 'A' )  + 'a' ) ;
		}
	}
	return outputChar;
}
void Sharpen_Drivers_Char_Keyboard_updateLED_0(void)
{
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(0x60, 0xED);
	while( ( Sharpen_Arch_PortIO_In8_1uint16_t_(0x64) & 2 )  > 0)
	{
	}
	;
	Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(0x60, classStatics_Sharpen_Drivers_Char_Keyboard.m_leds);
	while( ( Sharpen_Arch_PortIO_In8_1uint16_t_(0x64) & 2 )  > 0)
	{
	}
	;
}
void Sharpen_Drivers_Char_Keyboard_Init_0(void)
{
	Sharpen_Arch_IRQ_SetHandler_2int32_t_delegate_Sharpen_Arch_IRQ_IRQHandler_(1, Sharpen_Drivers_Char_Keyboard_handler_1struct_struct_Sharpen_Arch_Regs__);
	struct class_Sharpen_FileSystem_Device* device = classInit_Sharpen_FileSystem_Device();
	device->field_Name = "keyboard";
	device->field_node = classInit_Sharpen_FileSystem_Node();
	device->field_node->field_Read = Sharpen_Drivers_Char_Keyboard_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__;
	Sharpen_FileSystem_DevFS_RegisterDevice_1struct_class_Sharpen_FileSystem_Device__(device);
}
uint32_t Sharpen_Drivers_Char_Keyboard_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer)
{
	return Sharpen_Collections_Fifo_Read_3class_uint8_t__uint32_t_(classStatics_Sharpen_Drivers_Char_Keyboard.m_fifo, buffer, (uint16_t)size);
}
char Sharpen_Drivers_Char_Keyboard_Getch_0(void)
{
	classStatics_Sharpen_Drivers_Char_Keyboard.readingchar = 1;
	while(classStatics_Sharpen_Drivers_Char_Keyboard.readingchar != 0)
	{
		Sharpen_Arch_CPU_HLT_0();
	}
	;
	return classStatics_Sharpen_Drivers_Char_Keyboard.readchar;
}
void Sharpen_Drivers_Char_Keyboard_handler_1struct_struct_Sharpen_Arch_Regs__(struct struct_Sharpen_Arch_Regs* regsPtr)
{
	uint8_t scancode = Sharpen_Arch_PortIO_In8_1uint16_t_(0x60);
	if( ( scancode & 0x80 )  > 0){
		if(scancode == 0xAA)
				classStatics_Sharpen_Drivers_Char_Keyboard.m_shift &= 0x02;
		else if(scancode == 0xB6)
				classStatics_Sharpen_Drivers_Char_Keyboard.m_shift &= 0x01;
	}
	else
	{
		if(scancode == 0x3A){
			if(classStatics_Sharpen_Drivers_Char_Keyboard.m_capslock > 0){
				classStatics_Sharpen_Drivers_Char_Keyboard.m_capslock = 0;
				classStatics_Sharpen_Drivers_Char_Keyboard.m_leds = 0;
			}
			else
			{
				classStatics_Sharpen_Drivers_Char_Keyboard.m_capslock = 1;
				classStatics_Sharpen_Drivers_Char_Keyboard.m_leds = 4;
			}
			Sharpen_Drivers_Char_Keyboard_updateLED_0();
		}
		else if(scancode == 0x2A)
				classStatics_Sharpen_Drivers_Char_Keyboard.m_shift |= 0x01;
		else if(scancode == 0x36)
				classStatics_Sharpen_Drivers_Char_Keyboard.m_shift |= 0x02;
		else
		{
			classStatics_Sharpen_Drivers_Char_Keyboard.readchar = Sharpen_Drivers_Char_Keyboard_transformKey_1uint8_t_(scancode);
			Sharpen_Collections_Fifo_WriteByte_2class_uint8_t_(classStatics_Sharpen_Drivers_Char_Keyboard.m_fifo, (uint8_t)classStatics_Sharpen_Drivers_Char_Keyboard.readchar);
			classStatics_Sharpen_Drivers_Char_Keyboard.readingchar = 0;
		}
	}
}
struct class_Sharpen_Drivers_Char_KeyboardMap* classInit_Sharpen_Drivers_Char_KeyboardMap(void)
{
	struct class_Sharpen_Drivers_Char_KeyboardMap* object = calloc(1, sizeof(struct class_Sharpen_Drivers_Char_KeyboardMap));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Drivers_Char_KeyboardMap;
	return object;
}

inline void classCctor_Sharpen_Drivers_Char_KeyboardMap(void)
{
}
inline struct struct_Sharpen_Drivers_Char_SerialPortComport structInit_Sharpen_Drivers_Char_SerialPortComport(void)
{
	struct struct_Sharpen_Drivers_Char_SerialPortComport object;
	return object;
}
char* Sharpen_Drivers_Char_SerialPortComport_Name_getter(struct struct_Sharpen_Drivers_Char_SerialPortComport* obj)
{
	return obj->prop_Name;
}
char* Sharpen_Drivers_Char_SerialPortComport_Name_setter(struct struct_Sharpen_Drivers_Char_SerialPortComport* obj, char* value)
{
	obj->prop_Name = value;
	return value;
}
uint16_t Sharpen_Drivers_Char_SerialPortComport_Address_getter(struct struct_Sharpen_Drivers_Char_SerialPortComport* obj)
{
	return obj->prop_Address;
}
uint16_t Sharpen_Drivers_Char_SerialPortComport_Address_setter(struct struct_Sharpen_Drivers_Char_SerialPortComport* obj, uint16_t value)
{
	obj->prop_Address = value;
	return value;
}
struct class_Sharpen_Collections_Fifo* Sharpen_Drivers_Char_SerialPortComport_Buffer_getter(struct struct_Sharpen_Drivers_Char_SerialPortComport* obj)
{
	return obj->prop_Buffer;
}
struct class_Sharpen_Collections_Fifo* Sharpen_Drivers_Char_SerialPortComport_Buffer_setter(struct struct_Sharpen_Drivers_Char_SerialPortComport* obj, struct class_Sharpen_Collections_Fifo* value)
{
	obj->prop_Buffer = value;
	return value;
}
struct class_Sharpen_FileSystem_MountPoint* classInit_Sharpen_FileSystem_MountPoint(void)
{
	struct class_Sharpen_FileSystem_MountPoint* object = calloc(1, sizeof(struct class_Sharpen_FileSystem_MountPoint));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_FileSystem_MountPoint;
	return object;
}

struct class_Sharpen_FileSystem_Node* classInit_Sharpen_FileSystem_Node(void)
{
	struct class_Sharpen_FileSystem_Node* object = calloc(1, sizeof(struct class_Sharpen_FileSystem_Node));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_FileSystem_Node;
	return object;
}

struct class_Sharpen_FileSystem_VFS* classInit_Sharpen_FileSystem_VFS(void)
{
	struct class_Sharpen_FileSystem_VFS* object = calloc(1, sizeof(struct class_Sharpen_FileSystem_VFS));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_FileSystem_VFS;
	return object;
}

inline void classCctor_Sharpen_FileSystem_VFS(void)
{
	classStatics_Sharpen_FileSystem_VFS.m_dictionary = classInit_Sharpen_Collections_Dictionary();
}
void Sharpen_FileSystem_VFS_Init_0(void)
{
	struct class_Sharpen_FileSystem_MountPoint* mountPoint = classInit_Sharpen_FileSystem_MountPoint();
	mountPoint->field_Name = "mounts";
	mountPoint->field_Node = classInit_Sharpen_FileSystem_Node();
	mountPoint->field_Node->field_ReadDir = Sharpen_FileSystem_VFS_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_;
	Sharpen_FileSystem_VFS_AddMountPoint_1struct_class_Sharpen_FileSystem_MountPoint__(mountPoint);
}
struct struct_Sharpen_FileSystem_DirEntry* Sharpen_FileSystem_VFS_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_(struct class_Sharpen_FileSystem_Node* node, uint32_t index)
{
	if(index >= Sharpen_Collections_Dictionary_Count_1class_(classStatics_Sharpen_FileSystem_VFS.m_dictionary))
		return null;
	struct class_Sharpen_FileSystem_MountPoint* dev = (struct class_Sharpen_FileSystem_MountPoint*)Sharpen_Collections_Dictionary_GetAt_2class_int32_t_(classStatics_Sharpen_FileSystem_VFS.m_dictionary, (int32_t)index);
	if(dev == null)
		return null;
	struct struct_Sharpen_FileSystem_DirEntry* entry = (struct struct_Sharpen_FileSystem_DirEntry*)Sharpen_Heap_Alloc_1int32_t_(sizeof(struct struct_Sharpen_FileSystem_DirEntry));
	int32_t i = 0;
	for(;dev->field_Name[i] != '\0';i = i + 1
	)
	{
		entry->field_Name[i] = dev->field_Name[i];
	}
	;
	entry->field_Name[i] = '\0';
	return entry;
}
int64_t Sharpen_FileSystem_VFS_generateHash_1char__(char* inVal)
{
	int64_t hash = 0;
	for(int32_t i = 0;i <= 8;i = i + 1
	)
	{
		{
			char c = inVal[i];
			if(c == '\0')
						break;
			hash <<= 3;
			hash |= c;
		}
	}
	;
	return hash;
}
void Sharpen_FileSystem_VFS_AddMountPoint_1struct_class_Sharpen_FileSystem_MountPoint__(struct class_Sharpen_FileSystem_MountPoint* mountPoint)
{
	int64_t key = Sharpen_FileSystem_VFS_generateHash_1char__(mountPoint->field_Name);
	Sharpen_Collections_Dictionary_Add_3class_int64_t_void__(classStatics_Sharpen_FileSystem_VFS.m_dictionary, key, mountPoint);
}
struct class_Sharpen_FileSystem_MountPoint* Sharpen_FileSystem_VFS_FindMountByName_1char__(char* name)
{
	int64_t key = Sharpen_FileSystem_VFS_generateHash_1char__(name);
	return (struct class_Sharpen_FileSystem_MountPoint*)Sharpen_Collections_Dictionary_GetByKey_2class_int64_t_(classStatics_Sharpen_FileSystem_VFS.m_dictionary, key);
}
struct class_Sharpen_FileSystem_Node* Sharpen_FileSystem_VFS_GetByPath_1char__(char* path)
{
	int32_t index = Sharpen_Utilities_String_IndexOf_2char__char__(path, "://");
	if(index ==  - 1)
		return null;
	if(path[Sharpen_Utilities_String_Length_1char__(path) - 1] != '/')
		path = Sharpen_Utilities_String_Merge_2char__char__(path, "/");
	char* deviceName = Sharpen_Utilities_String_SubString_3char__int32_t_int32_t_(path, 0, index);
	char* AfterDeviceName = Sharpen_Utilities_String_SubString_3char__int32_t_int32_t_(path, index + 3, Sharpen_Utilities_String_Length_1char__(path) -  ( index + 3 ) );
	int32_t parts = Sharpen_Utilities_String_Count_2char__char_(AfterDeviceName, '/');
	struct class_Sharpen_FileSystem_MountPoint* mp = Sharpen_FileSystem_VFS_FindMountByName_1char__(deviceName);
	if(mp == null)
		return null;
	struct class_Sharpen_FileSystem_Node* lastNode = mp->field_Node;
	char* nodeName = AfterDeviceName;
	char* afterNodeName = AfterDeviceName;
	int32_t i = 0;
	while(parts > 0)
	{
		{
			index = Sharpen_Utilities_String_IndexOf_2char__char__(afterNodeName, "/");
			nodeName = Sharpen_Utilities_String_SubString_3char__int32_t_int32_t_(afterNodeName, 0, index);
			if(parts > 1)
						afterNodeName = Sharpen_Utilities_String_SubString_3char__int32_t_int32_t_(afterNodeName, index + 1, Sharpen_Utilities_String_Length_1char__(path) - 1);
			lastNode = lastNode->field_FindDir(lastNode, nodeName);
			if(lastNode == null)
						return null;
			parts = parts - 1
			;
			i = i + 1
			;
		}
	}
	;
	return lastNode;
}
void Sharpen_FileSystem_VFS_Open_2struct_class_Sharpen_FileSystem_Node__int32_t_(struct class_Sharpen_FileSystem_Node* node, int32_t fileMode)
{
	node->field_FileMode = fileMode;
	if(node->field_Open == null)
		return;
	node->field_Open(node);
}
void Sharpen_FileSystem_VFS_Close_1struct_class_Sharpen_FileSystem_Node__(struct class_Sharpen_FileSystem_Node* node)
{
	node->field_FileMode = enum_Sharpen_FileSystem_FileMode_O_NONE;
	if(node->field_Close == null)
		return;
	node->field_Close(node);
}
uint32_t Sharpen_FileSystem_VFS_Read_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer)
{
	if(node->field_Read == null)
		return 0;
	if(node->field_FileMode != enum_Sharpen_FileSystem_FileMode_O_RDWR && node->field_FileMode != enum_Sharpen_FileSystem_FileMode_O_RDONLY)
		return 0;
	return node->field_Read(node, offset, size, buffer);
}
uint32_t Sharpen_FileSystem_VFS_Write_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer)
{
	if(node->field_Write == null)
		return 0;
	if(node->field_FileMode != enum_Sharpen_FileSystem_FileMode_O_RDWR && node->field_FileMode != enum_Sharpen_FileSystem_FileMode_O_WRONLY)
		return 0;
	return node->field_Write(node, offset, size, buffer);
}
struct class_Sharpen_FileSystem_Node* Sharpen_FileSystem_VFS_FindDir_2struct_class_Sharpen_FileSystem_Node__char__(struct class_Sharpen_FileSystem_Node* node, char* name)
{
	if(node->field_FindDir == null)
		return null;
	return node->field_FindDir(node, name);
}
struct struct_Sharpen_FileSystem_DirEntry* Sharpen_FileSystem_VFS_ReadDir_2struct_class_Sharpen_FileSystem_Node__uint32_t_(struct class_Sharpen_FileSystem_Node* node, uint32_t index)
{
	if(node->field_ReadDir == null)
		return null;
	return node->field_ReadDir(node, index);
}
struct class_Sharpen_Heap* classInit_Sharpen_Heap(void)
{
	struct class_Sharpen_Heap* object = calloc(1, sizeof(struct class_Sharpen_Heap));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Heap;
	return object;
}

inline void classCctor_Sharpen_Heap(void)
{
}
inline struct struct_Sharpen_Heap_Block structInit_Sharpen_Heap_Block(void)
{
	struct struct_Sharpen_Heap_Block object;
	return object;
}
inline struct struct_Sharpen_Heap_BlockDescriptor structInit_Sharpen_Heap_BlockDescriptor(void)
{
	struct struct_Sharpen_Heap_BlockDescriptor object;
	return object;
}
void* Sharpen_Heap_CurrentEnd_getter(void)
{
	return classStatics_Sharpen_Heap.prop_CurrentEnd;
}
void* Sharpen_Heap_CurrentEnd_setter(void* value)
{
	classStatics_Sharpen_Heap.prop_CurrentEnd = value;
	return value;
}
void Sharpen_Heap_Init_1void__(void* start)
{
	Sharpen_Console_Write_1char__("[HEAP] Temporary start at ");
	Sharpen_Console_WriteHex_1int64_t_((int32_t)start);
	Sharpen_Console_PutChar_1char_('\n');
	Sharpen_Heap_CurrentEnd_setter(start);
}
int32_t Sharpen_Heap_getRequiredPageCount_1int32_t_(int32_t size)
{
	int32_t required = size / 0x1000;
	return  ( required < classStatics_Sharpen_Heap.MINIMALPAGES )  ? classStatics_Sharpen_Heap.MINIMALPAGES : required;
}
struct struct_Sharpen_Heap_BlockDescriptor* Sharpen_Heap_createBlockDescriptor_1int32_t_(int32_t size)
{
	size = Sharpen_Heap_getRequiredPageCount_1int32_t_(size) * 0x1000;
	struct struct_Sharpen_Heap_BlockDescriptor* descriptor = (struct struct_Sharpen_Heap_BlockDescriptor*)Sharpen_Arch_Paging_AllocatePhysical_1int32_t_(size);
	if(descriptor == null)
		return null;
	struct struct_Sharpen_Heap_Block* first = (struct struct_Sharpen_Heap_Block*) ( (int32_t)descriptor + sizeof(struct struct_Sharpen_Heap_BlockDescriptor) ) ;
	first->field_Next = null;
	first->field_Size = size - sizeof(struct struct_Sharpen_Heap_BlockDescriptor);
	first->field_Used = false;
	first->field_Descriptor = descriptor;
	descriptor->field_FreeSpace = size;
	descriptor->field_First = first;
	descriptor->field_Next = null;
	return descriptor;
}
struct struct_Sharpen_Heap_BlockDescriptor* Sharpen_Heap_getSufficientDescriptor_1int32_t_(int32_t size)
{
	struct struct_Sharpen_Heap_BlockDescriptor* descriptor = classStatics_Sharpen_Heap.firstDescriptor;
	while(true)
	{
		{
			if(descriptor->field_FreeSpace >= size)
						return descriptor;
			if(descriptor->field_Next == null)
						break;
			descriptor = descriptor->field_Next;
		}
	}
	;
	struct struct_Sharpen_Heap_BlockDescriptor* newDescriptor = Sharpen_Heap_createBlockDescriptor_1int32_t_(size);
	descriptor->field_Next = newDescriptor;
	return newDescriptor;
}
void Sharpen_Heap_SetupRealHeap_0(void)
{
	uint32_t address = (uint32_t)Sharpen_Heap_CurrentEnd_getter();
	if(address % 0x1000 != 0){
		address &= 0xFFFFF000;
		address += 0x1000;
	}
	Sharpen_Heap_CurrentEnd_setter((void*)address);
	classStatics_Sharpen_Heap.firstDescriptor = Sharpen_Heap_createBlockDescriptor_1int32_t_(classStatics_Sharpen_Heap.MINIMALPAGES * 0x1000);
	classStatics_Sharpen_Heap.m_realHeap = true;
	Sharpen_Console_Write_1char__("[HEAP] Currently at ");
	Sharpen_Console_WriteHex_1int64_t_(address);
	Sharpen_Console_PutChar_1char_('\n');
}
void* Sharpen_Heap_AlignedAlloc_2int32_t_int32_t_(int32_t alignment, int32_t size)
{
	if(
		classStatics_Sharpen_Heap.m_realHeap){
		size += sizeof(struct struct_Sharpen_Heap_Block);
		int32_t alignedSize = size * 2;
		if(alignedSize % alignment != 0){
			alignedSize = size -  ( size % alignment ) ;
			alignedSize += alignment;
		}
		struct struct_Sharpen_Heap_BlockDescriptor* descriptor = Sharpen_Heap_getSufficientDescriptor_1int32_t_(alignedSize);
		struct struct_Sharpen_Heap_Block* currentBlock = descriptor->field_First;
		struct struct_Sharpen_Heap_Block* previousBlock = null;
		while(true)
		{
			{
				if(currentBlock->field_Used || currentBlock->field_Size < size)
								goto nextBlock;
				int32_t currentData = (int32_t)currentBlock + sizeof(struct struct_Sharpen_Heap_Block);
				int32_t remainder = currentData % alignment;
				if(remainder != 0){
					int32_t gapSize = alignment - remainder;
					int32_t newSize = currentBlock->field_Size - gapSize;
					if(newSize < size)
										goto nextBlock;
					struct struct_Sharpen_Heap_Block* newNext = currentBlock->field_Next;
					int32_t newUsed = currentBlock->field_Used;
					currentBlock = (struct struct_Sharpen_Heap_Block*) ( (int32_t)currentBlock + gapSize ) ;
					currentBlock->field_Used = newUsed;
					currentBlock->field_Prev = previousBlock;
					currentBlock->field_Next = newNext;
					currentBlock->field_Size = newSize;
					currentBlock->field_Descriptor = descriptor;
					if(previousBlock != null){
						previousBlock->field_Next = currentBlock;
						previousBlock->field_Size += gapSize;
						if(currentBlock->field_Used)
												descriptor->field_FreeSpace -= gapSize;
					}
					else if(gapSize >= sizeof(struct struct_Sharpen_Heap_Block)){
						struct struct_Sharpen_Heap_Block* first = descriptor->field_First;
						descriptor->field_FreeSpace -= gapSize;
						first->field_Used = false;
						first->field_Prev = null;
						first->field_Next = currentBlock;
						first->field_Size = gapSize;
						first->field_Descriptor = descriptor;
						currentBlock->field_Prev = first;
					}
				}
				int32_t leftover = currentBlock->field_Size - size;
				currentBlock->field_Used = true;
				currentBlock->field_Descriptor = descriptor;
				currentBlock->field_Prev = previousBlock;
				descriptor->field_FreeSpace -= size;
				if(leftover > sizeof(struct struct_Sharpen_Heap_Block) + 4){
					struct struct_Sharpen_Heap_Block* afterBlock = (struct struct_Sharpen_Heap_Block*) ( (int32_t)currentBlock + size ) ;
					afterBlock->field_Size = leftover;
					afterBlock->field_Used = false;
					afterBlock->field_Next = currentBlock->field_Next;
					afterBlock->field_Prev = currentBlock;
					afterBlock->field_Descriptor = descriptor;
					if(currentBlock->field_Next != null)
										currentBlock->field_Next->field_Prev = afterBlock;
					currentBlock->field_Next = afterBlock;
					currentBlock->field_Size = size;
				}
				return (void*) ( (int32_t)currentBlock + sizeof(struct struct_Sharpen_Heap_Block) ) ;
				nextBlock:
				{
					{
						previousBlock = currentBlock;
						currentBlock = currentBlock->field_Next;
						if(currentBlock == null)
												return null;
					}
				}
				;
			}
		}
		;
	}
	else
	{
		if(alignment == 0x1000)
				return Sharpen_Heap_KAlloc_2int32_t_int32_t_(size, true);
		else
		{
			return Sharpen_Heap_KAlloc_2int32_t_int32_t_(size, false);
		}
	}
}
void* Sharpen_Heap_Alloc_1int32_t_(int32_t size)
{
	return Sharpen_Heap_AlignedAlloc_2int32_t_int32_t_(4, size);
}
struct struct_Sharpen_Heap_Block* Sharpen_Heap_getBlockFromPtr_1void__(void* ptr)
{
	struct struct_Sharpen_Heap_Block* block = (struct struct_Sharpen_Heap_Block*) ( (int32_t)ptr - sizeof(struct struct_Sharpen_Heap_Block) ) ;
	return block;
}
void* Sharpen_Heap_Expand_2void__int32_t_(void* ptr, int32_t newSize)
{
	struct struct_Sharpen_Heap_Block* block = Sharpen_Heap_getBlockFromPtr_1void__(ptr);
	void* newPtr = Sharpen_Heap_AlignedAlloc_2int32_t_int32_t_(4, newSize);
	Sharpen_Memory_Memcpy_3void__void__int32_t_(newPtr, ptr, block->field_Size);
	Sharpen_Heap_Free_1void__(ptr);
	return newPtr;
}
void Sharpen_Heap_Free_1void__(void* ptr)
{
	struct struct_Sharpen_Heap_Block* block = Sharpen_Heap_getBlockFromPtr_1void__(ptr);
	block->field_Used = false;
	block->field_Descriptor->field_FreeSpace += block->field_Size;
	if(block->field_Next != null &&  ! block->field_Next->field_Used){
		struct struct_Sharpen_Heap_Block* next = block->field_Next;
		block->field_Size += next->field_Size;
		block->field_Next = next->field_Next;
		if(next->field_Next != null)
				next->field_Next->field_Prev = block;
	}
	if(block->field_Prev != null &&  ! block->field_Prev->field_Used){
		struct struct_Sharpen_Heap_Block* prev = block->field_Prev;
		prev->field_Size += block->field_Size;
		prev->field_Next = block->field_Next;
		if(block->field_Next != null)
				block->field_Next->field_Prev = prev;
	}
}
void Sharpen_Heap_dumpBlock_1struct_struct_Sharpen_Heap_Block__(struct struct_Sharpen_Heap_Block* currentBlock)
{
	Sharpen_Console_Write_1char__("Block: size=");
	Sharpen_Console_WriteHex_1int64_t_(currentBlock->field_Size);
	Sharpen_Console_Write_1char__(" used:");
	Sharpen_Console_Write_1char__( ( currentBlock->field_Used ? "yes" : "no" ) );
	Sharpen_Console_Write_1char__(" prev=");
	Sharpen_Console_WriteHex_1int64_t_((int32_t)currentBlock->field_Prev);
	Sharpen_Console_Write_1char__(" next=");
	Sharpen_Console_WriteHex_1int64_t_((int32_t)currentBlock->field_Next);
	Sharpen_Console_Write_1char__(" i am=");
	Sharpen_Console_WriteHex_1int64_t_((int32_t)currentBlock);
	Sharpen_Console_WriteLine_1char__("");
}
void Sharpen_Heap_Dump_0(void)
{
	struct struct_Sharpen_Heap_BlockDescriptor* descriptor = classStatics_Sharpen_Heap.firstDescriptor;
	struct struct_Sharpen_Heap_Block* currentBlock = descriptor->field_First;
	while(true)
	{
		{
			Sharpen_Heap_dumpBlock_1struct_struct_Sharpen_Heap_Block__(currentBlock);
			Sharpen_Drivers_Char_Keyboard_Getch_0();
			currentBlock = currentBlock->field_Next;
			if(currentBlock == null)
						return;
		}
	}
	;
}
void* Sharpen_Heap_KAlloc_2int32_t_int32_t_(int32_t size, int32_t align)
{
	uint32_t address = (uint32_t)Sharpen_Heap_CurrentEnd_getter();
	if(align &&  ( address & 0xFFFFF000 )  > 0){
		address &= 0xFFFFF000;
		address += 0x1000;
	}
	Sharpen_Heap_CurrentEnd_setter((void*) ( address + size ) );
	return (void*)address;
}
struct class_Sharpen_Collections_Fifo* classInit_Sharpen_Collections_Fifo(void)
{
	struct class_Sharpen_Collections_Fifo* object = calloc(1, sizeof(struct class_Sharpen_Collections_Fifo));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Collections_Fifo;
	object->field_m_wait = true;
	object->field_m_head = 0;
	object->field_m_tail = 0;
	return object;
}

struct class_Sharpen_Collections_Fifo* Sharpen_Collections_Fifo_Fifo_2class_int32_t_(struct class_Sharpen_Collections_Fifo* obj, int32_t size)
{
	obj->field_m_buffer = calloc((size), sizeof(uint8_t));
	obj->field_m_size = size;
	return obj;
}
uint32_t Sharpen_Collections_Fifo_ReadWait_3class_uint8_t__uint32_t_(struct class_Sharpen_Collections_Fifo* obj, uint8_t* buffer, uint32_t size)
{
	uint32_t left = size;
	uint32_t offset = 0;
	while(left > 0)
	{
		{
			uint32_t sz = Sharpen_Collections_Fifo_Read_4class_uint8_t__uint32_t_uint32_t_(obj, buffer, left, offset);
			left -= sz;
			offset += sz;
		}
	}
	;
	return size;
}
uint32_t Sharpen_Collections_Fifo_Read_3class_uint8_t__uint32_t_(struct class_Sharpen_Collections_Fifo* obj, uint8_t* buffer, uint32_t size)
{
	return Sharpen_Collections_Fifo_Read_4class_uint8_t__uint32_t_uint32_t_(obj, buffer, size, 0);
}
uint32_t Sharpen_Collections_Fifo_Read_4class_uint8_t__uint32_t_uint32_t_(struct class_Sharpen_Collections_Fifo* obj, uint8_t* buffer, uint32_t size, uint32_t offset)
{
	uint32_t j = offset;
	if(
		obj->field_m_wait){
		while(obj->field_m_head == obj->field_m_tail)
		{
			{
				Sharpen_Arch_CPU_STI_0();
				Sharpen_Arch_CPU_HLT_0();
			}
		}
		;
	}
	for(uint32_t i = 0;i < size;i = i + 1
	)
	{
		{
			if(obj->field_m_tail != obj->field_m_head){
				buffer[j] = obj->field_m_buffer[obj->field_m_tail];
				j = j + 1
				;
				obj->field_m_tail = obj->field_m_tail + 1
				;
				if(obj->field_m_tail >= obj->field_m_size)
								obj->field_m_tail = 0;
			}
			else
			{
				return i;
			}
		}
	}
	;
	return size;
}
uint32_t Sharpen_Collections_Fifo_Write_3class_uint8_t__uint32_t_(struct class_Sharpen_Collections_Fifo* obj, uint8_t* buffer, uint32_t size)
{
	uint8_t* current = buffer;
	for(uint32_t i = 0;i < size;i = i + 1
	)
	{
		{
			if( ! Sharpen_Collections_Fifo_WriteByte_2class_uint8_t_(obj, *current++))
						return i;
		}
	}
	;
	return size;
}
int32_t Sharpen_Collections_Fifo_WriteByte_2class_uint8_t_(struct class_Sharpen_Collections_Fifo* obj, uint8_t byt)
{
	if( ( obj->field_m_head + 1 == obj->field_m_tail )  ||  (  ( obj->field_m_head + 1 == obj->field_m_size )  && obj->field_m_tail == 0 ) ){
		return false;
	}
	else
	{
		obj->field_m_buffer[obj->field_m_head] = byt;
		obj->field_m_head = obj->field_m_head + 1
		;
		if(obj->field_m_head >= obj->field_m_size)
				obj->field_m_head = 0;
	}
	return true;
}
struct class_Sharpen_Collections_List* classInit_Sharpen_Collections_List(void)
{
	struct class_Sharpen_Collections_List* object = calloc(1, sizeof(struct class_Sharpen_Collections_List));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Collections_List;
	object->field_m_currentCap = 4;
	object->prop_Count = 0;
	return object;
}

void** Sharpen_Collections_List_Item_getter(struct class_Sharpen_Collections_List* obj)
{
	return obj->prop_Item;
}
void** Sharpen_Collections_List_Item_setter(struct class_Sharpen_Collections_List* obj, void** value)
{
	obj->prop_Item = value;
	return value;
}
int32_t Sharpen_Collections_List_Count_getter(struct class_Sharpen_Collections_List* obj)
{
	return obj->prop_Count;
}
int32_t Sharpen_Collections_List_Count_setter(struct class_Sharpen_Collections_List* obj, int32_t value)
{
	obj->prop_Count = value;
	return value;
}
int32_t Sharpen_Collections_List_Capacity_getter(struct class_Sharpen_Collections_List* obj)
{
	return obj->field_m_currentCap;
}
int32_t Sharpen_Collections_List_Capacity_setter(struct class_Sharpen_Collections_List* obj, int32_t value)
{
	void** newArray = calloc((value), sizeof(void*));
	Sharpen_Memory_Memcpy_3void__void__int32_t_(Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(newArray), Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(Sharpen_Collections_List_Item_getter(obj)), obj->field_m_currentCap * sizeof(void*));
	Sharpen_Collections_List_Item_setter(obj, newArray);
	obj->field_m_currentCap = value;
	return value;
}
struct class_Sharpen_Collections_List* Sharpen_Collections_List_List_1class_(struct class_Sharpen_Collections_List* obj)
{
	Sharpen_Collections_List_Item_setter(obj, calloc((obj->field_m_currentCap), sizeof(void*)));
	return obj;
}
void Sharpen_Collections_List_EnsureCapacity_2class_int32_t_(struct class_Sharpen_Collections_List* obj, int32_t required)
{
	if(required < obj->field_m_currentCap)
		return;
	Sharpen_Collections_List_Capacity_setter(obj, Sharpen_Collections_List_Capacity_getter(obj) * 2);
}
void Sharpen_Collections_List_Add_2class_void__(struct class_Sharpen_Collections_List* obj, void* o)
{
	Sharpen_Collections_List_EnsureCapacity_2class_int32_t_(obj, Sharpen_Collections_List_Count_getter(obj) + 1);
	Sharpen_Collections_List_Item_getter(obj)[Sharpen_Collections_List_Count_getter(obj)] = o;
	Sharpen_Collections_List_Count_setter(obj, Sharpen_Collections_List_Count_getter(obj) + 1);
}
void Sharpen_Collections_List_RemoveAt_2class_int32_t_(struct class_Sharpen_Collections_List* obj, int32_t index)
{
	if(index >= Sharpen_Collections_List_Count_getter(obj))
		return;
	int32_t destination = (int32_t)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(Sharpen_Collections_List_Item_getter(obj)) +  ( index * sizeof(void*) ) ;
	int32_t source = (int32_t)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(Sharpen_Collections_List_Item_getter(obj)) +  (  ( index + 1 )  * sizeof(void*) ) ;
	Sharpen_Memory_Memcpy_3void__void__int32_t_((void*)destination, (void*)source,  ( Sharpen_Collections_List_Count_getter(obj) - index - 1 )  * sizeof(void*));
	Sharpen_Collections_List_Count_setter(obj, Sharpen_Collections_List_Count_getter(obj) - 1)
	;
	if(Sharpen_Collections_List_Count_getter(obj) * 2 < Sharpen_Collections_List_Capacity_getter(obj))
		Sharpen_Collections_List_Capacity_setter(obj, Sharpen_Collections_List_Capacity_getter(obj) / 2);
}
void Sharpen_Collections_List_Clear_1class_(struct class_Sharpen_Collections_List* obj)
{
	Sharpen_Memory_Memset_3void__int32_t_int32_t_(Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(Sharpen_Collections_List_Item_getter(obj)), 0, obj->field_m_currentCap * sizeof(void*));
	Sharpen_Collections_List_Count_setter(obj, 0);
}
int32_t Sharpen_Collections_List_Contains_2class_void__(struct class_Sharpen_Collections_List* obj, void* item)
{
	for(int32_t i = 0;i < Sharpen_Collections_List_Count_getter(obj);i = i + 1
	)
	{
		{
			if(Sharpen_Collections_List_Item_getter(obj)[i] == item)
						return true;
		}
	}
	;
	return false;
}
void Sharpen_Collections_List_CopyTo_2class_void___(struct class_Sharpen_Collections_List* obj, void** array)
{
	Sharpen_Collections_List_CopyTo_5class_int32_t_void___int32_t_int32_t_(obj, 0, array, 0, Sharpen_Collections_List_Count_getter(obj));
}
void Sharpen_Collections_List_CopyTo_3class_void___int32_t_(struct class_Sharpen_Collections_List* obj, void** array, int32_t arrayIndex)
{
	Sharpen_Collections_List_CopyTo_5class_int32_t_void___int32_t_int32_t_(obj, 0, array, arrayIndex, Sharpen_Collections_List_Count_getter(obj));
}
void Sharpen_Collections_List_CopyTo_5class_int32_t_void___int32_t_int32_t_(struct class_Sharpen_Collections_List* obj, int32_t index, void** array, int32_t arrayIndex, int32_t count)
{
	int32_t destination = (int32_t)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(array) +  ( sizeof(void*) * arrayIndex ) ;
	int32_t source = (int32_t)Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(Sharpen_Collections_List_Item_getter(obj)) +  ( sizeof(void*) * index ) ;
	Sharpen_Memory_Memcpy_3void__void__int32_t_((void*)destination, (void*)source, count);
}
int32_t Sharpen_Collections_List_IndexOf_2class_void__(struct class_Sharpen_Collections_List* obj, void* item)
{
	return Sharpen_Collections_List_IndexOf_4class_void__int32_t_int32_t_(obj, item, 0, Sharpen_Collections_List_Count_getter(obj));
}
int32_t Sharpen_Collections_List_IndexOf_3class_void__int32_t_(struct class_Sharpen_Collections_List* obj, void* item, int32_t index)
{
	return Sharpen_Collections_List_IndexOf_4class_void__int32_t_int32_t_(obj, item, index, Sharpen_Collections_List_Count_getter(obj) - index);
}
int32_t Sharpen_Collections_List_IndexOf_4class_void__int32_t_int32_t_(struct class_Sharpen_Collections_List* obj, void* item, int32_t index, int32_t count)
{
	for(int32_t i = index;i < Sharpen_Collections_List_Count_getter(obj) && i < count + index;i = i + 1
	)
	{
		{
			if(Sharpen_Collections_List_Item_getter(obj)[i] == item)
						return i;
		}
	}
	;
	return  - 1;
}
int32_t Sharpen_Collections_List_LastIndexOf_2class_void__(struct class_Sharpen_Collections_List* obj, void* item)
{
	return Sharpen_Collections_List_LastIndexOf_4class_void__int32_t_int32_t_(obj, item, Sharpen_Collections_List_Count_getter(obj) - 1, Sharpen_Collections_List_Count_getter(obj));
}
int32_t Sharpen_Collections_List_LastIndexOf_3class_void__int32_t_(struct class_Sharpen_Collections_List* obj, void* item, int32_t index)
{
	return Sharpen_Collections_List_LastIndexOf_4class_void__int32_t_int32_t_(obj, item, index, Sharpen_Collections_List_Count_getter(obj) - index);
}
int32_t Sharpen_Collections_List_LastIndexOf_4class_void__int32_t_int32_t_(struct class_Sharpen_Collections_List* obj, void* item, int32_t index, int32_t count)
{
	for(int32_t i = index;i >= 0 && i - index < count;i = i - 1
	)
	{
		{
			if(Sharpen_Collections_List_Item_getter(obj)[i] == item)
						return i;
		}
	}
	;
	return  - 1;
}
struct class_Sharpen_Collections_BitArray* classInit_Sharpen_Collections_BitArray(void)
{
	struct class_Sharpen_Collections_BitArray* object = calloc(1, sizeof(struct class_Sharpen_Collections_BitArray));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Collections_BitArray;
	return object;
}

struct class_Sharpen_Collections_BitArray* Sharpen_Collections_BitArray_BitArray_2class_int32_t_(struct class_Sharpen_Collections_BitArray* obj, int32_t N)
{
	obj->field_m_bitmap = calloc((N), sizeof(int32_t));
	{
		int32_t* ptr = obj->field_m_bitmap;
		{
			Sharpen_Memory_Memset_3void__int32_t_int32_t_(ptr, 0, N * sizeof(int32_t));
		}
	}
	obj->field_m_N = N;
	return obj;
}
void Sharpen_Collections_BitArray_SetBit_2class_int32_t_(struct class_Sharpen_Collections_BitArray* obj, int32_t k)
{
	int32_t bitmap = k >> 5;
	int32_t index = k &  ( 32 - 1 ) ;
	obj->field_m_bitmap[bitmap] |= 1 << index;
}
void Sharpen_Collections_BitArray_ClearBit_2class_int32_t_(struct class_Sharpen_Collections_BitArray* obj, int32_t k)
{
	int32_t bitmap = k >> 5;
	int32_t index = k &  ( 32 - 1 ) ;
	obj->field_m_bitmap[bitmap] &=  ~  ( 1 << index ) ;
}
int32_t Sharpen_Collections_BitArray_TestBit_2class_int32_t_(struct class_Sharpen_Collections_BitArray* obj, int32_t k)
{
	int32_t bitmap = k >> 5;
	int32_t index = k &  ( 32 - 1 ) ;
	return  (  ( obj->field_m_bitmap[bitmap] &  ( 1 << index )  )  > 0 ) ;
}
int32_t Sharpen_Collections_BitArray_FindFirstFree_1class_(struct class_Sharpen_Collections_BitArray* obj)
{
	for(int32_t i = 0;i < obj->field_m_N;i = i + 1
	)
	{
		{
			int32_t bitmap = obj->field_m_bitmap[i];
			if(bitmap ==  - 1)
						continue;
			for(int32_t j = 0;j < 32;j = j + 1
			)
			{
				{
					if( ( bitmap &  ( 1 << j )  )  == 0){
						return i * 32 + j;
					}
				}
			}
			;
		}
	}
	;
	return  - 1;
}
struct class_Sharpen_Lib_Audio* classInit_Sharpen_Lib_Audio(void)
{
	struct class_Sharpen_Lib_Audio* object = calloc(1, sizeof(struct class_Sharpen_Lib_Audio));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Lib_Audio;
	return object;
}

inline struct struct_Sharpen_Lib_Audio_SoundDevice structInit_Sharpen_Lib_Audio_SoundDevice(void)
{
	struct struct_Sharpen_Lib_Audio_SoundDevice object;
	return object;
}
void Sharpen_Lib_Audio_SetDevice_1struct_struct_Sharpen_Lib_Audio_SoundDevice_(struct struct_Sharpen_Lib_Audio_SoundDevice device)
{
	classStatics_Sharpen_Lib_Audio.m_device = device;
}
void Sharpen_Lib_Audio_RequestBuffer_2uint32_t_uint16_t__(uint32_t size, uint16_t* buffer)
{
}
void Sharpen_Lib_Audio_Init_0(void)
{
	struct class_Sharpen_FileSystem_Device* dev = classInit_Sharpen_FileSystem_Device();
	dev->field_Name = "Audio";
	dev->field_node = classInit_Sharpen_FileSystem_Node();
	dev->field_node->field_Write = Sharpen_Lib_Audio_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__;
	Sharpen_FileSystem_DevFS_RegisterDevice_1struct_class_Sharpen_FileSystem_Device__(dev);
}
uint32_t Sharpen_Lib_Audio_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__(struct class_Sharpen_FileSystem_Node* node, uint32_t offset, uint32_t size, uint8_t* buffer)
{
	return 0;
}
struct class_Sharpen_Memory* classInit_Sharpen_Memory(void)
{
	struct class_Sharpen_Memory* object = calloc(1, sizeof(struct class_Sharpen_Memory));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Memory;
	return object;
}

int32_t Sharpen_Memory_Compare_3char__char__int32_t_(char* s1, char* s2, int32_t n)
{
	for(int32_t i = 0;i < n;i = i + 1
	)
	{
		{
			if(s1[i] != s2[i])
						return false;
		}
	}
	;
	return true;
}
struct class_Sharpen_Multiboot* classInit_Sharpen_Multiboot(void)
{
	struct class_Sharpen_Multiboot* object = calloc(1, sizeof(struct class_Sharpen_Multiboot));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Multiboot;
	return object;
}

inline void classCctor_Sharpen_Multiboot(void)
{
}
inline struct struct_Sharpen_Multiboot_Header structInit_Sharpen_Multiboot_Header(void)
{
	struct struct_Sharpen_Multiboot_Header object;
	return object;
}
inline struct struct_Sharpen_Multiboot_MMAP structInit_Sharpen_Multiboot_MMAP(void)
{
	struct struct_Sharpen_Multiboot_MMAP object;
	return object;
}
inline struct struct_Sharpen_Multiboot_Module structInit_Sharpen_Multiboot_Module(void)
{
	struct struct_Sharpen_Multiboot_Module object;
	return object;
}
struct class_Sharpen_Panic* classInit_Sharpen_Panic(void)
{
	struct class_Sharpen_Panic* object = calloc(1, sizeof(struct class_Sharpen_Panic));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Panic;
	return object;
}

void Sharpen_Panic_DoPanic_1char__(char* str)
{
	Sharpen_Arch_CPU_CLI_0();
	Sharpen_Console_Attribute_setter(0x4F);
	Sharpen_Console_Clear_0();
	Sharpen_Console_WriteLine_1char__("");
	Sharpen_Console_WriteLine_1char__("  ____  _                                 ");
	Sharpen_Console_WriteLine_1char__(" / ___|| |__   __ _ _ __ _ __   ___ _ __  ");
	Sharpen_Console_WriteLine_1char__(" \\___ \\| '_ \\ / _` | '__| '_ \\ / _ \\ '_ \\ ");
	Sharpen_Console_WriteLine_1char__("  ___) | | | | (_| | |  | |_) |  __/ | | |");
	Sharpen_Console_WriteLine_1char__(" |____/|_| |_|\\__,_|_|  | .__/ \\___|_| |_|");
	Sharpen_Console_WriteLine_1char__("                        |_|               ");
	Sharpen_Console_WriteLine_1char__("");
	Sharpen_Console_Write_1char__("\tMessage: ");
	Sharpen_Console_Write_1char__(str);
	Sharpen_Arch_CPU_HLT_0();
}
struct class_Sharpen_Arch_PortIO* classInit_Sharpen_Arch_PortIO(void)
{
	struct class_Sharpen_Arch_PortIO* object = calloc(1, sizeof(struct class_Sharpen_Arch_PortIO));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Arch_PortIO;
	return object;
}

struct class_Sharpen_Arch_Paging* classInit_Sharpen_Arch_Paging(void)
{
	struct class_Sharpen_Arch_Paging* object = calloc(1, sizeof(struct class_Sharpen_Arch_Paging));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Arch_Paging;
	return object;
}

inline struct struct_Sharpen_Arch_Paging_PageTable structInit_Sharpen_Arch_Paging_PageTable(void)
{
	struct struct_Sharpen_Arch_Paging_PageTable object;
	return object;
}
inline struct struct_Sharpen_Arch_Paging_PageDirectory structInit_Sharpen_Arch_Paging_PageDirectory(void)
{
	struct struct_Sharpen_Arch_Paging_PageDirectory object;
	return object;
}
struct struct_Sharpen_Arch_Paging_PageDirectory* Sharpen_Arch_Paging_KernelDirectory_getter(void)
{
	return classStatics_Sharpen_Arch_Paging.prop_KernelDirectory;
}
struct struct_Sharpen_Arch_Paging_PageDirectory* Sharpen_Arch_Paging_KernelDirectory_setter(struct struct_Sharpen_Arch_Paging_PageDirectory* value)
{
	classStatics_Sharpen_Arch_Paging.prop_KernelDirectory = value;
	return value;
}
struct struct_Sharpen_Arch_Paging_PageDirectory* Sharpen_Arch_Paging_CurrentDirectory_getter(void)
{
	return classStatics_Sharpen_Arch_Paging.m_currentDirectory;
}
struct struct_Sharpen_Arch_Paging_PageDirectory* Sharpen_Arch_Paging_CurrentDirectory_setter(struct struct_Sharpen_Arch_Paging_PageDirectory* value)
{
	classStatics_Sharpen_Arch_Paging.m_currentDirectory = value;
	Sharpen_Arch_Paging_setDirectoryInternal_1struct_struct_Sharpen_Arch_Paging_PageDirectory__(value);
	return value;
}
int32_t Sharpen_Arch_Paging_FrameAddress_1int32_t_(int32_t a)
{
	return  ( a << 0xC ) ;
}
int32_t Sharpen_Arch_Paging_GetFrameAddress_1int32_t_(int32_t a)
{
	return  ( a >> 0xC ) ;
}
void Sharpen_Arch_Paging_Init_1uint32_t_(uint32_t memSize)
{
	Sharpen_Arch_Paging_KernelDirectory_setter((struct struct_Sharpen_Arch_Paging_PageDirectory*)Sharpen_Heap_AlignedAlloc_2int32_t_int32_t_(0x1000, sizeof(struct struct_Sharpen_Arch_Paging_PageDirectory)));
	Sharpen_Arch_Paging_CurrentDirectory_setter(Sharpen_Arch_Paging_KernelDirectory_getter());
	Sharpen_Memory_Memset_3void__int32_t_int32_t_(Sharpen_Arch_Paging_KernelDirectory_getter(), 0, sizeof(struct struct_Sharpen_Arch_Paging_PageDirectory));
	classStatics_Sharpen_Arch_Paging.m_bitmap = Sharpen_Collections_BitArray_BitArray_2class_int32_t_(classInit_Sharpen_Collections_BitArray(), (int32_t) ( memSize / 32 ) );
	int32_t address = 0;
	while(address < (int32_t)Sharpen_Heap_CurrentEnd_getter() +  ( sizeof(struct struct_Sharpen_Arch_Paging_PageDirectory) ) )
	{
		{
			int32_t flags = (int32_t)enum_Sharpen_Arch_Paging_PageFlags_Present | (int32_t)enum_Sharpen_Arch_Paging_PageFlags_Writable | (int32_t)enum_Sharpen_Arch_Paging_PageFlags_UserMode;
			Sharpen_Arch_Paging_MapPage_4struct_struct_Sharpen_Arch_Paging_PageDirectory__int32_t_int32_t_int32_t_(Sharpen_Arch_Paging_KernelDirectory_getter(), address, address, flags);
			Sharpen_Arch_Paging_SetFrame_1int32_t_(address);
			address += 0x1000;
		}
	}
	;
	Sharpen_Arch_Paging_Enable_0();
}
void Sharpen_Arch_Paging_MapPage_4struct_struct_Sharpen_Arch_Paging_PageDirectory__int32_t_int32_t_int32_t_(struct struct_Sharpen_Arch_Paging_PageDirectory* directory, int32_t phys, int32_t virt, int32_t flags)
{
	flags |= (int32_t)enum_Sharpen_Arch_Paging_PageFlags_UserMode;
	int32_t pageIndex = virt / 0x1000;
	int32_t tableIndex = pageIndex / 1024;
	if(directory->field_tables[tableIndex] == 0){
		struct struct_Sharpen_Arch_Paging_PageTable* newTable = (struct struct_Sharpen_Arch_Paging_PageTable*)Sharpen_Heap_AlignedAlloc_2int32_t_int32_t_(0x1000, sizeof(struct struct_Sharpen_Arch_Paging_PageTable));
		if(newTable == null)
				return;
		Sharpen_Memory_Memset_3void__int32_t_int32_t_(newTable, 0, sizeof(struct struct_Sharpen_Arch_Paging_PageTable));
		int32_t flaggedTable = (int32_t)newTable | flags;
		directory->field_tables[tableIndex] = flaggedTable;
	}
	struct struct_Sharpen_Arch_Paging_PageTable* table = (struct struct_Sharpen_Arch_Paging_PageTable*) ( directory->field_tables[tableIndex] & 0xFFFFF000 ) ;
	table->field_pages[pageIndex &  ( 1024 - 1 ) ] = Sharpen_Arch_Paging_FrameAddress_1int32_t_(phys / 0x1000) | flags;
}
void* Sharpen_Arch_Paging_GetPhysicalFromVirtual_1void__(void* virt)
{
	int32_t address = (int32_t)virt;
	int32_t remaining = address % 0x1000;
	int32_t frame = address / 0x1000;
	struct struct_Sharpen_Arch_Paging_PageTable* table = (struct struct_Sharpen_Arch_Paging_PageTable*) ( Sharpen_Arch_Paging_CurrentDirectory_getter()->field_tables[frame / 1024] & 0xFFFFF000 ) ;
	if(table == null)
		return null;
	int32_t page = table->field_pages[frame &  ( 1024 - 1 ) ];
	return (void*) ( Sharpen_Arch_Paging_GetFrameAddress_1int32_t_(page) * 0x1000 + remaining ) ;
}
int32_t Sharpen_Arch_Paging_GetPage_2struct_struct_Sharpen_Arch_Paging_PageDirectory__int32_t_(struct struct_Sharpen_Arch_Paging_PageDirectory* directory, int32_t address)
{
	return directory->field_tables[address &  ( 1024 - 1 ) ];
}
void Sharpen_Arch_Paging_SetFrame_1int32_t_(int32_t frame)
{
	Sharpen_Collections_BitArray_SetBit_2class_int32_t_(classStatics_Sharpen_Arch_Paging.m_bitmap, frame / 0x1000);
}
void Sharpen_Arch_Paging_ClearFrame_1int32_t_(int32_t frame)
{
	Sharpen_Collections_BitArray_ClearBit_2class_int32_t_(classStatics_Sharpen_Arch_Paging.m_bitmap, frame / 0x1000);
}
int32_t Sharpen_Arch_Paging_AllocateFrame_0(void)
{
	int32_t free = Sharpen_Collections_BitArray_FindFirstFree_1class_(classStatics_Sharpen_Arch_Paging.m_bitmap);
	int32_t address = free * 0x1000;
	Sharpen_Arch_Paging_SetFrame_1int32_t_(address);
	return address;
}
void Sharpen_Arch_Paging_FreeFrame_1int32_t_(int32_t page)
{
	Sharpen_Arch_Paging_ClearFrame_1int32_t_(Sharpen_Arch_Paging_GetFrameAddress_1int32_t_(page));
}
void* Sharpen_Arch_Paging_AllocatePhysical_1int32_t_(int32_t size)
{
	uint32_t sizeAligned = (uint32_t)size;
	if(sizeAligned % 0x1000 != 0){
		sizeAligned &= 0xFFFFF000;
		sizeAligned += 0x1000;
	}
	int32_t free = Sharpen_Collections_BitArray_FindFirstFree_1class_(classStatics_Sharpen_Arch_Paging.m_bitmap);
	int32_t start = free * 0x1000;
	int32_t address = start;
	int32_t end = (int32_t) ( address + sizeAligned ) ;
	while(address < end)
	{
		{
			int32_t flags = (int32_t)enum_Sharpen_Arch_Paging_PageFlags_Present | (int32_t)enum_Sharpen_Arch_Paging_PageFlags_Writable | (int32_t)enum_Sharpen_Arch_Paging_PageFlags_UserMode;
			Sharpen_Arch_Paging_MapPage_4struct_struct_Sharpen_Arch_Paging_PageDirectory__int32_t_int32_t_int32_t_(Sharpen_Arch_Paging_CurrentDirectory_getter(), address, address, flags);
			Sharpen_Arch_Paging_SetFrame_1int32_t_(address);
			address += 0x1000;
		}
	}
	;
	return (void*)start;
}
struct struct_Sharpen_Arch_Paging_PageDirectory* Sharpen_Arch_Paging_CloneDirectory_1struct_struct_Sharpen_Arch_Paging_PageDirectory__(struct struct_Sharpen_Arch_Paging_PageDirectory* source)
{
	struct struct_Sharpen_Arch_Paging_PageDirectory* destination = (struct struct_Sharpen_Arch_Paging_PageDirectory*)Sharpen_Heap_AlignedAlloc_2int32_t_int32_t_(0x1000, sizeof(struct struct_Sharpen_Arch_Paging_PageDirectory));
	if(destination == null)
		return null;
	Sharpen_Memory_Memset_3void__int32_t_int32_t_(destination, 0, sizeof(struct struct_Sharpen_Arch_Paging_PageDirectory));
	for(int32_t table = 0;table < 1024;table = table + 1
	)
	{
		{
			int32_t sourceTable = source->field_tables[table];
			if(sourceTable == 0)
						continue;
			struct struct_Sharpen_Arch_Paging_PageTable* sourceTablePtr = (struct struct_Sharpen_Arch_Paging_PageTable*) ( sourceTable & 0xFFFFF000 ) ;
			int32_t flags = sourceTable & 0xFFF;
			struct struct_Sharpen_Arch_Paging_PageTable* newTable = (struct struct_Sharpen_Arch_Paging_PageTable*)Sharpen_Heap_AlignedAlloc_2int32_t_int32_t_(0x1000, sizeof(struct struct_Sharpen_Arch_Paging_PageTable));
			Sharpen_Memory_Memcpy_3void__void__int32_t_(newTable, sourceTablePtr, sizeof(struct struct_Sharpen_Arch_Paging_PageTable));
			destination->field_tables[table] = (int32_t)newTable | flags;
		}
	}
	;
	return destination;
}
void Sharpen_Arch_Paging_FreeDirectory_1struct_struct_Sharpen_Arch_Paging_PageDirectory__(struct struct_Sharpen_Arch_Paging_PageDirectory* directory)
{
	for(int32_t table = 0;table < 1024;table = table + 1
	)
	{
		{
			int32_t pageTable = directory->field_tables[table];
			if(pageTable == 0)
						continue;
			struct struct_Sharpen_Arch_Paging_PageTable* pageTablePtr = (struct struct_Sharpen_Arch_Paging_PageTable*) ( pageTable & 0xFFFFF000 ) ;
			Sharpen_Heap_Free_1void__(pageTablePtr);
		}
	}
	;
}
struct class_Sharpen_Program* classInit_Sharpen_Program(void)
{
	struct class_Sharpen_Program* object = calloc(1, sizeof(struct class_Sharpen_Program));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Program;
	return object;
}

inline void classCctor_Sharpen_Program(void)
{
}
void Sharpen_Program_KernelMain_3struct_struct_Sharpen_Multiboot_Header__uint32_t_uint32_t_(struct struct_Sharpen_Multiboot_Header* header, uint32_t magic, uint32_t end)
{
	Sharpen_Console_Clear_0();
	void* heapStart = (void*)end;
	uint32_t memSize = 32;
	if(magic == classStatics_Sharpen_Multiboot.Magic){
		classStatics_Sharpen_Program.m_isMultiboot = true;
		{
			struct struct_Sharpen_Multiboot_Header* destination = &classStatics_Sharpen_Program.m_mbootHeader;
			{
				Sharpen_Memory_Memcpy_3void__void__int32_t_(destination, header, sizeof(struct struct_Sharpen_Multiboot_Header));
			}
		}
		memSize = classStatics_Sharpen_Program.m_mbootHeader.field_MemHi;
		if( ( classStatics_Sharpen_Program.m_mbootHeader.field_Flags & classStatics_Sharpen_Multiboot.FlagMods )  > 0){
			uint32_t modsCount = classStatics_Sharpen_Program.m_mbootHeader.field_ModsCount;
			Sharpen_Console_Write_1char__("[Multiboot] Detected - Modules: ");
			Sharpen_Console_WriteNum_1int32_t_((int32_t)modsCount);
			Sharpen_Console_PutChar_1char_('\n');
			for(int32_t i = 0;i < modsCount;i = i + 1
			)
			{
				{
					struct struct_Sharpen_Multiboot_Module** mods = (struct struct_Sharpen_Multiboot_Module**)classStatics_Sharpen_Program.m_mbootHeader.field_ModsAddr;
					struct struct_Sharpen_Multiboot_Module module = *mods[i];
					if((int32_t)module.field_End > (int32_t)heapStart){
						heapStart = module.field_End;
					}
				}
			}
			;
		}
		else
		{
			Sharpen_Console_WriteLine_1char__("[Multiboot] Detected - No modules");
		}
	}
	Sharpen_Heap_Init_1void__(heapStart);
	Sharpen_Arch_GDT_Init_0();
	Sharpen_Arch_PIC_Remap_0();
	Sharpen_Arch_IDT_Init_0();
	Sharpen_Drivers_Power_Acpi_Init_0();
	Sharpen_Arch_FPU_Init_0();
	Sharpen_Arch_Paging_Init_1uint32_t_(memSize);
	Sharpen_Heap_SetupRealHeap_0();
	Sharpen_Arch_PIT_Init_0();
	Sharpen_Arch_CMOS_UpdateTime_0();
	Sharpen_Drivers_Char_Keyboard_Init_0();
	Sharpen_FileSystem_DevFS_Init_0();
	Sharpen_FileSystem_VFS_Init_0();
	Sharpen_FileSystem_STDOUT_Init_0();
	Sharpen_Drivers_Char_SerialPort_Init_0();
	Sharpen_Arch_PCI_Init_0();
	Sharpen_Drivers_Other_VboxDev_Init_0();
	Sharpen_Drivers_Net_rtl8139_Init_0();
	Sharpen_Drivers_Block_ATA_Init_0();
	Sharpen_Task_Tasking_Init_0();
	Sharpen_Net_NetworkTools_WakeOnLan_1uint8_t__(calloc((6), sizeof(uint8_t)));
	struct class_Sharpen_FileSystem_Node* hddNode = Sharpen_FileSystem_VFS_GetByPath_1char__("devices://HDD0");
	Sharpen_FileSystem_Fat16_Init_2struct_class_Sharpen_FileSystem_Node__char__(hddNode, "C");
	int32_t error = Sharpen_Exec_Loader_StartProcess_2char__char___("C://shell", null);
	if(error != enum_Sharpen_ErrorCode_SUCCESS){
		Sharpen_Console_Write_1char__("Failed to start initial process: 0x");
		Sharpen_Console_WriteHex_1int64_t_((int32_t)error);
		Sharpen_Console_PutChar_1char_('\n');
	}
	while(true)
	{
		Sharpen_Arch_CPU_HLT_0();
	}
	;
}
struct class_Sharpen_Utilities_String* classInit_Sharpen_Utilities_String(void)
{
	struct class_Sharpen_Utilities_String* object = calloc(1, sizeof(struct class_Sharpen_Utilities_String));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Utilities_String;
	return object;
}

int32_t Sharpen_Utilities_String_Length_1char__(char* text)
{
	int32_t i = 0;
	for(;text[i] != '\0';i = i + 1
	)
	{
	}
	;
	return i;
}
int32_t Sharpen_Utilities_String_IndexOf_2char__char__(char* text, char* occurence)
{
	int32_t found =  - 1;
	int32_t foundCount = 0;
	int32_t textLength = Sharpen_Utilities_String_Length_1char__(text);
	int32_t occurenceLength = Sharpen_Utilities_String_Length_1char__(occurence);
	if(textLength == 0 || occurenceLength == 0)
		return  - 1;
	for(int32_t textIndex = 0;textIndex < textLength;textIndex = textIndex + 1
	)
	{
		{
			if(occurence[foundCount] == text[textIndex]){
				if(foundCount == 0)
								found = textIndex;
				foundCount = foundCount + 1
				;
				if(foundCount >= occurenceLength)
								return found;
			}
			else
			{
				foundCount = 0;
				if(found >= 0)
								textIndex = found;
				found =  - 1;
			}
		}
	}
	;
	return  - 1;
}
int32_t Sharpen_Utilities_String_Count_2char__char_(char* str, char occurence)
{
	int32_t matches = 0;
	for(int32_t i = 0;str[i] != '\0';i = i + 1
	)
	{
		if(str[i] == occurence)
				matches = matches + 1
		;
	}
	;
	return matches;
}
char* Sharpen_Utilities_String_SubString_3char__int32_t_int32_t_(char* str, int32_t start, int32_t count)
{
	if(count <= 0)
		return null;
	int32_t stringLength = Sharpen_Utilities_String_Length_1char__(str);
	if(start > stringLength)
		return "";
	char* ch = (char*)Sharpen_Heap_Alloc_1int32_t_(count + 1);
	int32_t j = 0;
	for(int32_t i = start;j < count;i = i + 1
	)
	{
		{
			if(str[i] == '\0'){
				break;
			}
			ch[j] = str[i];
			j = j + 1;
		}
	}
	;
	ch[j] = '\0';
	return Sharpen_Utilities_Util_CharPtrToString_1char__(ch);
}
int64_t Sharpen_Utilities_String_toLong_2class_char__(struct class_Sharpen_Utilities_String* obj, char* str)
{
	int32_t len = Sharpen_Utilities_String_Length_1char__(str);
	int32_t num = 0;
	for(int32_t i = 0;i < len;i = i + 1
	)
	{
		{
			num *= 16;
			if(str[i] > '9')
						num += str[i] - 'A' + 10;
			else
			{
				num += str[i] - '0';
			}
		}
	}
	;
	return num;
}
char* Sharpen_Utilities_String_Merge_2char__char__(char* first, char* second)
{
	int32_t firstLength = Sharpen_Utilities_String_Length_1char__(first);
	int32_t secondLength = Sharpen_Utilities_String_Length_1char__(second);
	int32_t totalLength = firstLength + secondLength;
	char* outVal = (char*)Sharpen_Heap_Alloc_1int32_t_(totalLength + 1);
	Sharpen_Memory_Memcpy_3void__void__int32_t_(outVal, Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(first), firstLength);
	Sharpen_Memory_Memcpy_3void__void__int32_t_((void*) ( (int32_t)outVal + firstLength ) , Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(second), secondLength);
	outVal[totalLength] = '\0';
	return Sharpen_Utilities_Util_CharPtrToString_1char__(outVal);
}
int32_t Sharpen_Utilities_String_Equals_2char__char__(char* one, char* two)
{
	int32_t oneLength = Sharpen_Utilities_String_Length_1char__(one);
	int32_t twoLength = Sharpen_Utilities_String_Length_1char__(two);
	if(oneLength != twoLength)
		return false;
	for(int32_t i = 0;i < oneLength;i = i + 1
	)
	{
		if(one[i] != two[i])
				return false;
	}
	;
	return true;
}
char Sharpen_Utilities_String_ToUpper_1char_(char c)
{
	return  ( c >= 'a' && c <= 'z' )  ? (char) ( c +  ( 'A' - 'a' )  )  : c;
}
char Sharpen_Utilities_String_ToLower_1char_(char c)
{
	if( ( c >= 65 )  &&  ( c <= 90 ) )
		c = (char) ( c + (int32_t)32 ) ;
	return c;
}
struct class_Sharpen_Time* classInit_Sharpen_Time(void)
{
	struct class_Sharpen_Time* object = calloc(1, sizeof(struct class_Sharpen_Time));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Time;
	return object;
}

int32_t Sharpen_Time_Seconds_getter(void)
{
	return classStatics_Sharpen_Time.prop_Seconds;
}
int32_t Sharpen_Time_Seconds_setter(int32_t value)
{
	classStatics_Sharpen_Time.prop_Seconds = value;
	return value;
}
int32_t Sharpen_Time_Minutes_getter(void)
{
	return classStatics_Sharpen_Time.prop_Minutes;
}
int32_t Sharpen_Time_Minutes_setter(int32_t value)
{
	classStatics_Sharpen_Time.prop_Minutes = value;
	return value;
}
int32_t Sharpen_Time_Hours_getter(void)
{
	return classStatics_Sharpen_Time.prop_Hours;
}
int32_t Sharpen_Time_Hours_setter(int32_t value)
{
	classStatics_Sharpen_Time.prop_Hours = value;
	return value;
}
int32_t Sharpen_Time_Day_getter(void)
{
	return classStatics_Sharpen_Time.prop_Day;
}
int32_t Sharpen_Time_Day_setter(int32_t value)
{
	classStatics_Sharpen_Time.prop_Day = value;
	return value;
}
int32_t Sharpen_Time_Month_getter(void)
{
	return classStatics_Sharpen_Time.prop_Month;
}
int32_t Sharpen_Time_Month_setter(int32_t value)
{
	classStatics_Sharpen_Time.prop_Month = value;
	return value;
}
int32_t Sharpen_Time_Year_getter(void)
{
	return classStatics_Sharpen_Time.prop_Year;
}
int32_t Sharpen_Time_Year_setter(int32_t value)
{
	classStatics_Sharpen_Time.prop_Year = value;
	return value;
}
struct class_Sharpen_Utilities_Util* classInit_Sharpen_Utilities_Util(void)
{
	struct class_Sharpen_Utilities_Util* object = calloc(1, sizeof(struct class_Sharpen_Utilities_Util));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Utilities_Util;
	return object;
}


static void* methods_Sharpen_Arch_CMOS[] = {Sharpen_Arch_CMOS_GetData_1uint8_t_,Sharpen_Arch_CMOS_BCD_TO_BIN_1int32_t_,Sharpen_Arch_CMOS_UpdateTime_0,};
static void* methods_Sharpen_Arch_CPU[] = {Sharpen_Arch_CPU_STI_0,Sharpen_Arch_CPU_CLI_0,Sharpen_Arch_CPU_HLT_0,};
static void* methods_Sharpen_Arch_FPU[] = {Sharpen_Arch_FPU_Init_0,Sharpen_Arch_FPU_StoreContext_1void__,Sharpen_Arch_FPU_RestoreContext_1void__,};
static void* methods_Sharpen_Arch_IDT[] = {Sharpen_Arch_IDT_Privilege_1int32_t_,Sharpen_Arch_IDT_Present_1int32_t_,Sharpen_Arch_IDT_SetEntry_4int32_t_void__uint16_t_uint8_t_,Sharpen_Arch_IDT_Init_0,Sharpen_Arch_IDT_flushIDT_1struct_struct_Sharpen_Arch_IDT_IDT_Pointer__,Sharpen_Arch_IDT_INTIgnore_0,Sharpen_Arch_IDT_ISR0_0,Sharpen_Arch_IDT_ISR1_0,Sharpen_Arch_IDT_ISR2_0,Sharpen_Arch_IDT_ISR3_0,Sharpen_Arch_IDT_ISR4_0,Sharpen_Arch_IDT_ISR5_0,Sharpen_Arch_IDT_ISR6_0,Sharpen_Arch_IDT_ISR7_0,Sharpen_Arch_IDT_ISR8_0,Sharpen_Arch_IDT_ISR9_0,Sharpen_Arch_IDT_ISR10_0,Sharpen_Arch_IDT_ISR11_0,Sharpen_Arch_IDT_ISR12_0,Sharpen_Arch_IDT_ISR13_0,Sharpen_Arch_IDT_ISR14_0,Sharpen_Arch_IDT_ISR15_0,Sharpen_Arch_IDT_ISR16_0,Sharpen_Arch_IDT_ISR17_0,Sharpen_Arch_IDT_ISR18_0,Sharpen_Arch_IDT_ISR19_0,Sharpen_Arch_IDT_ISR20_0,Sharpen_Arch_IDT_ISR21_0,Sharpen_Arch_IDT_ISR22_0,Sharpen_Arch_IDT_ISR23_0,Sharpen_Arch_IDT_ISR24_0,Sharpen_Arch_IDT_ISR25_0,Sharpen_Arch_IDT_ISR26_0,Sharpen_Arch_IDT_ISR27_0,Sharpen_Arch_IDT_ISR28_0,Sharpen_Arch_IDT_ISR29_0,Sharpen_Arch_IDT_ISR30_0,Sharpen_Arch_IDT_ISR31_0,Sharpen_Arch_IDT_IRQ0_0,Sharpen_Arch_IDT_IRQ1_0,Sharpen_Arch_IDT_IRQ2_0,Sharpen_Arch_IDT_IRQ3_0,Sharpen_Arch_IDT_IRQ4_0,Sharpen_Arch_IDT_IRQ5_0,Sharpen_Arch_IDT_IRQ6_0,Sharpen_Arch_IDT_IRQ7_0,Sharpen_Arch_IDT_IRQ8_0,Sharpen_Arch_IDT_IRQ9_0,Sharpen_Arch_IDT_IRQ10_0,Sharpen_Arch_IDT_IRQ11_0,Sharpen_Arch_IDT_IRQ12_0,Sharpen_Arch_IDT_IRQ13_0,Sharpen_Arch_IDT_IRQ14_0,Sharpen_Arch_IDT_IRQ15_0,Sharpen_Arch_IDT_Syscall_0,};
static void* methods_Sharpen_Arch_IRQ[] = {Sharpen_Arch_IRQ_SetHandler_2int32_t_delegate_Sharpen_Arch_IRQ_IRQHandler_,Sharpen_Arch_IRQ_RemoveHandler_1int32_t_,Sharpen_Arch_IRQ_Handler_1struct_struct_Sharpen_Arch_Regs__,};
static void* methods_Sharpen_Arch_ISR[] = {Sharpen_Arch_ISR_Handler_1struct_struct_Sharpen_Arch_Regs__,};
static void* methods_Sharpen_Drivers_Net_rtl8139[] = {Sharpen_Drivers_Net_rtl8139_initHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_,Sharpen_Drivers_Net_rtl8139_GetMac_1uint8_t__,Sharpen_Drivers_Net_rtl8139_Transmit_2uint8_t__uint32_t_,Sharpen_Drivers_Net_rtl8139_PrintRes_0,Sharpen_Drivers_Net_rtl8139_handler_1struct_struct_Sharpen_Arch_Regs__,Sharpen_Drivers_Net_rtl8139_turnOn_0,Sharpen_Drivers_Net_rtl8139_readMac_0,Sharpen_Drivers_Net_rtl8139_setInterruptMask_1uint16_t_,Sharpen_Drivers_Net_rtl8139_ackowledgeInterrupts_0,Sharpen_Drivers_Net_rtl8139_updateLinkStatus_0,Sharpen_Drivers_Net_rtl8139_exitHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_,Sharpen_Drivers_Net_rtl8139_Init_0,};
static void* methods_Sharpen_Exec_ELFLoader[] = {Sharpen_Exec_ELFLoader_isValidELF_1struct_struct_Sharpen_Exec_ELFLoader_ELF32__,Sharpen_Exec_ELFLoader_getSection_2struct_struct_Sharpen_Exec_ELFLoader_ELF32__uint32_t_,Sharpen_Exec_ELFLoader_getString_2struct_struct_Sharpen_Exec_ELFLoader_ELF32__uint32_t_,Sharpen_Exec_ELFLoader_Execute_3uint8_t__uint32_t_char___,};
static void* methods_Sharpen_Exec_Loader[] = {Sharpen_Exec_Loader_StartProcess_2char__char___,};
static void* methods_Sharpen_Exec_Syscalls[] = {Sharpen_Exec_Syscalls_Exit_1int32_t_,Sharpen_Exec_Syscalls_GetPID_0,Sharpen_Exec_Syscalls_Sbrk_1int32_t_,Sharpen_Exec_Syscalls_Fork_0,Sharpen_Exec_Syscalls_Write_3int32_t_uint8_t__uint32_t_,Sharpen_Exec_Syscalls_Read_3int32_t_uint8_t__uint32_t_,Sharpen_Exec_Syscalls_Open_2char__int32_t_,Sharpen_Exec_Syscalls_Close_1int32_t_,Sharpen_Exec_Syscalls_Seek_3int32_t_uint32_t_int32_t_,};
static void* methods_Sharpen_FileSystem_SubDirectory[] = {};
static void* methods_Sharpen_FileSystem_STDOUT[] = {Sharpen_FileSystem_STDOUT_Init_0,Sharpen_FileSystem_STDOUT_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__,};
static void* methods_Sharpen_Net_DHCP[] = {Sharpen_Net_DHCP_makeHeader_3uint32_t_uint32_t_uint8_t_,Sharpen_Net_DHCP_Sample_0,};
static void* methods_Sharpen_Net_Network[] = {Sharpen_Net_Network_Set_1struct_struct_Sharpen_Net_Network_NetDevice_,Sharpen_Net_Network_Transmit_2uint8_t__uint32_t_,Sharpen_Net_Network_GetMac_1uint8_t__,};
static void* methods_Sharpen_Net_NetworkTools[] = {Sharpen_Net_NetworkTools_WakeOnLan_1uint8_t__,};
static void* methods_Sharpen_Task_Task[] = {};
static void* methods_Sharpen_Task_Tasking[] = {Sharpen_Task_Tasking_Init_0,Sharpen_Task_Tasking_GetTaskByPID_1int32_t_,Sharpen_Task_Tasking_RemoveTaskByPID_1int32_t_,Sharpen_Task_Tasking_ScheduleTask_1struct_class_Sharpen_Task_Task__,Sharpen_Task_Tasking_AddTask_2void__int32_t_,Sharpen_Task_Tasking_FindNextTask_0,Sharpen_Task_Tasking_scheduler_1struct_struct_Sharpen_Arch_Regs__,Sharpen_Task_Tasking_writeSchedulerStack_4int32_t__int32_t_int32_t_void__,Sharpen_Task_Tasking_GetNodeFromDescriptor_1int32_t_,Sharpen_Task_Tasking_GetOffsetFromDescriptor_1int32_t_,Sharpen_Task_Tasking_AddNodeToDescriptor_1struct_class_Sharpen_FileSystem_Node__,Sharpen_Task_Tasking_ManualSchedule_0,};
static void* methods_Sharpen_Arch_PCI[] = {Sharpen_Arch_PCI_generateAddress_4uint32_t_uint32_t_uint32_t_uint32_t_,Sharpen_Arch_PCI_readWord_4uint16_t_uint16_t_uint16_t_uint16_t_,Sharpen_Arch_PCI_PCIRead_5uint16_t_uint16_t_uint16_t_uint16_t_uint32_t_,Sharpen_Arch_PCI_PCIWrite_5uint16_t_uint16_t_uint16_t_uint16_t_uint32_t_,Sharpen_Arch_PCI_PCIWrite_3struct_struct_Sharpen_Arch_PCI_PciDevice_uint16_t_uint32_t_,Sharpen_Arch_PCI_PCIReadWord_2struct_struct_Sharpen_Arch_PCI_PciDevice_uint16_t_,Sharpen_Arch_PCI_getDeviceID_3uint16_t_uint16_t_uint16_t_,Sharpen_Arch_PCI_getHeaderType_3uint16_t_uint16_t_uint16_t_,Sharpen_Arch_PCI_GetVendorID_3uint16_t_uint16_t_uint16_t_,Sharpen_Arch_PCI_GetClassID_3uint16_t_uint16_t_uint16_t_,Sharpen_Arch_PCI_GetSubClassID_3uint16_t_uint16_t_uint16_t_,Sharpen_Arch_PCI_checkBus_1uint8_t_,Sharpen_Arch_PCI_checkDevice_2uint8_t_uint8_t_,Sharpen_Arch_PCI_RegisterDriver_3uint16_t_uint16_t_struct_struct_Sharpen_Arch_PCI_PciDriver_,Sharpen_Arch_PCI_PrintDevices_0,Sharpen_Arch_PCI_Probe_0,Sharpen_Arch_PCI_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__,Sharpen_Arch_PCI_Init_0,};
static void* methods_Sharpen_Arch_PIC[] = {Sharpen_Arch_PIC_SendEOI_1uint8_t_,Sharpen_Arch_PIC_Remap_0,};
static void* methods_Sharpen_Arch_PIT[] = {Sharpen_Arch_PIT_Channel_1int32_t_,Sharpen_Arch_PIT_Access_1int32_t_,Sharpen_Arch_PIT_Operating_1int32_t_,Sharpen_Arch_PIT_Mode_1int32_t_,Sharpen_Arch_PIT_Init_0,Sharpen_Arch_PIT_Handler_1struct_struct_Sharpen_Arch_Regs__,};
static void* methods_Sharpen_Arch_Syscall[] = {Sharpen_Arch_Syscall_Handler_1struct_struct_Sharpen_Arch_Regs__,};
static void* methods_Sharpen_Utilities_ByteUtil[] = {Sharpen_Utilities_ByteUtil_ToBytes_1int32_t_,Sharpen_Utilities_ByteUtil_ToBytes_1int64_t_,Sharpen_Utilities_ByteUtil_ToBytes_2int64_t_uint8_t__,Sharpen_Utilities_ByteUtil_ToLong_1uint8_t__,Sharpen_Utilities_ByteUtil_ToShort_2uint8_t__int32_t_,Sharpen_Utilities_ByteUtil_ToInt_1uint8_t__,};
static void* methods_Sharpen_Drivers_Other_VboxDev[] = {Sharpen_Drivers_Other_VboxDev_getGuestInfo_0,Sharpen_Drivers_Other_VboxDev_initHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_,Sharpen_Drivers_Other_VboxDev_exitHander_1struct_struct_Sharpen_Arch_PCI_PciDevice_,Sharpen_Drivers_Other_VboxDev_Init_0,Sharpen_Drivers_Other_VboxDev_ChangePowerState_1int32_t_,Sharpen_Drivers_Other_VboxDev_GetSessionID_0,Sharpen_Drivers_Other_VboxDev_GetHostTime_0,};
static void* methods_Sharpen_Drivers_Other_VboxDevFSDriver[] = {Sharpen_Drivers_Other_VboxDevFSDriver_Init_0,Sharpen_Drivers_Other_VboxDevFSDriver_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_,Sharpen_Drivers_Other_VboxDevFSDriver_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__,Sharpen_Drivers_Other_VboxDevFSDriver_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__,Sharpen_Drivers_Other_VboxDevFSDriver_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__,};
static void* methods_Sharpen_Drivers_Power_Acpi[] = {Sharpen_Drivers_Power_Acpi_Find_0,Sharpen_Drivers_Power_Acpi_CheckSum_2void__uint32_t_,Sharpen_Drivers_Power_Acpi_parseS5Object_0,Sharpen_Drivers_Power_Acpi_getEntry_1char__,Sharpen_Drivers_Power_Acpi_Init_0,Sharpen_Drivers_Power_Acpi_Enable_0,Sharpen_Drivers_Power_Acpi_Disable_0,Sharpen_Drivers_Power_Acpi_Reset_0,Sharpen_Drivers_Power_Acpi_Shutdown_0,};
static void* methods_Sharpen_Drivers_Sound_IntelHD[] = {Sharpen_Drivers_Sound_IntelHD_initHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_,Sharpen_Drivers_Sound_IntelHD_exitHander_1struct_struct_Sharpen_Arch_PCI_PciDevice_,Sharpen_Drivers_Sound_IntelHD_Init_0,};
static void* methods_Sharpen_Drivers_Sound_AC97[] = {Sharpen_Drivers_Sound_AC97_InitHandler_1struct_struct_Sharpen_Arch_PCI_PciDevice_,Sharpen_Drivers_Sound_AC97_IRQHandler_1struct_struct_Sharpen_Arch_Regs__,Sharpen_Drivers_Sound_AC97_ExitHander_1struct_struct_Sharpen_Arch_PCI_PciDevice_,Sharpen_Drivers_Sound_AC97_Init_0,Sharpen_Drivers_Sound_AC97_Reader_1int32_t_,Sharpen_Drivers_Sound_AC97_Writer_2int32_t_uint32_t_,};
static void* methods_Sharpen_FileSystem_DevFS[] = {Sharpen_FileSystem_DevFS_Init_0,Sharpen_FileSystem_DevFS_GenerateHash_1char__,Sharpen_FileSystem_DevFS_RegisterDevice_1struct_class_Sharpen_FileSystem_Device__,Sharpen_FileSystem_DevFS_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__,Sharpen_FileSystem_DevFS_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_,};
static void* methods_Sharpen_FileSystem_Device[] = {};
static void* methods_Sharpen_Collections_Dictionary[] = {Sharpen_Collections_Dictionary_Clear_1class_,Sharpen_Collections_Dictionary_Count_1class_,Sharpen_Collections_Dictionary_GetAt_2class_int32_t_,Sharpen_Collections_Dictionary_Add_3class_int64_t_void__,Sharpen_Collections_Dictionary_GetByKey_2class_int64_t_,};
static void* methods_Sharpen_FileSystem_Fat16[] = {Sharpen_FileSystem_Fat16_initFAT_1struct_class_Sharpen_FileSystem_Node__,Sharpen_FileSystem_Fat16_parseBoot_0,Sharpen_FileSystem_Fat16_Data_clust_to_lba_1uint32_t_,Sharpen_FileSystem_Fat16_Init_2struct_class_Sharpen_FileSystem_Node__char__,Sharpen_FileSystem_Fat16_CreateNode_1struct_struct_Sharpen_FileSystem_FatDirEntry__,Sharpen_FileSystem_Fat16_findDirImpl_2struct_class_Sharpen_FileSystem_Node__char__,Sharpen_FileSystem_Fat16_FindFileInDirectory_2struct_class_Sharpen_FileSystem_SubDirectory__char__,Sharpen_FileSystem_Fat16_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_,Sharpen_FileSystem_Fat16_FindNextCluster_1uint32_t_,Sharpen_FileSystem_Fat16_FirstFirstFreeSector_0,Sharpen_FileSystem_Fat16_readFile_4uint32_t_uint32_t_uint32_t_uint8_t__,Sharpen_FileSystem_Fat16_readDirectory_1uint32_t_,Sharpen_FileSystem_Fat16_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__,Sharpen_FileSystem_Fat16_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__,};
static void* methods_Sharpen_Collections_LongIndex[] = {Sharpen_Collections_LongIndex_EnsureCapacity_2class_int32_t_,Sharpen_Collections_LongIndex_Add_2class_int64_t_,Sharpen_Collections_LongIndex_RemoveAt_2class_int32_t_,Sharpen_Collections_LongIndex_Clear_1class_,Sharpen_Collections_LongIndex_Contains_2class_int64_t_,Sharpen_Collections_LongIndex_CopyTo_2class_int64_t__,Sharpen_Collections_LongIndex_CopyTo_3class_int64_t__int32_t_,Sharpen_Collections_LongIndex_CopyTo_5class_int32_t_int64_t__int32_t_int32_t_,Sharpen_Collections_LongIndex_IndexOf_2class_int64_t_,Sharpen_Collections_LongIndex_IndexOf_3class_int64_t_int32_t_,Sharpen_Collections_LongIndex_IndexOf_4class_int64_t_int32_t_int32_t_,Sharpen_Collections_LongIndex_LastIndexOf_2class_int64_t_,Sharpen_Collections_LongIndex_LastIndexOf_3class_int64_t_int32_t_,Sharpen_Collections_LongIndex_LastIndexOf_4class_int64_t_int32_t_int32_t_,};
static void* methods_Sharpen_Console[] = {Sharpen_Console_PutChar_1char_,Sharpen_Console_Clear_0,Sharpen_Console_Write_1char__,Sharpen_Console_WriteLine_1char__,Sharpen_Console_WriteHex_1int64_t_,Sharpen_Console_WriteNum_1int32_t_,Sharpen_Console_MoveCursor_0,};
static void* methods_Sharpen_Arch_GDT[] = {Sharpen_Arch_GDT_Privilege_1int32_t_,Sharpen_Arch_GDT_setEntry_5int32_t_uint64_t_uint64_t_int32_t_int32_t_,Sharpen_Arch_GDT_setTSS_2int32_t_struct_struct_Sharpen_Arch_GDT_TSS__,Sharpen_Arch_GDT_Init_0,Sharpen_Arch_GDT_flushGDT_1struct_struct_Sharpen_Arch_GDT_GDT_Pointer__,Sharpen_Arch_GDT_flushTSS_0,};
static void* methods_Sharpen_Drivers_Block_ATA[] = {Sharpen_Drivers_Block_ATA_wait400ns_1uint32_t_,Sharpen_Drivers_Block_ATA_selectDrive_2uint8_t_uint8_t_,Sharpen_Drivers_Block_ATA_identify_2uint8_t_uint8_t_,Sharpen_Drivers_Block_ATA_poll_1uint32_t_,Sharpen_Drivers_Block_ATA_ReadSector_4uint32_t_uint32_t_uint8_t_uint8_t__,Sharpen_Drivers_Block_ATA_WriteSector_4uint32_t_uint32_t_uint8_t_uint8_t__,Sharpen_Drivers_Block_ATA_probe_0,Sharpen_Drivers_Block_ATA_Init_0,Sharpen_Drivers_Block_ATA_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__,Sharpen_Drivers_Block_ATA_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__,};
static void* methods_Sharpen_Drivers_Char_SerialPort[] = {Sharpen_Drivers_Char_SerialPort_initDevice_1int32_t_,Sharpen_Drivers_Char_SerialPort_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__,Sharpen_Drivers_Char_SerialPort_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__,Sharpen_Drivers_Char_SerialPort_isTransmitEmpty_1int32_t_,Sharpen_Drivers_Char_SerialPort_hasReceived_1int32_t_,Sharpen_Drivers_Char_SerialPort_read_1uint16_t_,Sharpen_Drivers_Char_SerialPort_write_2uint8_t_uint16_t_,Sharpen_Drivers_Char_SerialPort_readBda_0,Sharpen_Drivers_Char_SerialPort_Handler13_1struct_struct_Sharpen_Arch_Regs__,Sharpen_Drivers_Char_SerialPort_Handler24_1struct_struct_Sharpen_Arch_Regs__,Sharpen_Drivers_Char_SerialPort_Init_0,};
static void* methods_Sharpen_Drivers_Char_Keyboard[] = {Sharpen_Drivers_Char_Keyboard_transformKey_1uint8_t_,Sharpen_Drivers_Char_Keyboard_updateLED_0,Sharpen_Drivers_Char_Keyboard_Init_0,Sharpen_Drivers_Char_Keyboard_readImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__,Sharpen_Drivers_Char_Keyboard_Getch_0,Sharpen_Drivers_Char_Keyboard_handler_1struct_struct_Sharpen_Arch_Regs__,};
static void* methods_Sharpen_Drivers_Char_KeyboardMap[] = {};
static void* methods_Sharpen_FileSystem_MountPoint[] = {};
static void* methods_Sharpen_FileSystem_Node[] = {};
static void* methods_Sharpen_FileSystem_VFS[] = {Sharpen_FileSystem_VFS_Init_0,Sharpen_FileSystem_VFS_readDirImpl_2struct_class_Sharpen_FileSystem_Node__uint32_t_,Sharpen_FileSystem_VFS_generateHash_1char__,Sharpen_FileSystem_VFS_AddMountPoint_1struct_class_Sharpen_FileSystem_MountPoint__,Sharpen_FileSystem_VFS_FindMountByName_1char__,Sharpen_FileSystem_VFS_GetByPath_1char__,Sharpen_FileSystem_VFS_Open_2struct_class_Sharpen_FileSystem_Node__int32_t_,Sharpen_FileSystem_VFS_Close_1struct_class_Sharpen_FileSystem_Node__,Sharpen_FileSystem_VFS_Read_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__,Sharpen_FileSystem_VFS_Write_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__,Sharpen_FileSystem_VFS_FindDir_2struct_class_Sharpen_FileSystem_Node__char__,Sharpen_FileSystem_VFS_ReadDir_2struct_class_Sharpen_FileSystem_Node__uint32_t_,};
static void* methods_Sharpen_Heap[] = {Sharpen_Heap_Init_1void__,Sharpen_Heap_getRequiredPageCount_1int32_t_,Sharpen_Heap_createBlockDescriptor_1int32_t_,Sharpen_Heap_getSufficientDescriptor_1int32_t_,Sharpen_Heap_SetupRealHeap_0,Sharpen_Heap_AlignedAlloc_2int32_t_int32_t_,Sharpen_Heap_Alloc_1int32_t_,Sharpen_Heap_getBlockFromPtr_1void__,Sharpen_Heap_Expand_2void__int32_t_,Sharpen_Heap_Free_1void__,Sharpen_Heap_dumpBlock_1struct_struct_Sharpen_Heap_Block__,Sharpen_Heap_Dump_0,Sharpen_Heap_KAlloc_2int32_t_int32_t_,};
static void* methods_Sharpen_Collections_Fifo[] = {Sharpen_Collections_Fifo_ReadWait_3class_uint8_t__uint32_t_,Sharpen_Collections_Fifo_Read_3class_uint8_t__uint32_t_,Sharpen_Collections_Fifo_Read_4class_uint8_t__uint32_t_uint32_t_,Sharpen_Collections_Fifo_Write_3class_uint8_t__uint32_t_,Sharpen_Collections_Fifo_WriteByte_2class_uint8_t_,};
static void* methods_Sharpen_Collections_List[] = {Sharpen_Collections_List_EnsureCapacity_2class_int32_t_,Sharpen_Collections_List_Add_2class_void__,Sharpen_Collections_List_RemoveAt_2class_int32_t_,Sharpen_Collections_List_Clear_1class_,Sharpen_Collections_List_Contains_2class_void__,Sharpen_Collections_List_CopyTo_2class_void___,Sharpen_Collections_List_CopyTo_3class_void___int32_t_,Sharpen_Collections_List_CopyTo_5class_int32_t_void___int32_t_int32_t_,Sharpen_Collections_List_IndexOf_2class_void__,Sharpen_Collections_List_IndexOf_3class_void__int32_t_,Sharpen_Collections_List_IndexOf_4class_void__int32_t_int32_t_,Sharpen_Collections_List_LastIndexOf_2class_void__,Sharpen_Collections_List_LastIndexOf_3class_void__int32_t_,Sharpen_Collections_List_LastIndexOf_4class_void__int32_t_int32_t_,};
static void* methods_Sharpen_Collections_BitArray[] = {Sharpen_Collections_BitArray_SetBit_2class_int32_t_,Sharpen_Collections_BitArray_ClearBit_2class_int32_t_,Sharpen_Collections_BitArray_TestBit_2class_int32_t_,Sharpen_Collections_BitArray_FindFirstFree_1class_,};
static void* methods_Sharpen_Lib_Audio[] = {Sharpen_Lib_Audio_SetDevice_1struct_struct_Sharpen_Lib_Audio_SoundDevice_,Sharpen_Lib_Audio_RequestBuffer_2uint32_t_uint16_t__,Sharpen_Lib_Audio_Init_0,Sharpen_Lib_Audio_writeImpl_4struct_class_Sharpen_FileSystem_Node__uint32_t_uint32_t_uint8_t__,};
static void* methods_Sharpen_Memory[] = {Sharpen_Memory_Memcpy_3void__void__int32_t_,Sharpen_Memory_Compare_3char__char__int32_t_,Sharpen_Memory_Memset_3void__int32_t_int32_t_,};
static void* methods_Sharpen_Multiboot[] = {};
static void* methods_Sharpen_Panic[] = {Sharpen_Panic_DoPanic_1char__,};
static void* methods_Sharpen_Arch_PortIO[] = {Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_,Sharpen_Arch_PortIO_In8_1uint16_t_,Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_,Sharpen_Arch_PortIO_In16_1uint16_t_,Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_,Sharpen_Arch_PortIO_In32_1uint16_t_,};
static void* methods_Sharpen_Arch_Paging[] = {Sharpen_Arch_Paging_FrameAddress_1int32_t_,Sharpen_Arch_Paging_GetFrameAddress_1int32_t_,Sharpen_Arch_Paging_Init_1uint32_t_,Sharpen_Arch_Paging_MapPage_4struct_struct_Sharpen_Arch_Paging_PageDirectory__int32_t_int32_t_int32_t_,Sharpen_Arch_Paging_GetPhysicalFromVirtual_1void__,Sharpen_Arch_Paging_GetPage_2struct_struct_Sharpen_Arch_Paging_PageDirectory__int32_t_,Sharpen_Arch_Paging_SetFrame_1int32_t_,Sharpen_Arch_Paging_ClearFrame_1int32_t_,Sharpen_Arch_Paging_AllocateFrame_0,Sharpen_Arch_Paging_FreeFrame_1int32_t_,Sharpen_Arch_Paging_AllocatePhysical_1int32_t_,Sharpen_Arch_Paging_CloneDirectory_1struct_struct_Sharpen_Arch_Paging_PageDirectory__,Sharpen_Arch_Paging_FreeDirectory_1struct_struct_Sharpen_Arch_Paging_PageDirectory__,Sharpen_Arch_Paging_Enable_0,Sharpen_Arch_Paging_Disable_0,Sharpen_Arch_Paging_setDirectoryInternal_1struct_struct_Sharpen_Arch_Paging_PageDirectory__,Sharpen_Arch_Paging_ReadCR2_0,};
static void* methods_Sharpen_Program[] = {Sharpen_Program_KernelMain_3struct_struct_Sharpen_Multiboot_Header__uint32_t_uint32_t_,};
static void* methods_Sharpen_Utilities_String[] = {Sharpen_Utilities_String_Length_1char__,Sharpen_Utilities_String_IndexOf_2char__char__,Sharpen_Utilities_String_Count_2char__char_,Sharpen_Utilities_String_SubString_3char__int32_t_int32_t_,Sharpen_Utilities_String_toLong_2class_char__,Sharpen_Utilities_String_Merge_2char__char__,Sharpen_Utilities_String_Equals_2char__char__,Sharpen_Utilities_String_ToUpper_1char_,Sharpen_Utilities_String_ToLower_1char_,};
static void* methods_Sharpen_Time[] = {};
static void* methods_Sharpen_Utilities_Util[] = {Sharpen_Utilities_Util_CharPtrToString_1char__,Sharpen_Utilities_Util_BytePtrToByteArray_1uint8_t__,Sharpen_Utilities_Util_ObjectToVoidPtr_1void__,Sharpen_Utilities_Util_MethodToPtr_1void__,};

void init(void)
{
	classCctor_Sharpen_Arch_CMOS();
	classCctor_Sharpen_Arch_IDT();
	classCctor_Sharpen_Arch_IRQ();
	classCctor_Sharpen_Arch_ISR();
	classCctor_Sharpen_Drivers_Net_rtl8139();
	classCctor_Sharpen_Task_Tasking();
	classCctor_Sharpen_Arch_PCI();
	classCctor_Sharpen_Arch_PIC();
	classCctor_Sharpen_Arch_PIT();
	classCctor_Sharpen_Drivers_Other_VboxDevFSDriver();
	classCctor_Sharpen_Drivers_Power_Acpi();
	classCctor_Sharpen_Drivers_Sound_AC97();
	classCctor_Sharpen_FileSystem_DevFS();
	classCctor_Sharpen_FileSystem_Fat16();
	classCctor_Sharpen_Console();
	classCctor_Sharpen_Drivers_Block_ATA();
	classCctor_Sharpen_Drivers_Char_SerialPort();
	classCctor_Sharpen_Drivers_Char_Keyboard();
	classCctor_Sharpen_Drivers_Char_KeyboardMap();
	classCctor_Sharpen_FileSystem_VFS();
	classCctor_Sharpen_Heap();
	classCctor_Sharpen_Multiboot();
	classCctor_Sharpen_Program();
}
