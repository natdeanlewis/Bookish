using Bookish.DataAccess;

namespace Bookish
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            LibraryDatabaseClient.GetAllBooks();
            LibraryDatabaseClient.GetAllLoans(1);
        }
    }
}