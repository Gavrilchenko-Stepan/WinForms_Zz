using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    internal class Question
    {
        public string Text { get; set; }
        public string Section { get; set; } // "знать", "уметь", "владеть"

        public Question(string text, string section)
        {
            Text = text;
            Section = section;
        }
    }
}
