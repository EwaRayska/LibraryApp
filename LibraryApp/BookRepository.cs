namespace LibraryApp
{
    public class BookRepository : IBookRepository
    {
        private readonly List<Book> _books = new List<Book>
        {
            new (1, "W pustyni i w puszczy", "Henryk Sienkiewicz"),
            new (2, "Krzyżacy", "Henryk Sienkiewicz"),
            new (3, "Kubuś Puchatek", "A.A. Milne"),
            new (4, "Lalka", "Boleslaw Prus"),
            new (5, "Dżuma", "Albert Camus"),
            new (6, "Tango", "Slawomir Mrozek"),
            new (7, "Akademia Pana Kleksa", "Jan Brzechwa"),
            new (8, "Harry Potter", "J.K. Rowling"),
            new (9, "Hobbit", "J.R.R. Tolkien"),
            new (10, "Pan Tadeusz", "Adam Mickiewicz"),
            new (11, "Balladyna", "Juliusz Slowacki")
        };

        public IEnumerable<Book> GetAllBooks() => _books;

        public Book GetBookById(int id) => _books.FirstOrDefault(b => b.Id == id);

        public void AddBook(Book book) => _books.Add(book);

        public void UpdateBook(Book book)
        {
            var existingBook = GetBookById(book.Id);
            if(existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
            }
        }

        public void DeleteBook(int id)
        {
            var book = GetBookById(id);
            if(book != null)
            {
                _books.Remove(book);
            }
        }
    }
}
