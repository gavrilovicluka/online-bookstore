using Bookstore.Application.DTOs.Book;
using Bookstore.Domain.Contracts;
using Bookstore.Domain.Exceptions;
using MediatR;

namespace Bookstore.Application.CQRS.Books.Queries.GetBookByISBN;

public sealed class GetBookByISBNHandler : IRequestHandler<GetBookByISBNQuery, BookDto>
{
    private readonly IBookRepository _bookRepository;

    public GetBookByISBNHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<BookDto> Handle(GetBookByISBNQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetBookByISBN(request.ISBN);
        
        if (book == null)
        {
            throw new NotFoundException($"Book with ISBN: {request.ISBN} not found.");
        }

        return new BookDto(book.ISBN, book.Title, book.Author, book.Category, book.Price, book.Available);
    }
}