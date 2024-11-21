using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace chislo
{
    internal class Program
    {
        static int[] Plus(string number, int[] op_collection)
        {
            if (!int.TryParse(number, out int parseedNumber))
            {
                op_collection[0] += 1;
            }
            else
            {
                op_collection[1] += parseedNumber;
            }
            return op_collection;
        }

        static int[] Minus(string number, int[] op_collection)
        {
            if (!int.TryParse(number, out int parseedNumber))
            {                      
                op_collection[0] -= 1;
            }
            else
            {
                op_collection[1] -= parseedNumber;

            }
            return op_collection;
        }

        static int[] Pow(string number, int[] op_collection)
        {
            if (int.TryParse(number, out int parsedNumber)) 
            {
                op_collection[0] *= parsedNumber;
                op_collection[1] *= parsedNumber;
            }
            return op_collection;
        }

        static void Main(string[] args)
        {
            int operation_number = int.Parse(Console.ReadLine());
            int[] op_collection = new int[2];
            op_collection[0] = 1;
            for (int i = 0; i < operation_number; i++)
            {
                string number = "";
                string input_operation = Console.ReadLine();
                string operation = "";
                for (int j = 0; j < input_operation.Length; j++)
                {
                    
                    if (j == 0) { operation = Convert.ToString(input_operation[j]); }
                    else if (j == 1) { continue; }
                    else { number += input_operation[j]; }

                }
                switch (operation)
                {
                    case "+":
                        op_collection = Plus(number, op_collection);
                        break;
                    case "-":
                        op_collection = Minus(number, op_collection);
                        break;
                    case "*":
                        op_collection = Pow(number, op_collection);
                        break;
                }
                number = "";
            }
            int result = int.Parse(Console.ReadLine());
            float answer = (result - op_collection[1]) / op_collection[0];
            Console.WriteLine(answer);
        }
    }
}

