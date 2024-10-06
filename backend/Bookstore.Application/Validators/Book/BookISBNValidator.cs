using Bookstore.Application.CQRS.Books.Queries.GetBookByISBN;
using FluentValidation;

namespace Bookstore.Application.Validators.Book;

public class BookISBNValidator : AbstractValidator<GetBookByISBNQuery>
{
    public BookISBNValidator()
    {
        RuleFor(x => x.ISBN).NotEmpty().Length(13).WithMessage("ISBN is invalid! Length must be 13 numbers!");
    }
}