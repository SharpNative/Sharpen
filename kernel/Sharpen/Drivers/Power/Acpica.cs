using LibCS2C.Attributes;
using Sharpen.Arch;
using Sharpen.Drivers.Power;
using Sharpen.Mem;
using Sharpen.MultiTasking;
using Sharpen.Synchronisation;
using Sharpen.Utilities;

namespace Sharpen.Power
{
    unsafe class Acpica
    {
        #region Constants

        // Signal
        public const int ACPI_SIGNAL_FATAL = 0;
        public const int ACPI_SIGNAL_BREAKPOINT = 1;

        // Error codes
        public const int AE_OK = 0x00;
        public const int AE_ERROR = 0x01;
        public const int AE_NO_ACPI_TABLES = 0x02;
        public const int AE_NO_NAMESPACE = 0x03;
        public const int AE_NO_MEMORY = 0x04;
        public const int AE_NOT_FOUND = 0x05;
        public const int AE_NOT_EXIST = 0x06;
        public const int AE_ALREADY_EXISTS = 0x07;
        public const int AE_TYPE = 0x08;
        public const int AE_NULL_OBJECT = 0x09;
        public const int AE_NULL_ENTRY = 0x0A;
        public const int AE_BUFFER_OVERFLOW = 0x0B;
        public const int AE_STACK_OVERFLOW = 0x0C;
        public const int AE_STACK_UNDERFLOW = 0x0D;
        public const int AE_NOT_IMPLEMENTED = 0x0E;
        public const int AE_SUPPORT = 0x0F;
        public const int AE_LIMIT = 0x10;
        public const int AE_TIME = 0x11;
        public const int AE_ACQUIRE_DEADLOCK = 0x12;
        public const int AE_RELEASE_DEADLOCK = 0x13;
        public const int AE_NOT_ACQUIRED = 0x14;
        public const int AE_ALREADY_ACQUIRED = 0x15;
        public const int AE_NO_HARDWARE_RESPONSE = 0x16;
        public const int AE_NO_GLOBAL_LOCK = 0x17;
        public const int AE_ABORT_METHOD = 0x18;
        public const int AE_SAME_HANDLER = 0x19;
        public const int AE_NO_HANDLER = 0x1A;
        public const int AE_OWNER_ID_LIMIT = 0x1B;
        public const int AE_NOT_CONFIGURED = 0x1C;
        public const int AE_ACCESS = 0x1D;
        public const int AE_IO_ERROR = 0x1E;
        public const int AE_BAD_PARAMETER = 0x01 | 0x1000;
        public const int AE_BAD_CHARACTER = 0x02 | 0x1000;
        public const int AE_BAD_BAD_PATHNAME = 0x03 | 0x1000;
        public const int AE_BAD_DATA = 0x04 | 0x1000;
        public const int AE_BAD_HEX_CONSTANT = 0x05 | 0x1000;
        public const int AE_BAD_OCTAL_CONSTANT = 0x06 | 0x1000;
        public const int AE_BAD_DECIMAL_CONSTANT = 0x07 | 0x1000;
        public const int AE_MISSING_ARGUMENTS = 0x08 | 0x1000;
        public const int AE_BAD_ADDRESS = 0x09 | 0x1000;

        // Initialization sequence
        public const uint ACPI_FULL_INITIALIZATION = 0x00;
        public const uint ACPI_NO_ADDRESS_SPACE_INIT = 0x01;
        public const uint ACPI_NO_HARDWARE_INIT = 0x02;
        public const uint ACPI_NO_EVENT_INIT = 0x04;
        public const uint ACPI_NO_HANDLER_INIT = 0x08;
        public const uint ACPI_NO_ACPI_ENABLE = 0x10;
        public const uint ACPI_NO_DEVICE_INIT = 0x20;
        public const uint ACPI_NO_OBJECT_INIT = 0x40;
        public const uint ACPI_NO_FACS_INIT = 0x80;

        // Predefined sizes
        public const int ACPI_NAME_SIZE = 4;
        public const int ACPI_OEM_ID_SIZE = 6;
        public const int ACPI_OEM_TABLE_ID_SIZE = 8;

        // Values for flags field in AcpiGetObjectInfo
        public const int ACPI_PCI_ROOT_BRIDGE = 0x01;

        // Let ACPICA allocate the buffer
        public const uint ACPI_ALLOCATE_BUFFER = 0xFFFFFFFF;

