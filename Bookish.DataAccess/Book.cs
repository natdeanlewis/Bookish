using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Bookish
{
    public class Book
    {
        public int BookId;
        public string ISBN;
        public string Title;

        private string sql = "SELECT BookId, ISBN, Title FROM [Bookish].[dbo].[Book]";
        public void bookCatalogue()
        {
            using (IDbConnection db = new SqlConnection("Server=localhost;Database=Bookish;Trusted_Connection=True;"))
            {
                db.Open();
                var result = db.Query<Book>(sql);
                List<Book> books = new List<Book>();
                books = result.ToList();
                foreach (var book in books)
                {
                    Console.WriteLine(book.Title);
                }
            }
        }
    }
}