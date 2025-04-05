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
            Questions.Add(new Question(text, section));
            File.AppendAllLines(_filename, new List<string> { $"{section} | {text}" });
            Console.WriteLine($"Вопрос '{text}' добавлен в раздел '{section}'.");
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
            var sections = new List<string> { "знать", "уметь", "владеть" };
            foreach (var section in sections)
            {
                int count = 0;
                foreach (var question in Questions)
                {
                    if (question.Section == section)
                    {
                        count++;
                    }
                }
                if (count < numTickets)
                {
                    return false;
                }

            }
            return true;
        }
    }
}