        // Method names
        public const string METHOD_NAME__ADR = "_ADR";
        public const string METHOD_NAME__AEI = "_AEI";
        public const string METHOD_NAME__BBN = "_BBN";
        public const string METHOD_NAME__CBA = "_CBA";
        public const string METHOD_NAME__CID = "_CID";
        public const string METHOD_NAME__CRS = "_CRS";
        public const string METHOD_NAME__DDN = "_DDN";
        public const string METHOD_NAME__HID = "_HID";
        public const string METHOD_NAME__INI = "_INI";
        public const string METHOD_NAME__PLD = "_PLD";
        public const string METHOD_NAME__DSD = "_DSD";
        public const string METHOD_NAME__PRS = "_PRS";
        public const string METHOD_NAME__PRT = "_PRT";
        public const string METHOD_NAME__PRW = "_PRW";
        public const string METHOD_NAME__PS0 = "_PS0";
        public const string METHOD_NAME__PS1 = "_PS1";
        public const string METHOD_NAME__PS2 = "_PS2";
        public const string METHOD_NAME__PS3 = "_PS3";
        public const string METHOD_NAME__REG = "_REG";
        public const string METHOD_NAME__SB_ = "_SB_";
        public const string METHOD_NAME__SEG = "_SEG";
        public const string METHOD_NAME__SRS = "_SRS";
        public const string METHOD_NAME__STA = "_STA";
        public const string METHOD_NAME__SUB = "_SUB";
        public const string METHOD_NAME__UID = "_UID";

        public const uint ACPI_ROOT_OBJECT = 0xFFFFFFFF;

        #endregion

        public struct PredefinedName
        {
            public char* Name;
            public byte Type;
            public char* Val;
        }

        public struct TableHeader
        {
            public fixed char Signature[/*ACPI_NAME_SIZE*/4];
            public uint Length;
            public byte Revision;
            public byte Checksum;
            public fixed char OEMID[/*ACPI_OEM_ID_SIZE*/6];
            public fixed char OEMTableID[/*ACPI_OEM_TABLE_ID_SIZE*/8];
            public uint OEMRevision;
            public fixed char ASLCompilerID[/*ACPI_NAME_SIZE*/4];
            public uint ASLCompilerRevision;
        }

        public struct MemoryList
        {
            public char* ListName;
            public void* ListHead;
            public ushort ObjectSize;
            public ushort MaxDepth;
            public ushort CurrentDepth;
        }

        public struct PciID
        {
            public ushort Segment;
            public ushort Bus;
            public ushort Device;
            public ushort Function;
        }

        public struct TableDesc
        {
            public void* Address;
            public TableHeader* Pointer;
            public uint Length;
            public fixed char Signature[4];
            public byte OwnerID;
            public byte Flags;
            public ushort ValidationCount;
        }

        public struct PCIRoutingTable
        {
            public uint Length;
            public uint Pin;
            public ulong Address;
            public uint SourceIndex;
            public fixed char Source[4];
        }

        public enum ExecuteType
        {
            OSL_GLOBAL_LOCK_HANDLER,
            OSL_NOTIFY_HANDLER,
            OSL_GPE_HANDLER,
            OSL_DEBUGGER_MAIN_THREAD,
            OSL_DEBUGGER_EXEC_THREAD,
            OSL_EC_POLL_HANDLER,
            OSL_EC_BURST_HANDLER
        }

        public delegate uint OSDCallback(void* Context);
        public delegate void OSDExecCallback(void* Context);
        public delegate int WalkCallback(void* Object, uint NestingLevel, void* Context, void** ReturnValue);
        public delegate int WalkResourceCallback(AcpiObjects.Resource* Resource, void* Context);

        // Note: ACPI_STATUS is a 32bit unsigned integer
        // Note: ACPI_NAME is a 32bit unsigned integer
        // Note: ACPI_STRING is a null-terminated string
        // Note: ACPI_HANDLE is a void*
        // Note: ACPI_SIZE is a uint on 32bit systems and a ulong on 64bit system
        // Note: ACPI_PHYSICAL_ADDRESS is a ulong no matter if it's a 32bit or 64bit system (unless a preprocessor flag is set, but we don't set that)
        // Note: ACPI_IO_ADDRESS is ulong (same reason as ACPI_PHYSICAL_ADDRESS)
        // Note: ACPI_OBJECT_TYPE is a 32bit unsigned integer

        /// <summary>
        /// Initialize the OSL subsystem
        /// </summary>
        /// <returns>Status</returns>
        [Plug("AcpiOsInitialize")]
        public static int AcpiOsInitialize()
        {
            return AE_OK;
        }

