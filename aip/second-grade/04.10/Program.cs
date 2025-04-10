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
            if (count == list.Length) Array.Resize(ref list, list.Length+1);
            list[count] = item;
            count++;
        }

        public bool Remove(int index){
            if (index < 0 || index >= count){
                Console.WriteLine("Обка индекса");
                return false;
            }
            for (int i = index;i < count; i++){
                list[i] = list[i+1];
            }
            count--;
            return true;
        }

        public T Find(int index){
            if (index < 0 || index >= count){
                Console.WriteLine("Ошибка индекса");
                return default;
            }
            return list[index];
        }
        
    }
    class Program{
        static void Main(string[] args){
            var mylist1 = new CustomList<int>();
            mylist1.Add(3);
            mylist1.Add(5);
            mylist1.Add(7);
            Console.WriteLine(mylist1.Find(2));
            Console.WriteLine(mylist1.Find(1));
            var mylist2 = new CustomList<string>();
            mylist2.Add("aadsa");
            mylist2.Add("sadsadsa");
            mylist2.Add("sadsaru76434werdgf");
            Console.WriteLine(mylist2.Find(0));
            Console.WriteLine(mylist2.Find(2));  
    
            Console.WriteLine(mylist1.Find(10)); 
            Console.WriteLine(mylist2.Find(3));
        
        }
    }
}
