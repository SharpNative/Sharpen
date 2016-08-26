using Sharpen.Arch;
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
        public const int SYS_EXECVE = 9;
        public const int SYS_READDIR = 10;

        // Highest syscall number
        public const int SYSCALL_MAX = 10;
        
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
        public static unsafe int Sbrk(int increase)
        {
            return (int)Paging.AllocateVirtual(increase);
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
        /// Replaces the current process with another executable
        /// </summary>
        /// <param name="path">The path to the executable</param>
        /// <param name="argv">The arguments</param>
        /// <param name="envp">The environment path</param>
        /// <returns>Errorcode</returns>
        public static int Execve(string path, string[] argv, string[] envp)
        {
            // TODO: envp
            ErrorCode error = Loader.StartProcess(path, argv);
            if (error != ErrorCode.SUCCESS)
                return -(int)error;
            
            // We spawned a task but the current process should actually be replaced
            // So we must kill the current process
            Tasking.RemoveTaskByPID(Tasking.CurrentTask.PID);

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
    }
}
