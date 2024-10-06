using Bookstore.Domain.Entities;

namespace Bookstore.Domain.Contracts;

public interface IReviewRepository
{
    Review? GetReview(Guid reviewId);
    void CreateReview(Review review);
}