using Sharpen.Arch;
using Sharpen.Mem;

namespace Sharpen.MultiTasking
{
    public class Tasking
    {
        public static Task KernelTask { get; private set; }
        public static Task CurrentTask { get; private set; }
        public static bool IsActive { get; private set; }

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

            // Do initial task switch to setup kernel task
            IsActive = true;
            Yield();
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
            while (current.PID != pid && current.NextTask != KernelTask)
            {
                current = current.NextTask;
            }

            if (current.PID == pid)
                return current;

            return null;
        }

        /// <summary>
        /// Removes a task by its PID
        /// </summary>
        /// <param name="pid">The PID</param>
        public static unsafe void RemoveTaskByPID(int pid)
        {
            Task current = KernelTask;
            Task previous = null;
            while (current.PID != pid && current.NextTask != KernelTask)
            {
                previous = current;
                current = current.NextTask;
            }

            if (current == KernelTask)
                return;

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
                Yield();
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
        /// Gets the next task for the scheduler
        /// </summary>
        /// <returns>The next task</returns>
        public static Task GetNextTask()
        {
            Task current = CurrentTask;

            // Only keep going with this task if we're not sleeping
            if (!current.IsSleeping() && !current.HasFlag(Task.TaskFlag.STOPPED))
            {
                // Decrease the time, if there is still time left, keep going with this task
                current.TimeLeft--;
                if (current.TimeLeft > 0)
                    return current;

                // Time is up, reset time to full time
                current.TimeLeft = (int)current.Priority;
            }

            // Sleeping and stopped processes
            Task next = current.NextTask;
            while (!next.HasFlag(Task.TaskFlag.STOPPED))
            {
                next.AwakeThreads();
                if (!next.IsSleeping() && !next.HasFlag(Task.TaskFlag.STOPPED))
                    break;

                next = next.NextTask;
            }

            // Get the next task
            return next;
        }

        /// <summary>
        /// Scheduler
        /// </summary>
        /// <param name="context">Old context</param>
        /// <returns>New context</returns>
        public static unsafe void* Scheduler(void* context)
        {
            Task oldTask = CurrentTask;
            
            oldTask.StoreThreadContext(context);
            oldTask.SwitchToNextThread();
            
            Task nextTask = GetNextTask();
            if (oldTask != nextTask)
            {
                nextTask.PrepareContext();
            }

            void* newContext = nextTask.RestoreThreadContext();
            CurrentTask = nextTask;
            
            // Cleanup old task
            if (oldTask.HasFlag(Task.TaskFlag.DESCHEDULED))
            {
                oldTask.Cleanup();
                Heap.Free(oldTask);
            }

            // Return the next task context
            return newContext;
        }

        /// <summary>
        /// Manually calls the task scheduler
        /// </summary>
        public static extern void Yield();
    }
}
