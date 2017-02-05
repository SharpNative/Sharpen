﻿using Sharpen.FileSystem;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Net
{
    public class UDPFS
    {
        public enum OPT
        {
            LIST,
            BIND,
            SOCK
        }

        /// <summary>
        /// Initializes UDP filesystem in NetFS
        /// </summary>
        public static unsafe void Init()
        {
            Device dev = new Device();
            dev.Name = "udp";
            dev.Node = new Node();
            dev.Node.FindDir = findDirImpl;
            dev.Node.ReadDir = readDirImpl;
            dev.Node.Flags = NodeFlags.DIRECTORY;

            UDPFSCookie cookie = new UDPFSCookie(OPT.LIST);
            dev.Node.Cookie = (ICookie)cookie;

            NetFS.RegisterDevice(dev);
        }

        /// <summary>
        /// FS finddir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="name">The name to look for</param>
        /// <returns>The node</returns>
        private static unsafe Node findDirImpl(Node node, string name)
        {
            UDPFSCookie cookie = (UDPFSCookie)node.Cookie;
            OPT opt = cookie.Opt;

            if (opt == OPT.LIST)
            {
                if (name.Equals("bind"))
                    return byID(OPT.BIND);
                else if (name.Equals("connect"))
                    return byID(OPT.SOCK);
            }
            else if (opt == OPT.SOCK)
            {
                return UDPSocketDevice.Open(name);
            }
            else if (opt == OPT.BIND)
            {
                return UDPBindSocketDevice.Open(name);
            }

            return null;
        }

        /// <summary>
        /// Creates a node
        /// </summary>
        /// <param name="opt">The option</param>
        /// <returns>The node</returns>
        private static unsafe Node byID(OPT opt)
        {
            Node node = new Node();
            node.Flags = NodeFlags.DIRECTORY;
            node.FindDir = findDirImpl;

            UDPFSCookie cookie = new UDPFSCookie(opt);
            node.Cookie = (ICookie)cookie;

            return node;
        }

        /// <summary>
        /// Creates a directory entry with a name
        /// </summary>
        /// <param name="str">The entry name</param>
        /// <returns>The entry</returns>
        private static unsafe DirEntry* makeByName(string str)
        {
            DirEntry* entry = (DirEntry*)Heap.Alloc(sizeof(DirEntry));
            Memory.Memcpy(entry->Name, Util.ObjectToVoidPtr(str), str.Length + 1);
            return entry;
        }
        
        /// <summary>
        /// FS readdir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="index">The index</param>
        /// <returns>The directory entry</returns>
        private static unsafe DirEntry* readDirImpl(Node node, uint index)
        {
            UDPFSCookie cookie = (UDPFSCookie)node.Cookie;
            OPT opt = cookie.Opt;

            // Do list ;)
            if (opt == OPT.LIST)
            {
                if (index == 0)
                    return makeByName("bind");
                else if (index == 1)
                    return makeByName("connect");
            }

            return null;
        }
    }
}
