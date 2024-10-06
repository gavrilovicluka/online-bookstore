using Bookstore.Application.DTOs.Book;
using MediatR;

namespace Bookstore.Application.CQRS.Books.Queries.GetBooksByCategory;

public sealed record GetBooksByCategoryQuery(string Category) : IRequest<List<BookDto>>;