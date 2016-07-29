using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.Char
{
    class KeyboardMap
    {

        public static char[] Normal = new char[127];

        public static char[] Shifted = new char[127]
        {
            (char)27, '!', '@', '#', '$', '%', '^', '&', '*',	/* 9 */
            '(', ')', '_', '+', '\b',	/* Backspace */
            '\t',			/* Tab */
            'Q', 'W', 'E', 'R',	/* 19 */
            'T', 'Y', 'U', 'I', 'O', 'P', '[', ']', '\n',	/* Enter key */
            (char)0,			/* 29   - Control */
            'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', ':',	/* 39 */
            '\"', '~',   (char)0,		/* Left shift */
            '|', 'Z', 'X', 'C', 'V', 'B', 'N',			/* 49 */
            'M', '<', '>', '?',   (char)0,				/* Right shift */
            '*',
            (char)0,	/* Alt */
            ' ',	/* Space bar */
            (char)0,	/* Caps lock */
            (char)0,	/* 59 - F1 key ... > */
            (char)0,   (char)0,   (char)0,   (char)0,   (char)0,   (char)0,   (char)0,   (char)0,
            (char)0,	/* < ... F10 */
            (char)0,	/* 69 - Num lock*/
            (char)0,	/* Scroll Lock */
            (char)0,	/* Home key */
            (char)0,	/* Up Arrow */
            (char)0,	/* Page Up */
            '-',
            (char)0,	/* Left Arrow */
            (char)0,
            (char)0,	/* Right Arrow */
            '+',
            (char)0,	/* 79 - End key*/
            (char)0,	/* Down Arrow */
            (char)0,	/* Page Down */
            (char)0,	/* Insert Key */
            (char)0,	/* Delete Key */
            (char)0,   (char)0,   (char)0,
            (char)0,	/* F11 Key */
            (char)0,	/* F12 Key */
            (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0,
            (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0,
            (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0,/* All other keys are undefined */
        };

        public static void Fill()
        {
            Normal[2] = '1';
            Normal[3] = '2';
            Normal[4] = '3';
            Normal[5] = '4';
            Normal[6] = '5';
            Normal[7] = '6';
            Normal[8] = '7';
            Normal[9] = '8';
            Normal[10] = '9';
            Normal[11] = '0';
            Normal[12] = '-';
            Normal[13] = '+';
            Normal[14] = '\b';
        }
    }
}
