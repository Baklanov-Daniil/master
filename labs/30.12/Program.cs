using System;

namespace lab{
    class Program{
        public class Furnace{
            public int temperature;
            public int time;

            public Furnace(int temperature, int time){
                this.temperature = temperature;
                this.time = time;
            }
        }
        public class Bread: Furnace{
            public string name;
            public Bread(int temperature, int time, string name) : base (temperature, time){
                this.name = name;
            }
        }
        public class Cake: Furnace{
            public string name;
            public Cake(int temperature, int time, string name) : base (temperature, time){
                this.name = name;
            }
        }  

        public class Menu() {
            public Bread[] bread_data;
            public Cake[] cake_data;

            public void Base_menu(){
                bool Close = false;
                while (true) {
                    int action = Show_actions();
                    switch (action){
                        case 1:
                            this.Enter_base();
                            break;
                        case 2:
                            if (bread_data!=null || cake_data!=null){
                                this.Temp_selection();
                            }
                            else{
                                Console.WriteLine("В базе нет данных");
                            }
                            break;
                        case 3:
                            if (bread_data!=null || cake_data!=null){
                                this.Time_selection();
                            }
                            else{
                                Console.WriteLine("В базе нет данных");
                            }
                            break;
                        case 4:
                            Close = true;
                            break;
                        // вывод базы данных
                        // case 5:
                        //     Show_data();
                        //     break;
                    }
                    if (Close == true) {
                        break;
                    }
                }
            }

            public void Enter_base(){
                Console.Write("Введите тип объекта: торт/хлеб: ");
                string object_type = Console.ReadLine();
                Console.Write("Введите количество объектов: ");
                int object_count = int.Parse(Console.ReadLine());
                Console.WriteLine("Вводите данные по шаблону: температура время название");
                switch (object_type) {
                    case "торт":
                        this.cake_data = new Cake[object_count];
                        Cake[] cake_data = new Cake[object_count];
                        for (int i = 0; i<object_count; i++){
                            string[] i_cake = Console.ReadLine().Split();
                            this.cake_data[i] = new Cake(int.Parse(i_cake[0]), int.Parse(i_cake[1]), i_cake[2]);
                        }
                        break;
                    case "хлеб":
                        this.bread_data = new Bread[object_count];
                        Bread[] Bread_data = new Bread[object_count];
                        for (int i = 0; i<object_count; i++){
                            string[] i_bread = Console.ReadLine().Split();
                            this.bread_data[i] = new Bread(int.Parse(i_bread[0]), int.Parse(i_bread[1]), i_bread[2]);
                        }
                        break;
                }
            }

            public void Temp_selection(){
                Console.Write("Введите искомую темературу: ");
                int find_temp = int.Parse(Console.ReadLine());
                if (cake_data!=null){
                    foreach (Cake cake in this.cake_data)
                    {
                        if (cake.temperature == find_temp){
                            Console.WriteLine($"{cake.temperature} {cake.time} {cake.name}");
                        }
                    }
                }
                if (bread_data!=null) {
                    foreach (Bread bread in this.bread_data)
                    {
                        if (bread.temperature == find_temp){
                            Console.WriteLine($"{bread.temperature} {bread.time} {bread.name}");
                        }
                    }
                }
            }

            public void Time_selection(){
                Console.Write("Введите искомое время: ");
                int find_time = int.Parse(Console.ReadLine());
                if (cake_data!=null) {
                    foreach (Cake cake in this.cake_data)
                    {
                        if (cake.time == find_time){
                            Console.WriteLine($"{cake.temperature} {cake.time} {cake.name}");
                        }
                    }
                }
                if (bread_data!=null) {
                    foreach (Bread bread in this.bread_data)
                    {
                        if (bread.time == find_time){
                            Console.WriteLine($"{bread.temperature} {bread.time} {bread.name}");
                        }
                    }
                }
            }
            
            // вывод базы данных
            // public void Show_data(){
            //     foreach (Cake cake in this.cake_data)
            //     {
            //         Console.WriteLine($"{cake.temperature} {cake.time} {cake.name}");
            //     }
            //     foreach (Bread bread in this.bread_data)
            //     {
            //         Console.WriteLine($"{bread.temperature} {bread.time} {bread.name}");
            //     }
            // }

            public int Show_actions(){
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("Введите 1, чтобы заполнить базу данных");
                Console.WriteLine("Введите 2, чтобы получить выборку по температуре");
                Console.WriteLine("Введите 3, чтобы получить выборку по времени");
                Console.WriteLine("Введите 4, чтобы выйти");
                Console.WriteLine("------------------------------------------------");
                Console.Write("Ваше дейстиве: ");
                int action = int.Parse(Console.ReadLine());
                return action;
            }
        }
        static void Main(){
            Menu menu = new Menu();
            menu.Base_menu();
        }   
    }   
}
