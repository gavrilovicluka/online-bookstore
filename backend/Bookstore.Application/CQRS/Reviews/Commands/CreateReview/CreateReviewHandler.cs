using Bookstore.Application.Helpers;
using Bookstore.Domain.Contracts;
using Bookstore.Domain.Entities;
using Bookstore.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Bookstore.Application.Reviews.Commands.CreateReview;

public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, Unit>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IBookRepository _bookRepository;
    private readonly UserManager<User> _userManager;

    public CreateReviewHandler(IReviewRepository reviewRepository, IBookRepository bookRepository, UserManager<User> userManager)
    {
        _reviewRepository = reviewRepository;
        _bookRepository = bookRepository;
        _userManager = userManager;
    }
    
    public async Task<Unit> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var existingBook = await _bookRepository.GetBookByISBN(request.CreateReviewDto.BookISBN.ISBN);
        if (existingBook == null)
        {
            throw new BadRequestException($"Book with ISBN: {request.CreateReviewDto.BookISBN} does not exist.");
        }
        
        var user = await _userManager.GetUserAsync(UserHelper.GetUserClaimsPrincipal(new HttpContextAccessor()));
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }

        var hasUserBoughtBook = user.BooksISBN.Contains(request.CreateReviewDto.BookISBN.ISBN);
        if (!hasUserBoughtBook)
        {
            throw new BadRequestException($"User: {user.Id} has not bought book: {request.CreateReviewDto.BookISBN}.");
        }

        var newReview = new Review
        {
            UserId = user.Id,
            BookISBN = request.CreateReviewDto.BookISBN.ISBN,
            Rating = request.CreateReviewDto.Rating,
            ReviewText = request.CreateReviewDto.ReviewText
        };
        
        _reviewRepository.CreateReview(newReview);
        
        existingBook.ReviewIds.Add(newReview.Id);
        await _bookRepository.UpdateBook(existingBook);
        
        user.ReviewsIds.Add(newReview.Id);
        await _userManager.UpdateAsync(user);

        return await Unit.Task;
    }
}