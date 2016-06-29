namespace Sharpen
{
    class String
    {

        public static int Length(string text)
        {
            int i = 0;
            for (; text[i] != '\0'; i++) ;
            return i;
        }
    }
}
