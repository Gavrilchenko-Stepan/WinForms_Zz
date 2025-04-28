using MyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace MainForm
{
    public partial class AddQuestionForm : Form
    {
        private readonly QuestionManager _questionManager;
        private readonly string _filename;
        private readonly string _selectedCategory;
        public AddQuestionForm(QuestionManager questionManager, string filename, string selectedCategory)
        {
            InitializeComponent();
            _questionManager = questionManager;
            _filename = filename;
            _selectedCategory = selectedCategory;

            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = $"Добавление вопросов в '{_selectedCategory}'";
            this.StartPosition = FormStartPosition.CenterParent;

            // Настройка RichTextBox
            richTextBox1.Text = "Введите вопросы (каждый с новой строки):\nПример 1\nПример 2";
            richTextBox1.Enter += (s, e) =>
            {
                if (richTextBox1.Text.StartsWith("Введите вопросы"))
                    richTextBox1.Text = string.Empty;
            };
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            var questions = GetValidQuestions();
            if (questions.Count == 0)
            {
                MessageBox.Show("Нет вопросов для добавления!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AddQuestionsToManager(questions);
        }

        private List<string> GetValidQuestions()
        {
            return richTextBox1.Text.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.RemoveEmptyEntries)
                .Where(q => !string.IsNullOrWhiteSpace(q) &&
                       !q.StartsWith("Пример") &&
                       !q.StartsWith("Введите вопросы"))
                .Select(q => q.Trim())
                .ToList();
        }

        private void AddQuestionsToManager(List<string> questions)
        {
            try
            {
                int addedCount = 0;
                foreach (var question in questions)
                {
                    if (!_questionManager.Questions.Any(q =>
                        q.Text.Equals(question) &&
                        q.Section.Equals(_selectedCategory)))
                    {
                        _questionManager.AddQuestion(question, _selectedCategory);
                        addedCount++;
                    }
                }

                if (addedCount > 0)
                {
                    File.WriteAllLines(_filename,
                        _questionManager.Questions.Select(q => $"{q.Section}|{q.Text}"));

                    MessageBox.Show($"Добавлено {addedCount} вопрос(а/ов)!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Все вопросы уже существуют в этой категории!", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
