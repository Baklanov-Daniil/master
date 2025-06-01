// на вход объекты со следующей структурой:
// телефонный номер объекта и оператор. Необходимо построить бинарное дерево по следующему принципу:
// левая ветвь дерева значение телефона, меньшего чем узел. Правая ветвь - значение телефона большего, чем узел
// элемент дерева - структура с 4 полями: id, оператор, указатель на лево, указатель на право
using System;
using System.Runtime.InteropServices;
namespace aip
{
    unsafe class Functions
    {
        public static void ChoiceBranch(Phone* phone1, Phone* phone2)
        {
            if (phone2->Id < phone1->Id)
            {
                if (phone1->LeftBranch == null)
                    phone1->LeftBranch = phone2;
                else
                    ChoiceBranch(phone1->LeftBranch, phone2);
            }
            else
            {
                if (phone1->RightBranch == null)
                    phone1->RightBranch = phone2;
                else
                    ChoiceBranch(phone1->RightBranch, phone2);
            }
        }

        public static void PrintTree(Phone* phone)
        {
            if (phone != null)
            {
                if (phone->LeftBranch != null)
                    PrintTree(phone->LeftBranch);
                
                Console.WriteLine($"ID: {phone->Id}, Operator: {new string(phone->Operator)}");
                
                if (phone->RightBranch != null)
                    PrintTree(phone->RightBranch);
            }
        }
    }

    unsafe struct Phone
    {
        public int Id;
        public fixed char Operator[32];
        public Phone* LeftBranch;
        public Phone* RightBranch;

        public Phone(int id, string op)
        {
            Id = id;
            LeftBranch = null;
            RightBranch = null;
            
            fixed (char* dest = Operator)
            {
                for (int i = 0; i < op.Length && i < 31; i++)
                    dest[i] = op[i];
                dest[op.Length < 32 ? op.Length : 31] = '\0';
            }
        }
    }

    class Program
    {
        unsafe static void Main()
        {
            Console.Clear();

            Phone* nodes = stackalloc Phone[5];

            nodes[0] = new Phone(26, "A");
            nodes[1] = new Phone(28, "B");
            nodes[2] = new Phone(16, "C");
            nodes[3] = new Phone(14, "D");
            nodes[4] = new Phone(31, "E");

            Phone* head = &nodes[0];

            Functions.ChoiceBranch(head, &nodes[1]);
            Functions.ChoiceBranch(head, &nodes[2]);
            Functions.ChoiceBranch(head, &nodes[3]);
            Functions.ChoiceBranch(head, &nodes[4]);

            Console.WriteLine("Бинарное дерево:");
            Functions.PrintTree(head);
        }
    }
}
