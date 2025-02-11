using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace study
{

    internal class Program
    {
        static void Main(string[] args)
        {

            //задание 1

            int a, b;
            a = Convert.ToInt32(Console.ReadLine());
            b = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"до: {a} {b}");
            a = a + b;
            b = a - b;
            a = a - b;
            Console.WriteLine($"после: {a} {b}");
            Console.ReadLine();

            //задание 2

            int a, b, max, min;
            a = Convert.ToInt32(Console.ReadLine());
            b = Convert.ToInt32(Console.ReadLine());
            min = ((a + b) - Math.Abs(a - b)) / 2;
            max = ((a + b) + Math.Abs(a - b)) / 2;
            Console.WriteLine($"max: {max}");
            Console.ReadLine();

            //задание 3

            int n = Convert.ToInt32(Console.ReadLine());
            int l = 3;
            int m = 3;
            int p = 5;
            int path;
            path = 2 * n * p + (l + m) * n * 2 + l * n * (n - 1);
            Console.WriteLine(path);
            Console.ReadLine();
        }
    }
}