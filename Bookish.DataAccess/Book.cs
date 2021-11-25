using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookish
{
    public class Book
    {
        public List<string> Authors = new();
        public int BookId;
        public DateTime DueDate;
        public string FirstName;
        public string Isbn;
        public string LastName;
        public string Title;
        public int copies;

        public override string ToString()
        {
            if (Authors.Count == 1) return $"{Title} by {Authors.First()}";
            return $"{Title} by {string.Join(" and ", Authors)}";
        }
    }
}