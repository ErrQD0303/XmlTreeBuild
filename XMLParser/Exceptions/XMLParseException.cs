namespace XMLParser.Exceptions;

public class XMLParseException : Exception
{
    public XMLParseException(string message) : base(message)
    {
    }

    public XMLParseException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public XMLParseException()
    {
    }
}