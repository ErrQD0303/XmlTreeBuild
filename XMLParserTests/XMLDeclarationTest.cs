using System.Threading.Tasks;
using XMLParser.Models;

namespace XMLParserTests;

public class XMLDeclarationTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Parsexmldeclaration_validinput_returnsexpectedvalues()
    {
        var rawXmlDeclaration = @"<?xml-stylesheet type=""text/xsl"" href=""./students.xsl""?>";
        var xmlDeclaration = await XMLDeclaration.ParseAsync(rawXmlDeclaration);
        Assert.That(xmlDeclaration, Is.Not.Null);
        Assert.That(xmlDeclaration.Name, Is.EqualTo("xml-stylesheet"));
        Assert.That(xmlDeclaration.GetAttribute("type"), Is.EqualTo("text/xsl"));
        Assert.That(xmlDeclaration.GetAttribute("href"), Is.EqualTo("./students.xsl"));
        Assert.That(xmlDeclaration.Attributes.Count, Is.EqualTo(2));

        Assert.Pass();
    }

    [Test]
    public async Task ParseXMLProlog_GetToString_ThenParsePrologAgain_ThenCheckNameAndAttributeOfTwoXMLPrologObjects()
    {
        var rawXmlDeclaration = @"<?xml-stylesheet type=""text/xsl"" href=""./students.xsl""?>";
        var xmlDeclaration = await XMLDeclaration.ParseAsync(rawXmlDeclaration);
        Assert.That(xmlDeclaration, Is.Not.Null);
        var xmlDeclarationString = xmlDeclaration.ToString();

        var xmlDeclaration2 = await XMLDeclaration.ParseAsync(xmlDeclarationString);
        Assert.That(xmlDeclaration2, Is.Not.Null);
        Assert.That(xmlDeclaration2.Name, Is.EqualTo(xmlDeclaration.Name));
        Assert.That(xmlDeclaration2.Attributes.Count, Is.EqualTo(xmlDeclaration.Attributes.Count));
        foreach (var attribute in xmlDeclaration.Attributes)
        {
            Assert.That(xmlDeclaration2.GetAttribute(attribute.Name), Is.EqualTo(attribute.Value));
        }

        Assert.Pass();
    }
}
