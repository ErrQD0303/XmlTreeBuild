using System.Threading.Tasks;
using XMLParser.Models;

namespace XMLParserTests;

public class XMLPrologTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Parsexmlprolog_validinput_returnsexpectedvalues()
    {
        var rawProlog = "<?xml version=\"2.0\" encoding=\"UTF-7\" randomAttribute  = 'OK con de'?>";
        var prolog = await XMLProlog.ParseAsync(rawProlog);
        Assert.That(prolog, Is.Not.Null);
        Assert.That(prolog.Version, Is.EqualTo("2.0"));
        Assert.That(prolog.Encoding, Is.EqualTo("UTF-7"));
        Assert.That(prolog.GetAttribute("randomAttribute"), Is.EqualTo("OK con de"));
        Assert.That(prolog.GetAttribute("rAndomattribute"), Is.EqualTo("OK con de"));
        Assert.That(prolog.Attributes, Has.Count.EqualTo(3));

        Assert.Pass();
    }

    [Test]
    public async Task ParseXMLProlog_GetToString_ThenParsePrologAgain_ThenCheckNameAndAttributeOfTwoXMLPrologObjects()
    {
        var rawProlog = "<?xml version=\"2.0\" encoding=\"UTF-7\" randomAttribute  = 'OK con de'?>";
        var prolog = await XMLProlog.ParseAsync(rawProlog);
        Assert.That(prolog, Is.Not.Null);
        var prologString = prolog.ToString();

        var prolog2 = await XMLDeclaration.ParseAsync(prologString);
        Assert.That(prolog2, Is.Not.Null);
        Assert.That(prolog2.Name, Is.EqualTo(prolog.Name));
        Assert.That(prolog2.Attributes.Count, Is.EqualTo(prolog.Attributes.Count));
        foreach (var attribute in prolog.Attributes)
        {
            Assert.That(prolog2.GetAttribute(attribute.Name), Is.EqualTo(attribute.Value));
        }

        Assert.Pass();
    }
}
