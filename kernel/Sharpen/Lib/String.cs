using Sharpen.Mem;
using LibCS2C.Attributes;
using Sharpen.Utilities;

namespace Sharpen.Lib
{
    public sealed class String
    {
        /// <summary>
        /// Calculates the length of a string
        /// </summary>
        /// <param name="text">The string</param>
        /// <returns>Its length</returns>
        [Plug("string_Length_getter")]
        private static int getLengthImpl(string text)
        {
            int i = 0;
            for (; text[i] != '\0'; i++) ;
            return i;
        }

        /// <summary>
        /// Clones a string
        /// </summary>
        /// <param name="text">The original string</param>
        /// <returns>The clone</returns>
        public static unsafe string Clone(string text)
        {
            int textLength = text.Length;

            char* str = (char*)Heap.Alloc(textLength + 1);
            fixed (char* ptr = text)
            {
                Memory.Memcpy(str, ptr, textLength);
            }

            str[textLength] = '\0';

            return Util.CharPtrToString(str);
        }

        /// <summary>
        /// IndexOf implementation
        /// </summary>
        /// <param name="text">The string to search into</param>
        /// <param name="occurence">The string to search for</param>
        /// <param name="offset">The offset in the string</param>
        /// <returns>The index of the occurrence</returns>
        [Plug("System_String_IndexOf_3class_string_t_int32_t_")]
        private static int indexOfImpl(string text, string occurence, int offset)
        {
            int found = -1;
            int foundCount = 0;

            int textLength = text.Length;
            int occurenceLength = occurence.Length;

            if (textLength == 0 || occurenceLength == 0 || offset >= textLength)
                return -1;

            for (int textIndex = offset; textIndex < textLength; textIndex++)
            {
                if (occurence[foundCount] == text[textIndex])
                {
                    if (foundCount == 0)
                        found = textIndex;

                    foundCount++;
                    if (foundCount >= occurenceLength)
                        return found;
                }
                else
                {
                    foundCount = 0;

                    if (found >= 0)
                        textIndex = found;
                    found = -1;
                }
            }

            return -1;
        }

        /// <summary>
        /// IndexOf implementation
        /// </summary>
        /// <param name="text">The string to search into</param>
        /// <param name="occurence">The string to search for</param>
        /// <returns>The index of the occurrence</returns>
        [Plug("System_String_IndexOf_2class_string_t_")]
        private static int indexOfImpl(string text, string occurence)
        {
            return indexOfImpl(text, occurence, 0);
        }

