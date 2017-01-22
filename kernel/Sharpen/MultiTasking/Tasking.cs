using Sharpen.Arch;
using Sharpen.Mem;

namespace Sharpen.MultiTasking
{
    public class Tasking
    {
        private static Thread threadToClone = null;

        public static Task KernelTask { get; private set; }
        public static Task CurrentTask { get; private set; }

        /// <summary>
        /// Initializes tasking
        /// </summary>
        public static unsafe void Init()
        {
            // Kernel task
            // Note: The remaining data will be filled in when the first task switch happens
            Task kernel = new Task(TaskPriority.NORMAL, Task.SpawnFlags.KERNEL_TASK);
            kernel.Context.CreateNewContext(true);
            kernel.AddThread(new Thread());
            kernel.Name = "Kernel";
            kernel.CMDLine = "kernel";

            KernelTask = kernel;
            CurrentTask = kernel;
            kernel.NextTask = kernel;

            // Enable interrupts, because we can now do multitasking
            CPU.STI();
            ManualSchedule();
            Console.WriteLine("[Tasking] Initialized");
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
                current = current.NextTask;
                if (current == KernelTask)
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
                current = current.NextTask;
                if (current == KernelTask)
                    return;
            }

            // Critical section, a task switch may not occur now
            CPU.CLI();

            // Set pointer and set remove flag
            previous.NextTask = current.NextTask;
            current.AddFlag(Task.TaskFlag.DESCHEDULED);
            current.TimeLeft = 1;

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
            while (current.NextTask != KernelTask)
            {
                current = current.NextTask;
            }

            // Critical section, a task switch may not occur now
            CPU.CLI();

            // Add
            current.NextTask = task;
            task.NextTask = KernelTask;

            // End of critical section
            CPU.STI();
        }

        /// <summary>
        /// Forks the current task
        /// </summary>
        /// <returns>0 if child, PID if parent</returns>
        public static int SetForkingThread(Thread thread)
        {
            int pid = thread.OwningTask.PID;
            threadToClone = thread;
            ManualSchedule();
            return (pid == CurrentTask.PID ? Task.NextPID - 1 : 0);
        }

        /// <summary>
        /// Gets the next task for the scheduler
        /// </summary>
        /// <returns>The next task</returns>
        public static Task GetNextTask()
        {
            Task current = CurrentTask;

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

            // Sleeping
            Task next = current.NextTask;
            while (next.IsSleeping())
            {
                next.AwakeThreads();
                if (!next.IsSleeping())
                    break;
                next = next.NextTask;
            }

            // Get the next task
            return next;
        }

        /// <summary>
        /// Scheduler
        /// </summary>
        /// <param name="regsPtr">Old stack</param>
        /// <returns>New stack</returns>
        private static unsafe Regs* scheduler(Regs* regsPtr)
        {
            Task oldTask = CurrentTask;
            oldTask.StoreThreadContext(regsPtr);
            oldTask.SwitchToNextThread();

            Task nextTask = GetNextTask();

            if (oldTask != nextTask)
            {
                nextTask.PrepareContext();
            }

            void* stack = nextTask.RestoreThreadContext();
            CurrentTask = nextTask;

            // Check for cloning
            if (threadToClone != null)
            {
                Task newTask = threadToClone.OwningTask.Clone();
                newTask.AddThread(threadToClone.Clone());
                threadToClone = null;
                ScheduleTask(newTask);
            }

            // Cleanup old task
            if (oldTask.HasFlag(Task.TaskFlag.DESCHEDULED))
            {
                oldTask.Cleanup();
                Heap.Free(oldTask);
            }

            // Return the next task context
            return (Regs*)stack;
        }

        /// <summary>
        /// Manually calls the task scheduler
        /// </summary>
        public static extern void ManualSchedule();
    }
}
