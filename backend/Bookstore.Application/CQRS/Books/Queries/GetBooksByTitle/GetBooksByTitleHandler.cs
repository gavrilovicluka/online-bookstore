using Bookstore.Application.DTOs.Book;
using Bookstore.Application.Helpers;
using Bookstore.Domain.Contracts;
using MediatR;

namespace Bookstore.Application.CQRS.Books.Queries.GetBooksByTitle;

public class GetBooksByTitleHandler : IRequestHandler<GetBooksByTitleQuery, List<BookDto>>
{
    private readonly IBookRepository _bookRepository;

    public GetBooksByTitleHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<List<BookDto>> Handle(GetBooksByTitleQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetBooksByTitle(request.Title);

        var booksDtoList = BookHelper.BooksToDtos(books);

        return booksDtoList;
    }
}