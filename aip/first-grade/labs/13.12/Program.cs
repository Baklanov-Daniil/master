using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace _13._12
{

    internal class Program
    {
        // условие задачи:
        // дан класс, состоящий из 2 полей целого типа в классе реализовать 3 конструтора(по умолчанию (без параметров), с 1 параметром (инициализируем 1 элемент, 2 - 0), с 2 параметрами)
        // методы по подсчёту суммы, произведения, деления 1 на 2 (отследить деление на 0), вычитание из 1 2
        // в main 3 объекта с помощью разных конструткоров
        // для каждого объекта все 4 операции
        // вывод результата либо в методе, либо в main
        public class Task
        {
            // инициалицация полей
            public int first;
            public int second;
            //конструктор без параметров
            public Task()
            {
                this.first = 2;
                this.second = 0;
            }
            //конструктор с 1 параметром
            public Task(int first)
            {
                this.first = first;
                this.second = 0;
            }
            //конструктор с 2 параметрами
            public Task(int first, int second)
            {
                this.first = first;
                this.second = second;
            }
            // операции
            public void Sum()
            {
                Console.WriteLine(this.first + this.second);
            }

            public void Comp()
            {
                Console.WriteLine(this.first * this.second);
            }

            public void Div()
            {
                if (this.second == 0) { Console.WriteLine("Деление на 0 невозможно"); }
                else 
                {
                    float first = this.first;
                    float second = this.second;
                    Console.WriteLine(first/second); 
                }
            }

            public void Minus()
            {
                Console.WriteLine(this.first - this.second);
            }

        }

        static void Main()
        {
            int a = int.Parse(Console.ReadLine());
            int b = int.Parse(Console.ReadLine());
            int c = int.Parse(Console.ReadLine());
            Task ob1 = new Task();
            Task ob2 = new Task(a);
            Task ob3 = new Task(b,c);
            ob1.Sum();
            ob1.Comp();
            ob1.Div();
            ob1.Minus();
            ob2.Sum();
            ob2.Comp();
            ob2.Div();
            ob2.Minus();
            ob3.Sum();
            ob3.Comp();
            ob3.Div();
            ob3.Minus();
        }
    
    }
    
}
