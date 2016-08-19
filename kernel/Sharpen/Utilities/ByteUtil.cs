using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Utilities
{
    class ByteUtil
    {
        /// <summary>
        /// Converts an int to a byte array
        /// </summary>
        /// <param name="inValue">The value to convert</param>
        /// <returns>The byte array</returns>
        public static byte[] ToBytes(int inValue)
        {
            byte[] result = new byte[8];
            for (int i = 3; i >= 0; i--)
            {
                result[i] = (byte)(inValue & 0xFF);
                inValue >>= 8;
            }
            return result;
        }

        /// <summary>
        /// Converts a long to a byte array
        /// </summary>
        /// <param name="inValue">The value to convert</param>
        /// <returns>The byte array</returns>
        public static byte[] ToBytes(long inValue)
        {
            byte[] result = new byte[8];
            for (int i = 7; i >= 0; i--)
            {
                result[i] = (byte)(inValue & 0xFF);
                inValue >>= 8;
            }
            return result;
        }

        /// <summary>
        /// Converts a long to a byte array
        /// </summary>
        /// <param name="inValue">The value to convert</param>
        /// <param name="result">The place where to put the bytes into</param>
        /// <returns>The byte array</returns>
        public static byte[] ToBytes(long inValue, byte[] result)
        {
            for (int i = 7; i >= 0; i--)
            {
                result[i] = (byte)(inValue & 0xFF);
                inValue >>= 8;
            }
            return result;
        }

        /// <summary>
        /// Converts a byte array to a long
        /// </summary>
        /// <param name="b">The byte array</param>
        /// <returns>The long</returns>
        public static long ToLong(byte[] b)
        {
            long result = 0;
            for (int i = 0; i < 8; i++)
            {
                result <<= 8;
                result |= (byte)(b[i] & 0xFF);
            }

            return result;
        }

        /// <summary>
        /// Converts a byte array to a short
        /// </summary>
        /// <param name="b">The byte array</param>
        /// <param name="offset">The offset in the array</param>
        /// <returns>The short</returns>
        public static short ToShort(byte[] b, int offset)
        {
            short result = 0;
            for (int i = 0; i < 2; i++)
            {
                result <<= 8;
                result |= (byte)(b[offset] & 0xFF);
                offset++;
            }

            return result;
        }

        /// <summary>
        /// Converts a byte array to an integer
        /// </summary>
        /// <param name="b">The byte array</param>
        /// <returns>The integer</returns>
        public static int ToInt(byte[] b)
        {
            int result = 0;
            for (int i = 0; i < 4; i++)
            {
                result <<= 8;
                result |= (b[i] & 0xFF);
            }

            return result;
        }

        public static UInt16 ReverseBytes(UInt16 value)
        {
            UInt16 outVal = (UInt16)((value & 0xFF) << 8);
            outVal |= (UInt16)((value & 0xFF00) >> 8);
            
            return outVal;
        }

        public static UInt32 ReverseBytes(UInt32 value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }

        public static UInt64 ReverseBytes(UInt64 value)
        {
            return (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
                   (value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
                   (value & 0x000000FF00000000UL) >> 8 | (value & 0x0000FF0000000000UL) >> 24 |
                   (value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56;
        }

    }
}
