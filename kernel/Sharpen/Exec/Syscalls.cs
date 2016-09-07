using Sharpen.Arch;
using Sharpen.Drivers.Power;
using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.Task;

namespace Sharpen.Exec
{
    public class Syscalls
    {
        #region Syscall numbers

        public const int SYS_EXIT = 0;
        public const int SYS_GETPID = 1;
        public const int SYS_SBRK = 2;
        public const int SYS_FORK = 3;
        public const int SYS_WRITE = 4;
        public const int SYS_READ = 5;
        public const int SYS_OPEN = 6;
        public const int SYS_CLOSE = 7;
        public const int SYS_SEEK = 8;
        public const int SYS_FSTAT = 9;
        public const int SYS_STAT = 10;
        public const int SYS_EXECVE = 11;
        public const int SYS_RUN = 12;
        public const int SYS_WAITPID = 13;
        public const int SYS_READDIR = 14;
        public const int SYS_SHUTDOWN = 15;
        public const int SYS_REBOOT = 16;
        public const int SYS_GETTIMEOFDAY = 17;

        // Highest syscall number
        public const int SYSCALL_MAX = 17;
        
        #endregion

        /// <summary>
        /// Exit syscall
        /// </summary>
        /// <param name="status">The status code</param>
        /// <returns>Nothing</returns>
        public static int Exit(int status)
        {
            Tasking.RemoveTaskByPID(Tasking.CurrentTask.PID);
            return 0;
        }

        /// <summary>
        /// GetPID syscall
        /// </summary>
        /// <returns>The current PID</returns>
        public static int GetPID()
        {
            return Tasking.CurrentTask.PID;
        }

        /// <summary>
        /// Sbrk syscall
        /// </summary>
        /// <param name="increase">The amount to increase the memory with</param>
        /// <returns>The previous data space end</returns>
        public static unsafe void* Sbrk(int increase)
        {
            void* c = Paging.AllocateVirtual(increase);
            return c;
        }

        /// <summary>
        /// Fork syscall
        /// </summary>
        /// <returns>0 if we're child, PID of child if we're parent</returns>
        public static unsafe int Fork()
        {
            // Note that kernel stack and user stack are different
            Task.Task current = Tasking.CurrentTask;
            int diffRegs = (int)current.SysRegs - (int)current.StackStart;
            int diffESP = current.SysRegs->ESP - (int)current.StackStart;
            
            int pid = Tasking.Fork();
            
            // Update stack references within the stack itself
            current = Tasking.CurrentTask;
            current.SysRegs = (Regs*)((int)current.StackStart + diffRegs);
            current.SysRegs->ESP = (int)current.StackStart + diffESP;

            return pid;
        }

        /// <summary>
        /// Write to a file descriptor
        /// </summary>
        /// <param name="descriptor">The descriptor ID</param>
        /// <param name="buffer">The buffer</param>
        /// <param name="size">The size</param>
        /// <returns>The amount of bytes written</returns>
        public static int Write(int descriptor, byte[] buffer, uint size)
        {
            Node node = Tasking.GetNodeFromDescriptor(descriptor);
            if (node == null)
                return -(int)ErrorCode.EBADF;
            
            uint offset = Tasking.GetOffsetFromDescriptor(descriptor);
            Tasking.CurrentTask.FileDescriptors.Offsets[descriptor] += size;
            
            return (int)VFS.Write(node, offset, size, buffer);
        }

        /// <summary>
        /// Read from a file descriptor
        /// </summary>
        /// <param name="descriptor">The descriptor ID</param>
        /// <param name="buffer">The buffer</param>
        /// <param name="size">The size</param>
        /// <returns>The amount of bytes read</returns>
        public static int Read(int descriptor, byte[] buffer, uint size)
        {
            Node node = Tasking.GetNodeFromDescriptor(descriptor);
            if (node == null)
                return -(int)ErrorCode.EBADF;

            uint offset = Tasking.GetOffsetFromDescriptor(descriptor);
            Tasking.CurrentTask.FileDescriptors.Offsets[descriptor] += size;

            return (int)VFS.Read(node, offset, size, buffer);
        }

        /// <summary>
        /// Opens a file
        /// </summary>
        /// <param name="path">The path</param>
        /// <param name="flags">The flags</param>
        /// <returns>The file descriptor ID</returns>
        public static int Open(string path, int flags)
        {
            Node node = VFS.GetByPath(path);
            if (node == null)
                return -(int)ErrorCode.ENOENT;
            
            VFS.Open(node, (FileMode)flags);
            return Tasking.AddNodeToDescriptor(node);
        }

        /// <summary>
        /// Closes a file
        /// </summary>
        /// <param name="descriptor">The descriptor ID</param>
        /// <returns>The errorcode</returns>
        public static int Close(int descriptor)
        {
            Node node = Tasking.GetNodeFromDescriptor(descriptor);
            if (node == null)
                return -(int)ErrorCode.EBADF;

            VFS.Close(node);

            Tasking.CurrentTask.FileDescriptors.Nodes[descriptor] = null;
            Tasking.CurrentTask.FileDescriptors.Used--;

            return 0;
        }

