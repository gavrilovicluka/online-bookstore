using Bookstore.Application.DTOs.Review;
using MediatR;

namespace Bookstore.Application.Reviews.Queries.GetBookReviews;

public sealed record GetBookReviewsQuery(string BookISBN) : IRequest<List<GetBookReviewDto>>;