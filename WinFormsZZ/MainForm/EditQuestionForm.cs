using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLib;
using System.IO;
using System.Windows.Forms;

namespace MainForm
{
    public partial class EditQuestionForm : Form
    {
        public Question EditedQuestion { get; private set; }
        public bool WasEdited { get; private set; } = false;
        private readonly Question _questionToEdit;
        private readonly QuestionManager _questionManager;
        private readonly string _questionsFilePath;
        private readonly List<Ticket> _currentTickets;
        private readonly TicketGenerator _ticketGenerator;

        public EditQuestionForm(Question question, QuestionManager questionManager,
                              string questionsFilePath, List<Ticket> currentTickets,
                              TicketGenerator ticketGenerator)
        {
            InitializeComponent();

            _questionToEdit = question;
            _questionManager = questionManager;
            _questionsFilePath = questionsFilePath;
            _currentTickets = currentTickets;
            _ticketGenerator = ticketGenerator;

            Text = $"Редактирование вопроса: {question.Text}";
            richTextBox1.Text = question.Text;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string newText = richTextBox1.Text.Trim();
            if (string.IsNullOrEmpty(newText))
            {
                MessageBox.Show("Текст вопроса не может быть пустым", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Изменяем существующий вопрос, а не создаем новый
            _questionToEdit.Text = newText;
            WasEdited = true;

            // Сохраняем в файл
            File.WriteAllLines(_questionsFilePath,
                _questionManager.Questions.Select(q => $"{q.Section}|{q.Text}"));

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
