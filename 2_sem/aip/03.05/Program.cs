// 1) необходмо вычислить значение выражения, представленного в виде поствиксной польской записи (стэк) (защита не нужна)
// поствиксная запись - аргумент + оперция (2+2 -> 22+) (обработка деления на 0)
// 2) дан список из элементов:
// выдать элементы из которых составлен список(hash-set)
// определить чистоту появления каждого элемента(словарь)

using System;
using System.Collections.Generic;
namespace aip{
    class Program{
        static void Task1(){  
            static int calculate(ref Stack<double> my_stack, char operation){
                double res = 0;
                switch (operation){
                    case '+':
                        res = my_stack.Pop();
                        res = my_stack.Pop() + res;
                        break;
                    case '-':
                        res = my_stack.Pop();
                        res = my_stack.Pop() - res;
                        break;
                    case '*':
                        res = my_stack.Pop();
                        res = my_stack.Pop() * res;
                        break;
                    case '/':
                        if (my_stack.Peek()==0){
                            Console.WriteLine("Деление на 0");
                            return 0;
                        }
                        res = my_stack.Pop();
                        res = my_stack.Pop() / res;
                        break;
                }
                my_stack.Push(res);
                return 1;
            }

            Stack<double> my_stack = new Stack<double>();
            string input_string = "22+5-4*2/";//Console.ReadLine();
            bool flag = true;

            foreach (char i in input_string){
                if ((i == '+') || (i == '-') || (i == '*') || (i == '/')){
                    if (my_stack.Count==0 || my_stack.Count==1){
                        Console.WriteLine("Запись неверна");
                        flag = false;
                        break;
                    }
                    if (calculate(ref my_stack, i)=='0'){
                        flag = false;
                        break;
                    }
                }
                else{
                    my_stack.Push(double.Parse(i.ToString()));
                }
            }

            if ((my_stack.Count==1) && (flag==true)){          
                Console.WriteLine(my_stack.Peek());
            }
        }

        static void Task2(){
            List<int> my_list = new List<int>();
            my_list = [1, 2, 3, 3, 3, 4];
            HashSet<int> my_hash = new HashSet<int>();
            Dictionary<int, int> my_dict = new Dictionary<int, int>();
            foreach (int i in my_list){
                my_hash.Add(i);
            }
            foreach (int i in my_hash){
                int cnt = 0;
                for (int j = 0; j<my_list.Count; j++){
                    if (my_list[j]==i){
                        cnt += 1;
                    }
                }
                my_dict[i] = cnt;
            }
            foreach (var i in my_dict){
                Console.WriteLine($"{i.Key} : {i.Value}");
            }
        }
        static void Main(string[] args) {
            Task1();
            Task2();
        }
    }
}