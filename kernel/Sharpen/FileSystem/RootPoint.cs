namespace Sharpen.FileSystem
{
    public class RootPoint
    {
        public string Name { get; private set; }
        public Node Node { get; private set; }

        /// <summary>
        /// Creates a new RootPoint
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="node">The node</param>
        public RootPoint(string name, Node node)
        {
            Name = name;
            Node = node;
        }
    }
}
