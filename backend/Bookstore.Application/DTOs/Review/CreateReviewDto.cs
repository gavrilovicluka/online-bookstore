using Bookstore.Application.CQRS.Books.Queries.GetBookByISBN;

namespace Bookstore.Application.DTOs.Review;

public sealed record CreateReviewDto(GetBookByISBNQuery BookISBN, string ReviewText, int Rating);