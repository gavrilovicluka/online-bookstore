using Bookstore.Application.DTOs.Book;
using Bookstore.Domain.Contracts;
using Bookstore.Domain.Exceptions;
using MediatR;

namespace Bookstore.Application.CQRS.Books.Commands.UpdateBook;

public sealed class UpdateBookHandler : IRequestHandler<UpdateBookCommand, BookDto>
{
    private readonly IBookRepository _bookRepository;

    public UpdateBookHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<BookDto> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var existingBook = await _bookRepository.GetBookByISBN(request.ISBN);

        if (existingBook == null)
        {
            throw new NotFoundException($"Book with ISBN: {request.ISBN} not found.");
        }

        existingBook.Title = request.BookToUpdate.Title;
        existingBook.Author = request.BookToUpdate.Author;
        existingBook.Category = request.BookToUpdate.Category;
        existingBook.Price = request.BookToUpdate.Price;
        existingBook.Available = request.BookToUpdate.Available;

        await _bookRepository.UpdateBook(existingBook);

        return new BookDto(
            existingBook.ISBN, 
            existingBook.Title, 
            existingBook.Author, 
            existingBook.Category,
            existingBook.Price, 
            existingBook.Available);
    }
}