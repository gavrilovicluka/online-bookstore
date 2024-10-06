using MediatR;

namespace Bookstore.Application.Reviews.Queries.GetAverageRating;

public sealed record GetAverageRatingQuery(string BookISBN) : IRequest<decimal>;