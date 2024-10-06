using Bookstore.Application.DTOs.Review;
using Bookstore.Domain.Contracts;
using Bookstore.Domain.Entities;
using Bookstore.Domain.Exceptions;
using MediatR;

namespace Bookstore.Application.Reviews.Queries.GetBookReviews;

public class GetBookReviewsHandler : IRequestHandler<GetBookReviewsQuery, List<GetBookReviewDto>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IBookRepository _bookRepository;

    public GetBookReviewsHandler(IReviewRepository reviewRepository, IBookRepository bookRepository)
    {
        _reviewRepository = reviewRepository;
        _bookRepository = bookRepository;
    }
    
    public async Task<List<GetBookReviewDto>> Handle(GetBookReviewsQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetBookByISBN(request.BookISBN);
        if (book == null)
        {
            throw new BadRequestException($"Book with ISBN: {request.BookISBN} does not exist.");
        }

        List<Review> bookReviews = new List<Review>();
        foreach (var reviewId in book.ReviewIds)
        {
            var review = _reviewRepository.GetReview(reviewId);
            if (review == null)
            {
                throw new BadRequestException($"Review with ID: {reviewId} does not exist.");
            }
            bookReviews.Add(review);
        }

        return bookReviews.Select(r => new GetBookReviewDto(r.UserId, r.Rating, r.ReviewText!)).ToList();
    }
}