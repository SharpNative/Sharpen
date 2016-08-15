﻿using Sharpen.FileSystem;

namespace Sharpen.Exec
{
    public class Loader
    {
        /// <summary>
        /// Starts a new process based on the given path and arguments
        /// </summary>
        /// <param name="path">The path</param>
        /// <param name="argv">The arguments</param>
        /// <returns>Errorcode</returns>
        public static ErrorCode StartProcess(string path, string[] argv)
        {
            Node node = VFS.GetByPath(path);
            if (node == null)
                return ErrorCode.ENOENT;

            // Open and create buffer
            VFS.Open(node, FileMode.O_RDONLY);
            byte[] buffer = new byte[node.Size];
            if (buffer == null)
            {
                VFS.Close(node);
                return ErrorCode.ENOMEM;
            }

            // Fill buffer contents
            VFS.Read(node, 0, node.Size, buffer);
            VFS.Close(node);

            // Pass execution to ELF loader
            return ELFLoader.Execute(buffer, node.Size, argv);
        }
    }
}