using Bookstore.Application.DTOs.Book;
using Bookstore.Application.Helpers;
using Bookstore.Domain.Contracts;
using MediatR;

namespace Bookstore.Application.CQRS.Books.Queries.GetBooksByAuthor;

public class GetBooksByAuthorHandler : IRequestHandler<GetBooksByAuthorQuery, List<BookDto>>
{
    private readonly IBookRepository _bookRepository;

    public GetBooksByAuthorHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<List<BookDto>> Handle(GetBooksByAuthorQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetBooksByAuthor(request.Author);

        var booksDtoList = BookHelper.BooksToDtos(books);

        return booksDtoList;
    }
}