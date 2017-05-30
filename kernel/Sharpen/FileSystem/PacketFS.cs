using Sharpen.Collections;
using Sharpen.FileSystem.Cookie;
using Sharpen.Lib;
using Sharpen.Mem;
using Sharpen.MultiTasking;
using Sharpen.Synchronisation;
using Sharpen.Utilities;

namespace Sharpen.FileSystem
{
    public unsafe class PacketFSPacket
    {
        public uint Length { get; set; }

        public byte* buffer { get; set; }
    }


    class PacketFSEntry
    {

        /// <summary>
        /// Name of de packerfs entry
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Control PID
        /// </summary>
        public int ControlPID { get; set; }

        /// <summary>
        /// Control queue
        /// </summary>
        public Queue ControlQueue { get; set; }

        /// <summary>
        /// List of PID queue
        /// </summary>
        public Dictionary Queue { get; set; }

        /// <summary>
        /// Mutex to control
        /// </summary>
        public Mutex ControlMutex { get; set; }

        /// <summary>
        /// Mutex to control queues
        /// </summary>
        public Mutex QueueMutex { get; set; }
    }

    /// <summary>
    /// TODO: mutexes are a mess
    /// </summary>
    class PacketFS
    {
        
        private static StringDictionary m_children;
        
        private static Mutex m_mutex { get; set; }

        /// <summary>
        /// Initializes a filesystem that is a container
        /// </summary>
        public static unsafe void Init()
        {
            m_children = new StringDictionary(8);
            m_mutex = new Mutex();

            Node node = new Node();
            node.ReadDir = readDirImpl;
            node.FindDir = findDirImpl;
            node.Create = Create;
            
            RootPoint dev = new RootPoint("packetfs", node);
            VFS.RootMountPoint.AddEntry(dev);
        }

        #region Misc

        public static PacketFSEntry GetEntryByName(string name)
        {
            m_mutex.Lock();

            PacketFSEntry entry = (PacketFSEntry)m_children.Get(name);

            m_mutex.Unlock();

            return entry;
        }

        public static Queue GetQueueByPID(PacketFSEntry entry, int pid)
        {
            entry.QueueMutex.Lock();
            
            Queue queue = (Queue)entry.Queue.GetByKey(pid);

            entry.QueueMutex.Unlock();

            return queue;
        }

        #endregion

        #region Entry FS actions


        /// <summary>
        /// FS open enty
        /// </summary>
        /// <param name="node">The node</param>
        private static unsafe void open(Node node)
        {
            PacketFSCookie cookie = (PacketFSCookie)node.Cookie;

            PacketFSEntry entry = GetEntryByName(cookie.PacketFSEntry);

            // Entry found?
            if (entry == null)
                return;

            int pid = Tasking.CurrentTask.PID;

            // Do we need to create a queue for him?
            if(pid != entry.ControlPID)
            {

                entry.QueueMutex.Lock();

                Queue queue = (Queue)entry.Queue.GetByKey(pid); 

                // Do we even need to create him?
                if(queue != null)
                {
                    entry.QueueMutex.Unlock();

                    return;
                }

                // Create queue and add him
                Queue newFif = new Queue();

                entry.Queue.Add(pid, newFif);

                entry.QueueMutex.Unlock();
            }
        }

        /// <summary>
        /// FS write entry
        /// </summary>
        /// <param name="node"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="buffer"></param>
        private static unsafe uint write(Node node, uint offset, uint size, byte[] buffer)
        {

            PacketFSCookie cookie = (PacketFSCookie)node.Cookie;

            PacketFSEntry entry = GetEntryByName(cookie.PacketFSEntry);

            if (entry == null)
                return 0;

            int pid = Tasking.CurrentTask.PID;

            if (entry.ControlPID == pid)
            {

                if (size < 4)
                    return 0;

                byte* buf = (byte *)Util.ObjectToVoidPtr(buffer);
                byte* dataBuf = buf + 4;

                int packetSize = (int)size - 4;

                int targetPID = *(int *)buf;


                Queue queue = GetQueueByPID(entry, pid);

                // Queue for pid even exists?
                if (queue == null)
                    return 0;

                PacketFSPacket packet = new PacketFSPacket();
                packet.buffer = (byte *)Heap.Alloc(packetSize);
                packet.Length = (uint)packetSize;

                Memory.Memcpy(packet.buffer, dataBuf, packetSize);

                queue.Push(Util.ObjectToVoidPtr(packet));

                return (uint)packetSize;
            }
            else
            {

                int totalSize = (int)size + 4;
                
                byte* buf = (byte*)Heap.Alloc((int)size);
                byte* dataBuf = buf + 4;

                // Set PID
                *(int*)buf = pid;

                PacketFSPacket packet = new PacketFSPacket();
                packet.buffer = (byte*)Heap.Alloc(totalSize);
                packet.Length = (uint)totalSize;

                entry.ControlQueue.Push(Util.ObjectToVoidPtr(packet));

                return size;
            }
        }

