using System.Collections.Generic;
using System.Collections;

// namespace aip {
//     class Program{
//         class Phone{
//             public string number;
//             public string provider;
//             public string year;
//             public Phone(string number, string provider, string year){
//                 this.number = number;
//                 this.provider = provider;
//                 this.year = year;
//             }
//         }
        
//         class User {
//             public string name;
//             public string city;
//             public ArrayList phone_numbers;

//             public User(string name, string city, ArrayList phone_numbers) {
//                 this.name = name;
//                 this.city = city;
//                 this.phone_numbers = phone_numbers;        
//             }
//         }
//         class Menu{
//             public ArrayList phones = new ArrayList();
//             public ArrayList users = new ArrayList();

//             public void Base_menu(){
//                 int action;
//                 do {
//                     action = get_action();
//                     switch (action){
//                         case 1:
//                             Enter_base();
//                             break;
//                         case 2:
//                             Get_choise();
//                             break;
//                         case 3:
//                             Console.WriteLine("Завершение программы");
//                             break;
//                         case 4:
//                             Show_data();
//                             break;
//                         default:
//                             Console.WriteLine("Невеная команда");
//                             break;
//                     } 
//                 } while (action!=3);
//             }

//             public int get_action(){
//                 Console.WriteLine();
//                 Console.WriteLine("------------------------------------------------");
//                 Console.WriteLine("Введите 1, чтобы заполнить базу данных");
//                 Console.WriteLine("Введите 2, чтобы получить выборку");
//                 Console.WriteLine("Введите 3, чтобы выйти");
//                 Console.WriteLine("------------------------------------------------");
//                 Console.Write("Ваше дейстиве: ");
//                 int action = int.Parse(Console.ReadLine());
//                 Console.WriteLine();
//                 return action;
//             }
            
