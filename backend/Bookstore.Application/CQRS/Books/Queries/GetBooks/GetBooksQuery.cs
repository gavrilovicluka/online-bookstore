using Bookstore.Application.DTOs.Book;
using MediatR;

namespace Bookstore.Application.CQRS.Books.Queries.GetBooks;

public sealed record GetBooksQuery : IRequest<List<BookDto>>;