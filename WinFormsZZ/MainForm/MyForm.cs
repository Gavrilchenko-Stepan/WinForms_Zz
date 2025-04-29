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
using System.Reflection;
using Microsoft.VisualBasic;
using System.Globalization;

namespace MainForm
{
    public partial class Form1 : Form
    {
        private QuestionManager questionManager;
        private List<Ticket> _currentTickets;
        private string questionsFilePath;
        private TicketGenerator _ticketGenerator;

        private readonly List<string> _defaultCategories = new List<string>
        {
            "Знать",
            "Уметь",
            "Владеть"
        };

        public Form1()
        {
            InitializeComponent();
            _ticketGenerator = new TicketGenerator();
            listBoxCategories.SelectedIndexChanged += ListBoxCategories_SelectedIndexChanged;
            toolStripButton2.Click += ToolStripButton2_Click;

            this.KeyPreview = true;

            ShowDefaultCategories();
        }

        private void ShowDefaultCategories()
        {
            listBoxCategories.Items.Clear();
            listBoxCategories.Items.AddRange(_defaultCategories.ToArray());

            // Выбираем первую категорию, если есть
            if (listBoxCategories.Items.Count > 0)
            {
                listBoxCategories.SelectedIndex = 0;
            }
        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            if (listBoxCategories.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите категорию для редактирования");
                return;
            }

            if (listBoxQuestions.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите вопрос для редактирования");
                return;
            }

            string category = listBoxCategories.SelectedItem.ToString();
            int questionIndex = listBoxQuestions.SelectedIndex;

            var categoryQuestions = questionManager.Questions
                .Where(q => q.Section.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (questionIndex < 0 || questionIndex >= categoryQuestions.Count)
            {
                MessageBox.Show("Неверный индекс вопроса");
                return;
            }

            var questionToEdit = categoryQuestions[questionIndex];

            using (var editForm = new EditQuestionForm(
                questionToEdit,
                questionManager,
                questionsFilePath,
                _currentTickets,
                _ticketGenerator))
            {
                if (editForm.ShowDialog() == DialogResult.OK && editForm.WasEdited)
                {
                    // Обновляем отображение вопросов
                    DisplayQuestionsByCategory(category);

                    // Обновляем отображение билетов, если они есть
                    if (_currentTickets != null && _currentTickets.Count > 0)
                    {
                        // Не нужно вызывать UpdateTicketsQuestions, так как мы изменили существующий объект
                        textBoxOutput.Text = _ticketGenerator.FormatTickets(_currentTickets);
                    }
                }
            }
        }

        private void ListBoxCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxCategories.SelectedItem != null && questionManager != null)
            {
                string selectedCategory = listBoxCategories.SelectedItem.ToString();
                DisplayQuestionsByCategory(selectedCategory);
                listBoxQuestions.SelectedIndex = -1; // Сбрасываем выбор вопроса при смене категории
            }
        }

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
            }
            else
            {
                // Если вопросов нет, показываем категории по умолчанию
                ShowDefaultCategories();
                return;
            }

            // Если есть категории, выбираем первую
            if (listBoxCategories.Items.Count > 0)
            {
                listBoxCategories.SelectedIndex = 0;
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

            if (!int.TryParse(input, out int numTickets) || numTickets <= 0)
            {
                MessageBox.Show("Ошибка! Введите корректное число билетов.",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            var (tickets, message) = _ticketGenerator.GenerateTickets(questionManager, numTickets);

            if (tickets == null)
            {
                MessageBox.Show(message,
                    "Ошибка генерации билетов",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Сохраняем ссылку на текущие билеты
            _currentTickets = tickets;
            textBoxOutput.Text = _ticketGenerator.FormatTickets(tickets);
        }

        /*private void UpdateTicketsDisplay()
        {
            if (_currentTickets == null || _currentTickets.Count == 0)
            {
                textBoxOutput.Clear();
                return;
            }

            textBoxOutput.Text = _ticketGenerator.FormatTickets(_currentTickets);
        }*/

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
                        // Устанавливаем questionManager после загрузки
                        _ticketGenerator = new TicketGenerator(questionManager);
                    }
                }
            }
        }

        private void DisplayQuestionsByCategory(string category)
        {
            listBoxQuestions.Items.Clear();
            listBoxQuestions.SelectedIndex = -1; // Сбрасываем выбор вопроса

            var categoryQuestions = questionManager.Questions
                .Where(q => q.Section.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();

            for (int i = 0; i < categoryQuestions.Count; i++)
            {
                listBoxQuestions.Items.Add($"Вопрос {i + 1}: {categoryQuestions[i].Text}");
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentTickets == null || _currentTickets.Count == 0)
            {
                MessageBox.Show("Нет билетов для сохранения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word Documents|*.docx";
            saveFileDialog.Title = "Сохранить билеты";
            saveFileDialog.DefaultExt = "docx";
            saveFileDialog.AddExtension = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Получаем встроенный шаблон из ресурсов
                    var assembly = Assembly.GetExecutingAssembly();
                    const string templateResourceName = "MainForm.ШАБЛОН.docx"; // Убедитесь, что имя совпадает с ресурсом

                    using (Stream templateStream = assembly.GetManifestResourceStream(templateResourceName))
                    {
                        if (templateStream == null)
                        {
                            MessageBox.Show("Встроенный шаблон не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Сохраняем временную копию шаблона
                        string tempPath = Path.GetTempFileName();
                        using (FileStream fileStream = File.Create(tempPath))
                        {
                            templateStream.CopyTo(fileStream);
                        }

                        // Генерируем документ
                        WordTemplateFiller.GenerateFromTemplate(tempPath, saveFileDialog.FileName, _currentTickets);

                        // Удаляем временный файл
                        File.Delete(tempPath);
                    }

                    MessageBox.Show($"Билеты успешно сохранены в файл: {saveFileDialog.FileName}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Пытаемся открыть документ
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(saveFileDialog.FileName) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Документ сохранён, но не удалось открыть его: {ex.Message}", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении билетов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (questionManager == null || string.IsNullOrEmpty(questionsFilePath))
            {
                MessageBox.Show("Сначала загрузите файл с вопросами!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (listBoxCategories.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите категорию для добавления вопросов!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedCategory = listBoxCategories.SelectedItem.ToString().ToLower();

            using (var addForm = new AddQuestionForm(questionManager, questionsFilePath, selectedCategory))
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    // Обновляем данные
                    LoadQuestionsFromFile.TheQuestionLoader(questionManager);
                    UpdateCategoriesList();
                    DisplayQuestionsByCategory(selectedCategory);

                    // Обновляем билеты, если они есть
                    if (_currentTickets != null)
                    {
                        textBoxOutput.Text = _ticketGenerator.FormatTickets(_currentTickets);
                    }
                }
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (questionManager == null)
            {
                MessageBox.Show("Вопросы не загружены!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (listBoxCategories.SelectedIndex == -1 || listBoxQuestions.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите категорию и вопрос для удаления!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Подтверждение удаления
            var confirmResult = MessageBox.Show("Вы точно хотите удалить этот вопрос?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
                return;

            try
            {
                string category = listBoxCategories.SelectedItem.ToString();
                int questionIndex = listBoxQuestions.SelectedIndex;
                var categoryQuestions = questionManager.Questions
                    .Where(q => q.Section.Equals(category, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (questionIndex >= 0 && questionIndex < categoryQuestions.Count)
                {
                    var questionToRemove = categoryQuestions[questionIndex];

                    // Проверяем, используется ли вопрос в билетах
                    bool isUsedInTickets = _currentTickets != null &&
                        _currentTickets.Any(t => t.Questions.Contains(questionToRemove));

                    if (isUsedInTickets)
                    {
                        var ticketConfirmResult = MessageBox.Show("Этот вопрос используется в сгенерированных билетах!\n" +
                            "Удалить его и перегенерировать билеты?",
                            "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (ticketConfirmResult != DialogResult.Yes)
                            return;
                    }

                    // Удаляем вопрос
                    questionManager.Questions.Remove(questionToRemove);
                    File.WriteAllLines(questionsFilePath,
                        questionManager.Questions.Select(q => $"{q.Section}|{q.Text}"));

                    // Обновляем билеты, если они были сгенерированы
                    if (_currentTickets != null)
                    {
                        if (isUsedInTickets)
                        {
                            // Полная перегенерация билетов
                            var (newTickets, message) = _ticketGenerator.GenerateTickets(
                                questionManager, _currentTickets.Count);

                            if (newTickets != null)
                            {
                                _currentTickets = newTickets;
                                textBoxOutput.Text = _ticketGenerator.FormatTickets(_currentTickets);
                            }
                        }
                        else
                        {
                            // Точечное обновление билетов
                            _ticketGenerator.UpdateTicketsQuestions(_currentTickets, questionManager);
                            textBoxOutput.Text = _ticketGenerator.FormatTickets(_currentTickets);
                        }
                    }

                    // Обновляем список вопросов
                    DisplayQuestionsByCategory(category);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool IsQuestionUsed(List<Ticket> tickets, Question question)
        {
            return tickets?.Any(t => t.Questions.Contains(question)) ?? false;
        }

        private void listBoxQuestions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                toolStripButton4_Click(sender, e);
            }
        }
    }
}
