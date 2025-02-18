using System.Text;

namespace XMLParser.Models;

public abstract class XMLElement
{
    protected string _name = string.Empty;
    public virtual string Name
    {
        get => _name;
        set
        {
            XMLUtils.EnsureElementNameCorrect(value);
            _name = value;
        }
    }
    public virtual string? Text
    {
        get
        {
            var text = "";
            foreach (var child in Children.Where(c => c is XMLTextElement))
            {
                text += child.Text;
            }

            return text;
        }
    }

    public XMLElement? Parent { get; set; }

    public List<XMLElement> Children { get; set; } = [];
    public List<XMLAttribute> Attributes { get; set; } = [];

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
        var sb = new StringBuilder();

        sb.Append($"<{Name}");
        if (Attributes.Count > 0)
        {
            sb.Append(' ');
            sb.Append(string.Join(" ", Attributes.Select(a => a.ToString())));
        }
        sb.Append('>');

        foreach (var child in Children)
        {
            foreach (var line in child.ToString().Split('\n'))
            {
                sb.Append($"\n\t{line}");
            }
        }

        sb.Append($"\n</{Name}>");

        return sb.ToString();
    }
}