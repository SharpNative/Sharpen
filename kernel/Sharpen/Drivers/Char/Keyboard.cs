using Sharpen.Arch;
using Sharpen.Collections;
using Sharpen.FileSystem;

namespace Sharpen.Drivers.Char
{
    public sealed class Keyboard
    {
        /// <summary>
        /// Leds
        /// </summary>
        private static byte m_leds = 0x00;

        /// <summary>
        /// Capslock key enabled
        /// </summary>
        private static int m_capslock = 0;

        /// <summary>
        /// Shift
        /// 0x01 == left shift
        /// 0x02 == right shift
        /// </summary>
        private static byte m_shift = 0x00;

        /// <summary>
        /// Capslock enabled
        /// </summary>
        public static bool Capslock { get { return m_capslock > 0; } }

        /// <summary>
        /// Shift down
        /// </summary>
        public static byte Shift { get { return m_shift; } }

        private static char readchar;
        private static volatile int readingchar;
        private static Fifo m_fifo;

        /// <summary>
        /// Set keyboard leds
        /// </summary>
        public static byte Leds
        {
            get
            {
                return m_leds;
            }

            set
            {
                m_leds = value;
                updateLED();
            }
        }

        /// <summary>
        /// Transform charcode into char
        /// </summary>
        /// <param name="scancode">The charcode</param>
        /// <returns>The transformed char</returns>
        private static char transformKey(byte scancode)
        {
            char outputChar;

            int codeFixed = scancode;
            // test
            outputChar = (m_shift > 0 ? KeyboardMap.Shifted[codeFixed] : KeyboardMap.Normal[codeFixed]);

            // Caps lock enabled?
            if (m_capslock > 0)
            {
                if (outputChar >= 'a' && outputChar <= 'z')
                {
                    outputChar = (char)('A' + outputChar - 'a');
                }
                else if (outputChar >= 'A' && outputChar <= 'Z')
                {
                    outputChar = (char)((outputChar - 'A') + 'a');
                }
            }

            return outputChar;
        }

        /// <summary>
        /// Updates the LED
        /// </summary>
        private static void updateLED()
        {
            PortIO.Out8(0x60, 0xED);
            while ((PortIO.In8(0x64) & 2) > 0) ;
            PortIO.Out8(0x60, m_leds);
            while ((PortIO.In8(0x64) & 2) > 0) ;
        }

        /// <summary>
        /// Initialize keyboard
        /// </summary>
        public static unsafe void Init()
        {
            m_fifo = new Fifo(250, true);

            // Install the IRQ handler
            IRQ.SetHandler(1, handler);
            IOApicManager.CreateISARedirection(1, 1);
            
            Node node = new Node();
            node.Read = readImpl;
            node.GetSize = getSizeImpl;
            node.Flags = NodeFlags.DEVICE | NodeFlags.FILE;

            RootPoint dev = new RootPoint("keyboard", node);
            VFS.MountPointDevFS.AddEntry(dev);
        }

        /// <summary>
        /// Gets the size of the available data
        /// </summary>
        /// <param name="node">The pipe node</param>
        /// <returns>The size</returns>
        private static uint getSizeImpl(Node node)
        {
            return m_fifo.AvailableBytes;
        }

        /// <summary>
        /// Read from keyboard
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes read</returns>
        private static uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            return m_fifo.Read(buffer, size);
        }

        /// <summary>
        /// Get char from keyboard
        /// </summary>
        /// <returns>The char</returns>
        public static char Getch()
        {
            readingchar = 1;
            while (readingchar != 0) CPU.HLT();

            return readchar;
        }

        /// <summary>
        /// Keyboard IRQ handler
        /// </summary>
        /// <returns></returns>
        private static unsafe bool handler()
        {
            byte scancode = PortIO.In8(0x60);

            // Key up?
            if ((scancode & 0x80) > 0)
            {
                if (scancode == 0xAA)
                    m_shift &= 0x02;
                else if (scancode == 0xB6)
                    m_shift &= 0x01;
            }
            else
            {
                if (scancode == 0x3A)
                {
                    if (m_capslock > 0)
                    {
                        m_capslock = 0;
                        m_leds = 0;
                    }
                    else
                    {
                        m_capslock = 1;
                        m_leds = 4;
                    }
                    updateLED();
                }
                else if (scancode == 0x2A)
                    m_shift |= 0x01;
                else if (scancode == 0x36)
                    m_shift |= 0x02;
                else
                {
                    readchar = transformKey(scancode);

                    m_fifo.WriteByte((byte)readchar);

                    readingchar = 0;
                }
            }

            return true;
        }
    }
}
