using System;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                int result = 0;
                bool flag = true;
                int n = Convert.ToInt32(Console.ReadLine());
                if (n < 0)
                {
                    Console.WriteLine("Число меньше 0");
                    continue;
                }
                if (n == 0) { break; }
                while (n > 0)
                {
                    int c = n % 10;
                    n /= 10;
                    if (c % 2 == 1)
                    {
                        continue;
                    }
                    else
                    {
                        flag = false;
                        result = result * 10 + c;
                    }
                }
                if (flag == true) { Console.WriteLine("Нет четных цифр"); }
                else { Console.WriteLine(result); }
            }
