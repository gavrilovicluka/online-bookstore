using Bookstore.Application.DTOs.Book;
using Bookstore.Domain.Contracts;
using Bookstore.Domain.Entities;
using Bookstore.Domain.Exceptions;
using MediatR;

namespace Bookstore.Application.CQRS.Books.Commands.AddBook;

public sealed class AddBookHandler : IRequestHandler<AddBookCommand, BookDto>
{
    private readonly IBookRepository _bookRepository;

    public AddBookHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<BookDto> Handle(AddBookCommand request, CancellationToken cancellationToken)
    {
        var existingBook = await _bookRepository.GetBookByISBN(request.BookToAdd.ISBN!);

        if (existingBook != null)
        {
            throw new NotFoundException($"Book with ISBN: {request.BookToAdd.ISBN} already exists.");
        }

        var newBook = new Book()
        {
            ISBN = request.BookToAdd.ISBN,
            Title = request.BookToAdd.Title,
            Author = request.BookToAdd.Author,
            Category = request.BookToAdd.Category,
            Price = request.BookToAdd.Price,
            Available = request.BookToAdd.Available
        };

        await _bookRepository.AddBook(newBook);

        return request.BookToAdd;
    }
}