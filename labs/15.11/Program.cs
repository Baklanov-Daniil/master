using System.Numerics;
using System.Runtime.InteropServices;

namespace lab15._11
{
    internal class Program
    {
        static void Enter(int[][] arr, int i)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            int[] arr2 = new int[n];
            for (int j = 0; j < n; j++)
            {
                arr2[j] = Convert.ToInt32(Console.ReadLine());
            }
            arr[i] = arr2;
           
        }

        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            int[][] arr = new int[n][];
            for (int i = 0; i < n; i++)
            {
                Enter(arr, i);
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < arr[i].Length; j++)
                {
                    Console.Write(arr[i][j] + " ");
                }
                Console.WriteLine();
            }
            for (int i = 0; i < n; i++)
            {
                int mx = -2147483648;
                int mx0 = -2147483648;
                int mn = 2147483647;
                int mn0 = 2147483647;
                for (int j = 0; j < arr[i].Length; j++)
                {
                    if (arr[i][j] > mx && arr[i][j] % 2 == 0)
                    {
                        mx = arr[i][j];
                    }
                    if (arr[i][j] < mn && arr[i][j] % 2 == 0)
                    {
                        mn = arr[i][j];
                    }
                }
                if ((mx != mx0) && (mn != mn0)) { Console.WriteLine(i+1); }
            }
        }
    }
}
