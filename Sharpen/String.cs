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
    }
}
