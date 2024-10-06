using Bookstore.Application.DTOs.Book;
using MediatR;

namespace Bookstore.Application.CQRS.Books.Queries.GetBookByISBN;

public sealed record GetBookByISBNQuery(string ISBN) : IRequest<BookDto>;