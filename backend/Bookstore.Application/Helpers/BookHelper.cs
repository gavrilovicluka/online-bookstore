using Bookstore.Application.DTOs.Book;
using Bookstore.Domain.Entities;

namespace Bookstore.Application.Helpers;

public static class BookHelper
{
    public static List<BookDto> BooksToDtos(List<Book> books)
    {
        var booksDtoList = books.Select(book => new BookDto(book.ISBN, book.Title, book.Author, book.Category, book.Price, book.Available)).ToList();
        return booksDtoList;
    }
}