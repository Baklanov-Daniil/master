using System;

namespace practic_work
{
    class Program
    {
        static string Mix(string[] args, string answer)
        {
            string str = "";
            for (int i = 1; i < args.Length; i++)
            {
                str = str + args[i];
            }
            return answer + $"MX{str}XM";
        }

        static string Water(string[] args, string answer)
        {
            string str = "";
            for (int i = 1; i < args.Length; i++)
            {
                str = str + args[i];
            }
            return answer + $"WT{str}TW";
        }

        static string Dust(string[] args, string answer)
        {
            string str = "";
            for (int i = 1; i < args.Length; i++)
            {
                str = str + args[i];
            }
            return answer + $"DT{str}TD";
        }
        static string Fire(string[] args, string answer)
        {
            string str = "";
            for (int i = 1; i < args.Length; i++)
            {
                str = str + args[i];
            }
            return answer + $"FR{str}RF";
        }

        static void Main(string[] args)
        {
            string answer = "";
            string massive = "";
            while (true)
            {
                string enter = Console.ReadLine();
                if (enter == "END")
                {
                    break;
                }
                string[] text = enter.Split(" ");
                switch (text[0])
                {
                    case "MIX":
                        massive += Mix(text, answer) + " ";
                        break;
                    case "WATER":
                        massive += Water(text, answer) + " ";
                        break;
                    case "DUST":
                        massive += Dust(text, answer) + " ";
                        break;
                    case "FIRE":
                        massive += Fire(text, answer) + " ";
                        break;
                }
            }
            Console.WriteLine(massive);
            string[] magic = massive.Split(" ");
            Console.WriteLine(magic[2]);

            Type a = typeof(int);
            Console.WriteLine(a);
        }
    }
}
