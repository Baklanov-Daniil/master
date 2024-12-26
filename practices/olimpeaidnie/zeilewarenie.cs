using System;

namespace zeilevarenie
{
    class Program
    {

        static string Magic(string operations_before, string[] splited_input_string)
        {
            string answer = "";
            for (int i = 1; i < splited_input_string.Length; i++)
            {
                if (int.TryParse(splited_input_string[i], out int _))
                {
                    answer += operations_before.Split()[int.Parse(splited_input_string[i]) - 1];
                }
                else
                {
                    answer += splited_input_string[i];
                }
            }
            return answer;
        }

        static void Main(string[] args)
        {
            string memory = "";
            string start = "";
            string end = "";
            while (true)
            {
                string input_string = Console.ReadLine();
                if (input_string == "END")
                {
                    break;
                }
                string[] splited_input_string = input_string.Split();
                switch (splited_input_string[0])
                {
                    case "DUST":
                        start = "DT";
                        end = "TD";
                        break;
                    case "WATER":
                        start = "WT";
                        end = "TW";
                        break;
                    case "MIX":
                        start = "MX";
                        end = "XM";
                        break;
                    case "FIRE":
                        start = "FR";
                        end = "RF";
                        break;
                }
                memory += start + Magic(memory, splited_input_string) + end + " ";
            }
            string[] memory_splited = memory.Trim().Split();
            Console.WriteLine(memory_splited[^1]);
            Console.ReadLine();
        }
    }
}
