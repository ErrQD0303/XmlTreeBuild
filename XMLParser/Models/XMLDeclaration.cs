using System.Collections.Immutable;
using System.Threading.Tasks;

namespace XMLParser.Models;

public class XMLDeclaration
{
    protected readonly string _name;
    public string Name => _name;

    protected List<XMLAttribute> _attributes = [];
    public List<XMLAttribute> Attributes => [.. _attributes];

    public XMLDeclaration(string name, List<XMLAttribute> attributes)
    {
        _name = name;
        _attributes.AddRange(attributes);
    }

    public void AddAttribute(string name, string value)
    {
        if (_attributes.Any(a => string.Equals(a.Name, name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new XMLParseException("Attribute already exists");
        }
        _attributes.Add(new XMLAttribute(name, value));
    }

    public void RemoveAttribute(string name)
    {
        var attribute = _attributes.FirstOrDefault(a => string.Equals(a.Name, name, StringComparison.OrdinalIgnoreCase));
        if (attribute != null)
        {
            _attributes.Remove(attribute);
        }
    }

    public string GetAttribute(string name)
    {
        return _attributes.FirstOrDefault(a => string.Equals(a.Name, name, StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty;
    }

    public static async Task<XMLDeclaration> ParseAsync(string input)
    {
        var inputStream = await XMLUtils.CreateStreamFromStringAsync(input) ?? throw new XMLParseException("Invalid input stream");
        using var sr = new StreamReader(inputStream);
        var xmlDeclarationAttributes = new List<XMLAttribute>();
        var openingTagName = string.Empty;

        do
        {
            var currentChar = Convert.ToChar(sr.Read());
            if (currentChar == '<')
            {
                sr.Read();
                var nextChar = Convert.ToChar(sr.Peek());
                while (sr.Peek() != -1 && (Char.IsLetter(nextChar) || nextChar == '-' || nextChar == '?'))
                {
                    if (nextChar != '?')
                    {
                        openingTagName += Convert.ToChar(sr.Read());
                    }
                    nextChar = Convert.ToChar(sr.Peek());
                }
                EnsureXMLDeclarationTagNameValid(openingTagName);
            }
            else if (currentChar == ' ')
            {
                xmlDeclarationAttributes = XMLUtils.ParseAttributes(sr, '?');
            }
            else if (currentChar == '?' || currentChar == '>')
            {
                continue;
            }
            else
            {
                throw new XMLParseException("Invalid XML Declaration");
            }
        } while (sr.Peek() != -1);

        return new XMLDeclaration(openingTagName, xmlDeclarationAttributes);
    }

    protected static void EnsureXMLDeclarationTagNameValid(string tagName)
    {
        if (string.IsNullOrWhiteSpace(tagName))
        {
            throw new XMLParseException("XML Declaration tag name cannot be null or empty");
        }

        if (!tagName.StartsWith("xml", StringComparison.OrdinalIgnoreCase))
        {
            throw new XMLParseException("Invalid XML Declaration tag name");
        }
    }

    public override string ToString()
    {
        var attributes = string.Join(" ", _attributes.Select(a => a.ToString()));
        return String.Format("<?{0} {1} ?>", _name, attributes);
    }
}