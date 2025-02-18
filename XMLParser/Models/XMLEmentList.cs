namespace XMLParser.Models;

public class XMLElementList : List<XMLElement>
{
    public XMLElement this[string name]
    {
        get
        {
            return this.FirstOrDefault(e => e.Name == name) ?? throw new XMLParseException($"XML Element '{name}' not found");
        }
    }
}