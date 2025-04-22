using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace MyLib
{
     public class QuestionManager
    {
        // Список всех вопросов
        public List<Question> Questions { get; set; } = new List<Question>();
        public string _filename;
        public QuestionManager(string filename)
        {
            _filename = filename;
        }

        // Добавление нового вопроса
        public void AddQuestion(string text, string section)
        {
            if (!Question.ALL_SECTIONS.Contains(section.ToLower()))
                throw new ArgumentException("Неверная категория вопроса");

            Questions.Add(new Question(text, section));
        }

        // Получение случайного вопроса из указанного раздела
        public Question GetRandomQuestion(string section, List<Question> usedQuestions)
        {
            var AvailableQuestions = new List<Question>();
            foreach (var question in Questions)
            {
                if (question.Section == section && !usedQuestions.Contains(question))
                {
                    AvailableQuestions.Add(question);
                }
            }

            if (AvailableQuestions.Count == 0)
            {
                return null;
            }
            Random rnd = new Random();
            int index = rnd.Next(AvailableQuestions.Count);

            return AvailableQuestions[index];
        }

        // Проверка наличия достаточного количества вопросов для генерации билетов
        public bool HasEnoughQuestions(int numTickets)
        {
            foreach (var section in Question.ALL_SECTIONS)
            {
                int count = Questions.Count(q => q.Section == section);
                if (count < numTickets)
                {
                    return false;
                }
            }
            return true;
        }

        public bool QuestionExists(string text, string section)
        {
            return Questions.Any(q =>
                q.Text.Equals(text, StringComparison.OrdinalIgnoreCase) &&
                q.Section.Equals(section, StringComparison.OrdinalIgnoreCase));
        }
    }
}
