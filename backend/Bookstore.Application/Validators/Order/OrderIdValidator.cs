using FluentValidation;

namespace Bookstore.Application.Validators.Order;

public class OrderIdValidator : AbstractValidator<Guid>
{
    public OrderIdValidator()
    {
        RuleFor(x => x).NotEmpty().WithMessage("Id cannot be empty.");
    }
}