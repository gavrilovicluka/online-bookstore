using Bookstore.Application.DTOs.Book;
using MediatR;

namespace Bookstore.Application.CQRS.Books.Commands.AddBook;

public sealed record AddBookCommand(BookDto BookToAdd) : IRequest<BookDto>;