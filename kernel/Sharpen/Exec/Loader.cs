using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.MultiTasking;

namespace Sharpen.Exec
{
    public class Loader
    {
        /// <summary>
        /// Starts a new process based on the given path and arguments
        /// </summary>
        /// <param name="path">The path</param>
        /// <param name="argv">The arguments</param>
        /// <param name="flags">Spawn flags</param>
        /// <returns>Errorcode or PID</returns>
        public static int StartProcess(string path, string[] argv, Task.SpawnFlags flags)
        {
            if (argv == null)
                Panic.DoPanic("argv == null");
            
            Node node = VFS.GetByAbsolutePath(path);
            if (node == null)
                return -(int)ErrorCode.ENOENT;
            
            // Open and create buffer
            VFS.Open(node, (int)FileMode.O_RDONLY);
            byte[] buffer = new byte[node.Size];
            if (buffer == null)
            {
                Heap.Free(node);
                VFS.Close(node);
                return -(int)ErrorCode.ENOMEM;
            }
            
            // Fill buffer contents
            VFS.Read(node, 0, node.Size, buffer);
            VFS.Close(node);

            // Pass execution to ELF loader
            int status = ELFLoader.Execute(buffer, node.Size, argv, flags);
            Heap.Free(buffer);
            Heap.Free(node);
            return status;
        }
    }
}
