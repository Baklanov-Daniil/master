using System;

namespace practic_work
{
    задание 1

    class Program 
    {
        static void Main(string[] args) 
        {
            int n = Convert.ToInt32(Console.ReadLine());
            int[] arr = new int[n];
            bool Flag = true;
            for (int i = 0; i < n; i++)
            {
                arr[i] = Convert.ToInt32(Console.ReadLine());
            }
                for (int i = 0; i < n-2; i++){
                    if (arr[i+2]-arr[i+1]!=arr[i+1]-arr[i])
                    {
                        Flag = false;
                        break;
                    }
                }
            if (arr.Length==2) {Console.WriteLine("YES");}
            else 
            {
                if (Flag == true) {Console.WriteLine("YES");}
                else {Console.WriteLine("NO");}
            }
        }
    }

    задание 2
    class Program 
    {
        static void Main(string[] args) 
        {
            int n = Convert.ToInt32(Console.ReadLine());
            int[] arr = new int[n];
            for (int i = 0; i < n; i++)
            {
                arr[i] = Convert.ToInt32(Console.ReadLine());
            }
            string neg = "";
            string zero = "";
            string plus = "";
            foreach (int i in arr) 
            {
                switch (i) 
                {
                    case >0:
                        neg += Convert.ToString(i) + " ";
                        break;
                    case 0:
                        zero += Convert.ToString(i) + " ";
                        break;
                    case <0:
                        plus += Convert.ToString(i) + " ";
                        break;
                }
            }
            string[] answer = (plus + zero + neg).Split();
            foreach (string i in answer) {Console.Write(i + " ");}
        }
    }

    задание 3
    class Program 
    {
        public static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            int[] arr = new int[n];
            bool Flag = false;
            for (int i = 0; i < n; i++)
            {
                arr[i] = Convert.ToInt32(Console.ReadLine());
            }
            for (int i = 0; i < n; i++)
            {
                if (arr[i]<0) {continue;}
                if (arr[i]%2!=0) {continue;}
                if (i%2!=0) {continue;}
                else {Flag = true; break;}
            }
            if (Flag == true) {Console.WriteLine("YES");}
            else {Console.WriteLine("NO");}
        }
    }
}