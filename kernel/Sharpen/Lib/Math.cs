using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Lib
{
    class Math
    {
        public static int Abs(int num)
        {
            if (num > 0)
                return num;

            return num * -1;
        }

        public static uint Abs(uint num)
        {
            if (num > 0)
                return num;

            return num * -1;
        }

        public static int Ceil(double num)
        {
            int inum = (int)num;
            if (num == (double)inum)
            {
                return inum;
            }
            return inum + 1;
        }


        public static int Floor(double num)
        {
            return (int)num;
        }
    }
}
