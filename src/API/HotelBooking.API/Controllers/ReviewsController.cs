using Microsoft.AspNetCore.Mvc;
using Reviews.Application.Commands.CreateReview;
using Reviews.Application.Contracts;
using Reviews.Application.Query.GetAllReviewsByHotel;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewsModule _reviewsModule;

    public ReviewsController(IReviewsModule reviewsModule)
    {
        _reviewsModule = reviewsModule;
    }

    // GET
    [HttpGet("{id:guid}/reviews")]
    [HttpGet("/api/hotels/{id:guid}/reviews")]
    [ProducesResponseType(typeof(List<ReviewDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReviews(Guid id, CancellationToken ct)
    {
        var result = await _reviewsModule.ExecuteQueryAsync(new GetAllReviewsByHotelQuery(id), ct);
        return Ok(result);
    }
    
    // POST
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateReviewCommand command, CancellationToken cancellationToken)
    {
        var result = await _reviewsModule.ExecuteCommandAsync(command, cancellationToken);
        if (result.IsFailure)
            return this.ToProblem(result.Error);
        return StatusCode(StatusCodes.Status201Created);
    }
}
