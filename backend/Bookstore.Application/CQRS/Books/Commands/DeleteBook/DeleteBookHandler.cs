using Bookstore.Domain.Contracts;
using Bookstore.Domain.Exceptions;
using MediatR;

namespace Bookstore.Application.CQRS.Books.Commands.DeleteBook;

public sealed class DeleteBookHandler : IRequestHandler<DeleteBookCommand, string>
{
    private readonly IBookRepository _bookRepository;

    public DeleteBookHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<string> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var existingBook = await _bookRepository.GetBookByISBN(request.ISBN);

        if (existingBook == null)
        {
            throw new NotFoundException($"Book with ISBN: {request.ISBN} not found.");
        }

        await _bookRepository.DeleteBook(request.ISBN);

        return request.ISBN;
    }
}