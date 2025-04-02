// 2 переменные и необходимо использовать лямбда выржения необходимо определить + - * / с проверкой на деление
// список из переменных текстового типа с помощью лямбда выражения сделать выборку только тех,которые начинаются на символ "a"
using System;
using System.Collections;

namespace aip{

    
    class Program{
        static void Task1(){
            var sum = (int x, int y) => x+y;
            var minus = (int x, int y) => x-y;
            var multiply = (int x, int y) => x*y;
            var divide = (int x, int y) => y!=0 ? (object)(x/y) : (object)("Ошибка деленя на 0");
            int x = 18;
            int y = 9;
            Console.WriteLine(sum(x,y));
            Console.WriteLine(minus(x,y));
            Console.WriteLine(multiply(x,y));
            Console.WriteLine(divide(x,y));
        }

        static void Task2(){
            var chose = (string x) => x.StartsWith('a');
            List<string> strings = new List<string> { "apple", "banana", "apricot", "orange", "avocado" };
            foreach (string s in strings){
                if (chose(s)) Console.WriteLine(s);
            }
        }

        static void Main(string[] args){
            Task1();
            Task2();
        }
    }
}