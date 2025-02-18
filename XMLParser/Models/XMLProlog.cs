using System.Threading.Tasks;

namespace XMLParser.Models;

public class XMLProlog : XMLDeclaration
{
    public string? Version
    {
        get => _attributes
            .FirstOrDefault(a => string.Equals(a.Name, "version", StringComparison.OrdinalIgnoreCase))
            ?.Value;
        set
        {
            var attribute = _attributes
                .FirstOrDefault(a => string.Equals(a.Name, "version", StringComparison.OrdinalIgnoreCase));
            if (attribute != null)
            {
                attribute.Value = value ?? "1.0";
            }
        }
    }

    public string? Encoding
    {
        get => _attributes
            .FirstOrDefault(a => string.Equals(a.Name, "encoding", StringComparison.OrdinalIgnoreCase))
            ?.Value;
        set
        {
            var attribute = _attributes
                .FirstOrDefault(a => string.Equals(a.Name, "encoding", StringComparison.OrdinalIgnoreCase));
            if (attribute != null)
            {
                attribute.Value = value ?? "UTF-8";
            }
        }
    }

    public XMLProlog(List<XMLAttribute> attributes) : base("xml", attributes)
    {
        var versionAttribute = _attributes.FirstOrDefault(a => string.Equals(a.Name, "version", StringComparison.OrdinalIgnoreCase));
        if (versionAttribute == null)
        {
            _attributes.Add(new XMLAttribute("version", "1.0"));
        }
        var encodingAttribute = _attributes.FirstOrDefault(a => string.Equals(a.Name, "encoding", StringComparison.OrdinalIgnoreCase));
        if (encodingAttribute == null)
        {
            _attributes.Add(new XMLAttribute("encoding", "UTF-8"));
        }
    }

    public XMLProlog(XMLDeclaration declaration) : this(declaration.Attributes)
    {
        if (!string.Equals(declaration.Name, "xml", StringComparison.OrdinalIgnoreCase))
        {
            throw new XMLParseException("Prolog tag name must be 'xml'");
        }
    }

    public new static async Task<XMLProlog> ParseAsync(string input)
    {
        return new XMLProlog(await XMLDeclaration.ParseAsync(input));
    }

    protected new static void EnsureXMLDeclarationTagNameValid(string tagName)
    {
        if (string.IsNullOrWhiteSpace(tagName))
        {
            throw new XMLParseException("Prolog tag name cannot be null or empty");
        }

        if (!string.Equals(tagName, "xml", StringComparison.OrdinalIgnoreCase))
        {
            throw new XMLParseException("Prolog tag name must be 'xml'");
        }
    }
}