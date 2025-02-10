using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _5_practic_work
{
    internal class Program
    {
        // задание 1
        static void Enter(int n, int[] arr)
        {
            for (int i = 0; i < n; i++) { arr[i] = Convert.ToInt32(Console.ReadLine()); }
        }

        static void Change(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++) { arr[i] = arr[i] * arr[i]; }
        }

        static void Print(int[] arr)
        {
            foreach (int i in arr) { Console.WriteLine($"{i} "); }
        }

        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            int[] arr = new int[n];
            Enter(n, arr);
            Change(arr);
            Print(arr);
            Console.ReadLine();



            //задание 2

        static void Main(string[] args)
        {
            double mn = 1000;
            int company = 0;
            double price = 0;

            int N = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < N; i++)
            {
                double x1 = Convert.ToDouble(Console.ReadLine());
                double y1 = Convert.ToDouble(Console.ReadLine());
                double z1 = Convert.ToDouble(Console.ReadLine());
                double x2 = Convert.ToDouble(Console.ReadLine());
                double y2 = Convert.ToDouble(Console.ReadLine());
                double z2 = Convert.ToDouble(Console.ReadLine());
                double c1 = Convert.ToDouble(Console.ReadLine());
                double c2 = Convert.ToDouble(Console.ReadLine());
                double S1 = 2 * (x1 * y1 + x1 * z1 + y1 * z1);
                double S2 = 2 * (x2 * y2 + x2 * z2 + y2 * z2);
                double V1 = x1 * y1 * z1;
                double V2 = x2 * y2 * z2;
                price = Math.Round((S1 * c2 - S2 * c1) / (V2 * S1 - S2 * V1) * 1000, 2);
                if (price < mn)
                {
                    mn = price;
                    company = i;
                }
                Console.WriteLine();
            }

            Console.WriteLine($"{company} {mn}");
            Console.ReadLine();
        }
    }
}

