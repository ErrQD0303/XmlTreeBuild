using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace XMLParser.Helpers;

public partial class XMLUtils
{
    public static Regex ElementNameRegex { get; } = ValidateElementNameRegex();
    public static Regex AttributeNameRegex { get; } = ValidateAttributeNameRegex();
    public static void EnsureElementNameCorrect(string elementName)
    {
        if (string.IsNullOrWhiteSpace(elementName))
        {
            throw new XMLParseException("Element name cannot be null or empty");
        }

        if (!ElementNameRegex.IsMatch(elementName))
        {
            throw new XMLParseException($"Element name '{elementName}' is invalid");
        }
    }

    public static void EnsureAttributeNameCorrect(string attributeName)
    {
        if (string.IsNullOrWhiteSpace(attributeName))
        {
            throw new XMLParseException("Attribute name cannot be null or empty");
        }

        if (!AttributeNameRegex.IsMatch(attributeName))
        {
            throw new XMLParseException($"Attribute name '{attributeName}' is invalid");
        }
    }

    [GeneratedRegex(@"^(?i)(?!xml)(?-i)[a-zA-Z_][a-zA-Z0-9_-]*$")]
    private static partial Regex ValidateElementNameRegex();

    [GeneratedRegex(@"^[a-zA-Z_][a-zA-Z0-9_-]*$")]
    private static partial Regex ValidateAttributeNameRegex();


    public static async Task<Stream?> CreateStreamFromStringAsync(string content)
    {
        MemoryStream stream = new();
        StreamWriter writer = new(stream);
        writer.Write(content);
        await writer.FlushAsync();
        stream.Position = 0;
        return stream;
    }

    public static List<XMLAttribute> ParseAttributes(StreamReader sr, char endTagChar)
    {
        var attributes = new List<XMLAttribute>();
        var currentAttributeName = string.Empty;
        var currentAttributeValue = string.Empty;
        while (sr.Peek() != -1 && Convert.ToChar(sr.Peek()) != endTagChar)
        {
            if (string.IsNullOrEmpty(currentAttributeName))
            {
                RemoveWhitespace(sr);
                currentAttributeName = ReadAttributeName(sr, '=');

                RemoveWhitespace(sr);
                sr.Read(); // Remove the '=' character
            }
            else if (string.IsNullOrEmpty(currentAttributeValue))
            {
                RemoveWhitespace(sr);
                var currentChar = Convert.ToChar(sr.Read());
                if (currentChar == '"' || currentChar == '\'')
                {
                    currentAttributeValue = ReadAttributeValue(sr, currentChar);
                }

                RemoveWhitespace(sr);
                sr.Read(); // Remove the end tag character

                attributes.Add(new XMLAttribute(currentAttributeName, currentAttributeValue));
                currentAttributeName = currentAttributeValue = string.Empty;
            }
        }

        return attributes;
    }

    public static void RemoveWhitespace(StreamReader sr)
    {
        while (char.IsWhiteSpace(Convert.ToChar(sr.Peek())))
        {
            sr.Read();
        }
    }

    public static string ReadAttributeName(StreamReader sr, char endTagChar)
    {
        string name = string.Empty;
        while (char.IsLetterOrDigit(Convert.ToChar(sr.Peek())) || Convert.ToChar(sr.Peek()) == '_')
        {
            name += Convert.ToChar(sr.Read());
        }

        return name.ToLower();
    }

    public static string ReadAttributeValue(StreamReader sr, char endTagChar = '"')
    {
        string value = string.Empty;
        while (sr.Peek() != -1 && Convert.ToChar(sr.Peek()) != endTagChar)
        {
            value += Convert.ToChar(sr.Read());
        }

        return value;
    }
}