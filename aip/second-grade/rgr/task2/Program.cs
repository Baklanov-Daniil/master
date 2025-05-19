using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;

namespace aip_rgr
{
    public class Task2
    {
        static int[,] GetData(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            string[] firstLine = lines[0].Split();
            int linesCount = int.Parse(firstLine[0]);
            int fieldsCount = int.Parse(firstLine[1]);
            int[,] data = new int[linesCount, fieldsCount];

            for (int line = 1; line < 1 + linesCount; line++)
            {
                string[] lineData = lines[line].Split(' ');
                for (int i = 0; i < fieldsCount; i++)
                {
                    if (i < lineData.Length && !string.IsNullOrEmpty(lineData[i]))
                    {
                        data[line - 1, i] = int.Parse(lineData[i]);
                    }
                    else
                    {
                        data[line - 1, i] = int.MaxValue;
                    }
                }
            }
            return data;
        }

        static int[,] GetPathData(int[,] data)
        {
            int linesCount = data.GetLength(0);
            int fieldsCount = data.GetLength(1);
            int[,] pathData = new int[data.GetLength(0), data.GetLength(1)];
            for (int field = 0; field < fieldsCount; field++)
            {
                pathData[0, field] = data[0, field];
            }
            for (int line = 1; line < linesCount; line++)
            {
                for (int field = 0; field < fieldsCount; field++)
                {
                    List<int> fieldsBefore = new List<int>();
                    fieldsBefore.Add(pathData[line - 1, field]);
                    if (field > 0)
                    {
                        fieldsBefore.Add(pathData[line - 1, field - 1]);
                    }
                    if (field < fieldsCount - 1)
                    {
                        fieldsBefore.Add(pathData[line - 1, field + 1]);
                    }
                    pathData[line, field] = data[line, field] + fieldsBefore.Min();
                }
            }
            return pathData;
        }

        static int[] FindMinPath(int[,] pathData)
        {
            int linesCount = pathData.GetLength(0);
            int fieldsCount = pathData.GetLength(1);
            int[] answer = new int[pathData.GetLength(0)];
            List<int> currentLine = new List<int>();
            for (int field = 0; field < fieldsCount; field++)
            {
                currentLine.Add(pathData[linesCount - 1, field]);
            }
            answer[linesCount - 1] = currentLine.FindIndex(x => x == currentLine.Min());
            for (int line = linesCount - 2; line >= 0; line--)
            {
                int current_field = answer[line + 1];
                int minValue = int.MaxValue;
                int bestField = current_field;
                for (int field = Math.Max(0, current_field - 1); field <= Math.Min(fieldsCount - 1, current_field + 1); field++)
                {
                    if (pathData[line, field] < minValue)
                    {
                        minValue = pathData[line, field];
                        bestField = field;
                    }
                }
                answer[line] = bestField;
            }
            return answer;
        }

        static bool RunTests(string inputDir, string correctDir, string myDir)
        {
            bool allCorrect = true;

            foreach (var inputFile in Directory.GetFiles(inputDir).OrderBy(f => f))
            {
                string fileName = Path.GetFileName(inputFile);
                string correctFile = Path.Combine(correctDir, fileName.Replace("input", "output"));
                string myFile = Path.Combine(myDir, fileName);
                try
                {
                    string[] myAnswer = ProcessTest(inputFile);
                    File.WriteAllLines(myFile, myAnswer);
                    string[] correctAnswer = File.ReadAllLines(correctFile);
                    if (!myAnswer.SequenceEqual(correctAnswer))
                    {
                        Console.WriteLine($"тест {fileName} не пройден");
                        allCorrect = false;
                    }
                    else
                    {
                        Console.WriteLine($"тест {fileName} пройден");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return allCorrect;
        }

        static string[] ProcessTest(string filePath)
        {
            int[,] data = GetData(filePath);
            int[,] pathData = GetPathData(data);
            int[] answer = FindMinPath(pathData);
            string[] stringAnswer = Array.ConvertAll(answer, x => (x+1).ToString());
            return stringAnswer;
        }

        static void Main()
        {
            bool allTestsPassed = RunTests("test/test input", "test/test output", "test/my answer");
            Console.WriteLine(allTestsPassed ? "Все тесты пройдены" : "Обнаружены несоответствия");
        }
    }
}



