using System;

namespace _17._12
{
    internal class Program
    {
        public class Planet
        {
            public string Name { get; set; }
            public int Distance { get; set; }
            public int Diameter { get; set; }
            public int Satellite_count { get; set; }

            public Planet(string input_string)
            {
                string[] input_massive = input_string.Split(' ');
                this.Name = input_massive[0];
                this.Distance = int.Parse(input_massive[1]);
                this.Diameter = int.Parse(input_massive[2]);
                this.Satellite_count = int.Parse(input_massive[3]);
            }
        }

        static void Menu()
        {
            bool Close = false;
            Planet[] Base = { };
            while (true)
            {
                int action = Show_Actions();
                switch (action)
                {
                    case 1:
                        Base = Enter_Base();
                        break;
                    case 2:
                        Satellite_Choise(Base);
                        break;
                    case 3:
                        Distance_Choise(Base);
                        break;
                    case 4:
                        Close = true;
                        break;
                }
                Console.WriteLine();
                if (Close == true) { return; }
            }   
        }

        static Planet[] Enter_Base()
        {
            Console.Write("Введите количество планет: ");
            int count_objects = int.Parse(Console.ReadLine());
            Planet[] Base = new Planet[count_objects];
            Console.WriteLine("Введите данные по шаблону: название дистанция диаметр количество спутников");
            for (int i = 0; i < count_objects; i++)
            {
                Planet planet = new Planet(Console.ReadLine());
                Base[i] = planet;
            }
            return Base;
        }

        static int Satellite_Choise(Planet[] Base)
        {
            if (Base.Length == 0)
            {
                Console.WriteLine("База пуста, заполните её");
                return 0;
            }
            else
            {
                Console.Write("Введите количество спутников: ");
                int satellite_count = int.Parse(Console.ReadLine());
                foreach (Planet planet in Base)
                {
                    if (planet.Satellite_count == satellite_count)
                    {
                        Console.WriteLine($"{planet.Name} {planet.Distance} {planet.Diameter} {planet.Satellite_count}");
                    }
                }
            }
            return 1;
        }
        
        static int Distance_Choise(Planet[] Base)
        {
            
            if (Base.Length == 0)
            {
                Console.WriteLine("База пуста, заполните её");
                return 0;
            }
            else
            {
                Console.WriteLine("Введите дистанцию от центра галактики");
                int distence = int.Parse(Console.ReadLine());
                foreach (Planet planet in Base)
                {
                    if (planet.Distance == distence)
                    {
                        Console.WriteLine($"{planet.Name} {planet.Distance} {planet.Diameter} {planet.Satellite_count}");
                    }
                }
            }
            return 1;
        }

        static int Show_Actions()
        {
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("Введите 1, чтобы заполнить базу данных");
            Console.WriteLine("Введите 2, чтобы получить выборку по количеству спутников");
            Console.WriteLine("Введите 3, чтобы получить выборку по удалённости от центра галактики");
            Console.WriteLine("Введите 4, чтобы выйти");
            Console.WriteLine("Введите 5, чтобы получить информацию из базы данных");
            Console.WriteLine("------------------------------------------------------------------------");
            Console.Write("Ваше действие: ");
            int act = int.Parse(Console.ReadLine());
            return act;
        }
        
        static void Main(string[] args)
        {
            Menu();
            // Имеется класс, характеризующий планеты солнечной системы с полями: название, удалённость от центра галактики, диаметр, количество спутников
            // 1. Заполнение базы данных (у пользователя спрашивают количество объектов и все данные данные по каждому объекту, конструктор принимает все поля)
            // 2. Первая выборка (выборка по количеству спутников)
            // 3. выборка по удаленности (выбрать все объекты, которые удалены выше заданной длины7
            // 4. выход
            // при осуществлении выборки предусмотреть вариант, что данных в базе нет
            // методы вывода или в классе или функции или просто в main
        }
    }
}
