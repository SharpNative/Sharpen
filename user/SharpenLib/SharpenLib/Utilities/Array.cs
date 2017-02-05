namespace Sharpen.Utilities
{
    public sealed class Array
    {
        /// <summary>
        /// Creates a sub array from a bigger array
        /// </summary>
        /// <param name="original">The original array</param>
        /// <param name="start">The starting offset index</param>
        /// <param name="count">The size of the sub array</param>
        /// <returns>The sub array</returns>
        public static object[] CreateSubArray(object[] original, int start, int count)
        {
            if (start < 0 || count < 0)
                return null;

            object[] sub = new object[count];
            for (int i = 0; i < count; i++)
            {
                sub[i] = original[i + start];
            }
            return sub;
        }
    }
}
