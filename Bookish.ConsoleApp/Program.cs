using System;
using Bookish.DataAccess;

namespace Bookish
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            LibraryDatabaseClient.GetAllBooks();
            Console.WriteLine();
            LibraryDatabaseClient.GetAllLoans(1);
            Console.WriteLine();
            LibraryDatabaseClient.Search("am");
        }
    }
}