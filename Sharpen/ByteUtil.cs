using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen
{
    class ByteUtil
    {
        public static byte[] toBytes(int inValue)
        {
            byte[] result = new byte[8];
            for (int i = 3; i >= 0; i--)
            {
                result[i] = (byte)(inValue & 0xFF);
                inValue >>= 8;
            }
            return result;
        }

        public static byte[] toBytes(long inValue)
        {
            byte[] result = new byte[8];
            for (int i = 7; i >= 0; i--)
            {
                result[i] = (byte)(inValue & 0xFF);
                inValue >>= 8;
            }
            return result;
        }

        public static byte[] toBytes(long inValue, byte[] result)
        {
            for (int i = 7; i >= 0; i--)
            {
                result[i] = (byte)(inValue & 0xFF);
                inValue >>= 8;
            }
            return result;
        }

        public static long ToLong(byte[] b)
        {
            long result = 0;
            for (int i = 0; i < 8; i++)
            {
                result <<= 8;
                result |= (b[i] & 0xFF);
            }

            return result;
        }


        public static short ToShort(byte[] b, int offset)
        {
            short result = 0;
            for (int i = 0; i < 2; i++)
            {
                result <<= 8;
                result |= (short)(b[offset] & 0xFF);
                offset++;
            }

            return result;
        }

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
    }
}
