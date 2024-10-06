namespace Bookstore.Application.DTOs.Review;

public record GetBookReviewDto(Guid UserId, int Rating, string ReviewText);