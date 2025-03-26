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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            questionManager.AddQuestion("Какова разница между абстрактным классом и интерфейсом?", "знать");
            questionManager.AddQuestion("Как использовать делегаты и события в С#?", "знать");
            questionManager.AddQuestion("Как создать и использовать обобщенные методы?", "уметь");
            questionManager.AddQuestion("Как обрабатывать асинхронные операции с помощью async/await?", "уметь");
            questionManager.AddQuestion("Как оптимизировать производительность приложения?", "владеть");
            questionManager.AddQuestion("Как написать многопоточное приложение?", "владеть");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string input = toolStripTextBoxInput.Text;
            int numTickets;

            if (int.TryParse(input, out numTickets))
            {
                // Если преобразование прошло успешно, используем переменную numTickets
                Console.WriteLine($"Количество билетов: {numTickets}");
            }
            else
            {
                MessageBox.Show("Ошибка! Введите корректное число.");
            }
            var ticketGenerator = new TicketGenerator();
            var (tickets, message) = GenerateTickets(questionManager, numTickets);
            if (tickets == null)
            {
                MessageBox.Show(message);
            }
            else
            {
                textBoxOutput.Text = ticketGenerator.FormatTickets(tickets);
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
