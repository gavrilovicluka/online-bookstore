using Bookstore.Domain.Contracts;
using Bookstore.Domain.Entities;
using Bookstore.Persistence.Contexts;

namespace Bookstore.Persistence.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly MongoDbContext _mongoDbContext;

    public ReviewRepository(MongoDbContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
    }

    public Review? GetReview(Guid reviewId)
    {
        return _mongoDbContext.Reviews.FirstOrDefault(r => r.Id == reviewId);
    }

    public void CreateReview(Review review)
    {
        _mongoDbContext.Reviews.Add(review);

        _mongoDbContext.SaveChangesAsync();
    }
}