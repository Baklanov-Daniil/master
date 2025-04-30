// имеется класс, где объекты следущими полями: номер телефона, год подключения, оператор связи, фио
// с помощью языка запросов выполнить: группировку по дате подключеня, по оператору связи, поиск по номеру телефона, по фио владельца
// при любом раскладе выдать все данные по телефону
// сделать меню с выбором парамтра выборки
using System;
using System.Linq;
using System.Collections.Generic;
namespace aip
{
    public class Phone
    {
        public string PhoneNumber;
        public string Year;
        public string PhoneProvider;
        public string UserName;
        public Phone(string PhoneNumber, string Year, string PhoneProvider, string UserName)
        {
            this.PhoneNumber = PhoneNumber;
            this.Year = Year;
            this.PhoneProvider = PhoneProvider;
            this.UserName = UserName;
        }
    }

    public class Menu
    {
        public List<Phone> Phones = new List<Phone>();

        public int Run()
        {
            while (true)
            {
                ShowAction();
                Console.Write("ваше действие: ");
                int action = int.Parse(Console.ReadLine());
                switch (action)
                {
                    case 1:
                        EnterBase();
                        break;
                    case 2:
                        GetByNumber();
                        break;
                    case 3:
                        GetByYear();
                        break;
                    case 4:
                        GetByProvider();
                        break;
                    case 5:
                        GetByUserName();
                        break;
                    case 6:
                        Console.WriteLine("Завершене работы программы");
                        return 0;
                    default:
                        Console.WriteLine("Неверные данные");
                        break;
                }
            }
            
        }

        public void ShowAction()
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("1 - заполнить базу данных");
            Console.WriteLine("2 - получить информацию по номеру телефона");
            Console.WriteLine("3 - получить выбоку по году подключения");
            Console.WriteLine("4 - получить выбоку по оператору");
            Console.WriteLine("5 - получить выбоку по имени владельца");
            Console.WriteLine("6 - выйти из программы");
            Console.WriteLine("-----------------------------");
        }

        public void EnterBase()
        {
            Console.WriteLine($"Вводите информацию о телефоне по шаблону: номер год оператор владелец. Чтобы остановиться введите {"стоп"}");
            while (true)
            {
                string data = Console.ReadLine();
                if (data == "стоп") break;
                string[] phone_data = data.Split();
                Phone phone = new Phone(phone_data[0], phone_data[1], phone_data[2], phone_data[3]);
                this.Phones.Add(phone);
            }
        }

        public void GetByNumber()
        {
            Console.Write("Введите искомый номер: ");
            string number = Console.ReadLine();
            var NumberGet = from phone in this.Phones
                            where phone.PhoneNumber == number
                            select phone;
            ShowInfo(NumberGet.ToArray());
        }

        public void GetByYear()
        {
            Console.Write("Введите искомый год: ");
            string year = Console.ReadLine();
            var YaerGet = from phone in this.Phones
                            where phone.Year == year
                            select phone;
            ShowInfo(YaerGet.ToArray());
        }

        public void GetByProvider()
        {
            Console.Write("Введите искомого оператора: ");
            string provider = Console.ReadLine();
            var ProviderGet = from phone in this.Phones
                            where phone.PhoneProvider == provider
                            select phone;
            ShowInfo(ProviderGet.ToArray());
        }

        public void GetByUserName()
        {
            Console.Write("Введите искомое имя: ");
            string username = Console.ReadLine();
            var UserGet = from phone in this.Phones
                            where phone.UserName == username
                            select phone;
            ShowInfo(UserGet.ToArray());
        }

        public int ShowInfo(Phone[] phones)
        {
            if (phones.Length==0) 
            {
                Console.WriteLine("Таких телефонов нет");
                return 1;
            }
            foreach (Phone phone in phones)
            {
                Console.WriteLine($"{phone.PhoneNumber} {phone.Year} {phone.PhoneProvider} {phone.UserName}");
            }
            return 0;
        }
    }

    class Program 
    {
        static void Main()
        {
            Menu menu = new Menu();
            Phone phone1 = new Phone("123", "1", "asad", "sadsad");
            Phone phone2 = new Phone("123213", "1", "aaa", "asd");
            Phone phone3 = new Phone("213213", "3", "aaa", "sadsadsadsa");
            menu.Phones.Add(phone1);
            menu.Phones.Add(phone2);
            menu.Phones.Add(phone3);
            menu.Run();
        }
    }
}