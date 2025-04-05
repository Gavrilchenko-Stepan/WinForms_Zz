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
    public partial class EditQuestionsForm: Form
    {
        public List<Question> questions;

        public EditQuestionsForm(List<Question> questions)
        {
            InitializeComponent();
            this.questions = questions.ToList();

            // Заполнение списка категорий
            listBoxCategories.Items.AddRange(new[] { "Знать", "Уметь", "Владеть" });

            // Установка первой категории активной
            if (listBoxCategories.Items.Count > 0)
            {
                listBoxCategories.SelectedIndex = 0;
            }

            this.questions = questions;
        }

        private void DisplayQuestionsByCategory(string category)
        {
            // Очистка RichTextBox перед загрузкой новых вопросов
            textBoxQuestions.Clear();

            textBoxQuestions.AppendText($"Категория: {category}\n");
            int questionNumber = 1;

            // Выборка вопросов по выбранной категории
            foreach (var question in questions.Where(q => q.Section == category))
            {
                textBoxQuestions.AppendText($"\t{questionNumber}. {question.Text}\n");
                questionNumber++;
            }
            textBoxQuestions.AppendText("\n");
        }

        private void EditQuestionsForm_Load(object sender, EventArgs e)
        {
            if (listBoxCategories.SelectedItem != null)
            {
                string selectedCategory = listBoxCategories.SelectedItem.ToString();
                DisplayQuestionsByCategory(selectedCategory);
            }
        }

        private void buttonОК_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void listBoxCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxCategories.SelectedItem != null)
            {
                string selectedCategory = listBoxCategories.SelectedItem.ToString();
                DisplayQuestionsByCategory(selectedCategory);
            }
        }
    }
}
