using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Lib
{
    class Math
    {

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
