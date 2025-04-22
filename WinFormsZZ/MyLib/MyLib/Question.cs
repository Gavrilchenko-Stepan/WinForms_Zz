using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
     public class Question
    {
        public const string KNOW = "знать";
        public const string ABLE = "уметь";
        public const string MASTER = "владеть";

        public static readonly string[] ALL_SECTIONS = { KNOW, ABLE, MASTER };
        public string Text { get; set; }
        public string Section { get; set; }

        public Question(string text, string section)
        {
            Text = text;
            Section = section;
        }
    }
}
