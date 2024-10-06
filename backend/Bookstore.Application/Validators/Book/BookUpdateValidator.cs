using Bookstore.Application.DTOs.Book;
using FluentValidation;

namespace Bookstore.Application.Validators.Book;

public class BookUpdateValidator : AbstractValidator<UpdateBookDto>
{
    public BookUpdateValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Author).NotEmpty();
        RuleFor(x => x.Category).NotEmpty();
        RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Available).IsInEnum();
    }
}