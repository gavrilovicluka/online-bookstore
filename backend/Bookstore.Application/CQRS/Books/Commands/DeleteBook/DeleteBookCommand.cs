using MediatR;

namespace Bookstore.Application.CQRS.Books.Commands.DeleteBook;

public sealed record DeleteBookCommand(string ISBN) : IRequest<string>;