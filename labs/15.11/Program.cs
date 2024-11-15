using System.Numerics;
using System.Runtime.InteropServices;
using System.Transactions;
using System;
using System.Runtime.CompilerServices;

namespace lab
{
    internal class Program
    {
        static void Enter(int[][] arr, int i)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            int[] arr2 = new int[n];
            arr[i] = arr2;
            for (int j = 0; j < n; j++)
            {
                arr2[j] = Convert.ToInt32(Console.ReadLine());
            }
            
        }
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            int[][] arr = new int[n][];
            for (int i = 0; i < n; i++) { Enter(arr, i); }
            for (int i = 0; i < arr.Length; i++)
            {
                int mx = 0;
                int mn = 0;
                for (int j = 0; j < arr[i].Length; j++)
                {
                    if (j == 0)
                    {
                        mx = arr[i][j];
                        mn = arr[i][j];
                    }
                    else
                    {
                        if (mx < arr[i][j] && arr[i][j] % 2 == 0) { mx = arr[i][j]; }
                        if (mn > arr[i][j] && arr[i][j] % 2 == 0) { mn = arr[i][j]; }
                    }
                }
                if ((mx != mn) && (mx%2==0) && (mn % 2 == 0)) { Console.WriteLine(i+1); }
            }
            Console.ReadLine();
        }
    }
}