using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    public class TicketGenerator
    {
        public List<Ticket> GenerateTickets(QuestionManager questionManager, int numTickets)
        {
            if (!questionManager.HasEnoughQuestions(numTickets))
            {
                return null;
            }
            List<Ticket> tickets = new List<Ticket>();
            List<Question> usedQuestions = new List<Question>();
            for (int i = 0; i < numTickets; i++)
            {
                Ticket ticket = new Ticket();
                List<string> sections = new List<string> { "знать", "уметь", "владеть" };
                foreach (string section in sections)
                {
                    Question question = questionManager.GetRandomQuestion(section, usedQuestions);
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