        /// <summary>
        /// Terminate the OSL subsystem
        /// </summary>
        /// <returns>Status</returns>
        [Plug("AcpiOsTerminate")]
        public static int AcpiOsTerminate()
        {
            return AE_OK;
        }

        /// <summary>
        /// Obtain the root ACPI table pointer
        /// </summary>
        /// <returns>Status</returns>
        [Plug("AcpiOsGetRootPointer")]
        public static ulong AcpiOsGetRootPointer()
        {
            return (uint)Acpi.FindRSDP();
        }

        /// <summary>
        /// Allow the host OS to override a predefined ACPI object
        /// </summary>
        /// <param name="PredefinedObject">A pointer to the predefined object</param>
        /// <param name="NewValue">Where a new value for the predefined object is returned</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsPredefinedOverride")]
        public static int AcpiOsPredefinedOverride(PredefinedName* PredefinedObject, char** NewValue)
        {
            *NewValue = null;
            return AE_OK;
        }

        /// <summary>
        /// Allow the host OS to override a firmware ACPI table via a logical address
        /// </summary>
        /// <param name="ExistingTable">A pointer to the header of the existing ACPI table</param>
        /// <param name="NewTable">Where the pointer to the replacement table is returned</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsTableOverride")]
        public static int AcpiOsTableOverride(TableHeader* ExistingTable, TableHeader** NewTable)
        {
            *NewTable = null;
            return AE_OK;
        }

        /// <summary>
        /// Allow the host OS to override a firmware ACPI table via a physical address
        /// </summary>
        /// <param name="ExistingTable">A pointer to the header of the existing ACPI table</param>
        /// <param name="NewAddress">Where the physical address of the replacement table is returned</param>
        /// <param name="NewTableLength">Where the length of the replacement table is returned</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsPhysicalTableOverride")]
        public static int AcpiOsPhysicalTableOverride(TableHeader* ExistingTable, ulong* NewAddress, uint* NewTableLength)
        {
            *NewAddress = 0;
            return AE_OK;
        }

        /// <summary>
        /// Map physical memory into caller's address space
        /// </summary>
        /// <param name="PhysicalAddress">The physical address of the memory to be mapped</param>
        /// <param name="Length">The amount of memory to be mapped starting at the given physical address</param>
        /// <returns>Logical address</returns>
        [Plug("AcpiOsMapMemory")]
        public static void* AcpiOsMapMemory(ulong PhysicalAddress, uint Length)
        {
            // Align address down so we can get the offset
            uint down = Paging.AlignDown((uint)PhysicalAddress);
            uint offset = (uint)PhysicalAddress - down;

            // Length in rounded pagesize
            uint len = Paging.AlignUp(Length + offset);

            // Map
            void* mapped = Paging.MapToVirtual(Paging.CurrentDirectory, (int)down, (int)len, Paging.PageFlags.Present | Paging.PageFlags.Writable);
            return (void*)((uint)mapped + offset);
        }

        /// <summary>
        /// Remove a physical to logical memory mapping
        /// </summary>
        /// <param name="LogicalAddress">The logical address</param>
        /// <param name="Length">The amount of memory that was mapped</param>
        [Plug("AcpiOsUnmapMemory")]
        public static void AcpiOsUnmapMemory(void* LogicalAddress, uint Length)
        {
            // Align address down so we can get the offset
            uint down = Paging.AlignDown((uint)LogicalAddress);
            uint offset = (uint)LogicalAddress - down;

            // Length in rounded pagesize
            uint len = Paging.AlignUp(Length + offset);
            Paging.UnMapKeepPhysical((void*)down, (int)len);
        }

        /// <summary>
        /// Translate a logical address to a physical address
        /// </summary>
        /// <param name="LogicalAddress">The logical address to be translated</param>
        /// <param name="PhysicalAddress">The physical memory address of the logical address</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsGetPhysicalAddress")]
        public static int AcpiOsGetPhysicalAddress(void* LogicalAddress, ulong* PhysicalAddress)
        {
            *PhysicalAddress = (uint)Paging.GetPhysicalFromVirtual(LogicalAddress);
            return AE_OK;
        }

        /// <summary>
        /// Allocate memory from the dynamic memory pool
        /// </summary>
        /// <param name="Size">Amount of memory to allocate</param>
        /// <returns>A pointer to the allocated memory</returns>
        [Plug("AcpiOsAllocate")]
        public static void* AcpiOsAllocate(uint Size)
        {
            return Heap.Alloc((int)Size);
        }

