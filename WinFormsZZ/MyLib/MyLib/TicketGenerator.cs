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
        private QuestionManager _questionManager;
        public TicketGenerator(QuestionManager questionManager = null)
        {
            _questionManager = questionManager;
        }
        public (List<Ticket>, string) GenerateTickets(QuestionManager questionManager, int numTickets)
        {
            if (!questionManager.HasEnoughQuestions(numTickets))
            {
                return (null, "Ошибка: недостаточно вопросов для генерации билетов!");
            }

            List<Ticket> tickets = new List<Ticket>();
            List<Question> usedQuestions = new List<Question>();
            for (int i = 0; i < numTickets; i++)
            {
                Ticket ticket = new Ticket();
                List<string> sections = new List<string>(Question.ALL_SECTIONS);

                foreach (string section in sections)
                {
                    Question question = questionManager.GetRandomQuestion(section, usedQuestions);
                    if (question == null)
                    {
                        return (null, "Ошибка: недостаточно вопросов в разделе " + section);
                    }
                    ticket.Questions.Add(question);
                    usedQuestions.Add(question);
                }
                tickets.Add(ticket);
            }
            return (tickets, "");
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

        public void UpdateTicketsQuestions(List<Ticket> tickets, QuestionManager questionManager)
        {
            if (tickets == null || questionManager == null) return;

            foreach (var ticket in tickets)
            {
                for (int i = 0; i < ticket.Questions.Count; i++)
                {
                    var oldQuestion = ticket.Questions[i];
                    // Находим вопрос с тем же текстом и разделом в обновленном списке
                    var updatedQuestion = questionManager.Questions.FirstOrDefault(q =>
                        q.Text == oldQuestion.Text && q.Section == oldQuestion.Section);

                    if (updatedQuestion != null)
                    {
                        // Обновляем вопрос в билете
                        ticket.Questions[i] = updatedQuestion;
                    }
                }
            }
        }
    }
}
