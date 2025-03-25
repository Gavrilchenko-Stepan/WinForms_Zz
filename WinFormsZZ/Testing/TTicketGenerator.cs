using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLib;
using Microsoft.VisualBasic;
using System.Linq;

namespace Testing
{
    /// <summary>
    /// 1. Нужен тест-кейс, где бы было разное число вопросов в группах Знать, Уметь, Владеть
    /// 2. Нужен тест-кейс, где бы генерировалось НЕ 3 билета
    /// 3. Нужен тест-кейс, когда в принципе никаких вопросов
    /// 5. Нужен тест-кейс, где проверяются конкретные вопросы в билете (их фактические значения)
    /// 6. Нужна проверка, что вопросы в рамках сгенерированных билетов не повторяются
    /// </summary>
    [TestClass]
    public class TTicketGenerator
    {
        [TestMethod]
        public void TestGenerateTicketsWithRandomQuestions()
        {
            // Создаем тестовые данные
            QuestionManager questionManager = new QuestionManager();
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
            TicketGenerator generator = new TicketGenerator();

            // Генерация билетов
            var (tickets, expwarning) = generator.GenerateTickets(questionManager, 3);

            // Проверка результата
            Assert.IsNotNull(tickets, "Билеты были созданы.");
            Assert.AreEqual(3, tickets.Count, "Количество билетов соответствует ожидаемому.");

            // Проверка содержимого билетов
            foreach (Ticket ticket in tickets)
            {
                Assert.AreEqual(3, ticket.Questions.Count, "Билет содержит правильное количество вопросов.");
                Assert.AreEqual("знать", ticket.Questions[0].Section, "Верная последовательность секций в билете.");
                Assert.AreEqual("уметь", ticket.Questions[1].Section, "Верная последовательность секций в билете.");
                Assert.AreEqual("владеть", ticket.Questions[2].Section, "Верная последовательность секций в билете.");
            }
        }
        [TestMethod]
        public void TestRandomNumberQuestion() ///тест-кейс, где разное число вопросов в группах Знать, Уметь, Владеть
        {
            QuestionManager questionManager = new QuestionManager();
            questionManager.AddQuestion("Вопрос1", "знать");
            questionManager.AddQuestion("Вопрос2", "уметь");
            questionManager.AddQuestion("Вопрос3", "уметь");
            questionManager.AddQuestion("Вопрос4", "владеть");
            questionManager.AddQuestion("Вопрос5", "владеть");
            questionManager.AddQuestion("Вопрос6", "владеть");

            TicketGenerator generator = new TicketGenerator();

            var (tickets, expwarning) = generator.GenerateTickets(questionManager, 3);
             expwarning = "Ошибка! Не хватает вопросов!";
            

            Assert.IsNotNull(tickets, expwarning);
            Assert.AreEqual(3, tickets.Count, "Количество билетов соответствует ожидаемому.");

            foreach (Ticket ticket in tickets)
            {
                Assert.AreEqual(3, ticket.Questions.Count, "Билет содержит правильное количество вопросов.");
                Assert.AreEqual("знать", ticket.Questions[0].Section, "Верная последовательность секций в билете.");
                Assert.AreEqual("уметь", ticket.Questions[1].Section, "Верная последовательность секций в билете.");
                Assert.AreEqual("владеть", ticket.Questions[2].Section, "Верная последовательность секций в билете.");
            }
            ///Interaction.MsgBox("Недостаточно вопросов для генерации билета(-ов)!");
        }
        [TestMethod]
        public void TestGenerationZeroTicket()
        {
            // Создаем тестовые данные
            QuestionManager questionManager = new QuestionManager();
            questionManager.AddQuestion("Вопрос1", "знать");
            questionManager.AddQuestion("Вопрос2", "знать");
            questionManager.AddQuestion("Вопрос3", "знать");
            questionManager.AddQuestion("Вопрос4", "уметь");
            questionManager.AddQuestion("Вопрос5", "уметь");
            questionManager.AddQuestion("Вопрос6", "уметь");
            questionManager.AddQuestion("Вопрос7", "владеть");
            questionManager.AddQuestion("Вопрос8", "владеть");
            questionManager.AddQuestion("Вопрос9", "владеть");

            TicketGenerator generator = new TicketGenerator();

            // Пыпытка генерации 0 билетов
            var (tickets, expwarning) = generator.GenerateTickets(questionManager, 0);

            Assert.IsNotNull(tickets, "Список билетов был успешно создан.");
            Assert.AreEqual(0, tickets.Count, "Программа вернула пустой список билетов.");
        }

