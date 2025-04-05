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

namespace MainForm
{
    public partial class Form1 : Form
    {
        private QuestionManager questionManager;
        private string questionsFilePath;
        public Form1()
        {
            InitializeComponent();
        }
        private void LoadQuestions(string filePath)
        {
            try
            {
                // Проверка файла на ошибки
                if (LoadQuestionsFromFile.CheckFileForErrors(filePath, out var errors))
                {
                    questionManager = new QuestionManager(filePath);
                    LoadQuestionsFromFile.TheQuestionLoader(questionManager);

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

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (questionManager == null || questionManager.Questions.Count == 0)
            {
                MessageBox.Show("Вопросы не загружены. Пожалуйста, загрузите вопросы сначала.",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            EditQuestionsForm editForm = new EditQuestionsForm(questionManager.Questions);

            if (editForm.ShowDialog(this) == DialogResult.OK)
            {
                questionManager.Questions = editForm.questions;

                // Сохраняем изменения обратно в файл
                try
                {
                    File.WriteAllLines(questionsFilePath,
                        questionManager.Questions.Select(q => $"{q.Section}|{q.Text}"));

                    MessageBox.Show("Изменения успешно сохранены.",
                        "Сохранено",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            editForm.Dispose();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
                    LoadQuestionsFile(questionsFilePath);
                }
            }
        }
    }
}
