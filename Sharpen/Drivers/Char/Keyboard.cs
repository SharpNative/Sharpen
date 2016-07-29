using Sharpen.Arch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Char
{
    class Keyboard
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
                updateLeds();
            }
        }

        /// <summary>
        /// Transform charcode into char
        /// </summary>
        /// <param name="scancode">The charcode</param>
        /// <returns></returns>
        private static char transformKey(byte scancode)
        {
            char outputChar;

            int codeFixed = scancode;
            // test
            outputChar = (m_shift > 0 ? KeyboardMap.Shifted[codeFixed] : KeyboardMap.Normal[codeFixed]);
            
            /**
             * Capslock enabled?
             */
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

        /**
         * Update led byte
         */ 
        private static void updateLeds()
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
            KeyboardMap.Fill();

            // Install the IRQ handler
            IRQ.SetHandler(1, Handler);
        }

        /// <summary>
        /// Get char from keyboard
        /// </summary>
        /// <returns></returns>
        public static char Getch()
        {
            readingchar = 1;
            while (readingchar != 0) CPU.HLT();

            return readchar;
        }

        /// <summary>
        /// Keyboard IRQ handler
        /// </summary>
        /// <param name="regsPtr"></param>
        private static unsafe void Handler(Regs* regsPtr)
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
                readchar = transformKey(scancode);
                
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
                    updateLeds();
                }
                else if (scancode == 0x2A)
                    m_shift |= 0x01;
                else if (scancode == 0x36)
                    m_shift |= 0x02;

                readingchar = 0;
            }
        }
    }
}
