using Bookstore.Application.DTOs.Review;
using Bookstore.Application.Reviews.Commands.CreateReview;
using Bookstore.Application.Reviews.Queries.GetAverageRating;
using Bookstore.Application.Reviews.Queries.GetBookReviews;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers;

public class ReviewController : ControllerBase
{
    public ReviewController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Get all reviews for book
    /// </summary>
    /// <param name="bookISBN"> ISBN of book </param>
    /// <returns> All reviews </returns>
    [HttpGet("Book/{bookISBN}")]
    public async Task<ActionResult> GetBookReviews([FromRoute] string bookISBN)
    {
        return Ok(await _mediator.Send(new GetBookReviewsQuery(bookISBN)));
    }
    
    
    /// <summary>
    /// Creates new review
    /// </summary>
    /// <param name="createReviewDto"> Book ISBN, rating and review text </param>
    /// <returns> Success message </returns>
    [Authorize(Roles = "Admin, Customer")]
    [HttpPost]
    public async Task<ActionResult> CreateReview([FromBody] CreateReviewDto createReviewDto)
    {
        await _mediator.Send(new CreateReviewCommand(createReviewDto));
        return Ok("Review successfully created.");
    }

    /// <summary>
    /// Get average rating for book
    /// </summary>
    /// <param name="bookISBN"> ISBN of book </param>
    /// <returns> Average rating as decimal number </returns>
    [HttpGet("AverageRating/{bookISBN}")]
    public async Task<ActionResult> GetAverageRating(string bookISBN)
    {
        return Ok(await _mediator.Send(new GetAverageRatingQuery(bookISBN)));
    }
}