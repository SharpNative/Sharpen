namespace Sharpen
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
        /// <param name="text"></param>
        /// <param name="occurence"></param>
        /// <returns></returns>
        public static int indexOf(string text, string occurence)
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
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static unsafe string Substring(string str, int start, int count)
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
            
            // TODO: Find a method that doesn't give an error in intellisense (works in compiler)
            return (string)ch;
        }

        /// <summary>
        /// Merge 2 string
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static unsafe string Merge(string first, string second)
        {
            int firstLength = Length(first);
            int secondLength = Length(second);

            int allocLength = firstLength + secondLength + 1;

            char* outVal = (char*)Heap.Alloc(allocLength);

            int j = 0;

            // TODO: Can't remove brackets, compiler bug?
            for (int i = 0; i < firstLength; i++)
            {
                outVal[j++] = first[i];
            }

            for (int i = 0; i < secondLength; i++)
                outVal[j++] = second[i];
            outVal[j] = '\0';

            return (string)outVal; 
        }

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
    }
}
