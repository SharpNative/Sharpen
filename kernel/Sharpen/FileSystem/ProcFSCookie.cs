namespace Sharpen.FileSystem
{
    public class IDCookie : ICookie
    {
        public int ID;

        /// <summary>
        /// Creates a new ID cookie
        /// </summary>
        /// <param name="id">The ID</param>
        public IDCookie(int id)
        {
            ID = id;
        }

        /// <summary>
        /// Cleans up
        /// </summary>
        public void Dispose()
        {
        }
    }
}
