using System;

namespace _5_practic_work
{
    internal class Program
    {
        static void Main(string[] args)
        {
            float mn_price = 1000;
            int company = 0;
            float price = 0;
            int company_count = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < company_count; i++)
            {
                string[] input_string = Console.ReadLine().Split();
                float x1 = float.Parse(input_string[0]);
                float y1 = float.Parse(input_string[1]);
                float z1 = float.Parse(input_string[2]);
                float x2 = float.Parse(input_string[3]);
                float y2 = float.Parse(input_string[4]);
                float z2 = float.Parse(input_string[5]);
                float c1 = float.Parse(input_string[6]);
                float c2 = float.Parse(input_string[7]);
                float S1 = 2 * (x1 * y1 + x1 * z1 + y1 * z1);
                float S2 = 2 * (x2 * y2 + x2 * z2 + y2 * z2);
                float V1 = x1 * y1 * z1;
                float V2 = x2 * y2 * z2;
                price = (S1 * c2 - S2 * c1) / (V2 * S1 - S2 * V1) * 1000;
                if (price < mn_price)
                {
                    mn_price = price;
                    company = i + 1;
                }
            }
            Console.WriteLine($"{company} {String.Format("{0:0.00}", Math.Round(mn_price, 2))}");
            Console.ReadLine();
        }
    }
}

//float c1 = float.Parse(input_string[6].Replace(".", ","));
//float c2 = float.Parse(input_string[7].Replace(".", ","));
