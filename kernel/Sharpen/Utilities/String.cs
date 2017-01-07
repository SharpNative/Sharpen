using Sharpen.Mem;

namespace Sharpen.Utilities
{
    public sealed class String
    {
        /// <summary>
        /// Calculates the length of a string
        /// </summary>
        /// <param name="text">The string</param>
        /// <returns>Its length</returns>
        public static int Length(string text)
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
            int textLength = Length(text);

            char* str = (char*)Heap.Alloc(textLength + 1);
            fixed (char* ptr = text)
            {
                Memory.Memcpy(str, ptr, textLength);
            }

            str[textLength] = '\0';

            return Util.CharPtrToString(str);
        }

        /// <summary>
        /// Merge array with a string between the elements
        /// </summary>
        /// <param name="array">The array</param>
        /// <param name="elements">The amount of elements to merge</param>
        /// <param name="splitter">The string between the elements</param>
        /// <returns>The resulting string</returns>
        public static unsafe string MergeArray(string[] array, int elements, string splitter)
        {
            return MergeArray(array, elements, splitter, "");
        }

        /// <summary>
        /// Merge array with a string between the elements
        /// </summary>
        /// <param name="array">The array</param>
        /// <param name="elements">The amount of elements to merge</param>
        /// <param name="splitter">The string between the elements</param>
        /// <param name="prefix">The prefix</param>
        /// <returns>The resulting string</returns>
        public static unsafe string MergeArray(string[] array, int elements, string splitter, string prefix)
        {
            int prefixLength = Length(prefix);
            int totalLength = prefixLength;
            int splitterLength = Length(splitter);
            for (int i = 0; i < elements; i++)
            {
                totalLength += Length(array[i]);
                if (i < elements - 1)
                    totalLength += splitterLength;
            }

            char* ptr = (char*)Heap.Alloc(totalLength + 1);
            fixed (char* prefixPtr = prefix)
                Memory.Memcpy(ptr, prefixPtr, prefixLength);

            int offset = prefixLength;
            for (int i = 0; i < elements; i++)
            {
                int length = Length(array[i]);
                fixed (char* src = array[i])
                    Memory.Memcpy((char*)((int)ptr + offset), src, length);

                if (i < elements - 1)
                {
                    fixed (char* split = splitter)
                        Memory.Memcpy((char*)((int)ptr + offset + length), split, splitterLength);

                    offset += splitterLength;
                }

                offset += length;
            }

            ptr[offset] = '\0';

            return Util.CharPtrToString(ptr);
        }

        /// <summary>
        /// IndexOf implementation
        /// </summary>
        /// <param name="text">The string to search into</param>
        /// <param name="occurence">The string to search for</param>
        /// <param name="offset">The offset in the string</param>
        /// <returns>The index of the occurrence</returns>
        public static int IndexOf(string text, string occurence, int offset)
        {
            int found = -1;
            int foundCount = 0;

            int textLength = Length(text);
            int occurenceLength = Length(occurence);

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
        public static int IndexOf(string text, string occurence)
        {
            return IndexOf(text, occurence, 0);
        }

        /// <summary>
        /// Count number of occurences
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="occurence">The character to check for</param>
        /// <returns></returns>
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
        /// <param name="count">The count</param>
        /// <returns>The string</returns>
        public static unsafe string SubString(string str, int start, int count)
        {
            if (count <= 0)
                return Clone("");

            int stringLength = Length(str);
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
            int firstLength = Length(first);
            int secondLength = Length(second);

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
        public static unsafe bool Equals(string one, string two)
        {
            fixed (char* onePtr = one)
            {
                fixed (char* twoPtr = two)
                {
                    return Memory.Compare(onePtr, twoPtr, Length(one));
                }
            }
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
    }
}
