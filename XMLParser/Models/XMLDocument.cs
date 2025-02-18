using System.Text;

namespace XMLParser.Models;

public class XMLDocument
{
    public XMLElement? Root { get; set; }
    public XMLDeclaration? Prolog { get; set; }
    public XMLDeclaration? Declaration { get; set; }
    public XMLDocument(XMLElement? root)
    {
        Root = root;
    }
    public XMLDocument(XMLElement? root, XMLDeclaration? prolog) : this(root)
    {
        Prolog = prolog;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        if (Prolog != null)
        {
            sb.Append(Prolog.ToString());
        }
        if (Declaration != null)
        {
            sb.Append(Declaration.ToString());
        }
        if (Root != null)
        {
            sb.Append("\n" + Root.ToString());
        }

        return sb.ToString();
    }
}