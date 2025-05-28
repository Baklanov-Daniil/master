// база данных - библиотека, в которой хранится информация о книге, информация представлена в виде структуры и содержит
// ФИО автора, наименование книги, год издания, наименование издательства.
// Помимо этого когда выдали и когда сдали. Необходимо сделать выборку книг, которые не выдавались совсем и тех книг, которые на руках

using System;
using System.Collections;

namespace aip
{
    class Library
    {
        public List<Book> library = new List<Book>();
        public List<Book> give_books = new List<Book>();
        public List<Book> return_books = new List<Book>();


        public void AddBook(Book book)
        {
            this.library.Add(book);
        }
        
        public void Give_book(Book book)
        {
            this.give_books.Add(book);
        }
        public void Return_book(Book book)
        {
            this.return_books.Add(book);
        }

        public void GetDidntGet()
        {
            List<Book> answer = new List<Book>();
            bool is_find = false;
            foreach (Book book in this.library)
            {
                if (!give_books.Contains(book))
                {
                    answer.Add(book);
                    is_find = true;
                }
            }
            if (!is_find)
            {
                Console.WriteLine("Все книги выдавались");
            }
            else
            {
                Console.WriteLine("Книги, которые не выдавались");
                foreach (Book book in answer)
                {
                    Console.WriteLine($"{book.author_name} {book.book_name} {book.year} {book.publishing_name}");
                }
            }
            Console.WriteLine();
        }
        public void GetDidntReturn()
        {
            List<Book> answer = new List<Book>();
            bool is_find = false;
            foreach (Book book in this.library)
            {
                int givenCount = give_books.Count(b => b.Equals(book));
                int returnedCount = return_books.Count(b => b.Equals(book));

                if (givenCount != returnedCount && !answer.Contains(book))
                {
                    answer.Add(book);
                    is_find = true;
                }
            }
            if (!is_find)
            {
                Console.WriteLine("Все книги возвращены");
            }
            else
            {
                Console.WriteLine("Книги, которые не вернули");
                foreach (Book book in answer)
                {
                    Console.WriteLine($"{book.author_name} {book.book_name} {book.year} {book.publishing_name}");
                }
            }
            Console.WriteLine();
        }
    }

    struct Book{
        public string author_name;
        public string book_name;
        public int year;
        public string publishing_name;

        public Book(string author_name, string book_name, int year, string publishing_name)
        {
            this.author_name = author_name;
            this.book_name = book_name;
            this.year = year;
            this.publishing_name = publishing_name;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Library library = new Library();
            Book book1 = new Book("aaa", "aaaa", 1, "aaaa");
            Book book2 = new Book("bbb", "bbbb", 2, "bbbb");
            library.AddBook(book1);
            library.AddBook(book2);
            library.Give_book(book2);
            library.GetDidntGet();
            library.GetDidntReturn();
        }
    }
}