using Sharpen.Arch;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Task
{
    public class Tasking
    {
        private static int m_nextPid = 0;
        private static bool m_taskingEnabled = false;

        private static Task m_taskToClone = null;

        public static Task KernelTask { get; private set; }
        public static Task CurrentTask { get; private set; }

        public enum SpawnFlags
        {
            NONE = 0,
            SWAP_PID = 1
        }

        /// <summary>
        /// Initializes tasking
        /// </summary>
        public static unsafe void Init()
        {
            // Critical code, disable interrupts
            CPU.CLI();

            // Kernel task, the data will be filled in when the first schedule happens
            Task kernel = new Task();
            kernel.PID = m_nextPid++;
            kernel.GID = 0;
            kernel.UID = 0;
            kernel.PageDir = Paging.KernelDirectory;
            kernel.Next = null;
            kernel.FPUContext = Heap.AlignedAlloc(16, 512);
            kernel.Flags = Task.TaskFlags.NOFLAGS;
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
            current.Flags |= Task.TaskFlags.DESCHEDULED;

            // End of critical section
            CPU.STI();

            // Wait for task switch if this current task is descheduled
            while (true)
                ManualSchedule();
        }

        /// <summary>
        /// Dump info about tasks
        /// </summary>
        private static unsafe void dump()
        {
            Task current = KernelTask;
            while (true)
            {
                Console.WriteHex((int)Util.ObjectToVoidPtr(current));
                Console.Write(" ");
                Console.WriteHex(current.PID);
                Console.WriteLine("");

                current = current.Next;
                if (current == null)
                    return;
            }
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
        /// <param name="flags">Spawn flags</param>
        public static unsafe Task CreateTask(void* eip, TaskPriority priority, int[] initialStack, int initialStackSize, SpawnFlags flags)
        {
            // Fill in data
            Task newTask = new Task();
            
            newTask.PID = m_nextPid++;

            if ((flags & SpawnFlags.SWAP_PID) == SpawnFlags.SWAP_PID)
            {
                CPU.CLI();
                int old = newTask.PID;
                newTask.PID = CurrentTask.PID;
                CurrentTask.PID = old;
                CPU.STI();
            }

            newTask.GID = 0;
            newTask.UID = 0;
            newTask.TimeFull = (int)priority;
            newTask.TimeLeft = (int)priority;
            newTask.Flags = Task.TaskFlags.NOFLAGS;

            // Stack
            int* stacks = (int*)Heap.AlignedAlloc(16, 4096 + 8192);
            newTask.StackStart = (int*)((int)stacks + 4096);
            newTask.Stack = (int*)((int)newTask.StackStart + 8192);

            // Copy initial stack
            if (initialStackSize > 0)
            {
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

            // FS related stuff
            CurrentTask.CloneDescriptorsTo(newTask);
            newTask.CurrentDirectory = String.Clone(CurrentTask.CurrentDirectory);

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
            return (pid == CurrentTask.PID ? m_nextPid - 1 : 0);
        }

        /// <summary>
        /// Clones a task and schedules it
        /// </summary>
        /// <param name="sourceTask">The task to clone</param>
        private static unsafe void cloneTask(Task sourceTask)
        {
            // Fill in data
            Task newTask = new Task();
            newTask.PID = m_nextPid++;
            newTask.GID = sourceTask.GID;
            newTask.UID = sourceTask.UID;
            newTask.TimeFull = sourceTask.TimeFull;
            newTask.TimeLeft = sourceTask.TimeFull;
            newTask.Flags = Task.TaskFlags.NOFLAGS;

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

            // FS related stuff
            sourceTask.CloneDescriptorsTo(newTask);
            newTask.CurrentDirectory = String.Clone(sourceTask.CurrentDirectory);

            // FPU context
            newTask.FPUContext = Heap.AlignedAlloc(16, 512);
            Memory.Memcpy(newTask.FPUContext, sourceTask.FPUContext, 512);

            // Paging
            newTask.PageDir = Paging.CloneDirectory(sourceTask.PageDir);

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
            if ((oldTask.Flags & Task.TaskFlags.DESCHEDULED) == Task.TaskFlags.DESCHEDULED)
            {
                oldTask.Cleanup();
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
        /// Manually calls the task scheduler
        /// </summary>
        public static extern void ManualSchedule();
    }
}
