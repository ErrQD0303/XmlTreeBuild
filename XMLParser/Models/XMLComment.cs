using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XMLParser.Models
{
    public class XMLComment : XMLElement
    {
        public override string Text { get; }
        public XMLComment(string text) : base("comment")
        {
            Text = text;
        }

        public override string ToString()
        {
            return $"<!--{Text}-->";
        }
    }
}