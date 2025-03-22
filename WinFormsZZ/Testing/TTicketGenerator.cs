using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLib;
using Microsoft.VisualBasic;

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
            List<Ticket> tickets = generator.GenerateTickets(questionManager, 3);

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

            List<Ticket> tickets = generator.GenerateTickets(questionManager, 3);
            string expwarning = "Ошибка! Не хватает вопросов!";
            string 

            Assert.IsNotNull(tickets, warning);
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
            List<Ticket> tickets = generator.GenerateTickets(questionManager, 0);

            Assert.IsNotNull(tickets, "Список билетов был успешно создан.");
            Assert.AreEqual(0, tickets.Count, "Программа вернула пустой список билетов.");
        }

        [TestMethod]
        public void TestGenerateTicketsWithoutAnyQuestions()
        {
            QuestionManager questionManager = new QuestionManager();

            TicketGenerator generator = new TicketGenerator();

            List<Ticket> tickets = generator.GenerateTickets(questionManager, 3);

            Assert.IsNotNull(tickets, "Список билетов не был успешно создан, т.к. не был задан список вопросов.");
        }
    }
}
