

using Sharpen.Collections;
using Sharpen.Mem;

namespace Sharpen.Lib
{
    class BootParams
    {
        private static StringDictionary mParams; 

        public static string GetParam(string name)
        {

            return (string)mParams.Get(name);
        }

        public static bool HasParam(string name)
        {

            return mParams.Get(name) != null;
        }

        public static void Init(string querystring)
        {

            mParams = new StringDictionary(6);
            
            string[] splittedChars = querystring.Split(' ');
            
            int spaces = String.Count(querystring, ' ') + 1;
            
            for (int i = 0; i < spaces; i++)
            {
                string param = splittedChars[i];

                bool containsEquals = String.Count(param, '=') > 0;

                if(containsEquals)
                {
                    string[] paramAndValue = param.Split('=');

                    mParams.Add(paramAndValue[0], paramAndValue[1]);

                    Heap.Free(param);
                }
                else
                {
                    mParams.Add(param, null);
                }

            }
        }
    }
}