        /// <summary>
        /// Sets the file position of the descriptor to the given offset
        /// </summary>
        /// <param name="descriptor">The descriptor</param>
        /// <param name="offset">The offset</param>
        /// <param name="whence">The direction</param>
        /// <returns>The new offset from the beginning of the file in bytes</returns>
        public static int Seek(int descriptor, uint offset, FileWhence whence)
        {
            Node node = Tasking.GetNodeFromDescriptor(descriptor);
            if (node == null)
                return -(int)ErrorCode.EBADF;

            if (whence == FileWhence.SEEK_CUR)
                Tasking.CurrentTask.FileDescriptors.Offsets[descriptor] += offset;
            else if (whence == FileWhence.SEEK_SET)
                Tasking.CurrentTask.FileDescriptors.Offsets[descriptor] = offset;
            else /* if (whence == FileWhence.SEEK_END) */
                Tasking.CurrentTask.FileDescriptors.Offsets[descriptor] = node.Size - offset;

            return (int)Tasking.CurrentTask.FileDescriptors.Offsets[descriptor];
        }

        /// <summary>
        /// Gets the file status of a descriptor
        /// </summary>
        /// <param name="descriptor">The descriptor ID</param>
        /// <param name="st">The stat structure</param>
        /// <returns>The errorcode</returns>
        public static unsafe int FStat(int descriptor, Stat* st)
        {
            Node node = Tasking.GetNodeFromDescriptor(descriptor);
            if (node == null)
                return -(int)ErrorCode.EBADF;

            node.Stat(st);
            return 0;
        }

        /// <summary>
        /// Gets the file status of a path
        /// </summary>
        /// <param name="path">The path</param>
        /// <param name="st">The stat structure</param>
        /// <returns>The errorcode</returns>
        public static unsafe int Stat(string path, Stat* st)
        {
            Node node = VFS.GetByPath(path);
            if (node == null)
                return -(int)ErrorCode.ENOENT;

            node.Stat(st);
            return 0;
        }

        /// <summary>
        /// Replaces the current process with another executable
        /// </summary>
        /// <param name="path">The path to the executable</param>
        /// <param name="argv">The arguments</param>
        /// <param name="envp">The environment path</param>
        /// <returns>Errorcode</returns>
        public static int Execve(string path, string[] argv, string[] envp)
        {
            // TODO: envp
            int error = Loader.StartProcess(path, argv);
            if (error < 0)
                return error;

            // We spawned a task but the current process should actually be replaced
            // So we must kill the current process
            Tasking.RemoveTaskByPID(Tasking.CurrentTask.PID);

            return 0;
        }

        /// <summary>
        /// Creates another process from an executable
        /// </summary>
        /// <param name="path">The path to the executable</param>
        /// <param name="argv">The arguments</param>
        /// <param name="envp">The environment path</param>
        /// <returns>Errorcode</returns>
        public static int Run(string path, string[] argv, string[] envp)
        {
            // TODO: envp
            int pid = Loader.StartProcess(path, argv);
            return pid;
        }

        /// <summary>
        /// Waits for (a) process(es) to exit
        /// </summary>
        /// <param name="pid">The PID or other identification</param>
        /// <param name="status">Pointer to status</param>
        /// <param name="options">Options</param>
        /// <returns>The error code</returns>
        public static unsafe int WaitPID(int pid, int* status, int options)
        {
            // Wait for specific PID
            if (pid > 0)
            {
                // If the task is still found, it means it's still there
                while (Tasking.GetTaskByPID(pid) != null)
                    Tasking.ManualSchedule();
            }
            // Wait for any child process whose group ID == calling process group ID
            else if (pid == 0)
            {

            }
            // Wait for any child process
            else if (pid == -1)
            {

            }
            // Wait for any child process whose group ID == calling process group ID
            else if (pid < -1)
            {

            }

            return 0;
        }

        /// <summary>
        /// Reads a directory entry
        /// </summary>
        /// <param name="descriptor">The descriptor</param>
        /// <param name="entry">The directory entry</param>
        /// <param name="index">The index</param>
        /// <returns>Errorcode</returns>
        public static unsafe int Readdir(int descriptor, DirEntry* entry, uint index)
        {
            Node node = Tasking.GetNodeFromDescriptor(descriptor);
            if (node == null)
                return -(int)ErrorCode.EBADF;

            DirEntry* gotEntry = VFS.ReadDir(node, index);
            if (gotEntry == null)
                return -(int)ErrorCode.ENOENT;

            Memory.Memcpy(entry, gotEntry, sizeof(DirEntry));
            Heap.Free(gotEntry);
            return 0;
        }

        /// <summary>
        /// Shuts down the computer
        /// </summary>
        /// <returns>The error code</returns>
        public static int Shutdown()
        {
            if (Tasking.CurrentTask.UID > 0)
                return -(int)ErrorCode.EPERM;
            
            Acpi.Shutdown();
            return 0;
        }

        /// <summary>
        /// Reboots the computer
        /// </summary>
        /// <returns>The error code</returns>
        public static int Reboot()
        {
            if (Tasking.CurrentTask.UID > 0)
                return -(int)ErrorCode.EPERM;

            Acpi.Reset();
            return 0;
        }

        /// <summary>
        /// Gets the current time of day
        /// </summary>
        /// <param name="tv">The time structure</param>
        /// <returns>The errorcode</returns>
        public static unsafe int GetTimeOfDay(Time.Timeval* tv)
        {
            tv->tv_sec = (ulong)PIT.FullTicks;
            tv->tv_usec = (ulong)(PIT.SubTicks * 1000000 / PIT.Frequency);
            return 0;
        }
    }
}
