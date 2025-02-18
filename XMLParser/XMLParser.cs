
using System.ComponentModel;
using System.Reflection.Metadata;

namespace XMLParser;

public enum InputType
{
    Text,
    File
}
public class XMLParser
{
    private string _input = String.Empty;
    private Stream? _inputStream;
    private XMLDocument? _xmlDocument;
    private InputType _inputType;

    public string Input
    {
        get => _input;
    }
    public Stream? InputStream
    {
        get => _inputStream;
    }
    public XMLDocument? XmlDocument
    {
        get => _xmlDocument;
    }
    public InputType InputType
    {
        get => _inputType;
    }

    public XMLParser(InputType inputType, string input)
    {
        _inputType = inputType;
        _input = input;
    }

    public async Task<XMLDocument> Parse()
    {
        if (_xmlDocument != null)
        {
            return _xmlDocument;
        }

        _inputStream = _inputType switch
        {
            InputType.Text => await XMLUtils.CreateStreamFromStringAsync(_input),
            InputType.File => new FileStream(_input, FileMode.Open, FileAccess.Read),
            _ => throw new InvalidOperationException("Invalid input type"),
        };

        if (_inputStream == null)
        {
            throw new InvalidOperationException("Invalid input stream");
        }

        using var srd = new StreamReader(_inputStream);

        _xmlDocument = new XMLDocument(null);

        XMLElement? currentParentElement = null;
        XMLElement? currentElement = null;

        var currentChar = Convert.ToChar(srd.Read());
        bool isClosing = true;
        while (srd.Peek() != -1)
        {
            if (currentChar == '\r' || currentChar == '\n' || currentChar == '\t')
            {
                currentChar = Convert.ToChar(srd.Read());
            }
            else if (currentChar == '<')
            {
                var openningTagName = string.Empty;
                var nextChar = Convert.ToChar(srd.Peek());
                if (nextChar == '!')
                {
                    // Is Comment
                    srd.Read(); // Skip '-'
                    int dashCount = 0;
                    while (dashCount < 2)
                    {
                        currentChar = Convert.ToChar(srd.Read());
                        if (currentChar == '-')
                        {
                            dashCount++;
                        }
                        else
                        {
                            dashCount = 0;
                        }
                    }
                    currentChar = Convert.ToChar(srd.Read());

                    var comment = string.Empty;
                    while (srd.Peek() != -1 && currentChar != '-')
                    {
                        comment += currentChar;
                        currentChar = Convert.ToChar(srd.Read());
                    }

                    while (srd.Peek() != -1 && currentChar == '-' && dashCount < 4)
                    {
                        dashCount++;
                        currentChar = Convert.ToChar(srd.Read());
                    }

                    if (dashCount != 4)
                    {
                        throw new XMLParseException("Invalid XML Comment");
                    }

                    if (currentChar != '>')
                    {
                        throw new XMLParseException("Invalid XML Comment");
                    }

                    currentElement = new XMLComment(comment)
                    {
                        Parent = currentParentElement
                    };

                    currentParentElement?.AddChild(currentElement);

                    currentElement = currentParentElement;
                    currentParentElement = currentElement?.Parent;
                }
                else if (nextChar == '?')
                { // Is Prolog or Declaration
                    var prolog = string.Empty;
                    while (currentChar != '>')
                    {
                        prolog += currentChar;
                        currentChar = Convert.ToChar(srd.Read());
                    }
                    if (prolog.StartsWith("<?xml "))
                    {
                        _xmlDocument.Prolog = await XMLProlog.ParseAsync(prolog);
                    }
                    else
                    {
                        _xmlDocument.Declaration = await XMLDeclaration.ParseAsync(prolog);
                    }
                    currentChar = Convert.ToChar(srd.Read());
                }
                else if (nextChar == '/')
                { // is Closing Tag
                    if (isClosing)
                    {
                        currentParentElement = currentElement?.Parent;
                    }
                    srd.Read(); // Skip '/'

                    currentChar = Convert.ToChar(srd.Read());
                    var closingTagName = string.Empty;
                    while (currentChar != '>')
                    {
                        closingTagName += currentChar;
                        currentChar = Convert.ToChar(srd.Read());
                    }

                    XMLUtils.EnsureElementNameCorrect(closingTagName);

                    if (currentElement?.Name != closingTagName)
                    {
                        throw new XMLParseException($"Closing tag '{closingTagName}' does not match openning tag '{currentElement?.Name}'");
                    }

                    while (srd.Peek() != -1 && (currentChar == '\r' || currentChar == '\n' || currentChar == '\t'))
                    {
                        currentChar = Convert.ToChar(srd.Read());
                        nextChar = Convert.ToChar(srd.Peek());
                    }

                    currentElement = currentParentElement;
                    currentParentElement = currentElement?.Parent;
                    openningTagName = string.Empty;
                    isClosing = true;
                }
                else
                {
                    isClosing = false;
                    currentElement = null;
                    openningTagName = string.Empty;
                    while (srd.Peek() != -1 && nextChar != ' ' && nextChar != '>' && nextChar != '/')
                    {
                        currentChar = Convert.ToChar(srd.Read());
                        openningTagName += currentChar;
                        nextChar = Convert.ToChar(srd.Peek());
                    }

                    XMLUtils.EnsureElementNameCorrect(openningTagName);

                    // Make link between parent and child
                    currentElement = new XMLNodeElement(openningTagName)
                    {
                        Parent = currentParentElement
                    };

                    currentParentElement?.AddChild(currentElement);

                    // Read Attributes
                    var elementAttributes = XMLUtils.ParseAttributes(srd, '>', '/');
                    foreach (var attribute in elementAttributes)
                    {
                        currentElement.AddAttribute(attribute.Name, attribute.Value);
                    }

                    while (currentChar != '>' && currentChar != '/')
                    {
                        currentChar = Convert.ToChar(srd.Read());
                    }

                    _xmlDocument.Root ??= currentElement;
                }
            }
            else if (currentChar == '>')
            {
                var nextChar = Convert.ToChar(srd.Peek());

                currentParentElement = currentElement;

                while (nextChar == '\r' || nextChar == '\n' || nextChar == '\t')
                {
                    currentChar = Convert.ToChar(srd.Read());
                    nextChar = Convert.ToChar(srd.Peek());
                }

                if (nextChar == '<')
                {
                    currentChar = Convert.ToChar(srd.Read());

                    continue;
                }

                var textElementContent = string.Empty;
                currentChar = Convert.ToChar(srd.Read());
                while (currentChar != '<')
                {
                    textElementContent += currentChar;
                    currentChar = Convert.ToChar(srd.Read());
                    if (srd.Peek() != -1)
                    {
                        nextChar = Convert.ToChar(srd.Peek());
                    }
                }

                currentElement = new XMLTextElement(textElementContent)
                {
                    Parent = currentParentElement
                };

                currentParentElement?.AddChild(currentElement);

                currentElement = currentParentElement;
                currentParentElement = currentElement?.Parent;
            }
            else if (currentChar == '/')
            {
                currentChar = Convert.ToChar(srd.Read());
                if (currentChar != '>')
                {
                    throw new XMLParseException($"Invalid XML File Format at position {srd.BaseStream.Position}");
                }

                currentElement = currentParentElement;
                currentParentElement = currentElement?.Parent;

                isClosing = true;
            }
            else
            {
                throw new XMLParseException($"Invalid XML File Format at position {srd.BaseStream.Position}");
            }
        }

        return _xmlDocument;
    }

}