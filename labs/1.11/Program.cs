using System;

namespace lab
{
    class Program
    {

        // задание 1

        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            int[,] arr = new int[n, n];
            int[,] param = new int[n, 3];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    arr[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    param[i, 0] += arr[i, j];
                    if (arr[i, j] == 0) { param[i, 2] += 1; }
                    else { param[i, 1] += arr[i, j]; }
                }
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if ((param[i, 0] == param[j, 0]) && (param[i, 1] == param[j, 1]) && (param[i, 2] == param[j, 2]))
                    {
                        Console.WriteLine($"row {i + 1} row {j + 1}");
                        break;
                    }
                }
            }
        }
         //+ 0 * - параметра

         //задание 2

        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            int[,] arr = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    arr[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }
            int mx = arr[0, 0];
            int mn = arr[0, 0];
            int[] mxij = new int[2];
            int[] mnij = new int[2];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (arr[i, j] > mx)
                    {
                        mx = arr[i, j];
                        mxij[0] = i;
                        mxij[1] = j;
                    }
                    if (arr[i, j] < mn)
                    {
                        mn = arr[i, j];
                        mnij[0] = i;
                        mnij[1] = j;
                    }
                }
            }
            arr[mxij[0], mxij[1]] = mn;
            arr[mnij[0], mnij[1]] = mx;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(arr[i, j] + " ");
                }
                Console.WriteLine();
            }
        }


        // задание 3

        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            int[,] arr = new int[n, n];
            bool flag = true;
            int sum = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    arr[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }
            int mx = arr[0, 0];
            int mn = arr[0, 0];
            int[] mxij = new int[2];
            int[] mnij = new int[2];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    sum += arr[j, i];
                }
                if (sum % 2 != 0)
                {
                    flag = false;
                    Console.WriteLine(i);
                    break;
                }
                sum = 0;
            }
            if (flag == true) { Console.WriteLine("YES"); }
            else { Console.WriteLine("NO"); }

        }
    }
}