        /// <summary>
        /// Free previously allocated memory
        /// </summary>
        /// <param name="Memory">A pointer to the memory to be freed</param>
        [Plug("AcpiOsFree")]
        public static void AcpiOsFree(void* Memory)
        {
            Heap.Free(Memory);
        }

        /// <summary>
        /// Check if a memory region is readable
        /// </summary>
        /// <param name="Memory">A pointer to the memory region to be checked</param>
        /// <param name="Length">The length of the memory region</param>
        /// <returns>True if the memory region is readable</returns>
        [Plug("AcpiOsReadable")]
        public static bool AcpiOsReadable(void* Memory, uint Length)
        {
            return true;
        }

        /// <summary>
        /// Check if a memory region is writable
        /// </summary>
        /// <param name="Memory">A pointer to the memory region to be checked</param>
        /// <param name="Length">The length of the memory region</param>
        /// <returns>True if the memory region is writable</returns>
        [Plug("AcpiOsWritable")]
        public static bool AcpiOsWritable(void* Memory, uint Length)
        {
            return true;
        }

        /// <summary>
        /// Obtain the ID of the currently executing thread
        /// </summary>
        [Plug("AcpiOsGetThreadId")]
        public static ulong AcpiOsGetThreadId()
        {
            // Numeber must be non-zero, note that we can just add one because this is nowhere else
            // used but here in ACPI
            if (Tasking.IsActive)
                return (ulong)(Tasking.CurrentTask.CurrentThread.TID + 1);

            return 1;
        }

        /// <summary>
        /// Schedule a procedure for deferred execution
        /// </summary>
        /// <param name="Type">Type of callback function</param>
        /// <param name="Function">Address of the procedure to execute</param>
        /// <param name="Context">Context value to be passed to the called procedure</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsExecute")]
        public static int AcpiOsExecute(ExecuteType Type, OSDExecCallback Function, void* Context)
        {
            return AE_OK;
        }

        /// <summary>
        /// Suspend the running task
        /// </summary>
        /// <param name="Milliseconds">The amount of time to sleep, in milliseconds</param>
        [Plug("AcpiOsSleep")]
        public static void AcpiOsSleep(ulong Milliseconds)
        {
            if (Tasking.IsActive)
                Tasking.CurrentTask.CurrentThread.Sleep(0, (uint)Milliseconds * 1000);
            else
                AcpiOsStall((uint)Milliseconds);
        }

        /// <summary>
        /// Wait for a short amount of time
        /// </summary>
        /// <param name="Milliseconds">The amount of time to delay, in milliseconds</param>
        [Plug("AcpiOsStall")]
        public static void AcpiOsStall(uint Milliseconds)
        {
            uint divisor = PIT.PrepareSleep(Milliseconds * 1000);
            PIT.Sleep(divisor);
        }

        /// <summary>
        /// Wait for completion of async events
        /// </summary>
        [Plug("AcpiOsWaitEventsComplete")]
        public static void AcpiOsWaitEventsComplete()
        {
        }

        /// <summary>
        /// Create a semaphore
        /// </summary>
        /// <param name="MaxUnits">The maximum number of units this semaphore will be required to accept</param>
        /// <param name="InitialUnits">The initial number of units to be assigned to the semaphore</param>
        /// <param name="OutHandle">A pointer to a location where a handle to the semaphore is to be returned</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsCreateSemaphore")]
        public static int AcpiOsCreateSemaphore(uint MaxUnits, uint InitialUnits, void** OutHandle)
        {
            return AE_OK;
        }

        /// <summary>
        /// Delete a semaphore
        /// </summary>
        /// <param name="Handle">A handle to a semaphore object</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsDeleteSemaphore")]
        public static int AcpiOsDeleteSemaphore(void* Handle)
        {
            return AE_OK;
        }

        /// <summary>
        /// Wait for units from a semaphore
        /// </summary>
        /// <param name="Handle">A handle to a semaphore object</param>
        /// <param name="Units">The number of units the caller is requesting</param>
        /// <param name="Timeout">How long the caller is willing to wait for the requested units</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsWaitSemaphore")]
        public static int AcpiOsWaitSemaphore(void* Handle, uint Units, ushort Timeout)
        {
            return AE_OK;
        }

