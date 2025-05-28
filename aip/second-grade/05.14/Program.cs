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
    public class FirstTable
    {
        public List<int> product_number = new List<int>();
        public List<string> product_name = new List<string>();


        public void InsertData(List<int> product_number, List<string> product_name)
        {
            this.product_number = product_number;
            this.product_name = product_name;
        }
    }

    public class SecondTable
    {
        public List<int> supplier_number = new List<int>();
        public List<string> supplier_name = new List<string>();

        public List<string> supplier_phone = new List<string>();
        public void InsertData(List<int> supplier_number, List<string> supplier_name, List<string> supplier_phone)
        {
            this.supplier_number = supplier_number;
            this.supplier_name = supplier_name;
            this.supplier_phone = supplier_phone;
        }
    }

    public class ThirdTable
    {
        public List<int> supplier_number = new List<int>();
        public List<int> product_number = new List<int>();
        public List<DateTime> supply_date = new List<DateTime>();
        public List<int> quality = new List<int>();
        public List<int> price = new List<int>();

        public void InsertData(List<int> supplier_number, List<int> product_number, List<DateTime> supply_date, List<int> quality, List<int> price)
        {
            this.supplier_number = supplier_number;
            this.product_number = product_number;
            this.supply_date = supply_date;
            this.quality = quality;
            this.price = price;
        }
    }

    public class Menu
    {
        public FirstTable firstTable = new FirstTable();
        public SecondTable secondTable = new SecondTable();
        public ThirdTable thirdTable = new ThirdTable();

        public void FillData() // тестовые данные
        {
            this.firstTable.InsertData(
                new List<int> { 1, 2, 3, 4, 5 },
                new List<string> { "Ноутбук", "Монитор", "Клавиатура", "Мышь", "Наушники" }
            );

            this.secondTable.InsertData(
                new List<int> { 101, 102, 103, 104 },
                new List<string> { "TechCorp", "ElectroPlus", "GadgetWorld", "CompMaster" },
                new List<string> { "+79001112233", "+79002223344", "+79003334455", "+79004445566" }
            );

            this.thirdTable.InsertData(
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

        public void GetTheMostExpensiveSupplier()
        {
            var query = thirdTable.supplier_number
                        .Select((supplierNum, index) => new
                        {
                            SupplierNumber = supplierNum,
                            ProductNumber = thirdTable.product_number[index],
                            Quantity = thirdTable.quality[index],
                            Price = thirdTable.price[index],
                            Total = thirdTable.quality[index] * thirdTable.price[index]
                        })
                        .GroupBy(x => x.SupplierNumber)
                        .Select(g => new
                        {
                            SupplierNumber = g.Key,
                            TotalSum = g.Sum(x => x.Total),
                            SupplierName = secondTable.supplier_name[secondTable.supplier_number.IndexOf(g.Key)]
                        })
                        .OrderByDescending(x => x.TotalSum)
                        .FirstOrDefault();

            if (query != null)
            {
                Console.WriteLine($"Поставщик с наибольшей суммой поставок:");
                Console.WriteLine($"Номер: {query.SupplierNumber}");
                Console.WriteLine($"Название: {query.SupplierName}");
                Console.WriteLine($"Сумма поставок: {query.TotalSum} руб.");
            }
            else
            {
                Console.WriteLine("Нет данных о поставках.");
            }
        }

        public class Program
        {
            static void Main(string[] args)
            {
                Menu menu = new Menu();
                menu.FillData();
                menu.GetTheMostExpensiveSupplier();
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
