using System;

namespace Program
{

    class Program
    {
        static void Main(string[] args)
        {
            //задание 1

            int cnt = 0;
            int n = Convert.ToInt32(Console.ReadLine()) - 2;
            int prev = Convert.ToInt32(Console.ReadLine());
            int mid = Convert.ToInt32(Console.ReadLine());
            int next;
            for (int i = 0; i < n; i++)
            {
                next = Convert.ToInt32(Console.ReadLine());
                if (mid < prev && mid < next)
                {
                    cnt += 1;
                }
                prev = mid;
                mid = next;
            }
            Console.WriteLine(cnt);
            Console.ReadLine();

            //задание 2

            int n = Convert.ToInt32(Console.ReadLine());
            int a = Convert.ToInt32(Console.ReadLine());
            int b = Convert.ToInt32(Console.ReadLine());
            int max1 = Math.Max(a, b);
            int max2 = Math.Min(a, b);
            for (int i = 0; i < n - 2; i++)
            {
                int next = Convert.ToInt32(Console.ReadLine());
                if (max1 < next && next > max2)
                {
                    max2 = max1;
                    max1 = next;
                }
                else if (next > max2)
                {
                    max2 = next;
                }
            }
            Console.WriteLine(max2);
            Console.ReadLine();

            // задание 3

            int n = Convert.ToInt32(Console.ReadLine());
            int cnt = 0;
            int next;
            for (int i = 0; i < n; i++)
            {
                next = Math.Abs(Convert.ToInt32(Console.ReadLine()));
                if (next % 10 == 3)
                {
                    cnt += 1;
                }
            }
            Console.WriteLine(cnt);
            Console.ReadLine();

            // задание 4

            int n = Convert.ToInt32(Console.ReadLine());
            bool flag = true;
            for (int i = 0; i < n; i++)
            {
                int next = Convert.ToInt32(Console.ReadLine());
                if (!(Math.Abs(next) % 10 == 4 && next % 2 == 0))
                {
                    // если число оканчивается на 4, то оно итак четное
                    flag = false;
                }
            }
            if (flag)
            {
                Console.WriteLine("Да");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Нет");
                Console.ReadLine();
            }
        }
    }
}
