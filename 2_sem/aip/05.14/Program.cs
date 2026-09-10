// базза данных, состоящая из 3 таблиц (3 класса)
// 1 таблица - порядковый номер товара, наименование товара
// 2 таблица - номер поставщика, наименование поставщика, контактный телефон
// 3 таблица - отображает движение товара и хранит: номер поставщика, номер товара, дата поставки, количество, цена
// база данных целостная (не рассматриваем несуществующий товар, и не поставляет несуществующий поставщик) - данные заданы корректно
// используя меню и LINQ необходимо выполнить запросы:
// 1) выдать поставщика, который поставил товар на большую сумму
// 2) выдать поставки товаров, сгруперовыннх по датам ( которого нет списка,выдавать наименования)
// 3) выдать товары, сгруппированные по поставщику и отсортированные по количеству (наименование товар и название поставщика)
// 4) определить товар который поставлялся чаще всего 
// 5) выдать сумму поставок, сгруперовыннх по поставщику
using System;
using System.Collections.Generic;
using System.Linq;

namespace aip
{
    public class Products
    {
        public List<int> productIds = new List<int>();
        public List<string> productNames = new List<string>();

        public void InsertData(List<int> productIds, List<string> productNames)
        {
            this.productIds = productIds;
            this.productNames = productNames;
        }
    }

    public class Suppliers
    {
        public List<int> supplierNumbers = new List<int>();
        public List<string> supplierNames = new List<string>();
        public List<string> supplierPhones = new List<string>();

        public void InsertData(List<int> supplierNumbers, List<string> supplierNames, List<string> supplierPhones)
        {
            this.supplierNumbers = supplierNumbers;
            this.supplierNames = supplierNames;
            this.supplierPhones = supplierPhones;
        }
    }

    public class Transactions
    {
        public List<int> supplierNumbers = new List<int>();
        public List<int> productIds = new List<int>();
        public List<DateTime> supplyDates = new List<DateTime>();
        public List<int> qualities = new List<int>();
        public List<int> prices = new List<int>();

        public void InsertData(List<int> supplierNumbers, List<int> productIds, List<DateTime> supplyDates, List<int> qualities, List<int> prices)
        {
            this.supplierNumbers = supplierNumbers;
            this.productIds = productIds;
            this.supplyDates = supplyDates;
            this.qualities = qualities;
            this.prices = prices;
        }
    }

    public class Menu
    {
        public Products products = new Products();
        public Suppliers suppliers = new Suppliers();
        public Transactions transactions = new Transactions();

        public void FillData()
        {
            this.products.InsertData(
                new List<int> { 1, 2, 3, 4, 5 },
                new List<string> { "Ноутбук", "Монитор", "Клавиатура", "Мышь", "Наушники" }
            );

            this.suppliers.InsertData(
                new List<int> { 101, 102, 103, 104 },
                new List<string> { "TechCorp", "ElectroPlus", "GadgetWorld", "CompMaster" },
                new List<string> { "+79001112233", "+79002223344", "+79003334455", "+79004445566" }
            );

            this.transactions.InsertData(
                new List<int> { 101, 102, 101, 103, 104, 102, 101 },
                new List<int> { 1, 2, 3, 4, 5, 1, 2 },
                new List<DateTime>
                {
                    new DateTime(2024, 3, 15),
                    new DateTime(2024, 3, 16),
                    new DateTime(2024, 3, 17),
                    new DateTime(2024, 3, 18),
                    new DateTime(2024, 3, 19),
                    new DateTime(2024, 3, 20),
                    new DateTime(2024, 3, 21)
                },
                new List<int> { 10, 5, 20, 15, 8, 12, 7 },
                new List<int> { 50000, 25000, 3000, 1500, 8000, 52000, 26000 }
            );
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== МЕНЮ ===");
                Console.WriteLine("1. Поставщик с наибольшей суммой поставок");
                Console.WriteLine("2. Поставки по датам");
                Console.WriteLine("3. Товары по поставщикам");
                Console.WriteLine("4. Самый часто поставляемый товар");
                Console.WriteLine("5. Сумма поставок по поставщикам");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");
                int choice = int.Parse(Console.ReadLine());
                Console.Clear();
                switch (choice)
                {
                    case 1:
                        GetTheMostExpensiveSupplier();
                        break;
                    case 2:
                        GetSuppliesGroupedByDate();
                        break;
                    case 3:
                        GetProductsGroupedBySupplier();
                        break;
                    case 4:
                        GetMostFrequentlySuppliedProduct();
                        break;
                    case 5:
                        GetTotalSuppliesBySupplier();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
                Console.ReadKey();
            }
        }

        public void GetTheMostExpensiveSupplier()
        {
            var transactionItems = transactions.supplierNumbers
                .Select((supplierNum, index) => new
                {
                    supplierId = supplierNum,
                    quantity = transactions.qualities[index],
                    price = transactions.prices[index]
                });

            var supplierItems = suppliers.supplierNumbers
                .Select((supplierNum, index) => new
                {
                    id = supplierNum,
                    name = suppliers.supplierNames[index],
                    phone = suppliers.supplierPhones[index]
                });

            var suppliersWithTotals = (
                from t in transactionItems
                join s in supplierItems on t.supplierId equals s.id
                group t by s into grouped
                let total = grouped.Sum(i => i.quantity * i.price)
                orderby total descending
                select new
                {
                    supplier = grouped.Key,
                    total
                }
            ).ToList();
            var maxTotal = suppliersWithTotals.First().total;
            var topSuppliers = suppliersWithTotals.Where(x => x.total == maxTotal);

            Console.WriteLine("=== Поставщики с наибольшей суммой поставок ===");
            Console.WriteLine($"Максимальная сумма: {maxTotal} руб.");
            Console.WriteLine($"Количество поставщиков: {topSuppliers.Count()}\n");

            foreach (var result in topSuppliers)
            {
                Console.WriteLine($"Номер: {result.supplier.id}");
                Console.WriteLine($"Название: {result.supplier.name}");
                Console.WriteLine($"Телефон: {result.supplier.phone}");
                Console.WriteLine($"Сумма поставок: {result.total} руб.\n");
            }
        }

