namespace LibraryApp
{
    public class BookRepository : IBookRepository
    {
        private readonly List<Book> _books;

        public BookRepository(IEnumerable<Book> books)
        {
            _books = books.ToList();
        }

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
