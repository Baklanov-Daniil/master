using System;

namespace program
{
    class Program
    {
        static void Main(string[] args)
        {
            // задание 1

            int n = Convert.ToInt32(Console.ReadLine());
            int max = 0;
            int tmax = 0;
            int[] arr = new int[n];
            for (int i = 0; i < n; i++)
            {
                arr[i] = Convert.ToInt32(Console.ReadLine());
            }
            for (int i = 0; i < n; i++)
            {
                if (arr[i] != 0)
                {
                    tmax += 1;
                }
                else
                {
                    max = Math.Max(tmax, max);
                    tmax = 0;
                }
            }
            max = Math.Max(tmax, max);
            Console.WriteLine(max);

            // задание 2

            int n = Convert.ToInt32(Console.ReadLine());
            int min = n;
            int tmin = n;
            int[] arr = new int[n];
            for (int i = 0; i < n; i++)
            {
                arr[i] = Convert.ToInt32(Console.ReadLine());
            }
            for (int i = 0; i < n-1; i++)
            {
                if (arr[i] == arr[i + 1])
                {
                    tmin += 1;
                }
                else
                {
                    min = Math.Min(tmin, min);
                    tmin = 1;
                }
            }
            min = Math.Min(tmin, min);
            Console.WriteLine(min);

            // задание 3

            int n = Convert.ToInt32(Console.ReadLine());
            int sum = 0;
            int tsum = 0;
            bool flag = true;
            bool flag1 = true;
            int[] arr = new int[n];
            for (int i = 0; i < n; i++)
            {
                arr[i] = Convert.ToInt32(Console.ReadLine());
            }
            for (int i = 0; i < n; i++)
            {
                if (Math.Abs(arr[i])%2==0)
                {
                    if (flag == true)
                    {
                        tsum = arr[i];
                        flag = false;
                    }
                    else
                    {
                        tsum += arr[i];
                    }
                }
                else
                {
                    if (flag1==true)
                    {
                        sum = tsum;
                        flag1 = false;
                    }
                    sum = Math.Max(sum, tsum);
                    flag = true;
                }
            }
            if (flag1==true)
            {
                sum = tsum;
                flag1 = false;
            }
            if (flag == false){
                sum = Math.Max(tsum, sum);
            }

            Console.WriteLine(sum);
        }
    }
}