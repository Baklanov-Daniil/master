using System;

namespace Program;

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
        for (int i = 0; i<n; i++) {
            next = Convert.ToInt32(Console.ReadLine());
            if (mid<prev && mid<next) {
                cnt += 1;
            }
            prev = mid;
            mid = next;
        }
        Console.WriteLine(cnt);

        //задание 2

        int n = Convert.ToInt32(Console.ReadLine());
        int max1 = 0;
        int max2 = 0;
        for (int i = 0; i<n; i++) {
            int next = Convert.ToInt32(Console.ReadLine());
            max2 = Math.Max(max2, max1);
            max1 = Math.Max(max1, next);
        }
        Console.WriteLine(max2);

        // задание 3

        int n = Convert.ToInt32(Console.ReadLine());
        int cnt = 0;
        int next;
        for (int i = 0; i<n; i++) {
         next = Math.Abs(Convert.ToInt32(Console.ReadLine()));
            if (next%10==3) {
                cnt+=1;
            }
        }
        Console.WriteLine(cnt);
        
        // задание 4

        int n = Convert.ToInt32(Console.ReadLine());
        bool flag = true;
        for (int i = 0; i<n; i++) {
            int next = Convert.ToInt32(Console.ReadLine());
            if ((Math.Abs(next)%10==4 && next%2==0) == false) {
                // если число оканчивается на 4, то оно четное
                flag = false;
            }
        }
        if (flag){
            Console.WriteLine("Да");
        }
        else {
            Console.WriteLine("Нет");
        }
    }
    
}