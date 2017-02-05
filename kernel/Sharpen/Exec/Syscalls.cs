using Sharpen.Arch;
using Sharpen.Drivers.Power;
using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.MultiTasking;
using Sharpen.Utilities;

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
        public const int SYS_PIPE = 18;
        public const int SYS_DUP2 = 19;
        public const int SYS_SIG_SEND = 20;
        public const int SYS_SETSIGHANDLER = 21;
        public const int SYS_RETURN_FROM_SIGNAL = 22;
        public const int SYS_GETCWD = 23;
        public const int SYS_CHDIR = 24;
        public const int SYS_TIMES = 25;
        public const int SYS_SLEEP = 26;
        public const int SYS_TRUNCATE = 27;
        public const int SYS_FTRUNCATE = 28;
        public const int SYS_DUP = 29;
        public const int SYS_IOCTL = 30;
        public const int SYS_MKDIR = 31;
        public const int SYS_RMDIR = 32;
        public const int SYS_UNLINK = 33;

        // Highest syscall number
        public const int SYSCALL_MAX = 33;

        #endregion

        #region Constants

        private const int WNOHANG = 1;
        private const int O_NONBLOCK = 0x4000;
        private const int MAX_PATH = 4096;

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
            return Tasking.CurrentTask.Context.Sbrk(increase);
        }

        /// <summary>
        /// Fork syscall
        /// </summary>
        /// <returns>0 if we're child, PID of child if we're parent</returns>
        public static unsafe int Fork()
        {
            return Tasking.CurrentTask.Fork();
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
            FileDescriptors descriptors = Tasking.CurrentTask.FileDescriptors;

            Node node = descriptors.GetNode(descriptor);
            if (node == null)
                return -(int)ErrorCode.EBADF;

            uint offset = descriptors.GetOffset(descriptor);
            descriptors.SetOffset(descriptor, offset + size);

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
            FileDescriptors descriptors = Tasking.CurrentTask.FileDescriptors;

            Node node = descriptors.GetNode(descriptor);
            if (node == null)
                return -(int)ErrorCode.EBADF;

            // Can't do read from a directory
            if ((node.Flags & NodeFlags.DIRECTORY) == NodeFlags.DIRECTORY)
                return -(int)ErrorCode.EISDIR;

            bool isNonBlocking = ((node.OpenFlags & O_NONBLOCK) == O_NONBLOCK);
            
            // Wait until data is available if its blocking
            if (!isNonBlocking)
            {
                while (VFS.GetSize(node) == 0)
                    Tasking.Yield();
            }
            // Non-blocking but no data available?
            else
            {
                if (VFS.GetSize(node) == 0)
                    return -(int)ErrorCode.EAGAIN;
            }

            uint offset = descriptors.GetOffset(descriptor);
            uint readBytes = VFS.Read(node, offset, size, buffer);
            descriptors.SetOffset(descriptor, offset + readBytes);

            return (int)readBytes;
        }

        /// <summary>
        /// Opens a file
        /// </summary>
        /// <param name="path">The path</param>
        /// <param name="flags">The flags</param>
        /// <returns>The file descriptor ID</returns>
        public static int Open(string path, int flags)
        {
            if (path.Length == 0)
                return -(int)ErrorCode.EINVAL;
            
            Node node = VFS.GetByPath(path);
            if (node == null)
                return -(int)ErrorCode.ENOENT;

            VFS.Open(node, flags);

            FileDescriptors descriptors = Tasking.CurrentTask.FileDescriptors;
            return descriptors.AddNode(node);
        }

        /// <summary>
        /// Closes a file
        /// </summary>
        /// <param name="descriptor">The descriptor ID</param>
        /// <returns>The errorcode</returns>
        public static int Close(int descriptor)
        {
            FileDescriptors descriptors = Tasking.CurrentTask.FileDescriptors;
            return descriptors.Close(descriptor);
        }

        /// <summary>
        /// Sets the file position of the descriptor to the given offset
        /// </summary>
        /// <param name="descriptor">The descriptor</param>
        /// <param name="offset">The offset</param>
        /// <param name="whence">The direction</param>
        /// <returns>The new offset from the beginning of the file in bytes</returns>
        public static int Seek(int descriptor, int offset, FileWhence whence)
        {
            FileDescriptors descriptors = Tasking.CurrentTask.FileDescriptors;

            Node node = descriptors.GetNode(descriptor);
            if (node == null)
                return -(int)ErrorCode.EBADF;

            uint currentOffset = descriptors.GetOffset(descriptor);

            if (whence == FileWhence.SEEK_CUR)
            {
                currentOffset = (uint)(currentOffset + offset);
            }
            else if (whence == FileWhence.SEEK_SET)
            {
                if (offset < 0)
                    currentOffset = 0;
                else
                    currentOffset = (uint)offset;
            }
            else if (whence == FileWhence.SEEK_END)
            {
                if (offset > 0)
                    currentOffset = node.Size;
                else
                    currentOffset = (uint)(node.Size + offset);
            }
            else
            {
                return -(int)ErrorCode.EINVAL;
            }
            
            descriptors.SetOffset(descriptor, currentOffset);

            return (int)currentOffset;
        }

        /// <summary>
        /// Gets the file status of a descriptor
        /// </summary>
        /// <param name="descriptor">The descriptor ID</param>
        /// <param name="st">The stat structure</param>
        /// <returns>The errorcode</returns>
        public static unsafe ErrorCode FStat(int descriptor, Stat* st)
        {
            FileDescriptors descriptors = Tasking.CurrentTask.FileDescriptors;

            Node node = descriptors.GetNode(descriptor);
            if (node == null)
                return ErrorCode.EBADF;

            node.Stat(st);
            return ErrorCode.SUCCESS;
        }

        /// <summary>
        /// Gets the file status of a path
        /// </summary>
        /// <param name="path">The path</param>
        /// <param name="st">The stat structure</param>
        /// <returns>The errorcode</returns>
        public static unsafe ErrorCode Stat(string path, Stat* st)
        {
            Node node = VFS.GetByPath(path);
            if (node == null)
                return ErrorCode.ENOENT;

            node.Stat(st);
            Heap.Free(node);
            return ErrorCode.SUCCESS;
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
            path = VFS.CreateAbsolutePath(path);
            int error = Loader.StartProcess(path, argv, Task.SpawnFlags.SWAP_PID);
            Heap.Free(path);
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
            path = VFS.CreateAbsolutePath(path);
            int pid = Loader.StartProcess(path, argv, Task.SpawnFlags.NONE);
            Heap.Free(path);
            return pid;
        }

        /// <summary>
        /// Waits for (a) process(es) to exit
        /// </summary>
        /// <param name="pid">The PID or other identification</param>
        /// <param name="status">Pointer to status</param>
        /// <param name="options">Options</param>
        /// <returns>The error code</returns>
        public static unsafe ErrorCode WaitPID(int pid, int* status, int options)
        {
            // Wait for specific PID
            if (pid > 0)
            {
                // Don't wait, just check
                if ((options & WNOHANG) == WNOHANG)
                {
                    if (Tasking.GetTaskByPID(pid) != null)
                        return ErrorCode.SUCCESS;
                    else
                        return ErrorCode.ECHILD;
                }

                // If the task is still found, it means it's still there
                while (Tasking.GetTaskByPID(pid) != null)
                    Tasking.Yield();
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

            return ErrorCode.SUCCESS;
        }

        /// <summary>
        /// Reads a directory entry
        /// </summary>
        /// <param name="descriptor">The descriptor</param>
        /// <param name="entry">The directory entry</param>
        /// <param name="index">The index</param>
        /// <returns>Errorcode</returns>
        public static unsafe ErrorCode Readdir(int descriptor, DirEntry* entry, uint index)
        {
            FileDescriptors descriptors = Tasking.CurrentTask.FileDescriptors;

            Node node = descriptors.GetNode(descriptor);
            if (node == null)
                return ErrorCode.EBADF;

            DirEntry* gotEntry = VFS.ReadDir(node, index);
            if (gotEntry == null)
                return ErrorCode.ENOENT;

            Memory.Memcpy(entry, gotEntry, sizeof(DirEntry));
            Heap.Free(gotEntry);

            return ErrorCode.SUCCESS;
        }

        /// <summary>
        /// Shuts down the computer
        /// </summary>
        /// <returns>The error code</returns>
        public static ErrorCode Shutdown()
        {
            if (Tasking.CurrentTask.UID > 0)
                return ErrorCode.EPERM;

            Acpi.Shutdown();
            return ErrorCode.SUCCESS;
        }

        /// <summary>
        /// Reboots the computer
        /// </summary>
        /// <returns>The error code</returns>
        public static ErrorCode Reboot()
        {
            if (Tasking.CurrentTask.UID > 0)
                return ErrorCode.EPERM;

            Acpi.Reboot();
            return ErrorCode.SUCCESS;
        }

        /// <summary>
        /// Gets the current time of day
        /// </summary>
        /// <param name="tv">The time structure</param>
        /// <returns>The errorcode</returns>
        public static unsafe ErrorCode GetTimeOfDay(Time.Timeval* tv)
        {
            tv->tv_sec = PIT.FullTicks;
            tv->tv_usec = (PIT.SubTicks * 1000000 / PIT.Frequency);
            return ErrorCode.SUCCESS;
        }

        /// <summary>
        /// Creates a UNIX pipe
        /// </summary>
        /// <param name="pipefd">The pipe file descriptors (output)</param>
        /// <returns>The errorcode</returns>
        public static unsafe ErrorCode Pipe(int* pipefd)
        {
            Node[] nodes = new Node[2];
            ErrorCode error = PipeFS.Create(nodes, 4096);
            if (error != ErrorCode.SUCCESS)
            {
                Heap.Free(nodes);
                return error;
            }

            FileDescriptors descriptors = Tasking.CurrentTask.FileDescriptors;
            pipefd[0] = descriptors.AddNode(nodes[0]);
            pipefd[1] = descriptors.AddNode(nodes[1]);

            Heap.Free(nodes);

            return ErrorCode.SUCCESS;
        }

        /// <summary>
        /// Duplicates a file descriptor to the lowest unused file descriptor
        /// </summary>
        /// <param name="fd">The file descriptor to clone</param>
        /// <returns>The cloned file descriptor</returns>
        public static int Dup(int fd)
        {
            FileDescriptors descriptors = Tasking.CurrentTask.FileDescriptors;
            return descriptors.Dup(fd);
        }

        /// <summary>
        /// Replaces an old file descriptor with a new one
        /// </summary>
        /// <param name="oldfd">The old file descriptor</param>
        /// <param name="newfd">The new file descriptor</param>
        /// <returns>The new file descriptor or errorcode</returns>
        public static int Dup2(int oldfd, int newfd)
        {
            FileDescriptors descriptors = Tasking.CurrentTask.FileDescriptors;
            return descriptors.Dup2(oldfd, newfd);
        }
        
        /// <summary>
        /// Changes the current directory to newDir
        /// </summary>
        /// <param name="newDir">The new working directory</param>
        /// <returns>Errorcode</returns>
        public static ErrorCode ChDir(string newDir)
        {
            newDir = VFS.CreateAbsolutePath(newDir);
            Node node = VFS.GetByAbsolutePath(newDir);
            
            if (node == null)
            {
                Heap.Free(newDir);
                return ErrorCode.ENOENT;
            }
            
            if (node.Flags != NodeFlags.DIRECTORY)
            {
                Heap.Free(newDir);
                Heap.Free(node);
                return ErrorCode.ENOTDIR;
            }

            Heap.Free(node);
            
            // GetByAbsolutePath makes sure there's a slash on the end
            Tasking.CurrentTask.CurrentDirectory = newDir;

            return ErrorCode.SUCCESS;
        }

        /// <summary>
        /// Gets the current working directory
        /// </summary>
        /// <param name="destination">The destination buffer</param>
        /// <param name="size">The size of the destination buffer</param>
        /// <returns>Errorcode</returns>
        public static unsafe ErrorCode GetCWD(char* destination, int size)
        {
            if (size > MAX_PATH)
                size = MAX_PATH;

            fixed (char* ptr = Tasking.CurrentTask.CurrentDirectory)
            {
                Memory.Memcpy(destination, ptr, size);
                destination[size] = '\0';
            }

            return ErrorCode.SUCCESS;
        }

        /// <summary>
        /// Sleeps some time
        /// </summary>
        /// <param name="seconds">Whole seconds</param>
        /// <param name="usec">Microseconds</param>
        /// <returns>The amount of time the task still needs to sleep</returns>
        public static uint Sleep(uint seconds, uint usec)
        {
            return Tasking.CurrentTask.CurrentThread.Sleep(seconds, usec);
        }

        /// <summary>
        /// Truncates a file by path
        /// </summary>
        /// <param name="path">The path of the file to truncate</param>
        /// <param name="length">The length to truncate to</param>
        /// <returns>Errorcode</returns>
        public static int Truncate(string path, uint length)
        {
            Node node = VFS.GetByPath(path);
            if (node == null)
                return -(int)ErrorCode.ENOENT;

            VFS.Truncate(node, length);
            Heap.Free(node);
            return 0;
        }

        /// <summary>
        /// Truncates a file by a file descriptor
        /// </summary>
        /// <param name="descriptor">The file descriptor</param>
        /// <param name="length">The length to truncate to</param>
        /// <returns>Errorcode</returns>
        public static int FTruncate(int descriptor, uint length)
        {
            FileDescriptors descriptors = Tasking.CurrentTask.FileDescriptors;

            Node node = descriptors.GetNode(descriptor);
            if (node == null)
                return -(int)ErrorCode.EBADF;

            VFS.Truncate(node, length);
            return 0;
        }

        /// <summary>
        /// Manipulates underlying parameters in files
        /// </summary>
        /// <param name="fd">The file descriptor</param>
        /// <param name="request">The request</param>
        /// <param name="arg">An optional argument</param>
        /// <returns>The errorcode or return value from IOCtl</returns>
        public static unsafe int IOCtl(int fd, int request, void* arg)
        {
            FileDescriptors descriptors = Tasking.CurrentTask.FileDescriptors;

            Node node = descriptors.GetNode(fd);
            if (node == null)
                return -(int)ErrorCode.EBADF;

            return VFS.IOCtl(node, request, arg);
        }

        /// <summary>
        /// Creates a directory
        /// </summary>
        /// <param name="path">The path</param>
        /// <param name="mode">The mode</param>
        /// <returns>The errorcode</returns>
        public static int MkDir(string path, int mode)
        {
            // TODO: implement (needs splitting the path and passing the directory name to the parent directory in VFS)
            return -1;
        }

        /// <summary>
        /// Removes a directory
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The errorcode</returns>
        public static int RmDir(string path)
        {
            // TODO: implement (needs splitting the path and passing the directory name to the parent directory in VFS)
            return -1;
        }

        /// <summary>
        /// Removes a file
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The errorcode</returns>
        public static int Unlink(string path)
        {
            // TODO: implement (needs splitting the path and passing the directory name to the parent directory in VFS)
            return -1;
        }

        /// <summary>
        /// Sets a signal handler
        /// </summary>
        /// <param name="sig">The signal number</param>
        /// <param name="handler">The handler</param>
        /// <returns>The errorcode</returns>
        public static unsafe ErrorCode SetSigHandler(Signal sig, SignalAction.SigAction* newact, SignalAction.SigAction* oldact)
        {
            if (sig == Signal.SIGKILL || sig == Signal.SIGSTOP)
                return ErrorCode.EINVAL;

            if (newact == null)
                return ErrorCode.EFAULT;

            return Tasking.CurrentTask.SetSignalHandler(sig, newact, oldact);
        }

        /// <summary>
        /// Sends a signal to a process
        /// </summary>
        /// <param name="pid">The PID</param>
        /// <param name="sig">The signal</param>
        /// <returns>The errorcode</returns>
        public static ErrorCode SigSend(int pid, Signal sig)
        {
            if (pid <= 0)
                return ErrorCode.EINVAL;

            Task task = Tasking.GetTaskByPID(pid);
            if (task == null)
                return ErrorCode.ESRCH;

            return task.ProcessSignal(sig);
        }

        /// <summary>
        /// Returns from a signal (restores original context)
        /// </summary>
        public static void ReturnFromSignal()
        {
            Tasking.CurrentTask.CurrentThread.ReturnFromSignal();
        }
    }
}
