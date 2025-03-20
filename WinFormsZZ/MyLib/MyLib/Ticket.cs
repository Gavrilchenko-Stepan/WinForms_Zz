using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    public class Ticket
    {
        public List<Question> Questions { get; set; } = new List<Question>();
        public override string ToString()
        {
            string output = "-------------------\n";

            foreach (var question in Questions)
            {
                output += $"{question.Section}: {question.Text}\n";
            }

            output += "-------------------\n";

            return output;
        }
    }
}
