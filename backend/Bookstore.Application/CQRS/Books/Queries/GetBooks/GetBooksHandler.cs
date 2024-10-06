using Bookstore.Application.DTOs.Book;
using Bookstore.Domain.Contracts;
using MediatR;

namespace Bookstore.Application.CQRS.Books.Queries.GetBooks;

public sealed class GetBooksHandler : IRequestHandler<GetBooksQuery, List<BookDto>>
{
    private readonly IBookRepository _repo;

    public GetBooksHandler(IBookRepository repo)
    {
        _repo = repo;
    }
    
    public async Task<List<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _repo.GetAllBooks();

        return books
            .Select(b => new BookDto(b.ISBN, b.Title, b.Author, b.Category, b.Price, b.Available))
            .ToList();
    }
}