        /// <summary>
        /// Sends units to a semaphore
        /// </summary>
        /// <param name="Handle">A handle to a semaphore object</param>
        /// <param name="Units">The number of units the caller is requesting</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsSignalSemaphore")]
        public static int AcpiOsSignalSemaphore(void* Handle, uint Units)
        {
            return AE_OK;
        }

        /// <summary>
        /// Create a spin lock
        /// </summary>
        /// <param name="Handle">A pointer to a location where a handle to the lock is to be returned</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsCreateLock")]
        public static int AcpiOsCreateLock(void** OutHandle)
        {
            if (OutHandle == null)
                return AE_BAD_PARAMETER;

            Spinlock spinlock = new Spinlock();
            *OutHandle = Util.ObjectToVoidPtr(spinlock);

            return AE_OK;
        }

        /// <summary>
        /// Delete a spin lock
        /// </summary>
        /// <param name="Handle">A pointer to a location where a handle to the lock is to be returned</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsDeleteLock")]
        public static int AcpiOsDeleteLock(void* Handle)
        {
            if (Handle == null)
                return AE_BAD_PARAMETER;

            Heap.Free(Handle);

            return AE_OK;
        }

        /// <summary>
        /// Acquire a spin lock
        /// </summary>
        /// <param name="Handle">A pointer to a location where a handle to the lock is to be returned</param>
        /// <returns>Platform dependent CPU flags to be used when the lock is released</returns>
        [Plug("AcpiOsAcquireLock")]
        public static uint AcpiOsAcquireLock(void* Handle)
        {
            Spinlock spinlock = (Spinlock)Util.VoidPtrToObject(Handle);
            spinlock.Lock();
            return 0;
        }

        /// <summary>
        /// Release a spin lock
        /// </summary>
        /// <param name="Handle">A handle to a lock object</param>
        /// <param name="Flags">CPU flags</param>
        [Plug("AcpiOsReleaseLock")]
        public static void AcpiOsReleaseLock(void* Handle, uint Flags)
        {
            if (Handle == null)
                return;

            Spinlock spinlock = (Spinlock)Util.VoidPtrToObject(Handle);
            spinlock.Unlock();
        }

        /// <summary>
        /// Install handler for a hardware interrupt level
        /// </summary>
        /// <param name="InterruptLevel">Interrupt level that the handler will service</param>
        /// <param name="Handler">Address of the handler</param>
        /// <param name="Context">A context value that is passed to the handler when the interrupt is dispatched</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsInstallInterruptHandler")]
        public static int AcpiOsInstallInterruptHandler(uint InterruptLevel, OSDCallback Handler, void* Context)
        {
            //Console.WriteLine("AcpiOsInstallInterruptHandler ");
            //Console.WriteNum((int)InterruptLevel);
            //Console.WriteLine("");
            if (Handler == null)
                return AE_BAD_PARAMETER;

            return AE_OK;
        }

        /// <summary>
        /// Remove an interrupt handler
        /// </summary>
        /// <param name="InterruptNumber">Interrupt number that the handler is currently servicing</param>
        /// <param name="Handler">Address of the handler that was previously installed</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsRemoveInterruptHandler")]
        public static int AcpiOsRemoveInterruptHandler(uint InterruptNumber, OSDCallback Handler)
        {
            Console.WriteLine("AcpiOsRemoveInterruptHandler");
            return AE_OK;
        }

        /// <summary>
        /// Read a value from a memory location
        /// </summary>
        /// <param name="Address">Memory address to be read</param>
        /// <param name="Value">A pointer to a location where the data is to be returned</param>
        /// <param name="Width">The memory width in bits, either 8, 16, 32, 64</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsReadMemory")]
        public static int AcpiOsReadMemory(void* Address, ulong* Value, uint Width)
        {
            Console.WriteLine("AcpiOsReadMemory");
            return AE_OK;
        }

        /// <summary>
        /// Write a value from a memory location
        /// </summary>
        /// <param name="Address">Memory address to be write</param>
        /// <param name="Value">A pointer to a location where the data is to be returned</param>
        /// <param name="Width">The memory width in bits, either 8, 16, 32, 64</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsWriteMemory")]
        public static int AcpiOsWriteMemory(void* Address, ulong* Value, uint Width)
        {
            Console.WriteLine("AcpiOsWriteMemory");
            return AE_OK;
        }

