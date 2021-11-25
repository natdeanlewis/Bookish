using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Dapper;

namespace Bookish.DataAccess
{
    public static class LibraryDatabaseClient
    {
        public static void GetAllBooks()
        {
            var sql = "SELECT BookId, ISBN, Title FROM [Bookish].[dbo].[Book]";
            using (IDbConnection db = new SqlConnection("Server=localhost;Database=Bookish;Trusted_Connection=True;"))
            {
                db.Open();
                var result = db.Query<Book>(sql);
                var books = result.ToList();
                Console.WriteLine("Here's what's in the catalogue:");
                foreach (var book in books) Console.WriteLine(book.Title);
            }
        }


        public static void GetAllLoans(int memberId)
        {
            using (IDbConnection db = new SqlConnection("Server=localhost;Database=Bookish;Trusted_Connection=True;"))
            {
                db.Open();

                var parameters = new {MemberId = memberId};
                var sql =
                    "SELECT b.Title, bm.DueDate FROM Book b INNER JOIN BookMember bm ON b.BookId = bm.BookId INNER JOIN Member m ON bm.MemberId = m.MemberId WHERE m.MemberId = @MemberId;";

                var result = db.Query<Book>(sql, parameters);

                Console.WriteLine("Your loaned book(s) are:");

                foreach (var loanedBook in result)
                {
                    var date = loanedBook.DueDate.ToString("d", CultureInfo.CreateSpecificCulture("en-GB"));
                    Console.Write($"{loanedBook.Title}, due back on {date}");
                }
            }
        }
    }
}