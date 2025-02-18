
namespace XMLParser;

public enum InputType
{
    Text,
    File
}
public abstract class XMLParser
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

        var currentChar = Convert.ToChar(srd.Read());
        if (currentChar == '<')
        {
            if (srd.Peek() == '?')
            { // Is Prolog or Declaration
                var prolog = string.Empty;
                while (currentChar != '>')
                {
                    prolog += currentChar;
                    currentChar = Convert.ToChar(srd.Read());
                }
                if (prolog.StartsWith("<?xml "))
                {
                    _xmlDocument.Prolog = await XMLDeclaration.ParseAsync(prolog);
                }
                else
                {
                    _xmlDocument.Declaration = await XMLDeclaration.ParseAsync(prolog);
                }
            }
        }

        return _xmlDocument;
    }

}