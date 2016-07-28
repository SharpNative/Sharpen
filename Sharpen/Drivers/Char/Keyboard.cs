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
        /// Leds :D
        /// </summary>
        private static byte m_leds = 0x00;

        private static int m_capslock = 0;

        private static bool m_shift = false;

        /// <summary>
        /// Capslock enabled
        /// </summary>
        public static bool Capslock { get { return m_capslock > 0; } }

        /// <summary>
        /// Shift down
        /// </summary>
        public static bool Shift { get { return m_shift; } }

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

            int codeFixed = scancode - 128;

            outputChar = (m_shift ? KeyboardMap.Shifted[codeFixed] : KeyboardMap.Normal[codeFixed]);

            Console.WriteNum(codeFixed);
            Console.Write("-");
            Console.WriteHex(outputChar);
            Console.WriteLine("");

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

        public static unsafe void Init()
        {

            // Install the IRQ handler
            IRQ.SetHandler(1, Handler);
        }

        private static unsafe void Handler(Regs* regsPtr)
        {
            byte scancode = PortIO.In8(0x60);

            if ((scancode & 0x80) > 0)
            {
                if (scancode == 0xAA)
                    m_shift = !m_shift;
                else if (scancode == 0xB6)
                    m_shift = !m_shift;
                else
                {
                    readchar = transformKey(scancode);
                    readingchar = 0;


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
                        m_shift = !m_shift;
                    else if (scancode == 0x36)
                        m_shift = !m_shift;


                    if (readchar > 0)
                    {
                        Console.WriteNum(readchar);
                    }
                }
            }
        }
    }
}
