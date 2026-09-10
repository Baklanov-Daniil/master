// 1) класс телефон: номер и оператор
// с использованиаем словря определить частоту появления каждого опреатора
// 2) класс в котором 2 int переменные и реализованы 4 метода + - / * с использованием делегата (в нем записана группа опреаций). необходимо выполнить следующие групповые операции:
// 1 делегат - сложение потом * полученной суммы на 2 элемент потом вычитание из полученного значения второго элемента
// 2 делегат - разность потом полученную разность разделить на 2 элемент и полученное значение умножить на 2 элемент
// сделать проверку деления на 0
// в main создать 2 объекта класса переменных. К первому объекту делегат с группой действия первой, ко 2 объекту делегат с группой действия 2

using System;
using System.Collections.Generic;

namespace aip{
    public class Phone{
            public string number;
            public string provider;
            public Phone(string[] data){
                this.number = data[0];
                this.provider = data[1];
            }
        }

        public class Variables{
            public int x;
            public int y;

            public Variables(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public static int sum(int a, int b) => a+b;

            public static int minus(int a, int b) => a-b;

            public static int multiply(int a, int b) => a*b;

            public static int divide(int a, int b){
                if (b!=0){
                    return a/b;
                }
                Console.WriteLine("Дленеие на 0 невозможно");
                return 0;
            }
        }

    class Program{
        static void Task1(){
            List<Phone> phone_list = new List<Phone>();
            Console.WriteLine("Вводите номер по шаблону: номер оператор. Чтобы остановить ввдете: end");
            while (true){
                string data = Console.ReadLine();
                if (data=="end"){
                    break;
                }
                Phone phone = new Phone(data.Split());
                phone_list.Add(phone);
            }
            
            HashSet<string> provider_hash = new HashSet<string>();
            Dictionary<string, int> provider_dict = new Dictionary<string, int>();
            foreach (Phone phone in phone_list){
                provider_hash.Add(phone.provider);
            }
            foreach (string provider in provider_hash){
                int cnt = 0;
                foreach (Phone phone in phone_list){
                    if (phone.provider == provider){
                        cnt += 1;
                    }
                }
                provider_dict[provider] = cnt;
            }
            foreach (var i in provider_dict){
                Console.WriteLine($"{i.Key} : {i.Value}");
            }
        }

        delegate int dalegat1(Variables value);
        delegate int dalegat2(Variables value);
        static void Task2()
        {
            Variables value1 = new Variables(8, 4);
            Variables value2 = new Variables(10, 5);
            dalegat1 first_operation = delegate (Variables variables)
            {
                int result = Variables.sum(variables.x,variables.y);
                result = Variables.multiply(result, variables.y);
                result = Variables.minus(result, variables.y);
                return result;
            };
            dalegat2 second_operation = delegate (Variables variables)
            {
                int result = Variables.minus(variables.x, variables.y);
                result = Variables.divide(result, variables.y);
                result = Variables.multiply(result, variables.x);
                return result;
            };
            int result1 = first_operation(value1);
            int result2 = second_operation(value2);
            Console.WriteLine($"Первая операция: {result1}, вторая операция: {result2}");
        }

        static void Main(string[] args){
            // Task1();
            Task2();
        }
    }
}