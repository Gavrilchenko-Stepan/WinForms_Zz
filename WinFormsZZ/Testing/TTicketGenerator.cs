using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLib;
using Microsoft.VisualBasic;
using System.Linq;
using System.IO;

namespace Testing
{
    /// <summary>
    /// Тесты которые следует добавить:
    /// 



    /// </summary>
    [TestClass]
    public class TTicketGenerator
    {
        [TestMethod]
        public void TestGenerateTicketsWithRandomQuestions()
        {
            // Создаем временный файл для теста
            string tempFile = Path.GetTempFileName();
            QuestionManager questionManager = new QuestionManager(tempFile);

            try
            {
                // Добавляем тестовые вопросы (по 3 в каждой секции)
                questionManager.AddQuestion("Вопрос1", "знать");
                questionManager.AddQuestion("Вопрос2", "знать");
                questionManager.AddQuestion("Вопрос3", "знать");
                questionManager.AddQuestion("Вопрос4", "уметь");
                questionManager.AddQuestion("Вопрос5", "уметь");
                questionManager.AddQuestion("Вопрос6", "уметь");
                questionManager.AddQuestion("Вопрос7", "владеть");
                questionManager.AddQuestion("Вопрос8", "владеть");
                questionManager.AddQuestion("Вопрос9", "владеть");

                // Создаем генератор билетов
                TicketGenerator generator = new TicketGenerator(questionManager);

                // Генерация билетов
                var (tickets, errorMessage) = generator.GenerateTickets(questionManager, 3);

                // Проверка базовых условий
                Assert.IsNotNull(tickets, "Билеты должны быть созданы");
                Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Не должно быть сообщения об ошибке");
                Assert.AreEqual(3, tickets.Count, "Количество билетов должно соответствовать запрошенному");

                // Собираем все использованные вопросы для проверки уникальности
                HashSet<Question> usedQuestions = new HashSet<Question>();

                foreach (Ticket ticket in tickets)
                {
                    // Проверка структуры билета
                    Assert.AreEqual(3, ticket.Questions.Count, "Каждый билет должен содержать 3 вопроса");

                    // Проверка порядка секций
                    Assert.AreEqual("знать", ticket.Questions[0].Section, "Первый вопрос должен быть из секции 'знать'");
                    Assert.AreEqual("уметь", ticket.Questions[1].Section, "Второй вопрос должен быть из секции 'уметь'");
                    Assert.AreEqual("владеть", ticket.Questions[2].Section, "Третий вопрос должен быть из секции 'владеть'");

                    // Проверка уникальности вопросов в билетах
                    foreach (var question in ticket.Questions)
                    {
                        Assert.IsFalse(usedQuestions.Contains(question), "Вопросы не должны повторяться в разных билетах");
                        usedQuestions.Add(question);
                    }
                }

                // Дополнительная проверка - все вопросы должны быть из исходного списка
                foreach (var question in usedQuestions)
                {
                    Assert.IsTrue(questionManager.Questions.Contains(question),
                        "Все использованные вопросы должны быть из исходного списка");
                }
            }
            finally
            {
                // Удаляем временный файл после теста
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        [TestMethod]
        public void TestRandomNumberQuestion() ///тест-кейс, где разное число вопросов в группах Знать, Уметь, Владеть
        {
            // Создаем временный файл для теста
            string tempFile = Path.GetTempFileName();
            QuestionManager questionManager = new QuestionManager(tempFile);

            try
            {
                // Добавляем вопросы (разное количество в каждой секции)
                questionManager.AddQuestion("Вопрос1", "знать");  // 1 вопрос в "знать"
                questionManager.AddQuestion("Вопрос2", "уметь");  // 2 вопроса в "уметь"
                questionManager.AddQuestion("Вопрос3", "уметь");
                questionManager.AddQuestion("Вопрос4", "владеть"); // 3 вопроса в "владеть"
                questionManager.AddQuestion("Вопрос5", "владеть");
                questionManager.AddQuestion("Вопрос6", "владеть");

                TicketGenerator generator = new TicketGenerator(questionManager);

                // Пытаемся сгенерировать 3 билета (это должно провалиться, так как в "знать" только 1 вопрос)
                var (tickets, errorMessage) = generator.GenerateTickets(questionManager, 3);

                // Проверяем, что генерация билетов не удалась (tickets == null)
                Assert.IsNull(tickets, "Ожидалось, что генерация билетов не удастся из-за недостатка вопросов");

                // Проверяем сообщение об ошибке
                Assert.AreEqual("Ошибка: недостаточно вопросов для генерации билетов!", errorMessage,
                    "Неверное сообщение об ошибке");

                // Теперь попробуем сгенерировать 1 билет (это должно сработать)
                var (singleTicket, singleError) = generator.GenerateTickets(questionManager, 1);

                Assert.IsNotNull(singleTicket, "Ожидалось, что 1 билет будет сгенерирован");
                Assert.AreEqual(1, singleTicket.Count, "Ожидался ровно 1 билет");
                Assert.AreEqual(3, singleTicket[0].Questions.Count, "Билет должен содержать 3 вопроса");
            }
            finally
            {
                // Удаляем временный файл после теста
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }
        [TestMethod]
        public void TestGenerationZeroTicket()
        {
            // Создаем временный файл для теста
            string tempFile = Path.GetTempFileName();
            QuestionManager questionManager = new QuestionManager(tempFile);

            try
            {
                // Добавляем тестовые вопросы (по 3 в каждой секции)
                questionManager.AddQuestion("Вопрос1", "знать");
                questionManager.AddQuestion("Вопрос2", "знать");
                questionManager.AddQuestion("Вопрос3", "знать");
                questionManager.AddQuestion("Вопрос4", "уметь");
                questionManager.AddQuestion("Вопрос5", "уметь");
                questionManager.AddQuestion("Вопрос6", "уметь");
                questionManager.AddQuestion("Вопрос7", "владеть");
                questionManager.AddQuestion("Вопрос8", "владеть");
                questionManager.AddQuestion("Вопрос9", "владеть");

                TicketGenerator generator = new TicketGenerator(questionManager);

                // Попытка генерации 0 билетов
                var (tickets, errorMessage) = generator.GenerateTickets(questionManager, 0);

                // Проверяем результаты
                Assert.IsNotNull(tickets, "Список билетов должен быть создан (пустой)");
                Assert.AreEqual(0, tickets.Count, "Должен вернуться пустой список билетов");
                Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Не должно быть сообщения об ошибке");

                // Дополнительная проверка - убедимся, что можно сгенерировать билеты после этого
                var (realTickets, realError) = generator.GenerateTickets(questionManager, 1);
                Assert.IsNotNull(realTickets, "Последующая генерация билетов должна работать");
                Assert.AreEqual(1, realTickets.Count, "Должен сгенерироваться 1 билет");
            }
            finally
            {
                // Удаляем временный файл после теста
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        [TestMethod]
        public void TestGenerateTicketsWithoutAnyQuestions()
        {
            // Создаем временный файл (пустой)
            string tempFile = Path.GetTempFileName();
            QuestionManager questionManager = new QuestionManager(tempFile);

            try
            {
                TicketGenerator generator = new TicketGenerator(questionManager);

                // Попытка генерации 3 билетов без вопросов
                var (tickets, errorMessage) = generator.GenerateTickets(questionManager, 3);

                // Проверяем результаты
                Assert.IsNull(tickets, "При отсутствии вопросов не должно быть сгенерировано билетов");
                Assert.AreEqual("Ошибка: недостаточно вопросов для генерации билетов!", errorMessage,
                    "Должно быть сообщение об ошибке из-за отсутствия вопросов");

                // Дополнительная проверка - что список вопросов действительно пуст
                Assert.AreEqual(0, questionManager.Questions.Count, "Список вопросов должен быть пустым");
            }
            finally
            {
                // Удаляем временный файл после теста
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }



        //// MStest где была бы генерация не 3 билетов и проверрка этого, также проверка 
        ///на то что вопросы в рамках сгенерированых билетов не повторяются
        [TestMethod]
        public void TestGenerateVariableNumberOfTickets()
        {
            // Создаем временный файл для изоляции теста
            string tempFile = Path.GetTempFileName();

            try
            {
                // Инициализация менеджера вопросов
                var manager = new QuestionManager(tempFile);

                // Добавляем тестовые вопросы (по 2 в каждой секции)
                manager.AddQuestion("Вопрос 1", "знать");
                manager.AddQuestion("Вопрос 2", "уметь");
                manager.AddQuestion("Вопрос 3", "владеть");
                manager.AddQuestion("Вопрос 4", "знать");
                manager.AddQuestion("Вопрос 5", "уметь");
                manager.AddQuestion("Вопрос 6", "владеть");

                var generator = new TicketGenerator(manager);

                // Тестируем генерацию разного количества билетов
                TestTicketGenerationCase(manager, generator, 1, shouldSucceed: true);
                TestTicketGenerationCase(manager, generator, 2, shouldSucceed: true);
                TestTicketGenerationCase(manager, generator, 3, shouldSucceed: false); // Должно быть больше вопросов
            }
            finally
            {
                // Удаляем временный файл после теста
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        private void TestTicketGenerationCase(QuestionManager manager, TicketGenerator generator,
                                            int numTickets, bool shouldSucceed)
        {
            // Генерация билетов
            var (tickets, errorMessage) = generator.GenerateTickets(manager, numTickets);

            if (shouldSucceed)
            {
                // Проверка успешной генерации
                Assert.IsNotNull(tickets, $"Для {numTickets} билетов: список билетов не должен быть null");
                Assert.IsTrue(string.IsNullOrEmpty(errorMessage),
                    $"Для {numTickets} билетов: не должно быть ошибки ({errorMessage})");
                Assert.AreEqual(numTickets, tickets.Count,
                    $"Для {numTickets} билетов: количество не соответствует");

                // Проверка структуры билетов
                foreach (var ticket in tickets)
                {
                    Assert.AreEqual(3, ticket.Questions.Count,
                        "Каждый билет должен содержать 3 вопроса");

                    // Проверка порядка секций
                    Assert.AreEqual("знать", ticket.Questions[0].Section);
                    Assert.AreEqual("уметь", ticket.Questions[1].Section);
                    Assert.AreEqual("владеть", ticket.Questions[2].Section);
                }

                // Проверка уникальности вопросов между всеми билетами
                var allQuestions = tickets.SelectMany(t => t.Questions).ToList();
                var uniqueQuestions = allQuestions.Distinct().ToList();
                Assert.AreEqual(allQuestions.Count, uniqueQuestions.Count,
                    $"Для {numTickets} билетов: обнаружены повторяющиеся вопросы между билетами");
            }
            else
            {
                // Проверка неудачной генерации
                Assert.IsNull(tickets, $"Для {numTickets} билетов: список должен быть null");
                Assert.IsFalse(string.IsNullOrEmpty(errorMessage),
                    $"Для {numTickets} билетов: должно быть сообщение об ошибке");
            }
        }

        [TestMethod]
        public void TestSpecificQuestionsInTicket()
        {
            // Создаем временный файл для изоляции теста
            string tempFile = Path.GetTempFileName();
            QuestionManager manager = new QuestionManager(tempFile);

            try
            {
                // Создаем ожидаемые вопросы
                Question expectedKnowQuestion = new Question("a1", "знать");
                Question expectedCanQuestion = new Question("a2", "уметь");
                Question expectedMasterQuestion = new Question("a3", "владеть");

                // Добавляем только эти 3 вопроса (по одному в каждой секции)
                manager.AddQuestion(expectedKnowQuestion.Text, expectedKnowQuestion.Section);
                manager.AddQuestion(expectedCanQuestion.Text, expectedCanQuestion.Section);
                manager.AddQuestion(expectedMasterQuestion.Text, expectedMasterQuestion.Section);

                // Проверяем, что вопросы добавились корректно
                Assert.AreEqual(3, manager.Questions.Count, "Должно быть ровно 3 вопроса в менеджере");

                TicketGenerator generator = new TicketGenerator(manager);
                var (tickets, errorMessage) = generator.GenerateTickets(manager, 1);

                // Проверяем успешность генерации
                Assert.IsNotNull(tickets, "Билеты должны быть созданы");
                Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Не должно быть ошибки генерации");
                Assert.AreEqual(1, tickets.Count, "Должен быть создан ровно 1 билет");

                Ticket actualTicket = tickets[0];

                // Проверяем структуру билета
                Assert.AreEqual(3, actualTicket.Questions.Count, "Билет должен содержать 3 вопроса");

                // Проверяем конкретные вопросы и их порядок
                Assert.AreEqual(expectedKnowQuestion.Text, actualTicket.Questions[0].Text,
                    "Текст первого вопроса не совпадает с ожидаемым");
                Assert.AreEqual(expectedKnowQuestion.Section, actualTicket.Questions[0].Section,
                    "Секция первого вопроса не совпадает с ожидаемой");

                Assert.AreEqual(expectedCanQuestion.Text, actualTicket.Questions[1].Text,
                    "Текст второго вопроса не совпадает с ожидаемым");
                Assert.AreEqual(expectedCanQuestion.Section, actualTicket.Questions[1].Section,
                    "Секция второго вопроса не совпадает с ожидаемой");

                Assert.AreEqual(expectedMasterQuestion.Text, actualTicket.Questions[2].Text,
                    "Текст третьего вопроса не совпадает с ожидаемым");
                Assert.AreEqual(expectedMasterQuestion.Section, actualTicket.Questions[2].Section,
                    "Секция третьего вопроса не совпадает с ожидаемой");

                // Дополнительная проверка - убедимся, что это те же объекты вопросов
                Assert.AreSame(manager.Questions[0], actualTicket.Questions[0],
                    "Первый вопрос должен быть тем же объектом");
                Assert.AreSame(manager.Questions[1], actualTicket.Questions[1],
                    "Второй вопрос должен быть тем же объектом");
                Assert.AreSame(manager.Questions[2], actualTicket.Questions[2],
                    "Третий вопрос должен быть тем же объектом");
            }
            finally
            {
                // Удаляем временный файл после теста
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }
        [TestMethod]
        public void Test_AllTicketsHaveUniqueQuestions()
        {
            // Создаем временный файл для изоляции теста
            string tempFile = Path.GetTempFileName();
            var manager = new QuestionManager(tempFile);

            try
            {
                // Добавляем тестовые вопросы (по 2 в каждой секции)
                manager.AddQuestion("Вопрос 1", "знать");
                manager.AddQuestion("Вопрос 2", "уметь");
                manager.AddQuestion("Вопрос 3", "владеть");
                manager.AddQuestion("Вопрос 4", "знать");
                manager.AddQuestion("Вопрос 5", "уметь");
                manager.AddQuestion("Вопрос 6", "владеть");

                var generator = new TicketGenerator(manager);
                int numTickets = 2;

                // Генерация билетов
                var (tickets, errorMessage) = generator.GenerateTickets(manager, numTickets);

                // Проверка успешности генерации
                Assert.IsNotNull(tickets, "Список билетов не должен быть null");
                Assert.IsTrue(string.IsNullOrEmpty(errorMessage),
                    $"Ошибка при генерации билетов: {errorMessage}");
                Assert.AreEqual(numTickets, tickets.Count,
                    $"Должно быть сгенерировано {numTickets} билета");

                // Собираем все вопросы из всех билетов
                var allQuestions = tickets.SelectMany(ticket => ticket.Questions).ToList();

                // Проверяем что количество уникальных вопросов равно общему количеству
                var uniqueQuestions = allQuestions.Distinct().ToList();
                Assert.AreEqual(uniqueQuestions.Count, allQuestions.Count,
                    "Обнаружены повторяющиеся вопросы в разных билетах");

                // Дополнительная проверка - что все вопросы взяты из исходного списка
                var allQuestionIds = allQuestions.Select(q => q.Text).ToList();
                var managerQuestionIds = manager.Questions.Select(q => q.Text).ToList();

                foreach (var questionId in allQuestionIds)
                {
                    Assert.IsTrue(managerQuestionIds.Contains(questionId),
                        $"Вопрос '{questionId}' не найден в исходном списке вопросов");
                }

                // Проверка что каждый билет содержит уникальные вопросы
                foreach (var ticket in tickets)
                {
                    var ticketQuestions = ticket.Questions.Select(q => q.Text).ToList();
                    var uniqueTicketQuestions = ticketQuestions.Distinct().ToList();

                    Assert.AreEqual(uniqueTicketQuestions.Count, ticketQuestions.Count,
                        "Билет содержит повторяющиеся вопросы");
                }
            }
            finally
            {
                // Удаляем временный файл после теста
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }
    }

}