        /// <summary>
        /// Fs read entry
        /// </summary>
        /// <param name="node"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="buffer"></param>
        private static unsafe uint read(Node node, uint offset, uint size, byte[] buffer)
        {

            PacketFSCookie cookie = (PacketFSCookie)node.Cookie;

            PacketFSEntry entry = GetEntryByName(cookie.PacketFSEntry);

            if (entry == null)
                return 0;

            int pid = Tasking.CurrentTask.PID;

            if(entry.ControlPID == pid)
            {

                PacketFSPacket packet = (PacketFSPacket)Util.VoidPtrToObject(entry.ControlQueue.Pop());

                uint readSize = packet.Length;
                if (readSize > size)
                    readSize = size;

                Memory.Memcpy(Util.ObjectToVoidPtr(buffer), packet.buffer, (int)readSize);

                Heap.Free(packet.buffer);
                //Heap.Free(Util.ObjectToVoidPtr(packet));

                return readSize;
            }
            else
            {
                Queue queue = GetQueueByPID(entry, pid);
                
                if (queue == null)
                    return 0;
                
                PacketFSPacket packet = (PacketFSPacket)Util.VoidPtrToObject(queue.Pop());

                uint readSize = packet.Length;
                if (readSize > size)
                    readSize = size;

                Memory.Memcpy(Util.ObjectToVoidPtr(buffer), packet.buffer, (int)readSize);

                Heap.Free(packet.buffer);
                //Heap.Free(Util.ObjectToVoidPtr(packet));

                return readSize;

            }
        }

        /// <summary>
        /// FS close entry (eg: remove packetfsentry)
        /// </summary>
        /// <param name="node">The node</param>
        private static unsafe void close(Node node)
        {

            int pid = Tasking.CurrentTask.PID;
            PacketFSCookie cookie = (PacketFSCookie)node.Cookie;

            m_mutex.Lock();

            PacketFSEntry entry = (PacketFSEntry)m_children.Get(cookie.PacketFSEntry);

            // Entry even found?
            if (entry == null)
            {
                m_mutex.Unlock();

                return;
            }

            // Do we only need to remove the pid from the queue?
            if (pid != entry.ControlPID)
            {
                m_mutex.Unlock();

                entry.QueueMutex.Lock();

                entry.Queue.Remove(pid);

                entry.QueueMutex.Unlock();

                return;
            }

            // Remove entry
            m_children.Remove(cookie.PacketFSEntry);

            m_mutex.Unlock();
        }

        #endregion

        #region Root FS actions

        /// <summary>
        /// FS finddir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="name">The name to look for</param>
        /// <returns>The node</returns>
        private static unsafe Node findDirImpl(Node node, string name)
        {
            PacketFSCookie cookie = (PacketFSCookie)node.Cookie;

            PacketFSEntry entry = GetEntryByName(cookie.PacketFSEntry);
            if (entry == null)
                return null;

            Node returnNode = new Node();
            returnNode.Cookie = new PacketFSCookie(name);
            returnNode.Close = close;
            returnNode.Open = open;
            returnNode.Write = write;
            returnNode.Read = read;

            return returnNode;
        }

        private static unsafe Node Create(Node node, string name)
        {

            m_mutex.Lock();
            object obj = m_children.Get(name);

            if (obj != null)
            {
                m_mutex.Unlock();

                return null;
            }

            PacketFSEntry entry = CreatePacketFS(name, Tasking.CurrentTask.PID);

            m_children.Add(name, entry);

            m_mutex.Unlock();

            Node returnNode = new Node();
            returnNode.Cookie = new PacketFSCookie(name);
            returnNode.Close = close;
            returnNode.Open = open;
            returnNode.Write = write;
            returnNode.Read = read;

            return returnNode;
        }

        /// <summary>
        /// Create packet fs with current PID as controlPID
        /// </summary>
        /// <param name="name">Name of packetfs</param
        /// <param name="controlPID">Control PID</param>
        /// <returns>PacketFSEntry</returns>
        public static PacketFSEntry CreatePacketFS(string name, int controlPID)
        {

            PacketFSEntry fsEntry = new PacketFSEntry();
            fsEntry.Name = name;
            fsEntry.ControlPID = controlPID;
            fsEntry.ControlMutex = new Mutex();

            fsEntry.Queue = new Dictionary();
            fsEntry.QueueMutex = new Mutex();

            return fsEntry;
        }


        /// <summary>
        /// FS readdir
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="index">The index</param>
        /// <returns>The directory entry</returns>
        private static unsafe DirEntry* readDirImpl(Node node, uint index)
        {
            m_mutex.Lock();
            PacketFSEntry dev = (PacketFSEntry)m_children.GetAt(index);
            m_mutex.Unlock();

            if (dev == null)
                return null;

            DirEntry* entry = (DirEntry*)Heap.Alloc(sizeof(DirEntry));
            String.CopyTo(entry->Name, dev.Name);

            return entry;
        }

        #endregion
    }
}
