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
}