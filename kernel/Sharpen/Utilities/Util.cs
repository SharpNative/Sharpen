using Sharpen.Arch;
using Sharpen.Mem;
using System;

namespace Sharpen.Utilities
{
    public sealed class Util
    {
        /// <summary>
        /// Converts a char pointer to a string
        /// </summary>
        /// <param name="ptr">The char pointer</param>
        /// <returns>The string</returns>
        public static unsafe extern string CharPtrToString(char* ptr);

        /// <summary>
        /// Converts a byte pointer to a byte array
        /// </summary>
        /// <param name="ptr">The pointer</param>
        /// <returns>The array</returns>
        public static unsafe extern byte[] PtrToArray(byte* ptr);

        /// <summary>
        /// Converts a pointer to an array
        /// </summary>
        /// <param name="ptr">The pointer</param>
        /// <returns>The array</returns>
        public static unsafe extern object[] PtrToArray(void* ptr);

        /// <summary>
        /// Converts a string array pointer to a string array
        /// </summary>
        /// <param name="ptr">The pointer</param>
        /// <returns>The array</returns>
        public static unsafe extern string[] PtrToArray(char** ptr);

        /// <summary>
        /// Converts an object to a void pointer
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>The pointer</returns>
        public static unsafe extern void* ObjectToVoidPtr(object obj);

        /// <summary>
        /// Converts an object to a void pointer
        /// </summary>
        /// <param name="obj">The pointer</param>
        /// <returns>The object</returns>
        public static unsafe extern object VoidPtrToObject(void* ptr);

        /// <summary>
        /// Converts a method to a pointer
        /// </summary>
        /// <param name="method">The method</param>
        /// <returns>The pointer</returns>
        public static unsafe extern void* MethodToPtr(Action method);

        /// <summary>
        /// Uses a volatile pointer to write a 32bit value to an address, this is because C# doesn't support this
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="value">The value</param>
        public static unsafe extern void WriteVolatile32(uint address, uint value);

        /// <summary>
        /// Uses a volatile pointer to read a 32bit value from an address, this is because C# doesn't support this
        /// </summary>
        /// <param name="address">The address</param>
        /// <returns>The value</returns>
        public static unsafe extern uint ReadVolatile32(uint address);

        /// <summary>
        /// Prints a stack trace
        /// </summary>
        /// <param name="maxFrames">Maximum amount of frames to display</param>
        public static unsafe void PrintStackTrace(int maxFrames)
        {
            Console.WriteLine("\nStacktrace:");
            int* ebp = (int*)((int)&maxFrames - 2 * sizeof(int));
            for (int frame = 0; frame < maxFrames; frame++)
            {
                int eip = ebp[1];

                // No caller on the stack
                if (eip == 0)
                    break;

                void* addressOffset = null;
                string name = SymbolTable.FindSymbolName((void*)eip, &addressOffset);

                // Unwind previous stack frame
                ebp = (int*)ebp[0];
                void* testAddress = Paging.GetPhysicalFromVirtual((void*)ebp);
                if (testAddress == null)
                    break;

                // Display symbol and offset
                Console.Write('\t');
                Console.Write(name);
                Console.Write("+");
                Console.WriteHex((int)addressOffset);
                Console.Write(" (");
                Console.WriteHex(eip);
                Console.WriteLine(")");
            }
        }

        /// <summary>
        /// Finds a BIOS structure
        /// </summary>
        /// <param name="checkPtr">The signature check pointer</param>
        /// <param name="checkSize">The signature check size</param>
        /// <param name="start">The starting address</param>
        /// <param name="end">The ending address</param>
        /// <param name="size">The size of the structure</param>
        /// <returns>The pointer to the structure if found, otherwise null</returns>
        public static unsafe void* FindStructure(char* checkPtr, int checkSize, void* start, void* end, int size)
        {
            uint current = (uint)start;
            while (current < (uint)end)
            {
                if (Memory.Compare((char*)current, checkPtr, checkSize) && ZeroCheckSum((byte*)current, (uint)size))
                {
                    return (void*)current;
                }

                current += 16;
            }

            return null;
        }

        /// <summary>
        /// Checksum
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="length">The length</param>
        /// <returns>If the check was successfull</returns>
        public static unsafe bool ZeroCheckSum(void* address, uint length)
        {
            byte check = 0;

            byte* ptr = (byte*)address;
            for (int i = 0; i < length; i++)
            {
                check += *ptr++;
            }

            return (check == 0);
        }
    }
}
