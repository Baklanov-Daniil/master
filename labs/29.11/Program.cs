using System;
using System.Linq;
using System.Reflection;

namespace _29._11
{
    internal class Program
    {
        static void Task1(string input_string)
        {
            int mxlen = 1;
            int tlen = 1;
            bool Flag = false;
            for (int i = 0; i < input_string.Length - 1; i++)
            {
                if ((Flag != true) && (input_string[i] == 'X')) { Flag = true; }
                if (Flag == true)
                {
                    if ((input_string[i] == 'X') && (input_string[i + 1] == 'Y')) { tlen += 1; }
                    else if ((input_string[i] == 'Y') && (input_string[i + 1] == 'Z')) { tlen += 1; }
                    else if ((input_string[i] == 'Z') && (input_string[i + 1] == 'X')) { tlen += 1; }
                    else { mxlen = Math.Max(tlen, mxlen); tlen = 1; Flag = false; }
                }
            }
            if (tlen != 1) { mxlen = Math.Max(mxlen, tlen); }
            if (!input_string.Contains("X")) { mxlen = 0; }
            Console.WriteLine(mxlen);
        }

        static void Task2(string input_string)
        {
            int mn = 0;
            input_string = input_string.ToLower();
            string symbols = "";
            for (int i = 1; i < input_string.Length - 1; i++)
            {
                if ((input_string[i - 1] == 'a') && (input_string[i + 1] == 'b') && ((symbols.Contains(input_string[i])) == false)) { symbols += input_string[i]; }
            }
            for (int i = 0; i < symbols.Length; i++)
            {
                int tcount = 0;
                for (int j = 1; j < input_string.Length - 1; j++)
                {
                    if ((input_string[j - 1] == 'a') && (input_string[j] == symbols[i]) && (input_string[j + 1] == 'b')) { tcount += 1; }
                }
                if (mn == 0) { mn = tcount; }
                else { mn = Math.Min(mn, tcount); }
            }
            Console.WriteLine(mn);
        }

        static void Main(string[] args)
        {
            string s = Console.ReadLine();
            //Task1(s);
            Task2(s);
        }
    }
}
