using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLib;
using Microsoft.VisualBasic;
using System.Linq;

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
            TicketGenerator generator = new TicketGenerator(questionManager);

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

            TicketGenerator generator = new TicketGenerator(questionManager);

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

            TicketGenerator generator = new TicketGenerator(questionManager);

            // Пыпытка генерации 0 билетов
            var (tickets, expwarning) = generator.GenerateTickets(questionManager, 0);

            Assert.IsNotNull(tickets, "Список билетов был успешно создан.");
            Assert.AreEqual(0, tickets.Count, "Программа вернула пустой список билетов.");
        }

        [TestMethod]
        public void TestGenerateTicketsWithoutAnyQuestions()
        {
            QuestionManager questionManager = new QuestionManager();

            TicketGenerator generator = new TicketGenerator(questionManager);

            (List<Ticket> tickets, string expwarning) = generator.GenerateTickets(questionManager, 3);

            Assert.IsNotNull(tickets, "Список билетов не был успешно создан, т.к. не был задан список вопросов.");
        }



        //// MStest где была бы генерация не 3 билетов и проверрка этого, также проверка 
        ///на то что вопросы в рамках сгенерированых билетов не повторяются
        [TestMethod]
        public void TestGenerateVariableNumberOfTickets()
        {

            QuestionManager manager = new QuestionManager();
            manager.AddQuestion("Вопрос 1", "знать");
            manager.AddQuestion("Вопрос 2", "уметь");
            manager.AddQuestion("Вопрос 3", "владеть");
            manager.AddQuestion("Вопрос 4", "знать");
            manager.AddQuestion("Вопрос 5", "уметь");
            manager.AddQuestion("Вопрос 6", "владеть");


            const int expectedNumberOfTickets = 2;
            var generator = new TicketGenerator(manager);



            (List<Ticket> ActTickets, string errorMessage) = generator.GenerateTickets(manager, expectedNumberOfTickets);
            (List<Ticket> tickets, _) = generator.GenerateTickets(manager, 2);

            Assert.IsNotNull(ActTickets);
            Assert.AreEqual(expectedNumberOfTickets, ActTickets.Count);
            Assert.AreEqual(3, ActTickets[0].Questions.Count);
            Assert.AreEqual(3, ActTickets[expectedNumberOfTickets - 1].Questions.Count);

            foreach (var ticket in tickets)
            {

                int uniqueQuestionsCount = ticket.Questions.Select(q => q.Text).Distinct().Count();
                Assert.AreEqual(uniqueQuestionsCount, ticket.Questions.Count,
                    $"Билет содержит дублирующиеся вопросы! Количество уникальных вопросов:" +
                    $" {uniqueQuestionsCount}, общее количество вопросов: {ticket.Questions.Count}");
            }

        }

        [TestMethod]
        public void TestSpecificQuestionsInTicket() // тест где проверяются конкретные вопросы в билете (их фактические значения)
        {

            QuestionManager manager = new QuestionManager();
            Question expectedKnowQuestion = new Question("a1", "знать");
            Question expectedCanQuestion = new Question("a2", "уметь");
            Question expectedMasterQuestion = new Question("a3", "владеть");

            manager.AddQuestion(expectedKnowQuestion.Text, expectedKnowQuestion.Section);
            manager.AddQuestion(expectedCanQuestion.Text, expectedCanQuestion.Section);
            manager.AddQuestion(expectedMasterQuestion.Text, expectedMasterQuestion.Section);

            TicketGenerator generator = new TicketGenerator(manager);
            (List<Ticket> tickets, _) = generator.GenerateTickets(manager, 1);


            Ticket actualTicket = tickets[0];


            Assert.AreEqual(3, actualTicket.Questions.Count);
            Assert.AreEqual(expectedKnowQuestion.Text, actualTicket.Questions[0].Text);
            Assert.AreEqual(expectedKnowQuestion.Section, actualTicket.Questions[0].Section);
            Assert.AreEqual(expectedCanQuestion.Text, actualTicket.Questions[1].Text);
            Assert.AreEqual(expectedCanQuestion.Section, actualTicket.Questions[1].Section);
            Assert.AreEqual(expectedMasterQuestion.Text, actualTicket.Questions[2].Text);
            Assert.AreEqual(expectedMasterQuestion.Section, actualTicket.Questions[2].Section);
        }
        [TestMethod]
        public void Test_AllTicketsHaveUniqueQuestions() // проверка на то что вопросы не повторяются вообще во всех билетах
        {

            var manager = new QuestionManager();

            manager.AddQuestion("Вопрос 1", "знать");
            manager.AddQuestion("Вопрос 2", "уметь");
            manager.AddQuestion("Вопрос 3", "владеть");
            manager.AddQuestion("Вопрос 4", "знать");
            manager.AddQuestion("Вопрос 5", "уметь");
            manager.AddQuestion("Вопрос 6", "владеть");

            var generator = new TicketGenerator(manager);

            int NumTickets = 2;

            (List<Ticket> tickets, string expwarning) = generator.GenerateTickets(manager, NumTickets);

            // Assert.IsNull(expwarning, "ошибка при генерации билетов" + expwarning);
            Assert.IsNotNull(tickets, "Список билетовн не должен быть null");



            // var result = generator.GenerateTickets(manager, 2); 
            //var allTickets = result.Item1;

            var AllQuestions = tickets.SelectMany(ticket => ticket.Questions).ToList();
            var uniqueuestions = AllQuestions.Distinct().ToList();

            Assert.AreEqual(uniqueuestions.Count, AllQuestions.Count, "Некоторые вопросы повторяются в билетах");



        }
    }

}

