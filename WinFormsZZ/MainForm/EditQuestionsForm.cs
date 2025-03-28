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

namespace MainForm
{
    public partial class EditQuestionsForm: Form
    {
        public List<Question> questions;

        private string[] categories = { "знать", "уметь", "владеть" };

        private string selectedCategory;

        public List<Question> currentQuestions;
        public EditQuestionsForm()
        {
            InitializeComponent();
            InitCategories();
            UpdateCurrentQuestions(null);
            DisplayQuestions(currentQuestions);
        }

        private void InitCategories()
        {
            checkedListBox1.Items.Clear();
            foreach (var category in categories)
            {
                checkedListBox1.Items.Add(category, false);
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCategory = checkedListBox1.CheckedItems.Cast<string>().FirstOrDefault();
            UpdateCurrentQuestions(selectedCategory);
            DisplayQuestions(currentQuestions);
        }

        private void UpdateCurrentQuestions(string category)
        {
            currentQuestions = new List<Question>();

            if (category != null)
            {
                foreach (var question in questions)
                {
                    if (question.Section == category)
                    {
                        currentQuestions.Add(question);
                    }
                }
            }
            else
            {
                if (questions != null)
                {
                    currentQuestions = questions.ToList();
                }
                else
                {
                    currentQuestions = new List<Question>();
                }
            }
        }

        private void DisplayQuestions(List<Question> questions)
        {
            textBox1.Text = "";

            // Отображаем вопросы текущей категории
            foreach (var question in questions)
            {
                textBox1.AppendText($"{question.Text}{Environment.NewLine}{Environment.NewLine}");
            }
        }

        private void buttonОК_Click(object sender, EventArgs e)
        {
            SaveChanges();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        void SaveChanges()
        {
            string[] updatedQuestions = textBox1.Text.Split(new[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            // Обновляем тексты вопросов в списке currentQuestions
            for (int i = 0; i < Math.Min(updatedQuestions.Length, currentQuestions.Count); i++)
            {
                currentQuestions[i].Text = updatedQuestions[i];
            }

            for (int i = 0; i < currentQuestions.Count; i++)
            {
                questions[i].Text = currentQuestions[i].Text;
            }
        }
    }
}
