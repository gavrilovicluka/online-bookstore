using Bookstore.Domain.Entities;

namespace Bookstore.Domain.Contracts;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllBooks();
    Task<Book> GetBookByISBN(string ISBN);
    Task<List<Book>> GetBooksByTitle(string title);
    Task<List<Book>> GetBooksByAuthor(string author);
    Task<List<Book>> GetBooksByCategory(string category);
    Task AddBook(Book newBook);
    Task UpdateBook(Book updateBook);
    Task DeleteBook(string ISBN);
}