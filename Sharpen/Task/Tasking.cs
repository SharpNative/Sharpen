using Sharpen.Arch;

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
            newTask.PageDir = Paging.KernelDirectory;
            newTask.TimeFull = (int)priority;

            // Stack
            newTask.Stack = (int*)((int)Heap.AlignedAlloc(16, 8192) + 8192);
            newTask.Stack = writeSchedulerStack(newTask.Stack, 0x08, 0x10, eip);

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
            FPU.StoreContext(oldTask.FPUContext);

            // Switch to next task
            Task current = FindNextTask();
            Paging.CurrentDirectory = current.PageDir;
            FPU.RestoreContext(current.FPUContext);
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
