using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XMLParser.Models
{
    public class TextElement : XMLElement
    {
        public TextElement(string text) : base(string.Empty, text)
        {
        }

        public override string Name => _name;

        public override string ToString()
        {
            return Text ?? string.Empty;
        }
    }
}