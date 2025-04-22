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
        public Form1()
        {
            InitializeComponent();
            _ticketGenerator = new TicketGenerator();
            listBoxCategories.SelectedIndexChanged += ListBoxCategories_SelectedIndexChanged;
            toolStripButton2.Click += ToolStripButton2_Click;
        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            if (listBoxCategories.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите категорию для редактирования");
                return;
            }

            string category = listBoxCategories.SelectedItem.ToString();

            var categoryQuestions = questionManager.Questions
                .Where(q => q.Section.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Создаем текстовое представление всех вопросов
            StringBuilder sb = new StringBuilder();
            foreach (var question in categoryQuestions)
            {
                sb.AppendLine(question.Text);
            }

            Form editForm = new Form();
            editForm.Text = $"Редактирование категории: {category}";
            editForm.ClientSize = new Size(600, 400); // Используем ClientSize вместо Size
            editForm.StartPosition = FormStartPosition.CenterScreen;
            editForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            editForm.MaximizeBox = false;
            editForm.MinimizeBox = false;

            // Сначала создаем кнопки
            System.Windows.Forms.Button btnSave = new System.Windows.Forms.Button();
            btnSave.Text = "Сохранить";
            btnSave.Size = new Size(100, 30);
            btnSave.Font = new Font("Segoe UI", 9);

            System.Windows.Forms.Button btnCancel = new System.Windows.Forms.Button();
            btnCancel.Text = "Отмена";
            btnCancel.Size = new Size(100, 30);
            btnCancel.Font = new Font("Segoe UI", 9);
            btnCancel.DialogResult = DialogResult.Cancel;

            // Затем создаем RichTextBox с учетом места для кнопок
            RichTextBox rtb = new RichTextBox();
            rtb.Location = new Point(10, 10);
            rtb.Size = new Size(editForm.ClientSize.Width - 20, editForm.ClientSize.Height - 60);
            rtb.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rtb.Text = sb.ToString();
            rtb.Font = new Font("Segoe UI", 10);
            rtb.WordWrap = true;
            rtb.ScrollBars = RichTextBoxScrollBars.Vertical;

            // Позиционируем кнопки
            btnCancel.Location = new Point(editForm.ClientSize.Width - btnCancel.Width - 10,
                                         editForm.ClientSize.Height - btnCancel.Height - 10);
            btnSave.Location = new Point(btnCancel.Left - btnSave.Width - 10,
                                        editForm.ClientSize.Height - btnSave.Height - 10);

            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            btnSave.Click += (s, args) =>
            {
                string[] allLines = rtb.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                var newQuestions = allLines.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();

                if (newQuestions.Count != categoryQuestions.Count)
                {
                    MessageBox.Show($"Ожидалось {categoryQuestions.Count} вопросов, получено {newQuestions.Count}.\n" +
                                  "Количество вопросов не должно изменяться.",
                                  "Ошибка",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                    return;
                }

                // Сохраняем старые вопросы для обновления билетов
                var oldQuestions = new List<Question>(categoryQuestions);

                // Обновляем вопросы
                for (int i = 0; i < categoryQuestions.Count; i++)
                {
                    categoryQuestions[i].Text = newQuestions[i];
                }

                // Сохраняем в файл
                File.WriteAllLines(questionsFilePath,
                    questionManager.Questions.Select(q => $"{q.Section}|{q.Text}"));

                // Обновляем билеты, если они есть
                if (_currentTickets != null && _currentTickets.Count > 0)
                {
                    _ticketGenerator.UpdateTicketsQuestions(_currentTickets, questionManager);
                    textBoxOutput.Text = _ticketGenerator.FormatTickets(_currentTickets);
                }

                DisplayQuestionsByCategory(category);
                editForm.DialogResult = DialogResult.OK;
            };

            // Добавляем элементы в правильном порядке
            editForm.Controls.Add(rtb);
            editForm.Controls.Add(btnSave);
            editForm.Controls.Add(btnCancel);

            if (editForm.ShowDialog() == DialogResult.OK)
            {
                DisplayQuestionsByCategory(category);
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

        private void UpdateCategoriesList()
        {
            listBoxCategories.Items.Clear();

            if (questionManager != null && questionManager.Questions.Count > 0)
            {
                // Добавляем категории с заглавной буквы
                listBoxCategories.Items.Add(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Question.KNOW));
                listBoxCategories.Items.Add(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Question.ABLE));
                listBoxCategories.Items.Add(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Question.MASTER));

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
            var questions = questionManager.Questions
                .Where(q => q.Section.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();

            for (int i = 0; i < questions.Count; i++)
            {
                listBoxQuestions.Items.Add($"{i + 1}. {questions[i].Text}");
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
            if (listBoxCategories.SelectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, выберите категорию для добавления вопросов",
                               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedCategory = listBoxCategories.SelectedItem.ToString().ToLower();

            // Создаем форму для ввода нескольких вопросов
            Form addQuestionsForm = new Form();
            addQuestionsForm.Text = $"Добавить вопросы в категорию: {selectedCategory}";
            addQuestionsForm.ClientSize = new Size(600, 400);
            addQuestionsForm.StartPosition = FormStartPosition.CenterParent;
            addQuestionsForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            addQuestionsForm.MaximizeBox = false;
            addQuestionsForm.MinimizeBox = false;

            // Многострочное текстовое поле с подсказкой
            RichTextBox rtbQuestions = new RichTextBox();
            rtbQuestions.Multiline = true;
            rtbQuestions.Dock = DockStyle.Fill;
            rtbQuestions.Font = new Font("Consolas", 11); // Моноширинный шрифт
            rtbQuestions.AcceptsTab = true;
            rtbQuestions.WordWrap = false; // Отключаем перенос слов
            rtbQuestions.ScrollBars = RichTextBoxScrollBars.Both;

            rtbQuestions.KeyDown += (senderKey, eKey) =>
            {
                if (eKey.KeyCode == Keys.Enter && !eKey.Shift)
                {
                    eKey.SuppressKeyPress = true;
                    int pos = rtbQuestions.SelectionStart;
                    rtbQuestions.Text = rtbQuestions.Text.Insert(pos, Environment.NewLine);
                    rtbQuestions.SelectionStart = pos + Environment.NewLine.Length;
                }
            };
            rtbQuestions.Text = "Введите вопросы, каждый с новой строки:\nПример 1\nПример 2\nПример 3";

            // Очищаем подсказку при первом клике
            rtbQuestions.Enter += (s, args) =>
            {
                if (rtbQuestions.Text.Contains("Введите вопросы"))
                    rtbQuestions.Text = string.Empty;
            };

            // Кнопки
            System.Windows.Forms.Button btnOk = new System.Windows.Forms.Button() { Text = "Добавить все", DialogResult = DialogResult.OK };
            System.Windows.Forms.Button btnCancel = new System.Windows.Forms.Button() { Text = "Отмена", DialogResult = DialogResult.Cancel };

            // Панель для кнопок внизу формы
            Panel panel = new Panel();
            panel.Dock = DockStyle.Bottom;
            panel.Height = 50;
            panel.Padding = new Padding(10);

            btnOk.Anchor = btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(panel.Width - btnCancel.Width - 10, 10);
            btnOk.Location = new Point(btnCancel.Left - btnOk.Width - 10, 10);

            // Добавляем элементы
            panel.Controls.Add(btnCancel);
            panel.Controls.Add(btnOk);
            addQuestionsForm.Controls.Add(panel);
            addQuestionsForm.Controls.Add(rtbQuestions);
            addQuestionsForm.AcceptButton = btnOk;
            addQuestionsForm.CancelButton = btnCancel;

            if (addQuestionsForm.ShowDialog(this) == DialogResult.OK)
            {
                string[] allLines = rtbQuestions.Text.Split(
                    new[] { "\r\n", "\r", "\n" },
                     StringSplitOptions.RemoveEmptyEntries);

                // Фильтруем пустые строки и строки-подсказки
                var questions = allLines
                    .Where(line => !string.IsNullOrWhiteSpace(line) &&
                                  !line.Contains("Введите вопросы") &&
                                  !line.StartsWith("Пример вопроса"))
                    .Select(q => q.Trim())
                    .ToList();
                if (string.IsNullOrWhiteSpace(rtbQuestions.Text))
                {
                    MessageBox.Show("Не введено ни одного вопроса!",
                                  "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (questions.Count == 0)
                {
                    MessageBox.Show("Не обнаружено корректных вопросов для добавления",
                                  "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Добавляем все вопросы
                var addedQuestions = new List<string>();
                int addedCount = 0;

                foreach (var questionText in questions)
                {
                    if (questionManager.Questions.Any(q => q.Text.Equals(questionText) && q.Section.Equals(selectedCategory)))
                    {
                        // Вопрос уже существует
                        continue;
                    }
                    questionManager.AddQuestion(questionText, selectedCategory);
                    addedQuestions.Add(questionText);
                    addedCount++;
                }

                // Обновляем интерфейс и сохраняем
                DisplayQuestionsByCategory(selectedCategory);
                File.WriteAllLines(questionsFilePath,
                    questionManager.Questions.Select(q => $"{q.Section}|{q.Text}"));

                MessageBox.Show($"Успешно добавлено {addedCount} из {questions.Count} вопросов!",
                              "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
