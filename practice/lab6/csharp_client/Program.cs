using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace Lab6Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Лабораторная №6: Python Pipeline");

            Console.Write("Введите признак X1 (число): ");
            string x1 = Console.ReadLine();
            Console.Write("Введите признак X2 (число): ");
            string x2 = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(x1) || string.IsNullOrWhiteSpace(x2))
            {
                Console.WriteLine("Ошибка: Введите два числа.");
                return;
            }

            string currentDir = Directory.GetCurrentDirectory();
            string pipelineDir = Path.GetFullPath(Path.Combine(currentDir, "..", "ml_pipeline"));
            
            string pythonPath = Path.GetFullPath(Path.Combine(currentDir, "../..", "venv", "Scripts", "python.exe"));

            if (!File.Exists(pythonPath))
            {
                Console.WriteLine($"Не найден Python в venv. Проверьте путь: {pythonPath}");
                return;
            }
            if (!Directory.Exists(pipelineDir))
            {
                Console.WriteLine($"Не найдена папка ml_pipeline. Проверьте путь: {pipelineDir}");
                return;
            }

            try
            {
                Console.WriteLine($"\nЗапуск predict.py {x1} {x2}...");
                
                var processInfo = new ProcessStartInfo
                {
                    FileName = pythonPath,
                    Arguments = $"predict.py {x1} {x2}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = pipelineDir
                };

                using var process = new Process { StartInfo = processInfo };
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode == 0 && !string.IsNullOrEmpty(output))
                {
                    using var jsonDoc = JsonDocument.Parse(output);
                    var root = jsonDoc.RootElement;
                    
                    int predictedClass = root.GetProperty("class").GetInt32();
                    double probClass0 = root.GetProperty("probabilities")[0].GetDouble();
                    double probClass1 = root.GetProperty("probabilities")[1].GetDouble();

                    Console.WriteLine("\nУспешно!");
                    Console.WriteLine($"Предсказанный класс: {predictedClass}");
                    Console.WriteLine($"Вероятность класса 0: {probClass0:P2}");
                    Console.WriteLine($"Вероятность класса 1: {probClass1:P2}");
                }
                else
                {
                    Console.WriteLine($"\nОшибка выполнения скрипта (ExitCode: {process.ExitCode})");
                    if (!string.IsNullOrEmpty(error)) Console.WriteLine($"Python Error: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nИсключение: {ex.Message}");
            }
            
            Console.WriteLine("\nНажмите Enter для выхода...");
            Console.ReadLine();
        }
    }
}