using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    internal class TicketGenerator
    {


        public List<Question> Questions { get; set; } = new List<Question>();

        public Question GetRandomQuestion(string section, List<Question> usedQuestions)
        {
            var AvailableQuestions = new List<Question>();
            foreach (var question in Questions)
            {
                if (question.Section == section && !usedQuestions.Contains(question))
                {
                    AvailableQuestions.Add(question);
                }
            }

            if (AvailableQuestions.Count == 0)
            {
                return null;
            }
            Random rnd = new Random();
            int index = rnd.Next(AvailableQuestions.Count);

            return AvailableQuestions[index];
        }


       



        public List<Ticket> GenerateTickets(List<Question> text, int numTickets)
        {
            
            List<Ticket> tickets = new List<Ticket>();
            var usedQuestions = new List<Question>();







            for (int i = 0; i < numTickets; i++)
            {
                var ticket = new Ticket();
                var sections = new List<string> { "знать", "уметь", "владеть" };
                foreach (var section in sections)
                {
                    var question = .GetRandomQuestion(section, usedQuestions);
                    if (question == null)
                    {
                        return null; // Если нет вопросов в разделе для этого билета
                    }
                    ticket.Questions.Add(question);
                    usedQuestions.Add(question);
                }
                tickets.Add(ticket);
            }
            return tickets;
        }
    }
}
