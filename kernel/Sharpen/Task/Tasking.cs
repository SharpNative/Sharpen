using Sharpen.Arch;
using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Task
{
    public class Tasking
    {
        private static int m_lastPid = 0;
        private static bool m_taskingEnabled = false;

        private static Task m_taskToClone = null;

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
            kernel.Flags = TaskFlags.NOFLAGS;
            kernel.FileDescriptors.Capacity = 0;

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
            
            // Critical section, a task switch may not occur now
            CPU.CLI();

            // Set pointer and set remove flag
            previous.Next = current.Next;
            current.Flags |= TaskFlags.DESCHEDULED;

            // End of critical section
            CPU.STI();

            // Wait for task switch if this current task is descheduled
            while (true)
                ManualSchedule();
        }

        /// <summary>
        /// Schedules a task
        /// </summary>
        /// <param name="task">The task</param>
        public static void ScheduleTask(Task task)
        {
            // Find the last task
            Task current = CurrentTask;
            while (current.Next != null)
            {
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
        /// <param name="initialStack">Initial stack</param>
        /// <param name="initialStackSize">Initial stack size</param>
        public static unsafe Task AddTask(void* eip, TaskPriority priority, int[] initialStack, int initialStackSize)
        {
            // Fill in data
            Task newTask = new Task();
            newTask.PID = m_lastPid++;
            newTask.GID = 0;
            newTask.UID = 0;
            newTask.TimeFull = (int)priority;
            newTask.TimeLeft = (int)priority;
            newTask.Flags = TaskFlags.NOFLAGS;

            // Stack
            int* stacks = (int*)Heap.AlignedAlloc(16, 4096 + 8192);
            newTask.StackStart = (int*)((int)stacks + 4096);
            newTask.Stack = (int*)((int)newTask.StackStart + 8192);
            
            // Copy initial stack
            if (initialStackSize > 0)
            {
                // TODO: workaround for compiler bug
                int* stack = newTask.Stack;
                for (int i = 0; i < initialStackSize; i++)
                {
                    *--stack = initialStack[i];
                }
                newTask.Stack = stack;
            }

            // Continue with stacks
            newTask.Stack = writeSchedulerStack(newTask.Stack, 0x1B, 0x23, eip);
            newTask.KernelStackStart = stacks;
            newTask.KernelStack = (int*)((int)newTask.KernelStackStart + 4096);
            
            // Program data space end
            newTask.DataEnd = null;

            cloneDescriptors(newTask, CurrentTask);
            
            // FPU context
            newTask.FPUContext = Heap.AlignedAlloc(16, 512);
            FPU.StoreContext(newTask.FPUContext);

            return newTask;
        }

        /// <summary>
        /// Forks the current task
        /// </summary>
        /// <returns>0 if child, PID if parent</returns>
        public static int Fork()
        {
            int pid = CurrentTask.PID;
            m_taskToClone = CurrentTask;
            ManualSchedule();
            return (pid == CurrentTask.PID ? pid + 1 : 0);
        }

        /// <summary>
        /// Clones a task and schedules it
        /// </summary>
        /// <param name="sourceTask">The task to clone</param>
        private static unsafe void cloneTask(Task sourceTask)
        {
            // Fill in data
            Task newTask = new Task();
            newTask.PID = m_lastPid++;
            newTask.GID = sourceTask.GID;
            newTask.UID = sourceTask.UID;
            newTask.TimeFull = sourceTask.TimeFull;
            newTask.TimeLeft = sourceTask.TimeFull;
            newTask.Flags = TaskFlags.NOFLAGS;

            // Stack
            int* stacks = (int*)Heap.AlignedAlloc(16, 4096 + 8192);
            newTask.StackStart = (int*)((int)stacks + 4096);
            newTask.KernelStackStart = stacks;

            Memory.Memcpy(newTask.KernelStackStart, sourceTask.KernelStackStart, 4096 + 8192);

            int diffStack = (int)sourceTask.Stack - (int)sourceTask.StackStart;
            int diffKernelStack = (int)sourceTask.KernelStack - (int)sourceTask.KernelStackStart;

            newTask.Stack = (int*)((int)newTask.StackStart + diffStack);
            newTask.KernelStack = (int*)((int)newTask.KernelStackStart + diffKernelStack);

            // Program data space end
            newTask.DataEnd = sourceTask.DataEnd;

            cloneDescriptors(newTask, sourceTask);

            // FPU context
            newTask.FPUContext = Heap.AlignedAlloc(16, 512);
            Memory.Memcpy(newTask.FPUContext, sourceTask.FPUContext, 512);

            // Paging
            newTask.PageDir = Paging.CloneDirectory(sourceTask.PageDir);

            // Schedule
            ScheduleTask(newTask);
        }

        /// <summary>
        /// Clones descriptors
        /// </summary>
        /// <param name="destination">The destination</param>
        /// <param name="source">The source</param>
        private static void cloneDescriptors(Task destination, Task source)
        {
            // Fresh file descriptors
            if (source.FileDescriptors.Capacity == 0)
            {
                destination.FileDescriptors.Capacity = 16;
                destination.FileDescriptors.Used = 0;
                destination.FileDescriptors.Nodes = new Node[destination.FileDescriptors.Capacity];
                destination.FileDescriptors.Offsets = new uint[destination.FileDescriptors.Capacity];
                return;
            }
            
            // File descriptors
            destination.FileDescriptors.Capacity = source.FileDescriptors.Capacity;
            destination.FileDescriptors.Used = source.FileDescriptors.Used;
            destination.FileDescriptors.Nodes = new Node[destination.FileDescriptors.Capacity];
            destination.FileDescriptors.Offsets = new uint[destination.FileDescriptors.Capacity];

            // Copy file descriptors
            int cap = source.FileDescriptors.Capacity;
            for (int i = 0; i < cap; i++)
            {
                destination.FileDescriptors.Nodes[i] = source.FileDescriptors.Nodes[i];
                destination.FileDescriptors.Offsets[i] = source.FileDescriptors.Offsets[i];
            }
        }

        /// <summary>
        /// Cleans up the given task
        /// </summary>
        /// <param name="task">The task</param>
        private static unsafe void cleanupTask(Task task)
        {
            // TODO: more cleaning required
            Heap.Free(task.FPUContext);
            Heap.Free(task.KernelStackStart);
            Paging.FreeDirectory(task.PageDir);
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

            // Context is stored, now we can manipulate it
            // such as forking etc
            if (m_taskToClone != null)
            {
                cloneTask(m_taskToClone);
                m_taskToClone = null;
            }

            // Switch to next task
            Task current = FindNextTask();
            Paging.CurrentDirectory = current.PageDir;
            FPU.RestoreContext(current.FPUContext);
            GDT.TSS_Entry->ESP0 = (uint)current.KernelStack;
            CurrentTask = current;

            // Cleanup old task
            if ((oldTask.Flags & TaskFlags.DESCHEDULED) == TaskFlags.DESCHEDULED)
            {
                cleanupTask(oldTask);
            }

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
            *--ptr = 0x202;     // EFLAGS
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
                if (current.FileDescriptors.Nodes[i] == null)
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
