using Bookstore.Application.DTOs.Book;
using MediatR;

namespace Bookstore.Application.CQRS.Books.Commands.UpdateBook;

public sealed record UpdateBookCommand(string ISBN, UpdateBookDto BookToUpdate) : IRequest<BookDto>;