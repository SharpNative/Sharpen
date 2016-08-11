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
        /// IndexOf implementation
        /// </summary>
        /// <param name="text">The string to search into</param>
        /// <param name="occurence">The string to search for</param>
        /// <returns>The index of the occurrence</returns>
        public static int IndexOf(string text, string occurence)
        {
            int found = -1;
            int foundCount = 0;

            int textLength = Length(text);
            int occurenceLength = Length(occurence);

            if (textLength == 0 || occurenceLength == 0)
                return -1;

            for (int textIndex = 0; textIndex < textLength; textIndex++)
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
        /// Count number of occurences
        /// </summary>
        /// <param name="str"></param>
        /// <param name="occurence"></param>
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
                return null;

            int stringLength = Length(str);

            if (start > stringLength)
                return "";

            
            char* ch = (char *)Heap.Alloc(count + 1);
            int j = 0;
            
            for(int i = start; j < count; i++)
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
        public static bool Equals(string one, string two)
        {
            int oneLength = Length(one);
            int twoLength = Length(two);

            if (oneLength != twoLength)
                return false;

            for (int i = 0; i < oneLength; i++)
                if (one[i] != two[i])
                    return false;

            return true;
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
            if ((c >= 65) && (c <= 90))
                c = (char)(c + (int)32);

            return c;
        }
    }
}
