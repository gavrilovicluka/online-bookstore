using Bookstore.Application.DTOs.Book;
using Bookstore.Application.Helpers;
using Bookstore.Domain.Contracts;
using MediatR;

namespace Bookstore.Application.CQRS.Books.Queries.GetBooksByCategory;

public class GetBooksByCategoryHandler : IRequestHandler<GetBooksByCategoryQuery, List<BookDto>>
{
    private readonly IBookRepository _bookRepository;

    public GetBooksByCategoryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<List<BookDto>> Handle(GetBooksByCategoryQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetBooksByCategory(request.Category);

        var booksDtoList = BookHelper.BooksToDtos(books);

        return booksDtoList;
    }
}