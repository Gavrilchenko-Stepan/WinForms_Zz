using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    class Question
    {
        // Текст вопроса
        public string Text { get; set; }

        // Раздел вопроса ("знать", "уметь", "владеть")
        public string Section { get; set; }

        // Конструктор для создания объекта вопроса
        public Question(string text, string section)
        {
            Text = text;
            Section = section;
        }
    }
}
