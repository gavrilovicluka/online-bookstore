using Bookstore.Application.DTOs.Order;
using Bookstore.Application.Validators.Book;
using FluentValidation;

namespace Bookstore.Application.Validators.Order;

public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.BookISBNList).NotEmpty().WithMessage("List of books cannot be empty.");
        RuleForEach(x => x.BookISBNList).SetValidator(new BookISBNValidator());
    }
}