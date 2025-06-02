using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolLaptops
{
    // ======================== Классы для первого задания (ноутбуки в школе) ========================
    public class OperatingSystems
    {
        public List<int> osIds = new List<int>();
        public List<string> osNames = new List<string>();

        public void InsertData(List<int> osIds, List<string> osNames)
        {
            this.osIds = osIds;
            this.osNames = osNames;
        }
    }

    public class LaptopBrands
    {
        public List<int> brandIds = new List<int>();
        public List<string> brandNames = new List<string>();

        public void InsertData(List<int> brandIds, List<string> brandNames)
        {
            this.brandIds = brandIds;
            this.brandNames = brandNames;
        }
    }

    public class Students
    {
        public List<int> studentIds = new List<int>();
        public List<string> classes = new List<string>();
        public List<string> fullNames = new List<string>();
        public List<bool> hasLaptop = new List<bool>();
        public List<int> osIds = new List<int>();
        public List<int> brandIds = new List<int>();

        public void InsertData(
            List<int> studentIds, 
            List<string> classes, 
            List<string> fullNames, 
            List<bool> hasLaptop, 
            List<int> osIds, 
            List<int> brandIds)
        {
            this.studentIds = studentIds;
            this.classes = classes;
            this.fullNames = fullNames;
            this.hasLaptop = hasLaptop;
            this.osIds = osIds;
            this.brandIds = brandIds;
        }
    }
    
    // ======================== Классы для второго задания (автосервис) ========================
    public class CarBrands
    {
        public List<int> brandIds = new List<int>();
        public List<string> brandNames = new List<string>();

        public void InsertData(List<int> brandIds, List<string> brandNames)
        {
            this.brandIds = brandIds;
            this.brandNames = brandNames;
        }
    }

    public class Services
    {
        public List<int> serviceIds = new List<int>();
        public List<string> serviceNames = new List<string>();
        public List<int> servicePrices = new List<int>();

        public void InsertData(List<int> serviceIds, List<string> serviceNames, List<int> servicePrices)
        {
            this.serviceIds = serviceIds;
            this.serviceNames = serviceNames;
            this.servicePrices = servicePrices;
        }
    }

    public class Clients
    {
        public List<int> clientIds = new List<int>();
        public List<string> clientNames = new List<string>();
        public List<int> carBrandIds = new List<int>();

        public void InsertData(List<int> clientIds, List<string> clientNames, List<int> carBrandIds)
        {
            this.clientIds = clientIds;
            this.clientNames = clientNames;
            this.carBrandIds = carBrandIds;
        }
    }

    public class Visits
    {
        public List<int> visitIds = new List<int>();
        public List<int> clientIds = new List<int>();
        public List<DateTime> visitDates = new List<DateTime>();
        public List<int> serviceIds = new List<int>();

        public void InsertData(List<int> visitIds, List<int> clientIds, List<DateTime> visitDates, List<int> serviceIds)
        {
            this.visitIds = visitIds;
            this.clientIds = clientIds;
            this.visitDates = visitDates;
            this.serviceIds = serviceIds;
        }
    }

    // ======================== Меню и функции для первого задания (ноутбуки в школе) ========================

    public class MenuTask1
    {
        public OperatingSystems operatingSystems = new OperatingSystems();
        public LaptopBrands laptopBrands = new LaptopBrands();
        public Students students = new Students();

        public void FillData()
        {
            this.operatingSystems.InsertData(
                new List<int> { 1, 2, 3 },
                new List<string> { "Windows", "macOS", "Linux" }
            );

            this.laptopBrands.InsertData(
                new List<int> { 101, 102, 103, 104 },
                new List<string> { "Apple", "Dell", "HP", "Lenovo" }
            );

            this.students.InsertData(
                new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 },
                new List<string> { "10A", "10A", "10B", "10B", "11A", "11A", "11B", "11B" },
                new List<string> { "Иванов Иван", "Петров Петр", "Сидоров Сидор", "Кузнецова Анна",
                                 "Смирнов Алексей", "Федорова Мария", "Николаев Дмитрий", "Павлова Елена" },
                new List<bool> { true, false, true, true, false, true, true, false },
                new List<int> { 1, 0, 2, 1, 0, 3, 1, 0 },
                new List<int> { 101, 0, 102, 103, 0, 104, 102, 0 }
            );
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== МЕНЮ ШКОЛЬНЫЕ НОУТБУКИ ===");
                Console.WriteLine("1. Список учеников с ноутбуками");
                Console.WriteLine("2. Ученики по классам (сортировка по марке ноутбука)");
                Console.WriteLine("3. Поиск учеников по марке ноутбука");
                Console.WriteLine("4. Группировка учеников по ОС");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.Clear();
                    switch (choice)
                    {
                        case 1:
                            GetStudentsWithLaptops();
                            break;
                        case 2:
                            GetStudentsGroupedByClass();
                            break;
                        case 3:
                            GetStudentsByBrand();
                            break;
                        case 4:
                            GetStudentsGroupedByOS();
                            break;
                        case 0:
                            return;
                        default:
                            Console.WriteLine("Неверный выбор!");
                            break;
                    }
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        private void GetStudentsWithLaptops()
        {
            var studentItems = students.studentIds
                .Select((id, index) => new
                {
                    id,
                    className = students.classes[index],
                    fullName = students.fullNames[index],
                    hasLaptop = students.hasLaptop[index],
                    osId = students.osIds[index],
                    brandId = students.brandIds[index]
                })
                .Where(s => s.hasLaptop);

            var result = (
                from s in studentItems
                join os in operatingSystems.osIds.Select((id, idx) => new { id, osName = operatingSystems.osNames[idx] })
                    on s.osId equals os.id
                join brand in laptopBrands.brandIds.Select((id, idx) => new { id, brandName = laptopBrands.brandNames[idx] })
                    on s.brandId equals brand.id
                select new
                {
                    s.id,
                    s.className,
                    s.fullName,
                    os.osName,
                    brand.brandName
                }
            );

            Console.WriteLine("=== Ученики с ноутбуками ===");
            foreach (var student in result)
            {
                Console.WriteLine($"{student.fullName} ({student.className})");
                Console.WriteLine($"  Ноутбук: {student.brandName} с ОС {student.osName}\n");
            }
        }

        private void GetStudentsGroupedByClass()
        {
            var studentItems = students.studentIds
                .Select((id, index) => new
                {
                    id,
                    className = students.classes[index],
                    fullName = students.fullNames[index],
                    hasLaptop = students.hasLaptop[index],
                    brandId = students.brandIds[index]
                })
                .Where(s => s.hasLaptop);

            var result = (
                from s in studentItems
                join brand in laptopBrands.brandIds.Select((id, idx) => new { id, brandName = laptopBrands.brandNames[idx] })
                    on s.brandId equals brand.id
                group new { s.fullName, brand.brandName } by s.className into grouped
                orderby grouped.Key
                select new
                {
                    Class = grouped.Key,
                    Students = grouped.OrderBy(x => x.brandName)
                }
            );

            Console.WriteLine("=== Ученики по классам (сортировка по марке ноутбука) ===");
            foreach (var group in result)
            {
                Console.WriteLine($"\nКласс: {group.Class}");
                foreach (var student in group.Students)
                {
                    Console.WriteLine($"  {student.fullName} ({student.brandName})");
                }
            }
        }

        private void GetStudentsByBrand()
        {
            Console.Write("Введите марку ноутбука для поиска: ");
            string searchBrand = Console.ReadLine();

            var studentItems = students.studentIds
                .Select((id, index) => new
                {
                    id,
                    className = students.classes[index],
                    fullName = students.fullNames[index],
                    hasLaptop = students.hasLaptop[index],
                    brandId = students.brandIds[index]
                })
                .Where(s => s.hasLaptop);

            var result = (
                from s in studentItems
                join brand in laptopBrands.brandIds.Select((id, idx) => new { id, brandName = laptopBrands.brandNames[idx] })
                    on s.brandId equals brand.id
                where brand.brandName.Equals(searchBrand, StringComparison.OrdinalIgnoreCase)
                select new
                {
                    s.fullName,
                    s.className,
                    brand.brandName
                }
            );

            Console.WriteLine($"\n=== Ученики с ноутбуками {searchBrand} ===");
            foreach (var student in result)
            {
                Console.WriteLine($"{student.fullName} ({student.className})");
            }
        }

        private void GetStudentsGroupedByOS()
        {
            var studentItems = students.studentIds
                .Select((id, index) => new
                {
                    id,
                    className = students.classes[index],
                    fullName = students.fullNames[index],
                    hasLaptop = students.hasLaptop[index],
                    osId = students.osIds[index]
                })
                .Where(s => s.hasLaptop);

            var result = (
                from s in studentItems
                join os in operatingSystems.osIds.Select((id, idx) => new { id, osName = operatingSystems.osNames[idx] })
                    on s.osId equals os.id
                group new { s.fullName, s.className } by os.osName into grouped
                select new
                {
                    OS = grouped.Key,
                    Students = grouped
                }
            );

            Console.WriteLine("=== Ученики по типам ОС ===");
            foreach (var group in result)
            {
                Console.WriteLine($"\nОС: {group.OS}");
                foreach (var student in group.Students)
                {
                    Console.WriteLine($"  {student.fullName} ({student.className})");
                }
            }
        }
    }

    // ======================== Меню и функции для второго задания (автосервис) ========================

    public class MenuTas2
    {
        public CarBrands carBrands = new CarBrands();
        public Services services = new Services();
        public Clients clients = new Clients();
        public Visits visits = new Visits();

        public void FillData()
        {
            this.carBrands.InsertData(
                new List<int> { 1, 2, 3, 4 },
                new List<string> { "Toyota", "BMW", "Audi", "Mercedes" }
            );

            this.services.InsertData(
                new List<int> { 101, 102, 103, 104 },
                new List<string> { "Замена масла", "Ремонт двигателя", "Покраска", "Замена тормозов" },
                new List<int> { 3000, 15000, 20000, 8000 }
            );

            this.clients.InsertData(
                new List<int> { 1, 2, 3, 4, 5, 6 },
                new List<string> { "Иванов И.И.", "Петров П.П.", "Сидоров С.С.", "Кузнецова А.А.", "Смирнов А.А.", "Федорова М.М." },
                new List<int> { 1, 2, 1, 3, 4, 2 }
            );

            this.visits.InsertData(
                new List<int> { 1001, 1002, 1003, 1004, 1005, 1006, 1007 },
                new List<int> { 1, 2, 3, 4, 5, 1, 2 },
                new List<DateTime>
                {
                    new DateTime(2023, 1, 10),
                    new DateTime(2023, 1, 15),
                    new DateTime(2023, 2, 5),
                    new DateTime(2023, 2, 20),
                    new DateTime(2023, 3, 1),
                    new DateTime(2023, 3, 10),
                    new DateTime(2023, 3, 15)
                },
                new List<int> { 101, 102, 103, 104, 101, 102, 103 }
            );
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== МЕНЮ АВТОСЕРВИС ===");
                Console.WriteLine("1. Клиенты по маркам авто");
                Console.WriteLine("2. Визиты по датам");
                Console.WriteLine("3. Сумма услуг за период");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.Clear();
                    switch (choice)
                    {
                        case 1:
                            GetClientsByCarBrand();
                            break;
                        case 2:
                            GetVisitsByDate();
                            break;
                        case 3:
                            GetServicesTotalByPeriod();
                            break;
                        case 0:
                            return;
                        default:
                            Console.WriteLine("Неверный выбор!");
                            break;
                    }
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        private void GetClientsByCarBrand()
        {
            var clientItems = clients.clientIds
                .Select((id, index) => new
                {
                    id,
                    name = clients.clientNames[index],
                    brandId = clients.carBrandIds[index]
                });

            var result = (
                from c in clientItems
                join brand in carBrands.brandIds.Select((id, idx) => new { id, brandName = carBrands.brandNames[idx] })
                    on c.brandId equals brand.id
                group c by brand.brandName into grouped
                orderby grouped.Key
                select grouped
            );

            Console.WriteLine("=== Клиенты по маркам авто ===");
            foreach (var group in result)
            {
                Console.WriteLine($"\nМарка: {group.Key}");
                foreach (var client in group)
                {
                    Console.WriteLine($"  {client.name}");
                }
            }
        }

        private void GetVisitsByDate()
        {
            var visitItems = visits.visitDates
                .Select((date, index) => new
                {
                    date,
                    clientId = visits.clientIds[index],
                    serviceId = visits.serviceIds[index]
                });

            var result = (
                from v in visitItems
                join client in clients.clientIds.Select((id, idx) => new { id, clientName = clients.clientNames[idx] })
                    on v.clientId equals client.id
                join service in services.serviceIds.Select((id, idx) => new { id, serviceName = services.serviceNames[idx] })
                    on v.serviceId equals service.id
                group new { client.clientName, service.serviceName } by v.date into grouped
                orderby grouped.Key
                select new
                {
                    Date = grouped.Key,
                    Visits = grouped.OrderBy(x => x.serviceName)
                }
            );

            Console.WriteLine("=== Визиты по датам ===");
            foreach (var group in result)
            {
                Console.WriteLine($"\nДата: {group.Date:d}");
                foreach (var visit in group.Visits)
                {
                    Console.WriteLine($"  {visit.clientName} - {visit.serviceName}");
                }
            }
        }

        private void GetServicesTotalByPeriod()
        {
            Console.Write("Введите начальную дату (гггг-мм-дд): ");
            DateTime startDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Введите конечную дату (гггг-мм-дд): ");
            DateTime endDate = DateTime.Parse(Console.ReadLine());

            var visitItems = visits.visitDates
                .Select((date, index) => new
                {
                    date,
                    serviceId = visits.serviceIds[index]
                })
                .Where(v => v.date >= startDate && v.date <= endDate);

            var result = (
                from v in visitItems
                join service in services.serviceIds.Select((id, idx) => new { id, price = services.servicePrices[idx] })
                    on v.serviceId equals service.id
                group service.price by 1 into g
                select new
                {
                    Total = g.Sum()
                }
            ).FirstOrDefault();

            Console.WriteLine($"\n=== Сумма услуг за период {startDate:d} - {endDate:d} ===");
            Console.WriteLine($"Итого: {result?.Total ?? 0} руб.");
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            int task = 1;
            if (task == 1)
            {
                MenuTask1 menu = new MenuTask1();
                menu.FillData();
                menu.ShowMenu();
            }
            else
            {
                MenuTas2 menu = new MenuTas2();
                menu.FillData();
                menu.ShowMenu();
            }
        }
    }
}