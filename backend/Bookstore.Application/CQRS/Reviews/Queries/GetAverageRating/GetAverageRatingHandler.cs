using Bookstore.Domain.Contracts;
using Bookstore.Domain.Exceptions;
using MediatR;

namespace Bookstore.Application.Reviews.Queries.GetAverageRating;

public class GetAverageRatingHandler : IRequestHandler<GetAverageRatingQuery, decimal>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IBookRepository _bookRepository;

    public GetAverageRatingHandler(IReviewRepository reviewRepository, IBookRepository bookRepository)
    {
        _reviewRepository = reviewRepository;
        _bookRepository = bookRepository;
    }
    
    public async Task<decimal> Handle(GetAverageRatingQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetBookByISBN(request.BookISBN);
        if (book == null)
        {
            throw new BadRequestException($"Book with ISBN: {request.BookISBN} does not exist.");
        }

        var bookReviewIds = book.ReviewIds;
        decimal ratingSum = 0;
        decimal count = bookReviewIds.Count;
        foreach (var reviewId in bookReviewIds)
        {
            var review = _reviewRepository.GetReview(reviewId);
            if (review == null)
            {
                throw new BadRequestException($"Review with ID: {reviewId} does not exist.");
            }
            ratingSum += review.Rating;
        }

        return ratingSum / count;
    }
}