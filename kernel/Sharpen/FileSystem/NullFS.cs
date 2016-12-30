﻿using Sharpen.Mem;
using Sharpen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.FileSystem
{
    class NullFS
    {
        private static Node m_currentNode;
        
        /// <summary>
        /// Initializes Null device
        /// </summary>
        public unsafe static void Init()
        {

            Device device = new Device();
            device.Name = "null";
            device.node = new Node();
            device.node.Read = readImpl;
            device.node.Write = writeImpl;

            DevFS.RegisterDevice(device);
        }

        /// <summary>
        /// Read from null device
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes read</returns>
        private unsafe static uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            Memory.Memset((char *)Util.ObjectToVoidPtr(buffer) + offset, 0x00, (int)size);

            return size;
        }


        /// <summary>
        /// Write method for filesystem
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes written</returns>
        private static uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            return size;
        }
    }
}