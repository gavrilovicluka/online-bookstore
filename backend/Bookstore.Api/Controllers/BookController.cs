using Bookstore.Application.CQRS.Books.Commands.AddBook;
using Bookstore.Application.CQRS.Books.Commands.DeleteBook;
using Bookstore.Application.CQRS.Books.Commands.UpdateBook;
using Bookstore.Application.CQRS.Books.Queries.GetBookByISBN;
using Bookstore.Application.CQRS.Books.Queries.GetBooks;
using Bookstore.Application.CQRS.Books.Queries.GetBooksByAuthor;
using Bookstore.Application.CQRS.Books.Queries.GetBooksByCategory;
using Bookstore.Application.CQRS.Books.Queries.GetBooksByTitle;
using Bookstore.Application.DTOs.Book;
using Bookstore.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers;

public class BookController : ControllerBase
{
    public BookController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Returns all books
    /// </summary>
    /// <returns> This endpoint returns a list of Books</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
    {
        return Ok(await _mediator.Send(new GetBooksQuery()));
    }
    
    /// <summary>
    /// Returns book by ISBN
    /// </summary>
    /// <param name="request"> <see cref="GetBookByISBNQuery"/>.</param>
    /// <returns> One book if it exists or null if it does not exist </returns>
    /// <exception cref="BadRequestException"> If ISBN is not valid </exception>
    [HttpGet("ISBN", Name = "GetBookByISBN")]
    public async Task<ActionResult<BookDto>> GetBookByISBN([FromQuery] GetBookByISBNQuery request)
    {      
        return Ok(await _mediator.Send(request));
    }

    /// <summary>
    /// Returns books by title
    /// </summary>
    /// <param name="request"> <see cref="GetBooksByTitleQuery"/> </param>
    /// <returns> List of books </returns>
    [HttpGet("Title")]
    public async Task<ActionResult> GetBooksByTitle([FromQuery] GetBooksByTitleQuery request)
    {
        return Ok(await _mediator.Send(request));
    }
    
    /// <summary>
    /// Returns books by author
    /// </summary>
    /// <param name="request"> <see cref="GetBooksByAuthorQuery"/> </param>
    /// <returns> List of books </returns>
    [HttpGet("Author")]
    public async Task<ActionResult> GetBooksByAuthor([FromQuery] GetBooksByAuthorQuery request)
    {
        return Ok(await _mediator.Send(request));
    }
    
    /// <summary>
    /// Returns books by category
    /// </summary>
    /// <param name="request"> <see cref="GetBooksByCategoryQuery"/> </param>
    /// <returns> List of books </returns>
    [HttpGet("Category")]
    public async Task<ActionResult> GetBooksByCategory([FromQuery] GetBooksByCategoryQuery request)
    {
        return Ok(await _mediator.Send(request));
    }
    
    /// <summary>
    /// Creates a new book
    /// </summary>
    /// <param name="bookDto"> New book to add </param>
    /// <returns> Added book </returns>
    /// <exception cref="BadRequestException"> If fields of new book are not valid </exception>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] BookDto bookDto)
    {
        var createdBook = await _mediator.Send(new AddBookCommand(bookDto));
        return CreatedAtRoute(nameof(GetBookByISBN), new { isbn = createdBook.ISBN }, createdBook);
    }
    
    /// <summary>
    /// Updates existing book, or adds new if book with given ISBN does not exist 
    /// </summary>
    /// <param name="isbn"> ISBN of a book to update </param>
    /// <param name="bookDto"> New values of book properties </param>
    /// <returns> Changed book </returns>
    /// <exception cref="BadRequestException"> If fields of new book are not valid </exception>
    [Authorize(Roles = "Admin")]
    [HttpPut("{isbn}")]
    public async Task<ActionResult<BookDto>> UpdateBook(string isbn, [FromBody] UpdateBookDto bookDto)
    {
        var updatedBook = await _mediator.Send(new UpdateBookCommand(isbn, bookDto));
        return Ok(updatedBook with { ISBN = isbn });
    }
    
    /// <summary>
    /// Deleting book
    /// </summary>
    /// <param name="isbn"> ISBN of book to delete </param>
    /// <returns> Success message </returns>
    /// <exception cref="BadRequestException">  If ISBN is not valid  </exception>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{isbn}")]
    public async Task<ActionResult<string>> DeleteBook(string isbn)
    { 
        var response = await _mediator.Send(new DeleteBookCommand(isbn));
        return Ok($"Book with ISBN: {response} successfully deleted.");
    }
}