        /// <summary>
        /// Read a value from an input port
        /// </summary>
        /// <param name="Address">Hardware I/O port address to read from</param>
        /// <param name="Value">A pointer to a location where the data is to be returned</param>
        /// <param name="Width">The port width in bits, either 8, 16, 32</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsReadPort")]
        public static int AcpiOsReadPort(ulong Address, uint* Value, uint Width)
        {
            if (Width == 8)
                *Value = PortIO.In8((ushort)Address);
            else if (Width == 16)
                *Value = PortIO.In16((ushort)Address);
            else if (Width == 32)
                *Value = PortIO.In32((ushort)Address);

            return AE_OK;
        }

        /// <summary>
        /// Write a value from an input port
        /// </summary>
        /// <param name="Address">Hardware I/O port address to read from</param>
        /// <param name="Value">A pointer to a location where the data is to be returned</param>
        /// <param name="Width">The port width in bits, either 8, 16, 32</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsWritePort")]
        public static int AcpiOsWritePort(ulong Address, uint Value, uint Width)
        {
            if (Width == 8)
                PortIO.Out8((ushort)Address, (byte)Value);
            else if (Width == 16)
                PortIO.Out16((ushort)Address, (ushort)Value);
            else if (Width == 32)
                PortIO.Out32((ushort)Address, Value);

            return AE_OK;
        }

        /// <summary>
        /// Read a value from a PCI configuration register
        /// </summary>
        /// <param name="PciId">The full PCI configuration space address</param>
        /// <param name="Register">The PCI register address to be read from</param>
        /// <param name="Value">A pointer to a location where the data is to be returned</param>
        /// <param name="Width">The register width in bits, either 8, 16, 32, 64</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsReadPciConfiguration")]
        public static int AcpiOsReadPciConfiguration(PciID* PciId, uint Register, ulong* Value, uint Width)
        {
            *Value = PCI.PCIRead(PciId->Bus, PciId->Device, PciId->Function, (ushort)Register, Width);;
            return AE_OK;
        }

        /// <summary>
        /// Write a value from a PCI configuration register
        /// </summary>
        /// <param name="PciId">The full PCI configuration space address</param>
        /// <param name="Register">The PCI register address to be read from</param>
        /// <param name="Value">Data to be written</param>
        /// <param name="Width">The register width in bits, either 8, 16, 32, 64</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsWritePciConfiguration")]
        public static int AcpiOsWritePciConfiguration(PciID* PciId, uint Register, ulong Value, uint Width)
        {
            PCI.PCIWrite(PciId->Bus, PciId->Device, PciId->Function, (ushort)Register, (uint)Value, Width);
            return AE_OK;
        }

        /// <summary>
        /// Formatted stream output
        /// </summary>
        /// <param name="Format">A standard printf format string</param>
        /// <param name="Args">Variable printf parameter list</param>
        [Plug("AcpiOsPrintf")]
        public static void AcpiOsPrintf(string Format, params object[] Args)
        {
            //Console.WriteLine(Format);
            //Console.WriteHex((int)Util.ObjectToVoidPtr(Args));
            //Console.WriteLine("<<<<<");
        }

        /// <summary>
        /// Formatted stream output
        /// </summary>
        /// <param name="Format">A standard printf format string</param>
        /// <param name="Args">Variable printf parameter list</param>
        [Plug("AcpiOsVprintf")]
        public static void AcpiOsVprintf(string Format, void* Args)
        {
            //Console.WriteLine(Format);
        }

        /// <summary>
        /// Redirect the debug output
        /// </summary>
        /// <param name="Destination">A pointer</param>
        [Plug("AcpiOsRedirectOutput")]
        public static void AcpiOsRedirectOutput(void* Destination)
        {
        }

        /// <summary>
        /// Obtain an ACPI table via a physical address
        /// </summary>
        /// <param name="Address">Memory physical address of the requested ACPI table</param>
        /// <param name="OutTable">A pointer to the location where the table is to be returned</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsGetTableByAddress")]
        public static int AcpiOsGetTableByAddress(ulong Address, TableHeader** OutTable)
        {
            Console.WriteLine("AcpiOsGetTableByAddress");
            return AE_OK;
        }

        /// <summary>
        /// Obtain an installed ACPI table via an index
        /// </summary>
        /// <param name="TableIndex">Index of the requested table</param>
        /// <param name="OutTable">A pointer to location where the table is to be returned</param>
        /// <param name="OutAddress">A pointer to location where the physical address of the table is returned</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsGetTableByIndex")]
        public static int AcpiOsGetTableByIndex(uint TableIndex, TableHeader** OutTable, ulong** OutAddress)
        {
            Console.WriteLine("AcpiOsGetTableByIndex");
            return AE_OK;
        }

