using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XMLParser.Models
{
    public class XMLTextElement : XMLElement
    {
        public override string Text { get; }
        public XMLTextElement(string text) : base("text")
        {
            Text = text;
        }

        public override string Name => _name;

        public override string ToString()
        {
            return Text ?? string.Empty;
        }
    }
}