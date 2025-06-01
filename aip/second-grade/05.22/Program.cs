// Задание 1
// Работа с указателями, выделение памяти стэкалок, кол-во элементов либо вводим либо задаём сами, инициализация сразу в программе, либо считывать с клавиатуры
// Необходимо создать массив заданной длины, выделив память, в массиве элементы целые. Необходимо выдать только те элементы, которые являются полиндромами

// Задание 2
// на входе текст из нескольких строк, необходимо сформировать массив, который хранит частоту появления каждого символа
// symbolsCount символов в таблице

using System;

namespace aip
{
    class Program
    {
        unsafe static void Task1()
        {
            int count = 3;
            Console.WriteLine($"Введите {count} чисел");
            int* array = stackalloc int[count];

            for (int* ptr = array; ptr < array + count; ptr++)
            {
                *ptr = int.Parse(Console.ReadLine());
            }

            Console.WriteLine("Палиндромы:");
            for (int* ptr = array; ptr < array + count; ptr++)
            {
                int num = *ptr;
                int reversed = 0;
                int original = num;
                
                while (num > 0)
                {
                    reversed = reversed * 10 + num % 10;
                    num /= 10;
                }

                if (original == reversed)
                {
                    Console.WriteLine(original);
                }
            }
        }

        unsafe static void Task2()
        {
            const int symbolsCount = 256;
            int* asciiCount = stackalloc int[symbolsCount];
            
            for (int* ptr = asciiCount; ptr < asciiCount + symbolsCount; ptr++)
            {
                *ptr = 0;
            }

            Console.WriteLine("Введите текст (для завершения оставьте строку пустой):");
            string line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                fixed (char* pLine = line)
                {
                    for (char* pChar = pLine; *pChar != '\0'; pChar++)
                    {
                        asciiCount[*pChar]++;
                    }
                }
            }

            Console.WriteLine("Частота символов:");
            for (int i = 0; i < symbolsCount; i++)
            {
                if (asciiCount[i] > 0)
                {
                    Console.WriteLine($"Символ '{(char)i}': {asciiCount[i]} раз");
                }
            }
        }

        static void Main(string[] args)
        {
            int task = 2;
            switch (task)
            {
                case 1:
                    Task1();
                    break;
                case 2:
                    Task2();
                    break;
            }
        }
        
    }
}

