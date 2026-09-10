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
            ;
        }

        static void Main(string[] args)
        {
            bool allTestsPassed = RunTests("test/test input", "test/test output", "test/my answer");
            Console.WriteLine(allTestsPassed ? "Все тесты пройдены" : "Обнаружены несоответствия");
        }
    }
}