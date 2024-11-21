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
            static void Main(string[] args)
            {
                float mn = 1000;
                int company = 0;
                float price = 0;

                int N = Convert.ToInt32(Console.ReadLine());
                for (int i = 0; i < N; i++)
                {
                    float x1 = float.Parse(Console.ReadLine());
                    float y1 = float.Parse(Console.ReadLine());
                    float z1 = float.Parse(Console.ReadLine());
                    float x2 = float.Parse(Console.ReadLine());
                    float y2 = float.Parse(Console.ReadLine());
                    float z2 = float.Parse(Console.ReadLine());
                    float c1 = float.Parse(Console.ReadLine());
                    float c2 = float.Parse(Console.ReadLine());
                    float S1 = 2 * (x1 * y1 + x1 * z1 + y1 * z1);
                    float S2 = 2 * (x2 * y2 + x2 * z2 + y2 * z2);
                    float V1 = x1 * y1 * z1;
                    float V2 = x2 * y2 * z2;
                    price = (S1 * c2 - S2 * c1) / (V2 * S1 - S2 * V1) * 1000;
                    if (price < mn)
                    {
                        mn = price;
                        company = i + 1;
                    }
                    Console.WriteLine();
                }

                Console.WriteLine($"{company} {Math.Round(mn, 2)}");
                Console.ReadLine();
            }
        }
    }


