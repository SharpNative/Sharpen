using Sharpen.Arch;
using Sharpen.FileSystem;
using Sharpen.Utilities;

namespace Sharpen.Task
{
    public class Tasking
    {
        private static int m_lastPid = 0;
        private static bool m_taskingEnabled = false;

        public static Task KernelTask { get; private set; }
        public static Task CurrentTask { get; private set; }

        /// <summary>
        /// Initializes tasking
        /// </summary>
        public static unsafe void Init()
        {
            // Critical code, disable interrupts
            CPU.CLI();

            // Kernel task, the data will be filled in when the first schedule happens
            Task kernel = new Task();
            kernel.PID = m_lastPid++;
            kernel.GID = 0;
            kernel.UID = 0;
            kernel.PageDir = Paging.KernelDirectory;
            kernel.Next = null;
            kernel.FPUContext = Heap.AlignedAlloc(16, 512);

            KernelTask = kernel;
            CurrentTask = kernel;

            // Schedule, we are now multitasking
            m_taskingEnabled = true;
            CPU.STI();
            ManualSchedule();
        }

        /// <summary>
        /// Gets a task by its PID
        /// </summary>
        /// <param name="pid">The PID</param>
        /// <returns>The task</returns>
        public static Task GetTaskByPID(int pid)
        {
            Task current = KernelTask;
            while (true)
            {
                // Check
                if (current.PID == pid)
                    return current;

                // Get next if it exists, if we still haven't found
                // it means there is no such task
                current = current.Next;
                if (current == null)
                    return null;
            }
        }

        /// <summary>
        /// Removes a task by its PID
        /// </summary>
        /// <param name="pid">The PID</param>
        public static unsafe void RemoveTaskByPID(int pid)
        {
            Task current = KernelTask;
            Task previous = null;
            while (true)
            {
                // Check
                if (current.PID == pid)
                    break;

                // Get next if it exists, if we still haven't found
                // it means there is no such task
                previous = current;
                current = current.Next;
                if (current == null)
                    return;
            }

            // Wait for task switch if this current task will be removed
            if (current == CurrentTask)
                CPU.HLT();

            // Critical section, a task switch may not occur now
            CPU.CLI();

            // Set pointer and free task data
            previous.Next = current.Next;
            Heap.Free(current.FPUContext);
            Heap.Free(current.Stack);
            Paging.FreeDirectory(current.PageDir);

            // End of critical section
            CPU.STI();
        }

        /// <summary>
        /// Schedules a task
        /// </summary>
        /// <param name="task">The task</param>
        public static void ScheduleTask(Task task)
        {
            // Find the last task
            Task current = CurrentTask;
            while (true)
            {
                if (current.Next == null)
                    break;

                current = current.Next;
            }
            
            // Critical section, a task switch may not occur now
            CPU.CLI();

            // Add
            current.Next = task;

            // End of critical section
            CPU.STI();
        }
        
        /// <summary>
        /// Adds a task
        /// </summary>
        /// <param name="eip">The initial EIP</param>
        /// <param name="priority">The task priority</param>
        public static unsafe void AddTask(void* eip, TaskPriority priority)
        {
            // Fill in data
            Task newTask = new Task();
            newTask.PID = m_lastPid++;
            newTask.GID = 0;
            newTask.UID = 0;
            newTask.PageDir = Paging.CloneDirectory(Paging.CurrentDirectory);
            newTask.TimeFull = (int)priority;

            // Stack
            newTask.Stack = (int*)((int)Heap.AlignedAlloc(16, 8192) + 8192);
            newTask.Stack = writeSchedulerStack(newTask.Stack, 0x1B, 0x23, eip);
            newTask.KernelStack = (int*)((int)Heap.AlignedAlloc(16, 4096) + 4096);

            // Program data space end
            newTask.DataEnd = null;

            // File descriptor
            newTask.FileDescriptors.Capacity = 16;
            newTask.FileDescriptors.Used = 0;
            newTask.FileDescriptors.Nodes = new Node[newTask.FileDescriptors.Capacity];
            newTask.FileDescriptors.Offsets = new uint[newTask.FileDescriptors.Capacity];

            // FPU context
            newTask.FPUContext = Heap.AlignedAlloc(16, 512);
            FPU.StoreContext(newTask.FPUContext);

            // Schedule
            ScheduleTask(newTask);
        }

        /// <summary>
        /// Finds the next task for the scheduler
        /// </summary>
        /// <returns>The next task</returns>
        public static unsafe Task FindNextTask()
        {
            Task current = CurrentTask;

            current.TimeLeft--;
            if (current.TimeLeft > 0)
                return current;
            
            current.TimeLeft = current.TimeFull;

            Task next = CurrentTask.Next;
            if (next == null)
                return KernelTask;

            return next;
        }