        /// <summary>
        /// Obtain an installed ACPI table via a specified name
        /// </summary>
        /// <param name="Signature">The uppercase ACPI signature for the requested table</param>
        /// <param name="Instance">Used to obtain tables that are allowed to have multiple instances</param>
        /// <param name="OutTable">A pointer to location where the table is to be returned</param>
        /// <param name="OutAddress">A pointer to location where the physical address of the table is returned</param>
        /// <returns>Status</returns>
        [Plug("AcpiOsGetTableByName")]
        public static int AcpiOsGetTableByName(string Signature, uint Instance, TableHeader** OutTable, ulong** OutAddress)
        {
            Console.WriteLine("AcpiOsGetTableByName");
            return AE_OK;
        }

        /// <summary>
        /// Get current value of the system timer
        /// </summary>
        /// <returns>The current value of the system timer in 100-nanosecond units</returns>
        [Plug("AcpiOsGetTimer")]
        public static ulong AcpiOsGetTimer()
        {
            Console.WriteLine("AcpiOsGetTimer");
            return 0;
        }

        /// <summary>
        /// Break to the debugger or display a breakpoint message
        /// </summary>
        /// <param name="Function">Signal to be sent to the host operating system</param>
        /// <param name="Info"></param>
        /// <returns>Status</returns>
        [Plug("AcpiOsSignal")]
        public static int AcpiOsSignal(uint Function, void* Info)
        {
            if (Function == ACPI_SIGNAL_BREAKPOINT)
            {
                Console.Write("[ACPI] ");
                Console.Write(Util.CharPtrToString((char*)Info));
                Console.Write('\n');
            }
            else /* ACPI_SIGNAL_FATAL */
            {
                Console.WriteLine("[ACPI] Fatal error in AML");
            }

            return AE_OK;
        }

        /// <summary>
        /// A hook before writing sleep registers to enter the sleep state (return AE_CRTL_TERMINATE to skip further sleep register writes)
        /// </summary>
        /// <param name="SleepState">Which sleep state to enter</param>
        /// <param name="RegaValue">Register A value</param>
        /// <param name="RegbValue">Register B value</param>
        /// <returns></returns>
        [Plug("AcpiOsEnterSleep")]
        public static int AcpiOsEnterSleep(byte SleepState, uint RegaValue, uint RegbValue)
        {
            return AE_OK;
        }

        /// <summary>
        /// Initialize all ACPICA globals and sub-components
        /// </summary>
        /// <returns>Status</returns>
        [Extern("AcpiInitializeSubsystem")]
        public static extern int AcpiInitializeSubsystem();

        /// <summary>
        /// Initialize the ACPICA table manager
        /// </summary>
        /// <param name="InitialTableArray">Pointer to an array of pre-allocated memory to put tables. If null, the memory is allocated dynamically</param>
        /// <param name="InitialTableCount">Requested size of the array (if number of tables)</param>
        /// <param name="AllowResize">If resizing the pre-allocated memory is allowed. Ignored if InitialTableArray is null</param>
        /// <returns></returns>
        [Extern("AcpiInitializeTables")]
        public static extern int AcpiInitializeTables(TableDesc* InitialTableArray, uint InitialTableCount, bool AllowResize);

        /// <summary>
        /// Load the ACPI tables and build internal ACPI namespace
        /// </summary>
        /// <returns>Status</returns>
        [Extern("AcpiLoadTables")]
        public static extern int AcpiLoadTables();

        /// <summary>
        /// Complete the ACPICA subsystem initialization and enable ACPI operations
        /// </summary>
        /// <param name="Flags">Specifies how the subsystem should be initialized</param>
        /// <returns>Status</returns>
        [Extern("AcpiEnableSubsystem")]
        public static extern int AcpiEnableSubsystem(uint Flags);

        /// <summary>
        /// Initialize objects within the ACPI namespace
        /// </summary>
        /// <param name="Flags">Specifies how the subsystem should be initialized</param>
        /// <returns>Status</returns>
        [Extern("AcpiInitializeObjects")]
        public static extern int AcpiInitializeObjects(uint Flags);

