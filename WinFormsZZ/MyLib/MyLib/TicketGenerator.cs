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
        public (List<Ticket>, string) GenerateTickets(QuestionManager questionManager, int numTickets)
        {
            if (!questionManager.HasEnoughQuestions(numTickets))
            {
                return (null, "Ошибка!");
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
                        return (null, "Ошибка!");
                    }
                    ticket.Questions.Add(question);
                    usedQuestions.Add(question);
                }
                tickets.Add(ticket);
            }
            return (tickets, "Ошибка!");
        }

        public string FormatTickets(List<Ticket> tickets)
        {
            StringBuilder outputBuilder = new StringBuilder();

            for (int i = 0; i < tickets.Count; i++)
            {
                Ticket ticket = tickets[i];
                outputBuilder.AppendLine($"Билет #{i + 1}:");

                foreach (var question in ticket.Questions)
                {
                    outputBuilder.AppendLine($"  - Вопрос: {question.Text}");
                    outputBuilder.AppendLine($"    Раздел: {question.Section}");
                    outputBuilder.AppendLine();
                }

                outputBuilder.AppendLine(new string('-', 40)); // Разделитель между билетами
            }

            return outputBuilder.ToString();
        }
    }
}
