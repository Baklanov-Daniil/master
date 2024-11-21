using System;
namespace program
{
    class program
    {
        static int Count_days(string[] data)
        {
            int[] months = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            int days_count = 0;
            days_count += 365 * Convert.ToInt32(data[2]);
            days_count += Convert.ToInt32(data[2]) / 4;
            for (int i = 0; i < Convert.ToInt32(data[1]); i++)
            {
                days_count += months[i];
            }
            days_count += Convert.ToInt32(data[0]);
            return days_count;
        }
        static void Main(string[] arg)
        {
            string[] first_data = Console.ReadLine().Split(".");
            string[] second_data = Console.ReadLine().Split(".");
            int start = Convert.ToInt32(Console.ReadLine());
            int days1 = Count_days(first_data);
            int days2 = Count_days(second_data);
            int dif = days2 - days1 + 1;
            int answer = (2 * start + dif - 1) * dif / 2;
            Console.WriteLine(answer);
        }
    }
}
