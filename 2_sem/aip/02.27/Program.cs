// на вход подаётся математичская выражение, в котором используются числовые данные, арефметические операции и присутствуют скобки 3 видов (){}[]
// c помощью стека необходимо определить правильно ли в выражении расставлены скобки
using System;
using System.Collections.Generic;

namespace aip{
    class Program{
        static void Main(string[] args){
            string test = "([])";
            bool check = true;
            if (test.Length==0){
                Console.WriteLine("Строка пуста");
            }
            else{
                Stack<char> my_stack = new Stack<char>();
                foreach (char symb in test){
                    if (symb=='(' || symb=='[' || symb=='{'){
                        my_stack.Push(symb);
                    }
                    else if (symb==')' || symb==']' || symb=='}'){
                        if (my_stack.Count == 0){
                            check = false;
                            break;
                        }
                        if ((my_stack.Peek()=='(' && symb!=')') || (my_stack.Peek()=='[' && symb!=']') || (my_stack.Peek()=='{' && symb!='}')){
                            Console.WriteLine($"{symb}, {my_stack.Peek()}");
                            check = false;
                            break;
                        }  
                        my_stack.Pop();
                    }
                }
                switch (check)
                {
                    case true:
                        Console.WriteLine("Выражение задано правильно");
                        break;
                    case false:
                        Console.WriteLine("Выражение задано неправильно");
                        break;
                }
            }
        }
    }
}