        /// <summary>
        /// Scheduler
        /// </summary>
        /// <param name="regsPtr">Pointer to registers</param>
        /// <returns>Pointer to registers</returns>
        private static unsafe Regs* scheduler(Regs* regsPtr)
        {
            // Only do this if tasking is enabled
            if (!m_taskingEnabled)
                return regsPtr;

            // Store old context
            Task oldTask = CurrentTask;
            oldTask.Stack = (int*)regsPtr;
            oldTask.KernelStack = (int*)GDT.TSS_Entry->ESP0;
            FPU.StoreContext(oldTask.FPUContext);

            // Switch to next task
            Task current = FindNextTask();
            Paging.CurrentDirectory = current.PageDir;
            FPU.RestoreContext(current.FPUContext);
            GDT.TSS_Entry->ESP0 = (uint)current.KernelStack;
            CurrentTask = current;

            // Return the next task context
            return (Regs*)current.Stack;
        }

        /// <summary>
        /// Writes a scheduler stack
        /// </summary>
        /// <param name="ptr">The pointer to the stack</param>
        /// <param name="cs">The Code Segment</param>
        /// <param name="ds">The Data Segment</param>
        /// <param name="eip">The return EIP value</param>
        /// <returns>The new pointer</returns>
        private static unsafe int* writeSchedulerStack(int* ptr, int cs, int ds, void* eip)
        {
            int esp = (int)ptr;

            // Data pushed by CPU
            *--ptr = ds;        // Data Segment
            *--ptr = esp;       // Old stack
            *--ptr = 0x200;     // EFLAGS
            *--ptr = cs;        // Code Segment
            *--ptr = (int)eip;  // Initial EIP

            // Pushed by pusha
            *--ptr = 0;         // EAX
            *--ptr = 0;         // ECX
            *--ptr = 0;         // EDX
            *--ptr = 0;         // EBX
             --ptr;
            *--ptr = 0;         // EBP
            *--ptr = 0;         // ESI
            *--ptr = 0;         // EDI

            // Data segments
            *--ptr = ds;        // DS
            *--ptr = ds;        // ES
            *--ptr = ds;        // FS
            *--ptr = ds;        // GS

            // New location of stack
            return ptr;
        }

        /// <summary>
        /// Gets a node from the descriptor
        /// </summary>
        /// <param name="descriptor">The descriptor</param>
        /// <returns>The node</returns>
        public static Node GetNodeFromDescriptor(int descriptor)
        {
            Task current = CurrentTask;
            if (descriptor >= current.FileDescriptors.Capacity)
                return null;

            return current.FileDescriptors.Nodes[descriptor];
        }

        /// <summary>
        /// Gets an offset of a node from the descriptor
        /// </summary>
        /// <param name="descriptor">The descriptor</param>
        /// <returns>The offset</returns>
        public static uint GetOffsetFromDescriptor(int descriptor)
        {
            Task current = CurrentTask;
            if (descriptor >= current.FileDescriptors.Capacity)
                return 0;

            return current.FileDescriptors.Offsets[descriptor];
        }

        /// <summary>
        /// Adds a node to the file descriptor
        /// </summary>
        /// <param name="node">The node to add</param>
        /// <returns>The file descriptor ID</returns>
        public static unsafe int AddNodeToDescriptor(Node node)
        {
            Task current = CurrentTask;
            if (current.FileDescriptors.Used == current.FileDescriptors.Capacity)
            {
                // Expand if needed
                int oldCap = current.FileDescriptors.Capacity;
                current.FileDescriptors.Capacity += 8;

                Node[] newNodeArray = new Node[current.FileDescriptors.Capacity];
                uint[] newOffsetArray = new uint[current.FileDescriptors.Capacity];

                Memory.Memcpy(Util.ObjectToVoidPtr(newNodeArray), Util.ObjectToVoidPtr(current.FileDescriptors.Nodes), oldCap * sizeof(void*));
                Memory.Memcpy(Util.ObjectToVoidPtr(newOffsetArray), Util.ObjectToVoidPtr(current.FileDescriptors.Offsets), oldCap * sizeof(uint));

                current.FileDescriptors.Nodes = newNodeArray;
                current.FileDescriptors.Offsets = newOffsetArray;
            }

            // Find a free descriptor
            int i = 0;
            for (; i < current.FileDescriptors.Capacity; i++)
            {
                if(current.FileDescriptors.Nodes[i] == null)
                {
                    current.FileDescriptors.Nodes[i] = node;
                    current.FileDescriptors.Offsets[i] = 0;
                    break;
                }
            }

            current.FileDescriptors.Used++;
            return i;
        }

        /// <summary>
        /// Manually calls the task scheduler
        /// </summary>
        public static extern void ManualSchedule();
    }
}