        public void GetSuppliesGroupedByDate()
        {
            var transactionItems = transactions.supplyDates
                .Select((date, index) => new
                {
                    date,
                    productId = transactions.productIds[index],
                    quantity = transactions.qualities[index],
                    price = transactions.prices[index]
                });

            var productItems = products.productIds
                .Select((id, index) => new
                {
                    id,
                    name = products.productNames[index]
                });

            var result = (
                from t in transactionItems
                join p in productItems on t.productId equals p.id
                group new { p.name, t.quantity, t.price } by t.date into grouped
                orderby grouped.Key
                select grouped
            );

            Console.WriteLine("\nПоставки по датам:");
            foreach (var group in result)
            {
                Console.WriteLine($"\nДата: {group.Key:d}");
                foreach (var item in group)
                {
                    Console.WriteLine($"  {item.name}: {item.quantity} шт. по {item.price} руб.");
                }
            }
        }

        public void GetProductsGroupedBySupplier()
        {
            var transactionItems = transactions.supplierNumbers
                .Select((supplierNum, index) => new
                {
                    supplierId = supplierNum,
                    productId = transactions.productIds[index],
                    quantity = transactions.qualities[index]
                });

            var supplierItems = suppliers.supplierNumbers
                .Select((supplierNum, index) => new
                {
                    id = supplierNum,
                    name = suppliers.supplierNames[index]
                });

            var productItems = products.productIds
                .Select((id, index) => new
                {
                    id,
                    name = products.productNames[index]
                });

            var result = (
                from t in transactionItems
                join s in supplierItems on t.supplierId equals s.id
                join p in productItems on t.productId equals p.id
                group new { p.name, t.quantity } by s.name into grouped
                select new
                {
                    supplier = grouped.Key,
                    products = grouped.OrderByDescending(x => x.quantity)
                }
            );

            Console.WriteLine("\nТовары по поставщикам:");
            foreach (var group in result)
            {
                Console.WriteLine($"\nПоставщик: {group.supplier}");
                foreach (var product in group.products)
                {
                    Console.WriteLine($"  {product.name}: {product.quantity} шт.");
                }
            }
        }

        public void GetMostFrequentlySuppliedProduct()
        {
            var result = transactions.productIds
                .GroupBy(id => id)
                .Select(g => new
                {
                    productId = g.Key,
                    count = g.Count()
                })
                .OrderByDescending(x => x.count)
                .FirstOrDefault();

            if (result != null)
            {
                var productName = products.productNames[products.productIds.IndexOf(result.productId)];
                Console.WriteLine($"\nСамый часто поставляемый товар: {productName} ({result.count} поставок)");
            }
        }

        public void GetTotalSuppliesBySupplier()
        {
            var transactionItems = transactions.supplierNumbers
                .Select((supplierNum, index) => new
                {
                    supplierId = supplierNum,
                    quantity = transactions.qualities[index],
                    price = transactions.prices[index]
                });

            var supplierItems = suppliers.supplierNumbers
                .Select((supplierNum, index) => new
                {
                    id = supplierNum,
                    name = suppliers.supplierNames[index]
                });

            var result = (
                from t in transactionItems
                join s in supplierItems on t.supplierId equals s.id
                group t by s.name into grouped
                select new
                {
                    supplier = grouped.Key,
                    total = grouped.Sum(x => x.quantity * x.price)
                }
            );

            Console.WriteLine("\nСумма поставок по поставщикам:");
            foreach (var item in result)
            {
                Console.WriteLine($"  {item.supplier}: {item.total} руб.");
            }
        }

        public class Program
        {
            static void Main(string[] args)
            {
                Menu menu = new Menu();
                menu.FillData();
            menu.ShowMenu();
            }
        }
    }
}

// задание на субботу
// 1 задание) имеется база данных из 3 таблиц:
// 1 - порядковый номер, наименование ос
// 2 - порядковый номер, наименование марки ноутбука
// 3 - порядковый номре ученика, класс ученика, фио, есть ноутбук?, номер операциооной система, номер марки ноутбука. Могут быть нулевые поля
// сделать меню и запросы:
// 1) выдать список учеников, которые имеют ноутбук
// 2) выдать список учеников, сгрупированных по классу, отсортерованные по марке ноутбука
// 3) выборка всех учеников, у которых марка ноутбука соотвествует заданному
// 4) группировка учеников по типу ос

// 2 задание) имеется база данных из нексольких (количество надо определить самому) таблиц (нужно распределить данные самому, чтобы не было повторений):
// 1) выдать всех пользователей, сгруппированные по марке машины
// 2) выдать пользователей сгруппированных по дате обращения в автосорвис отсортированных по услуе
// 3) выдать на какую сумму оказаны услуги в заданный интервал времени