//             public void Enter_base(){
//                 Console.Write("Ведите тип данных (телефон/пользователь): ");
//                 string type = Console.ReadLine();
//                 Console.Write("Введите количество объектов: ");
//                 int count = int.Parse(Console.ReadLine());
//                 switch (type){
//                     case "телефон":
//                         Console.WriteLine("Вводите данные по шаблону: номер оператор год");
//                         for (int i=0; i<count;i++){
//                             Console.Write($"Введите {i+1} телефон: ");
//                             string[] input_data = Console.ReadLine().Split(); 
//                             Phone phone = new Phone(input_data[0], input_data[1], input_data[2]);
//                             this.phones.Add(phone);
//                         }
//                         break;
//                     case "пользователь":
//                         Console.WriteLine("Вводите данные по шаблону: фио город телефон");              
//                         for (int i = 0;i<count;i++){
//                             Console.Write($"Введите {i+1} пользователя: ");
//                             string[] input_data = Console.ReadLine().Split(); 
//                             ArrayList phone_numbers = new ArrayList();
//                             phone_numbers.Add(input_data[2]);
//                             bool flag = true;
//                             do {
//                                 Console.Write("Если вы хотите добавить еще номер, ввдеите его. Иначе введите \"нет\" ");
//                                 string act = Console.ReadLine();
//                                 switch (act){
//                                     case "нет":
//                                         flag = false;
//                                         break;
//                                     default:
//                                         phone_numbers.Add(act);
//                                         break;
//                             }
//                             } while (flag);
//                             users.Add(new User(input_data[0], input_data[1], phone_numbers));
//                         }
//                         break;
//                 }
//             }
//             public void Get_choise(){
//                 Console.Write("Ведите параметр выборки (номер, оператор, год): ");
//                 string type = Console.ReadLine();
//                 Console.Write("Введите значение параметра: ");
//                 string value = Console.ReadLine();
//                 bool find = false;
//                 switch (type){
//                     case "номер":
//                     foreach (Phone phone in this.phones){
//                         string user_data = "нет пользователя";
//                         string phone_data = null;
//                         if (phone.number == value){
//                             phone_data = $"Телефон: {phone.number} {phone.provider} {phone.year} принадлежит ";
//                             find = true;
//                         }
//                         if (phone_data!=null){
//                             foreach (User user in this.users){
//                                 foreach (string user_phone in user.phone_numbers){
//                                     if (user_phone.Equals(value)){
//                                         string user_phones = "";
//                                         foreach(string number in user.phone_numbers){
//                                             user_phones += number + " ";
//                                         }
//                                         user_data = $"{user.name} {user.city} {user_phones}";
//                                     }
//                                 }
//                             }
//                             Console.WriteLine(phone_data+user_data);
//                             break;
//                         }
//                     }
//                     break;
//                     case "оператор":
//                         foreach (Phone phone in this.phones){
//                             string user_data = "нет пользователя";
//                             string phone_data = null;
//                             string phone_value = null;
//                             if (phone.provider == value){
//                                 phone_data = $"Телефон: {phone.number} {phone.provider} {phone.year} принадлежит ";
//                                 phone_value = phone.number;
//                                 find = true;
//                             }
//                             if (phone_data!=null){
//                                 foreach (User user in this.users){
//                                     foreach (string user_phone in user.phone_numbers){
//                                         if (user_phone.Equals(phone_value)){
//                                             string user_phones = "";
//                                             foreach(string number in user.phone_numbers){
//                                                 user_phones += number + " ";
//                                             }
//                                             user_data = $"{user.name} {user.city} {user_phones}";
//                                         }
//                                     }
//                                 }
//                                 Console.WriteLine(phone_data+user_data);
//                             }
//                         }
//                         break;
//                     case "год":
//                     foreach (Phone phone in this.phones){
//                             string user_data = "нет пользователя";
//                             string phone_data = null;
//                             string phone_value = null;
//                             if (phone.year == value){
//                                 phone_data = $"Телефон: {phone.number} {phone.provider} {phone.year} принадлежит ";
//                                 phone_value = phone.number;
//                                 find = true;
//                             }
//                             if (phone_data!=null){
//                                 foreach (User user in this.users){
//                                     foreach (string user_phone in user.phone_numbers){
//                                         if (user_phone.Equals(phone_value)){
//                                             string user_phones = "";
//                                             foreach(string number in user.phone_numbers){
//                                                 user_phones += number + " ";
//                                             }
//                                             user_data = $"{user.name} {user.city} {user_phones}";
//                                         }
//                                     }
//                                 }
//                                 Console.WriteLine(phone_data+user_data);
//                             }
//                         }
//                         break;
//                 }
//                 if (find == false){
//                     Console.WriteLine("Таких данных нет");
//                 }
//             }
//             public void Show_data(){
//                 if (phones.Count > 0){
//                     Console.WriteLine("Телефоны: ");
//                     foreach (Phone i in this.phones){
//                         Console.WriteLine($"{i.number} {i.provider} {i.year}");
//                     }
//                 }
//                 Console.WriteLine("Пользователи: ");
//                 if (users.Count > 0){
//                     foreach (User i in this.users){
//                         Console.Write($"{i.name} {i.city} ");
//                         foreach(string number in i.phone_numbers){
//                             Console.Write(number+" ");
//                         }
//                         Console.WriteLine();
//                     }
//                 }
//             }
//         }
//         static void Main(string[] args){
//             Menu menu = new Menu();
//             menu.Base_menu();
//         }
//     }
// }

namespace aip{
    class Phone{
            public string number;
            public string provider;
            public string year;
            public Phone(string number, string provider, string year){
                this.number = number;
                this.provider = provider;
                this.year = year;
            }
        }
        
        class User {
            public string name;
            public string city;
            public ArrayList phone_numbers;

            public User(string name, string city, ArrayList phone_numbers) {
                this.name = name;
                this.city = city;
                this.phone_numbers = phone_numbers;        
            }
        }

