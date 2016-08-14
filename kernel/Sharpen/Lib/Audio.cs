using System;
using Sharpen.FileSystem;

namespace Sharpen.Lib
{
    public enum AudioActions
    {
        Master = 0,
        PCM_OUT = 1
    }

    class Audio
    {
        public struct SoundDevice
        {
            public string Name;
            public SoundDeviceWriter Writer;
            public SoundDeviceReader Reader;
        }

        public unsafe delegate void SoundDeviceWriter(AudioActions action, uint value);
        public unsafe delegate uint SoundDeviceReader(AudioActions action);

        private static SoundDevice m_device;

        /// <summary>
        /// Set sound Device
        /// </summary>
        /// <param name="device"></param>
        public static void SetDevice(SoundDevice device)
        {
            m_device = device;
        }

        /// <summary>
        /// Request a new audio buffer
        /// </summary>
        /// <param name="size">The required size</param>
        /// <param name="buffer">The buffer to put the data into</param>
        public unsafe static void RequestBuffer(uint size, ushort* buffer)
        {

        }

        public unsafe static void Init()
        {
            Device dev = new Device();
            dev.Name = "Audio";
            dev.node = new Node();
            dev.node.Write = writeImpl;

            DevFS.RegisterDevice(dev);
        }

        private static uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            return 0;
        }
    }
}
