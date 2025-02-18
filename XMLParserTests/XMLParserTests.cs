namespace XMLParserTests;

public class XMLParserTests
{
    [Test]
    public async Task TestParseText()
    {
        var parser = new XMLParser.XMLParser(XMLParser.InputType.Text, "<root><child>text</child></root>");
        var document = await parser.Parse();

        Assert.Multiple(() =>
        {
            Assert.That(document?.Root?.Name, Is.EqualTo("root"));
            Assert.That(document?.Root?.Children, Has.Count.EqualTo(1));
        });
        Assert.Multiple(() =>
        {
            Assert.That(document?.Root?.Children[0].Name, Is.EqualTo("child"));
            Assert.That(document?.Root?.Children[0]?.Children, Has.Count.EqualTo(1));
        });
        Assert.That(document?.Root?.Children[0]?.Children[0], Is.TypeOf<XMLParser.Models.XMLTextElement>());
        Assert.That(document?.Root?.Children[0]?.Children[0]?.Text, Is.EqualTo("text"));
    }

    [Test]
    public async Task TestParseFile_Valid()
    {
        var parser = new XMLParser.XMLParser(XMLParser.InputType.File, @"E:\Udemy\NAM.NET-repos\XmlTreeBuild\backend\wwwroot\upload\students.xsl");
        var document = await parser.Parse();

        Assert.Pass();
    }
}