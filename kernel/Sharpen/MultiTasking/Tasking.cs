using Sharpen.Arch;
using Sharpen.Collections;

namespace Sharpen.MultiTasking
{
    public class Tasking
    {
        // TODO: mutexes (note that some code is guaranteed to run only in one place, so no mutexes there)

        private static bool taskingEnabled = false;

        private static List sleepingTasks;

        private static Task taskToClone = null;

        public static Task KernelTask { get; private set; }
        public static Task CurrentTask { get; private set; }
        
        /// <summary>
        /// Initializes tasking
        /// </summary>
        public static unsafe void Init()
        {
            // Keeps track of which tasks need to wake up
            sleepingTasks = new List();

            // Kernel task, the remaining data will be filled in when the first schedule happens
            // The Idle task has a low priority
            Task kernel = new Task(TaskPriority.LOW, Task.SpawnFlags.KERNEL_TASK);
            kernel.Context.CreateKernelContext();
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
            oldTask.StoreContext(regsPtr);

            // Switch to next task
            Task current = FindNextTask();
            void* stack = current.RestoreContext();
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
                Task newTask = taskToClone.Clone();
                taskToClone = null;
                ScheduleTask(newTask);
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
