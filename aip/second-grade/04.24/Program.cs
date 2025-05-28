// дан входной файл, в котором набор строк, в каждой строке могут быть различные символы, необходиом создать выходной файл, в который
// кладутся только те строки, где имеется хотябы 1 четное число - число считается, если стоит с другими символами (не цифрами)
// (число 123abc - 123, ab123 - 123, abc - none, ab2cd - 2, ab12 - 12)
using System;
using System.Collections.Generic;
using System.IO;

namespace aip 
{
    class Program
    {
        static bool FindNumber(string s)
        {
            int length = s.Length;
            int pointer = 0;
            while (pointer < length)
            {
                if (char.IsDigit(s[pointer]))
                {
                    int start = pointer;
                    while (pointer < length && char.IsDigit(s[pointer]))
                    {
                        pointer++;
                    }
                    string numStr = s.Substring(start, pointer - start);
                    if (int.TryParse(numStr, out int num) && num % 2 == 0)
                    {
                        return true;
                    }
                }
                else
                {
                    pointer++;
                }
            }
            return false;
        }
        static void Main(string[] args)
        {
            string inputPath = "data.txt";
            string outputPath = "answer.txt";
            string[] lines = File.ReadAllLines(inputPath);
            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                foreach (string line in lines)
                {
                    if (FindNumber(line))
                    {
                        writer.WriteLine(line);
                    }
                }
            }
        }
    }
}