        /// <summary>
        /// Obtain a specified installed ACPI table
        /// </summary>
        /// <param name="Signature">A pointer to the ACPI signature for the requested table</param>
        /// <param name="Instance">Which table instance, if multiple instances of this table are allowed. (One based)</param>
        /// <param name="Table">A pointer to where the address of the requested ACPI table is returned</param>
        /// <returns>Exception code</returns>
        [Extern("AcpiGetTable")]
        public static extern int AcpiGetTable(string Signature, uint Instance, TableHeader** Table);

        /// <summary>
        /// Prepare to enter a system sleep state
        /// </summary>
        /// <param name="SleepState">The sleep state (1-5)</param>
        /// <returns>Status</returns>
        [Extern("AcpiEnterSleepStatePrep")]
        public static extern int AcpiEnterSleepStatePrep(byte SleepState);

        /// <summary>
        /// Enters a system sleep state
        /// </summary>
        /// <param name="SleepState">The sleep state (1-5)</param>
        /// <returns>Status</returns>
        [Extern("AcpiEnterSleepState")]
        public static extern int AcpiEnterSleepState(byte SleepState);

        /// <summary>
        /// Performs a system reset
        /// </summary>
        /// <returns>Status</returns>
        [Extern("AcpiReset")]
        public static extern int AcpiReset();

        /// <summary>
        /// Get the object handle associated with an ACPI name
        /// </summary>
        /// <param name="Parent">A handle to the parent of the object specified by Pathname</param>
        /// <param name="Pathname">Name or pathname to ACPI object</param>
        /// <param name="OutHandle">A pointer to a location where a handle to the object is to be returned</param>
        /// <returns></returns>
        [Extern("AcpiGetHandle")]
        public static extern int AcpiGetHandle(void* Parent, string Pathname, void** OutHandle);

        /// <summary>
        /// Find and evaluate the given object
        /// </summary>
        /// <param name="Handle">Object handle (optional)</param>
        /// <param name="Pathname">Object pathname (optional)</param>
        /// <param name="ExternalParams">List of parameters</param>
        /// <param name="ReturnBuffer">Where to put the return value of the method. If null, no value is returned</param>
        /// <returns></returns>
        [Extern("AcpiEvaluateObject")]
        public static extern int AcpiEvaluateObject(void* Handle, string Pathname, AcpiObjects.ObjectList* ExternalParams, AcpiObjects.BufferObject* ReturnBuffer);

        /// <summary>
        /// Walk the ACPI namespace to find all object of type Device
        /// </summary>
        /// <param name="HID">A device hardware ID to search for. If null, all objects of type Device are passed to the UserFunction</param>
        /// <param name="UserFunction">A pointer to a function that is called</param>
        /// <param name="UserContext">A value that will be passed as a parameter to the user function each time it is invoked</param>
        /// <param name="ReturnValue">A pointer to a location where the return value is to be placed if the walk terminated early. If null, this is ignored</param>
        /// <returns>Status</returns>
        [Extern("AcpiGetDevices")]
        public static extern int AcpiGetDevices(string HID, WalkCallback UserFunction, void* UserContext, void** ReturnValue);

        /// <summary>
        /// Get information about an ACPI namespace object
        /// </summary>
        /// <param name="Object">A handle to an ACPI object for which information is to be returned</param>
        /// <param name="OutBuffer">A pointer to a location where the device info pointer is returned</param>
        /// <returns>Status</returns>
        [Extern("AcpiGetObjectInfo")]
        public static extern int AcpiGetObjectInfo(void* Object, AcpiObjects.DeviceInfo** OutBuffer);

        /// <summary>
        /// Get the ACPI IRQ Routing Table for an ACPI-related device
        /// </summary>
        /// <param name="Device">A handle to a device object for which the IRQ routing table is to be returned</param>
        /// <param name="OutBuffer">A pointer to a location where the IRQ routing table is to be returned</param>
        /// <returns>Status</returns>
        [Extern("AcpiGetIrqRoutingTable")]
        public static extern int AcpiGetIrqRoutingTable(void* Device, AcpiObjects.Buffer* OutBuffer);

        /// <summary>
        /// Retrieves the current or possible resource list for the specified device. The UserFunction is called once for every resource in the list
        /// </summary>
        /// <param name="DeviceHandle">Handle to the device object</param>
        /// <param name="Name">Method name of the resource</param>
        /// <param name="UserFunction">Called for each resource</param>
        /// <param name="Context">Passed to UserFunction</param>
        /// <returns>Status</returns>
        [Extern("AcpiWalkResources")]
        public static extern int AcpiWalkResources(void* DeviceHandle, string Name, WalkResourceCallback UserFunction, void* Context);
    }
}
