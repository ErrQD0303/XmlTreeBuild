namespace XMLParser.Models;

public class XMLAttribute
{
    private readonly string _name = string.Empty;
    public string Name
    {
        get => _name; init
        {
            XMLUtils.EnsureAttributeNameCorrect(value);
            _name = value;
        }
    }
    public string Value { get; set; }
    public XMLElement? Parent { get; set; }

    public XMLAttribute(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public override string ToString()
    {
        return $"{Name}=\"{Value}\"";
    }
}