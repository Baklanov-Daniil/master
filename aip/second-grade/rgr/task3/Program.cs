using System;
using System.Collections.Generic;
using System.IO;

namespace aip_rgr
{
    class Program
    {
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
        
        static int[] GetData(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            
            string[] city1 = lines[0].Split();
            string[] city2 = lines[1].Split();
            int[] data = { int.Parse(city1[0]), int.Parse(city1[1]), int.Parse(city2[0]), int.Parse(city2[1]), int.Parse(lines[2]) }; 

            return data;
        }

        static string[] ProcessTest(string filePath)
        {
            int[] data = GetData(filePath);
            double lat1 = data[0] * Math.PI / 180;
            double lon1 = data[1] * Math.PI / 180;
            double lat2 = data[2] * Math.PI / 180;
            double lon2 = data[3] * Math.PI / 180;
            double R = data[4];

            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            double underRoot = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                                Math.Cos(lat1) * Math.Cos(lat2) *              
                                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double angle = 2 * Math.Asin(Math.Sqrt(underRoot));
            double distance = R * angle;
            string[] answer = { distance.ToString("F3").Replace(",", ".") };
            return answer;
        }

        static void Main(string[] args)
        {
            bool allTestsPassed = RunTests("test/test input", "test/test output", "test/my answer");
            Console.WriteLine(allTestsPassed ? "Все тесты пройдены" : "Обнаружены несоответствия");
        }
    }
}