        [TestMethod]
        public void TestGenerateTicketsWithoutAnyQuestions()
        {
            QuestionManager questionManager = new QuestionManager();

            TicketGenerator generator = new TicketGenerator();

            var (tickets, expwarning) = generator.GenerateTickets(questionManager, 3);

            Assert.IsNotNull(tickets, "Список билетов не был успешно создан, т.к. не был задан список вопросов.");
        }


        [TestMethod]
        public void TestGenerateVariableNumberOfTickets() //тест , где бы генерировалось НЕ 3 билета
        {
            // Arrange
            var manager = new QuestionManager();
            manager.AddQuestion("Вопрос 1", "знать");
            manager.AddQuestion("Вопрос 2", "уметь");
            manager.AddQuestion("Вопрос 3", "владеть");
            manager.AddQuestion("Вопрос 4", "знать");
            manager.AddQuestion("Вопрос 5", "уметь");
            manager.AddQuestion("Вопрос 6", "владеть");

            var generator = new TicketGenerator();
            const int numberOfTickets = 2; // Генерируем 2 билетов

            // Act
            var (tickets, errorMessage) = generator.GenerateTickets(manager, numberOfTickets);

            // Assert
            Assert.IsNotNull(tickets);
            Assert.AreEqual(numberOfTickets, tickets.Count);
            Assert.AreEqual(3, tickets[0].Questions.Count);
            Assert.AreEqual(3, tickets[numberOfTickets - 1].Questions.Count);
        }
        [TestMethod] //тест где будет проверка, что вопросы в рамках сгенерированных билетов не повторяются
        public void TestUniqueQuestionsInEachTicket()
        {
            // Arrange
            var manager = new QuestionManager();
            manager.AddQuestion("Вопрос 1", "знать");
            manager.AddQuestion("Вопрос 2", "уметь");
            manager.AddQuestion("Вопрос 3", "владеть");
            manager.AddQuestion("Вопрос 4", "знать");
            manager.AddQuestion("Вопрос 5", "уметь");
            manager.AddQuestion("Вопрос 6", "владеть");

            var generator = new TicketGenerator();
            var (tickets, _) = generator.GenerateTickets(manager, 2);

            // Act & Assert
            foreach (var ticket in tickets)
            {
                // Проверим, что все вопросы в билете уникальны
                var uniqueQuestionsCount = ticket.Questions.Select(q => q.Text).Distinct().Count();
                Assert.AreEqual(uniqueQuestionsCount, ticket.Questions.Count,
                    $"Билет содержит дублирующиеся вопросы! Количество уникальных вопросов: {uniqueQuestionsCount}, общее количество вопросов: {ticket.Questions.Count}");
            }
        }
        [TestMethod]
        public void TestSpecificQuestionsInTicket() // тест где проверяются конкретные вопросы в билете (их фактические значения)
        {
            // Arrange
            var manager = new QuestionManager();
            var expectedKnowQuestion = new Question("a1", "знать");
            var expectedCanQuestion = new Question("a2", "уметь");
            var expectedMasterQuestion = new Question("a3", "владеть");

            manager.AddQuestion(expectedKnowQuestion.Text, expectedKnowQuestion.Section);
            manager.AddQuestion(expectedCanQuestion.Text, expectedCanQuestion.Section);
            manager.AddQuestion(expectedMasterQuestion.Text, expectedMasterQuestion.Section);

            var generator = new TicketGenerator();
            var (tickets, _) = generator.GenerateTickets(manager, 1);

            // Act
            var actualTicket = tickets[0];

            // Assert
            Assert.AreEqual(3, actualTicket.Questions.Count);
            Assert.AreEqual(expectedKnowQuestion.Text, actualTicket.Questions[0].Text);
            Assert.AreEqual(expectedKnowQuestion.Section, actualTicket.Questions[0].Section);
            Assert.AreEqual(expectedCanQuestion.Text, actualTicket.Questions[1].Text);
            Assert.AreEqual(expectedCanQuestion.Section, actualTicket.Questions[1].Section);
            Assert.AreEqual(expectedMasterQuestion.Text, actualTicket.Questions[2].Text);
            Assert.AreEqual(expectedMasterQuestion.Section, actualTicket.Questions[2].Section);
        }
    }
}
