using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib
{
    class QuestionManager
    {
        // Список всех вопросов
        public List<Question> Questions { get; set; } = new List<Question>();

        // Добавление нового вопроса
        public void AddQuestion(string text, string section)
        {
            Questions.Add(new Question(text, section));
        }

        // Получение случайного вопроса из указанного раздела
        public Question GetRandomQuestion(string section, List<Question> usedQuestions)
        {
            // Выбираем доступные вопросы из нужного раздела, исключая использованные
            var availableQuestions = Questions
                .Where(q => q.Section == section && !usedQuestions.Contains(q))
                .ToList();

            // Если доступных вопросов нет, возвращаем null
            if (availableQuestions.Count == 0)
            {
                return null;
            }

            // Выбираем случайный вопрос из доступных
            Random rnd = new Random();
            int index = rnd.Next(availableQuestions.Count);
            return availableQuestions[index];
        }

        // Проверка наличия достаточного количества вопросов для генерации билетов
        public bool HasEnoughQuestions(int numTickets)
        {
            // Проверяем наличие необходимого количества вопросов в каждом разделе
            var sections = new List<string> { "знать", "уметь", "владеть" };
            foreach (var sec in sections)
            {
                int count = Questions.Count(q => q.Section == sec);
                if (count < numTickets)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
