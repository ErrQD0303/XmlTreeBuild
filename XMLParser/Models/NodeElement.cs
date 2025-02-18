namespace XMLParser.Models
{
    public class NodeElement : XMLElement
    {
        public NodeElement(string name, string text
        ) : base(name, text)
        {
        }

        public NodeElement(string name) : base(name)
        {
        }

    }
}