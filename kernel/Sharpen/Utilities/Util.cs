using Sharpen.Arch;
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
    }
}
