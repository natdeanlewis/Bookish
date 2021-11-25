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
            using (IDbConnection db = new SqlConnection("Server=localhost;Database=Bookish;Trusted_Connection=True;"))
            {
                db.Open();
                var sql = "SELECT BookId FROM Book ORDER BY Title";

                var result = db.Query<int>(sql);
                var bookIds = result.ToList();
                Console.WriteLine("Here's what's in the catalogue:");
                foreach (var bookId in bookIds) PrintBookInfo(bookId);
            }
        }


        public static void GetAllLoans(int memberId)
        {
            using (IDbConnection db = new SqlConnection("Server=localhost;Database=Bookish;Trusted_Connection=True;"))
            {
                db.Open();

                var parameters = new {MemberId = memberId};
                var sql =
                    "SELECT b.Title, bm.DueDate FROM Book b " +
                    "INNER JOIN BookMember bm ON b.BookId = bm.BookId " +
                    "INNER JOIN Member m ON bm.MemberId = m.MemberId " +
                    "WHERE m.MemberId = @MemberId ORDER BY bm.DueDate";

                var result = db.Query<Book>(sql, parameters);

                Console.WriteLine("Your loaned book(s) are:");

                foreach (var loanedBook in result)
                {
                    var date = loanedBook.DueDate.ToString("d", CultureInfo.CreateSpecificCulture("en-GB"));
                    Console.WriteLine($"{loanedBook.Title}, due back on {date}");
                }
            }
        }

        public static void Search(string request)
        {
            var parameters = new {Request = request};

            using (IDbConnection db = new SqlConnection("Server=localhost;Database=Bookish;Trusted_Connection=True;"))
            {
                db.Open();

                var sql = "SELECT b.BookId FROM Book b " +
                          "INNER JOIN BookAuthor ba ON b.BookId = ba.BookId " +
                          "INNER JOIN Author a ON ba.AuthorId = a.AuthorId " +
                          "WHERE b.Title LIKE '%'+ @Request + '%' " +
                          "OR a.LastName LIKE '%'+ @Request + '%' " +
                          "OR a.FirstName LIKE '%'+ @Request + '%'";

                var bookIds = db.Query<int>(sql, parameters);

                bookIds = bookIds.Distinct().ToList();

                Console.WriteLine("Here are the books matching your search:");
                foreach (var bookId in bookIds) PrintBookInfo(bookId);
            }
        }

        private static void PrintBookInfo(int bookId)
        {
            var book = new Book();
            book.Authors = GetBookAuthors(bookId);
            book.Title = GetBookTitle(bookId);
            Console.WriteLine(book);
        }

        private static string GetBookTitle(int bookId)
        {
            using (IDbConnection db = new SqlConnection("Server=localhost;Database=Bookish;Trusted_Connection=True;"))
            {
                db.Open();

                var parameters = new {BookId = bookId};
                var sql =
                    "SELECT b.Title FROM Book b " +
                    "WHERE b.BookId = @BookId";

                var result = db.Query<string>(sql, parameters).First();

                return result;
            }
        }

        private static List<string> GetBookAuthors(int bookId)
        {
            using (IDbConnection db = new SqlConnection("Server=localhost;Database=Bookish;Trusted_Connection=True;"))
            {
                db.Open();

                var parameters = new {BookId = bookId};
                var sql =
                    "SELECT a.FirstName, a.LastName FROM Book b " +
                    "INNER JOIN BookAuthor ba ON b.BookId = ba.BookId " +
                    "INNER JOIN Author a ON ba.AuthorId = a.AuthorId " +
                    "WHERE b.BookId = @BookId";

                var result = db.Query<Author>(sql, parameters);

                var authors = new List<string>();
                foreach (var author in result) authors.Add($"{author.FirstName} {author.LastName}");
                return authors;
            }
        }
    }
}