using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Bookish
{
    class Program
    {
        public static void Main(string[] args)
        {
            Book book = new Book();
            book.bookCatalogue();


        }
    }
}

