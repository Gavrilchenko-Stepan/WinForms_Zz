using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualBasic;


namespace MyLib
{
    public class LoadQuestionsFromFile
    {
        static public void TheQuestionLoader(QuestionManager questionmanager)
        {
            questionmanager.Questions.Clear(); // Очистка списка перед загрузкой

            if (!File.Exists(questionmanager._filename))
            {
                Console.WriteLine($"Ошибка: файл {questionmanager._filename} не найден.");
                return;
            }

            string[] lines;
            try
            {
                lines = File.ReadAllLines(questionmanager._filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
                return;
            }

            int errorCount = 0;
            List<string> errorLines = new List<string>();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    errorCount++;
                    errorLines.Add($"Пустая строка");
                    continue;
                }

                var parts = line.Split('|');
                if (parts.Length == 2)
                {
                    var section = parts[0].Trim();
                    var text = parts[1].Trim();

                    if (string.IsNullOrEmpty(section)
                        || string.IsNullOrEmpty(text))
                    {
                        errorCount++;
                        errorLines.Add($"Пустое значение в строке: {line}");
                        continue;
                    }

                    questionmanager.Questions.Add(new Question(text, section));
                }
                else
                {
                    errorCount++;
                    errorLines.Add($"Ошибка формата (должно быть 2 части разделенные '|'): {line}");
                }
            }

            Console.WriteLine($"Загружено {questionmanager.Questions.Count} из {lines.Length} вопросов.");

            // Вывод информации об ошибках, если они есть
            if (errorCount > 0)
            {
                Console.WriteLine($"\nНайдено {errorCount} ошибок в файле:");
                foreach (var error in errorLines)
                {
                    Console.WriteLine($"- {error}");
                }
            }
        }

        static public bool CheckFileForErrors(string filePath, out List<string> errors)
        {
            errors = new List<string>();


            if (!File.Exists(filePath))
            {
                errors.Add($"Файл {filePath} не найден.");
                return false;
            }

            string[] lines;
            try
            {
                lines = File.ReadAllLines(filePath);
            }
            catch (Exception ex)
            {
                errors.Add($"Ошибка при чтении файла: {ex.Message}");
                return false;
            }

            bool hasErrors = false;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    errors.Add($"Строка {i + 1}: Пустая строка");
                    hasErrors = true;
                    continue;
                }

                var parts = line.Split('|');
                if (parts.Length != 2)
                {
                    errors.Add($"Строка {i + 1}: Ошибка формата (должно быть 2 части разделенные '|')");
                    hasErrors = true;
                }
                else if (string.IsNullOrEmpty(parts[0].Trim())
                      || string.IsNullOrEmpty(parts[1].Trim()))
                {
                    errors.Add($"Строка {i + 1}: Пустое значение секции или текста вопроса");
                    hasErrors = true;
                }
            }

            return !hasErrors;
        }
    }
}
