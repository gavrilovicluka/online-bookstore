using Bookstore.Application.DTOs.Review;
using Bookstore.Application.Validators.Book;
using FluentValidation;

namespace Bookstore.Application.Validators.Review;

public class CreateReviewValidator : AbstractValidator<CreateReviewDto>
{
    public CreateReviewValidator()
    {
        RuleFor(x => x.ReviewText).NotEmpty().WithMessage("Review must have text.");
        RuleFor(x => x.BookISBN).SetValidator(new BookISBNValidator());
        RuleFor(x => x.Rating).GreaterThan(0).WithMessage("Rating must be greater than 0")
            .LessThanOrEqualTo(5).WithMessage("Maximal value for rating is 5.");
    }
}