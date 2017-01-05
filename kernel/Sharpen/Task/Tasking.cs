using Sharpen.Arch;
using Sharpen.Collections;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Task
{
    public class Tasking
    {
        // TODO: mutexes (note that some code is guaranteed to run only in one place, so no mutexes there)

        private static bool taskingEnabled = false;

        private static List sleepingTasks;

        private static Task taskToClone = null;

        public static Task KernelTask { get; private set; }
        public static Task CurrentTask { get; private set; }

        public enum SpawnFlags
        {
            NONE = 0,
            SWAP_PID = 1,
            KERNEL_TASK = 2
        }

        /// <summary>
        /// Initializes tasking
        /// </summary>
        public static unsafe void Init()
        {
            // Keeps track of which tasks need to wake up
            sleepingTasks = new List();

            // Kernel task, the remaining data will be filled in when the first schedule happens
            // The Idle task has a low priority
            Task kernel = new Task(TaskPriority.LOW);
            kernel.PageDirVirtual = Paging.KernelDirectory;
            kernel.PageDirPhysical = Paging.KernelDirectory;
            kernel.FPUContext = Heap.AlignedAlloc(16, 512);

            KernelTask = kernel;
            CurrentTask = kernel;

            // Enable interrupts, because we can now do multitasking
            taskingEnabled = true;
            CPU.STI();
            ManualSchedule();

            Console.WriteLine("[Tasking] Initialized");
        }

        /// <summary>
        /// Adds a task to the sleeping list
        /// </summary>
        /// <param name="task">The task</param>
        public static void AddToSleepingList(Task task)
        {
            sleepingTasks.Add(task);
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
            current.AddFlag(Task.TaskFlag.DESCHEDULED);

            // End of critical section
            CPU.STI();

            // The task is descheduled, we need to wait until this task is removed, do task switch
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
        /// <param name="flags">Spawn flags</param>
        public static unsafe Task CreateTask(void* eip, TaskPriority priority, int[] initialStack, int initialStackSize, SpawnFlags flags)
        {
            // Fill in data
            Task newTask = new Task(priority);

            if ((flags & SpawnFlags.SWAP_PID) == SpawnFlags.SWAP_PID)
            {
                int old = newTask.PID;
                newTask.PID = CurrentTask.PID;
                CurrentTask.PID = old;
            }

            newTask.GID = 0;
            newTask.UID = 0;

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

            int cs = Task.USERSPACE_CS;
            int ds = Task.USERSPACE_DS;
            if ((flags & SpawnFlags.KERNEL_TASK) != SpawnFlags.KERNEL_TASK)
            {
                // FS related stuff
                newTask.SetFileDescriptors(CurrentTask.FileDescriptors.Clone());
                newTask.CurrentDirectory = String.Clone(CurrentTask.CurrentDirectory);
            }
            else
            {
                // Kernel descriptors
                cs = Task.KERNEL_CS;
                ds = Task.KERNEL_DS;
            }

            // Continue with stacks
            newTask.Stack = writeSchedulerStack(newTask.Stack, cs, ds, eip);
            newTask.KernelStackStart = stacks;
            newTask.KernelStack = (int*)((int)newTask.KernelStackStart + 4096);

            // Program data space end
            newTask.DataEnd = null;

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
            taskToClone = CurrentTask;
            CPU.STI();
            CPU.HLT();
            CPU.CLI();
            return (pid == CurrentTask.PID ? Task.NextPID - 1 : 0);
        }

        /// <summary>
        /// Clones a task and schedules it
        /// </summary>
        /// <param name="sourceTask">The task to clone</param>
        private static unsafe void cloneTask(Task sourceTask)
        {
            // Fill in data
            Task newTask = new Task(sourceTask.Priority);
            newTask.GID = sourceTask.GID;
            newTask.UID = sourceTask.UID;

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
            newTask.SetFileDescriptors(sourceTask.FileDescriptors.Clone());
            newTask.CurrentDirectory = String.Clone(sourceTask.CurrentDirectory);

            // FPU context
            newTask.FPUContext = Heap.AlignedAlloc(16, 512);
            Memory.Memcpy(newTask.FPUContext, sourceTask.FPUContext, 512);

            // Paging
            newTask.PageDirVirtual = Paging.CloneDirectory(sourceTask.PageDirVirtual);
            newTask.PageDirPhysical = newTask.PageDirVirtual->PhysicalDirectory;

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
            Task next = null;

            // Check if there are tasks that need to wake up
            int sleeping = sleepingTasks.Count;
            if (sleeping > 0)
            {
                for (int i = sleeping - 1; i >= 0; i--)
                {
                    Task task = (Task)sleepingTasks.Item[i];
                    if (!task.IsSleeping())
                    {
                        sleepingTasks.RemoveAt(i);
                        next = task;
                    }
                }
            }

            // If a task has woke up, switch to that task
            if (next != null)
                return next;

            // Only keep going with this task if we're not sleeping
            if (!current.IsSleeping())
            {
                // Decrease the time, if there is still time left, keep going with this task
                current.TimeLeft--;
                if (current.TimeLeft > 0)
                    return current;

                // Time is up, reset time to full time
                current.TimeLeft = (int)current.Priority;
            }

            // Get the next task to switch to and if there is no next task, go to the kernel task
            next = current.Next;
            if (next == null)
                return KernelTask;

            // Skip until a non-sleeping task has been found
            while (next.IsSleeping())
            {
                // Get next task
                next = next.Next;

                // No next task? Go the the Kernel Idle task
                if (next == null)
                {
                    next = KernelTask;
                    break;
                }
            }

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
            if (!taskingEnabled)
                return regsPtr;

            // Store old context
            Task oldTask = CurrentTask;
            oldTask.Stack = (int*)regsPtr;
            oldTask.KernelStack = (int*)GDT.TSS_Entry->ESP0;
            FPU.StoreContext(oldTask.FPUContext);

            // Switch to next task
            Task current = FindNextTask();
            Paging.SetPageDirectory(current.PageDirVirtual, current.PageDirPhysical);
            FPU.RestoreContext(current.FPUContext);
            GDT.TSS_Entry->ESP0 = (uint)current.KernelStack;
            CurrentTask = current;

            // Cleanup old task
            if (oldTask.HasFlag(Task.TaskFlag.DESCHEDULED))
            {
                oldTask.Cleanup();
            }

            // Context is stored, now we can manipulate it
            // such as forking etc
            if (taskToClone != null)
            {
                Task toClone = taskToClone;
                taskToClone = null;
                cloneTask(toClone);
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
