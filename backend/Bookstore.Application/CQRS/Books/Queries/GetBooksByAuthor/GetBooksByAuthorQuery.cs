using Bookstore.Application.DTOs.Book;
using MediatR;

namespace Bookstore.Application.CQRS.Books.Queries.GetBooksByAuthor;

public sealed record GetBooksByAuthorQuery(string Author) : IRequest<List<BookDto>>;