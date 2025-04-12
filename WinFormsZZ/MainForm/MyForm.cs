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
using System.IO;
using Microsoft.VisualBasic;
using System.Globalization;

namespace MainForm
{
    public partial class Form1 : Form
    {
        private QuestionManager questionManager;
        private string questionsFilePath;
        public Form1()
        {
            InitializeComponent();
            listBoxCategories.SelectedIndexChanged += ListBoxCategories_SelectedIndexChanged;
            toolStripButton2.Click += ToolStripButton2_Click;
        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            // Проверяем, выбран ли вопрос
            if (listBoxQuestions.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите вопрос для редактирования");
                return;
            }

            // Получаем текст вопроса (без номера)
            string currentText = listBoxQuestions.SelectedItem.ToString();
            if (currentText.Contains(":"))
                currentText = currentText.Split(':')[1].Trim();

            // Диалог редактирования через InputBox
            string newText = Microsoft.VisualBasic.Interaction.InputBox(
                "Редактирование вопроса:",  // Заголовок
                "Введите новый текст",       // Подсказка
                currentText,                // Текст по умолчанию
                -1, -1                      // Позиция окна (центр экрана)
            );

            // Если нажали OK и ввели текст
            if (!string.IsNullOrEmpty(newText) && newText != currentText)
            {
                // Обновляем данные
                string category = listBoxCategories.SelectedItem.ToString();
                int selectedIndex = listBoxQuestions.SelectedIndex;

                var question = questionManager.Questions
                    .FirstOrDefault(q => q.Section.Equals(category, StringComparison.OrdinalIgnoreCase)
                                 && q.Text.Equals(currentText));

                if (question != null)
                {
                    question.Text = newText;

                    // Сохраняем в файл
                    File.WriteAllLines(questionsFilePath,
                        questionManager.Questions.Select(q => $"{q.Section}|{q.Text}"));

                    // Обновляем отображение
                    DisplayQuestionsByCategory(category);
                    listBoxQuestions.SelectedIndex = selectedIndex;
                }
            }
        }

        private void ListBoxCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxCategories.SelectedItem != null && questionManager != null)
            {
                string selectedCategory = listBoxCategories.SelectedItem.ToString();
                DisplayQuestionsByCategory(selectedCategory);
            }
        }

        /*private void LoadQuestions(string filePath)
{
   try
   {
       // Проверка файла на ошибки
       if (LoadQuestionsFromFile.CheckFileForErrors(filePath, out var errors))
       {
           questionManager = new QuestionManager(filePath);
           LoadQuestionsFromFile.TheQuestionLoader(questionManager);

           UpdateCategoriesList();

           MessageBox.Show($"Успешно загружено {questionManager.Questions.Count} вопросов.",
                         "Загрузка завершена",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information);
       }
       else
       {
           // Показываем ошибки пользователю
           var errorMessage = new StringBuilder();
           errorMessage.AppendLine("Найдены ошибки в файле вопросов:");
           foreach (var error in errors)
           {
               errorMessage.AppendLine($"- {error}");
           }
           errorMessage.AppendLine("\nПожалуйста, исправьте ошибки и попробуйте снова.");

           MessageBox.Show(errorMessage.ToString(),
                         "Ошибки в файле вопросов",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Error);
       }
   }
   catch (Exception ex)
   {
       MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}",
                     "Ошибка",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
   }
}*/

        private void UpdateCategoriesList()
        {
            listBoxCategories.Items.Clear();

            if (questionManager != null && questionManager.Questions.Count > 0)
            {
                // Получаем уникальные категории из вопросов
                var categories = questionManager.Questions
                .Select(q => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(q.Section.ToLower()))
                .Distinct()
                .ToList();

                listBoxCategories.Items.AddRange(categories.ToArray());

                // Если есть категории, выбираем первую
                if (listBoxCategories.Items.Count > 0)
                {
                    listBoxCategories.SelectedIndex = 0;
                }
            }
        }

        private bool LoadQuestionsFile(string filePath)
        {
            try
            {
                // Проверка файла на ошибки
                if (LoadQuestionsFromFile.CheckFileForErrors(filePath, out var errors))
                {
                    questionManager = new QuestionManager(filePath);
                    LoadQuestionsFromFile.TheQuestionLoader(questionManager);

                    UpdateCategoriesList();

                    MessageBox.Show($"Успешно загружено {questionManager.Questions.Count} вопросов.",
                        "Загрузка завершена",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    // Показываем ошибки пользователю
                    var errorMessage = new StringBuilder();
                    errorMessage.AppendLine("Найдены ошибки в файле вопросов:");
                    foreach (var error in errors)
                    {
                        errorMessage.AppendLine($"- {error}");
                    }
                    errorMessage.AppendLine("\nПожалуйста, исправьте ошибки и попробуйте снова.");

                    MessageBox.Show(errorMessage.ToString(),
                        "Ошибки в файле вопросов",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (questionManager == null || questionManager.Questions.Count == 0)
            {
                MessageBox.Show("Вопросы не загружены. Пожалуйста, загрузите вопросы сначала.",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            string input = toolStripTextBoxInput.Text;
            int numTickets;

            if (int.TryParse(input, out numTickets))
            {
                if (numTickets <= 0)
                {
                    MessageBox.Show("Число билетов должно быть положительным.",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                TicketGenerator ticketGenerator = new TicketGenerator(questionManager);
                var (tickets, message) = ticketGenerator.GenerateTickets(questionManager, numTickets);

                if (tickets == null)
                {
                    MessageBox.Show(message,
                        "Ошибка генерации билетов",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else
                {
                    textBoxOutput.Text = ticketGenerator.FormatTickets(tickets);
                }
            }
            else
            {
                MessageBox.Show("Ошибка! Введите корректное число билетов.",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Выберите файл с вопросами";
                openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    questionsFilePath = openFileDialog.FileName;
                    if (LoadQuestionsFile(questionsFilePath))
                    {
                        // После успешной загрузки обновляем список категорий
                        UpdateCategoriesList();
                    }
                }
            }
        }

        private void DisplayQuestionsByCategory(string category)
        {
            listBoxQuestions.Items.Clear();

            var categoryQuestions = questionManager.Questions
                .Where(q => q.Section.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();

            for (int i = 0; i < categoryQuestions.Count; i++)
            {
                listBoxQuestions.Items.Add($"Вопрос {i + 1}: {categoryQuestions[i].Text}");
            }
        }
    }
}
