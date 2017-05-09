using Sharpen.Arch;
using Sharpen.Collections;
using Sharpen.FileSystem.Cookie;
using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.FileSystem
{
    // TODO: change this
    public enum AudioActions
    {
        Master = 0,
        PCM_OUT = 1
    }

    class AudioFS
    {
        public const int BufferSize = 4096;

        public struct SoundDevice
        {
            public string Name;
            public SoundDeviceWriter Writer;
            public SoundDeviceReader Reader;
        }

        public unsafe delegate void SoundDeviceWriter(AudioActions action, uint value);
        public unsafe delegate uint SoundDeviceReader(AudioActions action);

        private static SoundDevice m_device;

        private static List m_buffers;
        private static ContainerFS m_container;
        private static ushort[] m_tmpBuffer;

        /// <summary>
        /// Set sound Device
        /// </summary>
        /// <param name="device"></param>
        public static void SetDevice(SoundDevice device)
        {
            // TODO: support multiple audio devices
            m_device = device;

            Node node = new Node();
            node.Open = openImpl;
            node.Write = writeImpl;
            node.Close = closeImpl;

            RootPoint point = new RootPoint("default", node);
            m_container.AddEntry(point);
        }

        /// <summary>
        /// Request a new audio buffer
        /// </summary>
        /// <param name="size">The required size</param>
        /// <param name="buffer">The buffer to put the data into</param>
        public unsafe static void RequestBuffer(uint size, ushort[] buffer)
        {
            fixed (ushort* ptr = m_tmpBuffer)
                Memory.Memclear(ptr, BufferSize * sizeof(ushort));

            for (int i = 0; i < m_buffers.Count; i++)
            {
                Fifo fifo = (Fifo)m_buffers.Item[i];

                uint read = fifo.Read(Util.PtrToArray((byte*)Util.ObjectToVoidPtr(m_tmpBuffer)), size * sizeof(ushort));

                for (int j = 0; j < /*read / 2*/BufferSize; j++)
                {
                    buffer[j] = (ushort)((m_tmpBuffer[j] >> 8) | (m_tmpBuffer[j] & 0xFF));
                }
            }
        }

        /// <summary>
        /// Initializes the audio filesystem
        /// </summary>
        public unsafe static void Init()
        {
            m_buffers = new List();
            m_tmpBuffer = new ushort[BufferSize];
            m_container = new ContainerFS();

            RootPoint dev = new RootPoint("audio", m_container.Node);
            VFS.RootMountPoint.AddEntry(dev);
        }

        private static void openImpl(Node node)
        {
            AudioDataCookie cookie = new AudioDataCookie(BufferSize * sizeof(ushort));
            m_buffers.Add(cookie.Buffer);
            node.Cookie = cookie;
        }

        private static void closeImpl(Node node)
        {
            AudioDataCookie cookie = (AudioDataCookie)node.Cookie;

            int index = m_buffers.IndexOf(cookie.Buffer);
            if (index > -1)
                m_buffers.RemoveAt(index);

            cookie.Dispose();
            Heap.Free(cookie);
            node.Cookie = null;
        }

        private static unsafe uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            AudioDataCookie cookie = (AudioDataCookie)node.Cookie;
            if (cookie == null)
                return 0;
            
            fixed (byte* ptr = buffer)
            {
                uint off = 0;
                while (size > 0)
                {
                    uint written = cookie.Buffer.Write(ptr + off, size);
                    if (written == 0)
                        MultiTasking.Tasking.Yield();

                    size -= written;
                    off += written;
                }

            }
            
            return size;
        }
    }
}
