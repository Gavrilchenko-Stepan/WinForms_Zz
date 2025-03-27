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
        private List<Question> questions;

        private string[] categories = { "знать", "уметь", "владеть" };

        private string selectedCategory;

        private List<Question> currentQuestions;
        public EditQuestionsForm()
        {
            InitializeComponent();
        }

        private void InitCategories()
        {
            // Заполняем CheckedListBox категориями
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
                currentQuestions = questions.ToList();
            }
        }
    }
}
