using Sharpen.Mem;
using Sharpen.Utilities;

namespace Sharpen.Lib
{
    sealed class Array
    {
        /// <summary>
        /// Joins elements from an array to a string
        /// </summary>
        /// <param name="arr">The array</param>
        /// <param name="length">The amount of items to copy from the array to form the string</param>
        /// <param name="separator">What will be between the array elements</param>
        /// <returns>The joined array string</returns>
        public static unsafe string Join(string[] arr, int length, string separator)
        {
            int separatorLen = separator.Length;
            int totalLength = 0;

            // Calculate total length
            int[] lengthArray = new int[length];
            for (int i = 0; i < length; i++)
            {
                int len = arr[i].Length;
                lengthArray[i] = len;
                totalLength += len;

                // Separator
                if (i < length - 1)
                    totalLength += separatorLen;
            }

            // Include NULL-character
            char* ptr = (char*)Heap.Alloc(totalLength + 1);
            ptr[totalLength] = '\0';

            // Copy elements
            int offset = 0;
            for (int i = 0; i < length; i++)
            {
                Memory.Memcpy((char*)((int)ptr + offset), Util.ObjectToVoidPtr(arr[i]), lengthArray[i]);
                offset += lengthArray[i];

                // Separator
                if (i < length - 1)
                {
                    Memory.Memcpy((char*)((int)ptr + offset), Util.ObjectToVoidPtr(separator), separatorLen);
                    offset += separatorLen;
                }
            }

            Heap.Free(lengthArray);

            return Util.CharPtrToString(ptr);
        }
    }
}
