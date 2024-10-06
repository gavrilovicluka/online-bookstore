using Bookstore.Application.DTOs.Review;
using MediatR;

namespace Bookstore.Application.Reviews.Commands.CreateReview;

public sealed record CreateReviewCommand(CreateReviewDto CreateReviewDto) : IRequest<Unit>;