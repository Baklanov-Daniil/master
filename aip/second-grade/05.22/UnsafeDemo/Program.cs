// Работа с указателями, выделение памяти стэкалок, кол-во элементов либо вводим либо задаём сами, инициализация сразу в программе, либо считывать с клавы
// Необходимо создать массив заданной длины, выделив память, в массиве элементы целые и необходимо выдать только те элементы, которые являются полиндромами

// на входе текст из нескольких строк, необходимо сформировать массив, который хранит частоту появления каждого символа
// symbolsCount символов в таблице
using System;
using System.Runtime.InteropServices;

namespace aip
{
    unsafe class Program
    {
        static void Task1()
        {
            static bool IsPalindrome(int number)
            {
                if (number < 0) return false;
                int reversed = 0;
                int original = number;
                while (number > 0)
                {
                    reversed = reversed * 10 + number % 10;
                    number /= 10;
                }
                return original == reversed;
            }

            int count = 3;
            int* array = stackalloc int[count];
            int* ptr = array;
            for (int i = 0; i < count; i++)
            {
                *ptr = int.Parse(Console.ReadLine());
                ptr++; 
            }
            Console.WriteLine("Палиндромы в массиве:");
            ptr = array;
            for (int i = 0; i < count; i++)
                {
                    if (IsPalindrome(*ptr))
                    {
                        Console.WriteLine($"Найден палиндром: {*ptr}");
                    }
                    ptr++;
                }
        }

        static void Task2()
        {
            int symbolsCount = 256;
            int* asciiCount = stackalloc int[symbolsCount];
            for (int* ptr = asciiCount; *ptr < symbolsCount; ptr++) *ptr = 0;
            string line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                fixed (char* pLine = line)
                {
                    char* pChar = pLine;
                    while (*pChar != '\0')
                    {
                        if (*pChar < symbolsCount)
                            asciiCount[*pChar]++;
                        pChar++;
                    }
                }
            }

            Console.WriteLine("Частота символов:");
            for (int i = 0; i < symbolsCount; i++)
            {
                int* ptr = asciiCount + i;
                if (*ptr > 0)
                {
                    Console.WriteLine($"Символ {(char)i}: {*ptr} раз");
                }
            }
        }
        
        
        static void Main(string[] args)
        {
            // Task1();
            Task2();
        }
    }
}

