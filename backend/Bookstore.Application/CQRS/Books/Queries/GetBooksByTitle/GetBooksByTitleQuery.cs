using Bookstore.Application.DTOs.Book;
using MediatR;

namespace Bookstore.Application.CQRS.Books.Queries.GetBooksByTitle;

public sealed record GetBooksByTitleQuery(string Title) : IRequest<List<BookDto>>;