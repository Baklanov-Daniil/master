using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;

namespace _27._11
{
    internal class Program
    {
        // задание 1
        static string Task1(string input_string, string par = "")
        {
            string[] s = input_string.Split();
            string out_string = "";
            foreach (string i in s)
            {
                if (i!=string.Empty) { out_string += i + " ";}
            }
            if (par == "answer" ) {
                Console.WriteLine(out_string);
                return "0";
            }
            else { return out_string; }
            
        }

        // задание 2
        static void Task2(string input_string)
        {
            string[] arr = Task1(input_string).Split(); // форматрирование строки 
            string polindroms = "";
            bool Flag = true;
            foreach (string i in arr)
            {
                for (int j = 0; j < i.Length; j++)
                {
                    if (i[j] != i[^(j + 1)]) { Flag = false; break; }
                }
                if (Flag == true) { polindroms += i + " "; }
                Flag = true;
            }
            Console.WriteLine(polindroms);

        }

        // задание 3
        static void Task3(string input_string)
        {
            string[] sogl = {"B", "C", "D", "F", "G", "H", "J", "K", "L", "M", "N", "P",
                "Q", "R", "S", "T", "V", "W", "X", "Z"};
            string[] glas = { "A", "E", "I", "O", "U", "Y" };
            string[] arr = Task1(input_string).Split();
            int answer_count = 0;
            foreach (string i in arr)
            {
                int glas_count = 0;
                int sogl_count = 0;
                for (int j = 0; j < i.Length; j++)
                {
                    string s = Convert.ToString(i[j]).ToUpper();
                    if (sogl.Contains(s)) { sogl_count += 1; }
                    else { glas_count += 1; }
                }
                if (sogl_count < glas_count) { answer_count += 1; }
            }
            Console.WriteLine(answer_count);
        }

        static void Main(string[] args)
        {
            string s = Console.ReadLine();
            Task1(s, "answer");
            Task2(s);
            Task3(s);
        }
    }
}
 