namespace XMLParser.Models;

public abstract class XMLElement
{
    protected string _name = string.Empty;
    protected string? _text;
    public virtual string Name
    {
        get => _name;
        set
        {
            XMLUtils.EnsureElementNameCorrect(value);
            _name = value;
        }
    }
    public string? Text
    {
        get => _text;
        set
        {
            _text = value;
        }
    }
    public List<XMLElement> Children { get; set; } = [];
    public List<XMLAttribute> Attributes { get; set; } = [];

    public XMLElement(string name, string text
    )
    {
        Name = name;
        Text = text;
    }

    public XMLElement(string name)
    {
        Name = name;
    }

    public void AddChild(XMLElement element)
    {
        Children.Add(element);
    }

    public void AddAttribute(string name, string value)
    {
        Attributes.Add(new XMLAttribute(name, value));
    }

    public override string ToString()
    {
        return Name;
    }
}