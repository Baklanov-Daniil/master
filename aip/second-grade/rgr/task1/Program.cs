using System;
using System.Collections.Generic;

namespace aip_rgr
{
    class Program
    {
        static int[,] GetData(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            string[] firstLine = lines[0].Split();
            int vertexCount = int.Parse(firstLine[0]);
            int edgeCount = int.Parse(firstLine[1]);
            int[,] data = new int[vertexCount, vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                for (int j = 0; j < vertexCount; j++)
                {
                    data[i, j] = (i == j) ? 0 : int.MaxValue;
                }
            }
            for (int line = 1; line <= edgeCount; line++)
            {
                string[] parts = lines[line].Split();
                int vertex1 = int.Parse(parts[0]) - 1;
                int vertex2 = int.Parse(parts[1]) - 1;
                int weight = int.Parse(parts[2]);
                if (weight < data[vertex1, vertex2])
                {
                    data[vertex1, vertex2] = weight;
                    data[vertex2, vertex1] = weight;
                }
            }
            return data;
        }

        static int[,] AlgorithmFloida(int[,] data)
        {
            int n = data.GetLength(0);
            for (int baseVertex = 0; baseVertex < n; baseVertex++)
            {
                for (int firstVertex = 0; firstVertex < n; firstVertex++)
                {
                    for (int secondVertex = 0; secondVertex < n; secondVertex++)
                    {
                        if (data[firstVertex, baseVertex] != int.MaxValue && data[baseVertex, secondVertex] != int.MaxValue)
                        {
                            int newDistance = data[firstVertex, baseVertex] + data[baseVertex, secondVertex];
                            if (data[firstVertex, secondVertex] > newDistance)
                            {
                                data[firstVertex, secondVertex] = newDistance;
                            }
                        }
                    }
                }
            }
            return data;
        }

        static int get_max(int[,] data)
        {
            int mx = 0;
            for (int firstVertex=0; firstVertex < data.GetLength(0); firstVertex++)
            {
                for (int secondVertex=0; secondVertex < data.GetLength(0); secondVertex++)
                {                    
                    if (data[firstVertex,secondVertex]>mx) mx = data[firstVertex, secondVertex];
                }
            }
            return mx;
        }

        static bool RunTests(string inputDir, string correctDir, string myDir)
        {
            bool allCorrect = true;

            foreach (var inputFile in Directory.GetFiles(inputDir).OrderBy(f => f))
            {
                string fileName = Path.GetFileName(inputFile);
                string correctFile = Path.Combine(correctDir, fileName.Replace("input", "output"));
                string myFile = Path.Combine(myDir, fileName);
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
            return allCorrect;
        }

        static string[] ProcessTest(string path)
        {
            int[,] data = GetData(path);
            data = AlgorithmFloida(data);
            int mx = get_max(data);
            string[] answer = { Convert.ToString(mx) };
            return answer;
        }

        static void Main(string[] args)
        {
            bool allTestsPassed = RunTests("test/test input", "test/test output", "test/my answer");
            Console.WriteLine(allTestsPassed ? "Все тесты пройдены" : "Обнаружены несоответствия");
        }
    }
}