// Необходимо создать обобщенный класс для работы с массивом любого типа данных. В классе будут реализованы метода добавления элемента (в конец), 
// удаление элемента по индексу и вывод значения элемента по индексу. В удалении и в выводе элемента проследить чтоб не выйти за пределами

using System;
namespace aip{
    public class CustomList<T>{
        public T[] list;
        public int count{get; set;}
        public CustomList(int capacity = 1){
            list = new T[capacity];
            count = 0;
        }

        public void Add(T item){
            if (count == list.Length) Array.Resize(ref list, list.Length*2);
            list[count] = item;
            count++;
        }

        public bool Remove(int index){
            if (index < 0 || index >= count){
                Console.WriteLine("Ошибка индекса");
                return false;
            }
            for (int i = index; i < count-1; i++){
                list[i] = list[i+1];
            }
            count--;
            return true;
        }

        public T Find(int index){
            if (index < 0 || index >= count){
                Console.Write("Ошибка индекса ");
                return default;
            }
            return list[index];
        }
        
    }
    class Program{
        static void Main(string[] args)
        {
            var intList = new CustomList<int>();
            intList.Add(10);
            intList.Add(20);
            intList.Add(30);
            Console.WriteLine(intList.Find(0));
            Console.WriteLine(intList.Find(1));
            Console.WriteLine(intList.Find(2));
            Console.WriteLine(intList.Find(-1));
            Console.WriteLine(intList.Find(3));
            intList.Remove(1);
            Console.WriteLine(intList.Find(1));
            intList.Remove(5);
            Console.WriteLine(intList.count);
            
            var stringList = new CustomList<string>();
            stringList.Add("Hello");
            stringList.Add("World");
            stringList.Add("!");
            Console.WriteLine(stringList.Find(0));
            Console.WriteLine(stringList.Find(1));
            Console.WriteLine(stringList.Find(2));
            Console.WriteLine(stringList.Find(-1));
            Console.WriteLine(stringList.Find(3));
            stringList.Remove(0);
            Console.WriteLine(stringList.Find(0)); 
            stringList.Remove(5);
            Console.WriteLine(stringList.count);
        }
    }
}