        class Menu{
            public ArrayList users = new ArrayList();

            public void Run(){
                int action;
                do {
                    action = get_action();
                    switch (action){
                        case 1:
                            Enter_base();
                            break;
                        case 2:
                            Get_choise();
                            break;
                        case 3:
                            Console.WriteLine("Завершение программы");
                            break;
                        case 4:
                            Show_data();
                            break;
                        default:
                            Console.WriteLine("Неверная команда");
                            break;
                    } 
                } while (action!=3);
            }

            public int get_action(){
                Console.WriteLine();
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("Введите 1, чтобы заполнить базу данных");
                Console.WriteLine("Введите 2, чтобы получить выборку");
                Console.WriteLine("Введите 3, чтобы выйти");
                Console.WriteLine("------------------------------------------------");
                Console.Write("Ваше дейстиве: ");
                int action = int.Parse(Console.ReadLine());
                Console.WriteLine();
                return action;
            }
            
            public void Enter_base(){
                Console.Write("Введите количество пользователей: ");
                int count = int.Parse(Console.ReadLine());
                Console.WriteLine("Вводите данные по шаблону: фио город");              
                for (int i = 0; i < count;i++){
                        Console.Write($"Введите {i+1} пользователя: ");
                        string[] input_data_user = Console.ReadLine().Split(); 
                        ArrayList phone_numbers = new ArrayList();
                        Console.WriteLine("Вводите данные по шаблону: номер оператор год. Чтобы закончить, введите \"стоп\"");

                        while (true){
                            Console.Write($"Введите {phone_numbers.Count+1} телефон: ");
                            string[] input_data_phone = Console.ReadLine().Split(); 
                            if (input_data_phone[0]=="стоп") break;
                            Phone phone = new Phone(input_data_phone[0], input_data_phone[1], input_data_phone[2]);
                            phone_numbers.Add(phone);
                        }
                        users.Add(new User(input_data_user[0], input_data_user[1], phone_numbers));
                }
            }
            public int Get_choise(){
                if (this.users.Count == 0){
                    Console.WriteLine("База данных пуста");
                    return 0;
                }
                Console.Write("Ведите параметр выборки (номер, оператор, год): ");
                string type = Console.ReadLine();
                Console.Write("Введите значение параметра: ");
                string value = Console.ReadLine();
                bool find = false;
                switch (type){
                    case "номер":
                        foreach (User user in users){
                            foreach (Phone phone in user.phone_numbers){
                                if (phone.number == value){
                                    Console.WriteLine($"{user.name} {user.city}: {phone.number} {phone.provider} {phone.year}");
                                    find = true;
                                }
                            }
                        }
                        break;
                    case "оператор":
                        foreach (User user in users){
                            foreach (Phone phone in user.phone_numbers){
                                if (phone.provider == value){
                                    Console.WriteLine($"{user.name} {user.city}: {phone.number} {phone.provider} {phone.year}");
                                    find = true;
                                }
                            }
                        }
                        break;
                    case "год":
                        foreach (User user in users){
                            foreach (Phone phone in user.phone_numbers){
                                if (phone.year == value){
                                    Console.WriteLine($"{user.name} {user.city}: {phone.number} {phone.provider} {phone.year}");
                                    find = true;
                                }
                            }
                        }
                        break;
                }
                if (find == false){
                    Console.WriteLine("Таких данных нет");
                }
                return 1;
            }
            public void Show_data(){
                Console.WriteLine("Пользователи: ");
                if (users.Count > 0){
                    foreach (User i in this.users){
                        Console.Write($"{i.name} {i.city}: ");
                        foreach(Phone phone in i.phone_numbers){
                            Console.Write($"{phone.number} {phone.provider} {phone.year},");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
    class Program{
        
        static void Main(string[] args){
            Menu menu = new Menu();
            menu.Run();
        }
    }
}