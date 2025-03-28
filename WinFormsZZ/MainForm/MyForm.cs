﻿using MyLib;
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
                Console.WriteLine($"Количество билетов: {numTickets}");
            }
            else
            {
                MessageBox.Show("Ошибка! Введите корректное число.");
            }
            TicketGenerator ticketGenerator = new TicketGenerator(questionManager);
            var (tickets, message) = ticketGenerator.GenerateTickets(questionManager, numTickets);
            if (tickets == null)
            {
                MessageBox.Show(message);
            }
            else
            {
                textBoxOutput.Text = ticketGenerator.FormatTickets(tickets);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            EditQuestionsForm editForm = new EditQuestionsForm();

            editForm.questions = questionManager.Questions; // Передача списка вопросов в форму редактирования

            DialogResult result = editForm.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                questionManager.Questions = editForm.currentQuestions; // Обновление списка вопросов после закрытия формы редактирования
            }

            editForm.Dispose();
        }
    }
}
