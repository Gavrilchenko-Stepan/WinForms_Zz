using MyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm
{
    public partial class Form1 : Form
    {
        private QuestionManager questionManager;
        public Form1()
        {
            InitializeComponent();
            questionManager = new QuestionManager();

            questionManager.AddQuestion("Что такое объект?", "знать");
            questionManager.AddQuestion("Какие бывают типы данных?", "знать");
            questionManager.AddQuestion("Как создать функцию в С#?", "уметь");
            questionManager.AddQuestion("Как обработать исключение?", "уметь");
            questionManager.AddQuestion("Как работать с файлами?", "владеть");
            questionManager.AddQuestion("Как взаимодействовать с базой данных?", "владеть");
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            int numTickets = (int)numericUpDown1.Value; // Предполагаем, что есть NumericUpDown для выбора количества билетов
            var (tickets, message) = GenerateTickets(questionManager, numTickets);
            if (tickets == null)
            {
                MessageBox.Show(message);
            }
            else
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
                textBoxOutput.Text = outputBuilder.ToString();
            }
        }

        private (List<Ticket>, string) GenerateTickets(QuestionManager questionManager, int numTickets)
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
                List<string> sections = new List<string> { "знать", "уметь", "владеть" };

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
    }
}
