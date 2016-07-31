using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public unsafe static void RequestBuffer(uint size, ushort* buffer)
        {

        }
    }
}
