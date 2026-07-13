using Bookings.Application.Command.CheckInBooking;
using Bookings.Application.Command.CheckOutBooking;
using Bookings.Application.Command.CreateBooking;
using Bookings.Application.Contracts;
using Bookings.Application.Query.GetBookingById;
using Bookings.Application.Query.GetBookingsByUserId;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("booking")]
[Route("api/bookings")]
public class BookingController : ControllerBase
{
private readonly IBookingsModule _bookingsModule;
    
    public BookingController( IBookingsModule bookingsModule)
    {
        _bookingsModule = bookingsModule;
    }

    // GET
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _bookingsModule.ExecuteQueryAsync(new GetByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    
    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(List<BookingDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _bookingsModule.ExecuteQueryAsync(new GetBookingsByUserIdQuery(userId), cancellationToken);
        return Ok(result);
    }

    // POST
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBookingCommand command, CancellationToken cancellationToken)
    {
        var result = await _bookingsModule.ExecuteCommandAsync(command, cancellationToken);
        if (result.IsFailure)
            return this.ToProblem(result.Error);
        return StatusCode(StatusCodes.Status201Created, result.Value);
    }
    
    // [HttpPost("{id:guid}/cancel")]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    // {
    //     var result = await _bookingsModule.ExecuteCommandAsync(new CancelBookingCommand(id));
    //     if (result.IsFailure)
    //         return BadRequest(new { result.Error.Code, result.Error.Message });
    //     return NoContent();
    // }

    [HttpPost("{id:guid}/checkin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CheckIn(Guid id, CancellationToken cancellationToken)
    {
        var result = await _bookingsModule.ExecuteCommandAsync(new CheckInBookingCommand(id), cancellationToken);
        if (result.IsFailure)
            return this.ToProblem(result.Error);
        return NoContent();
    }

    [HttpPost("{id:guid}/checkout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CheckOut(Guid id, CancellationToken cancellationToken)
    {
        var result = await _bookingsModule.ExecuteCommandAsync(new CheckOutBookingCommand(id), cancellationToken);
        if (result.IsFailure)
            return this.ToProblem(result.Error);
        return NoContent();
    }
}