        /// <summary>
        /// IndexOf implementation
        /// </summary>
        /// <param name="text">The string to search into</param>
        /// <param name="occurence">The character to search for</param>
        /// <param name="offset">The offset in the string</param>
        /// <returns>The index of the occurrence</returns>
        [Plug("System_String_IndexOf_3class_char_int32_t_")]
        private static int indexOfImpl(string text, char occurence, int offset)
        {
            int length = text.Length;
            if (offset < 0 || offset >= length)
                return -1;

            for(int i = offset; i < length; i++)
            {
                if (text[i] == occurence)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// IndexOf implementation
        /// </summary>
        /// <param name="text">The string to search into</param>
        /// <param name="occurence">The character to search for</param>
        /// <returns>The index of the occurrence</returns>
        [Plug("System_String_IndexOf_2class_char_")]
        private static int indexOfImpl(string text, char occurence)
        {
            return indexOfImpl(text, occurence, 0);
        }

        /// <summary>
        /// Count number of occurences
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="occurence">The character to check for</param>
        /// <returns>The number of occurences</returns>
        public static int Count(string str, char occurence)
        {
            int matches = 0;
            for (int i = 0; str[i] != '\0'; i++)
                if (str[i] == occurence)
                    matches++;
            
            return matches;
        }

        /// <summary>
        /// Substring implementation
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="start">The starting index</param>
        /// <returns>The substring</returns>
        [Plug("System_String_Substring_2class_int32_t_")]
        private static unsafe string subStringImpl(string str, int start)
        {
            return subStringImpl(str, start, str.Length - start);
        }

        /// <summary>
        /// Substring implementation
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="start">The starting index</param>
        /// <param name="count">The count</param>
        /// <returns>The substring</returns>
        [Plug("System_String_Substring_3class_int32_t_int32_t_")]
        private static unsafe string subStringImpl(string str, int start, int count)
        {
            if (count <= 0)
                return Clone("");

            int stringLength = str.Length;
            if (start > stringLength)
                return Clone("");

            char* ch = (char*)Heap.Alloc(count + 1);
            int j = 0;

            for (int i = start; j < count; i++)
            {
                if (str[i] == '\0')
                {
                    break;
                }

                ch[j++] = str[i];
            }
            ch[j] = '\0';

            return Util.CharPtrToString(ch);
        }

        /// <summary>
        /// Merge 2 strings
        /// </summary>
        /// <param name="first">The first string</param>
        /// <param name="second">The second string</param>
        /// <returns>The merged string</returns>
        public static unsafe string Merge(string first, string second)
        {
            int firstLength = first.Length;
            int secondLength = second.Length;

            int totalLength = firstLength + secondLength;
            char* outVal = (char*)Heap.Alloc(totalLength + 1);

            Memory.Memcpy(outVal, Util.ObjectToVoidPtr(first), firstLength);
            Memory.Memcpy((void*)((int)outVal + firstLength), Util.ObjectToVoidPtr(second), secondLength);

            outVal[totalLength] = '\0';

            return Util.CharPtrToString(outVal);
        }

        /// <summary>
        /// Checks if two string are equal
        /// </summary>
        /// <param name="one">First string</param>
        /// <param name="two">Second string</param>
        /// <returns>If the two string are equal</returns>
        [Plug("System_String_Equals_2class_string_t_")]
        private static unsafe bool equalsImpl(string one, string two)
        {
            int lengthOne = one.Length;
            int lengthTwo = two.Length;

            if (lengthOne != lengthTwo)
                return false;

            fixed (char* onePtr = one)
            {
                fixed (char* twoPtr = two)
                {
                    return Memory.Compare(onePtr, twoPtr, lengthOne);
                }
            }
        }

        /// <summary>
        /// Splits a string
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="ch">The character</param>
        /// <returns>The array of split strings</returns>
        [Plug("System_String_Split_2class_char__")]
        public static unsafe string[] Split(string str, char ch)
        {
            int count = Count(str, ch) + 1;
            string[] parts = new string[count];

            int length = str.Length;
            int offset = 0;
            for (int i = 0; i < count; i++)
            {
                int nextOffset = str.IndexOf(ch, offset);
                if (nextOffset == -1)
                    nextOffset = length;
                
                parts[i] = str.Substring(offset, nextOffset - offset);
                offset = nextOffset + 1;
            }

            return parts;
        }

        /// <summary>
        /// Converts a char to uppercase
        /// </summary>
        /// <param name="c">The character</param>
        /// <returns>The uppercase character</returns>
        public static char ToUpper(char c)
        {
            return (c >= 'a' && c <= 'z') ? (char)(c + ('A' - 'a')) : c;
        }

        /// <summary>
        /// Converts a char to lowercase
        /// </summary>
        /// <param name="c">The character</param>
        /// <returns>The lowercase character</returns>
        public static char ToLower(char c)
        {
            return (c >= 'A' && c <= 'Z') ? (char)(c + 32) : c;
        }

        /// <summary>
        /// An implementation of the sdbm string hashing algorithm
        /// </summary>
        /// <param name="str">The string</param>
        /// <returns>The hashcode</returns>
        public static uint GetHashCode(string str)
        {
            uint hash = 0;
            for (int i = 0; str[i] != '\0'; i++)
                hash = str[i] + (hash << 6) + (hash << 16) - hash;

            return hash;
        }
    }
}
