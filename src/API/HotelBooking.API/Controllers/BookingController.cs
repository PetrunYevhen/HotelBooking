using Bookings.Application.Command.CancelBooking;
using Bookings.Application.Command.CheckInBooking;
using Bookings.Application.Command.CheckOutBooking;
using Bookings.Application.Command.CreateBooking;
using Bookings.Application.Contracts;
using Bookings.Application.Query.GetBookingById;
using Bookings.Application.Query.GetBookingsByUserId;
using Bookings.Application.Query.GetBookingUserId;
using Bookings.Application.Query.GetBookingQuote;
using Bookings.Application.Services.AddOns;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();
        if (!User.IsInRole("Admin"))
        {
            var bookingUserId = await _bookingsModule.ExecuteQueryAsync(new GetBookingUserIdQuery(id), cancellationToken);
            if (!bookingUserId.HasValue)
                return NotFound();
            if (bookingUserId.Value != userId)
                return Forbid();
        }
        var result = await _bookingsModule.ExecuteQueryAsync(new GetByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(List<BookingDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();
        var result = await _bookingsModule.ExecuteQueryAsync(new GetBookingsByUserIdQuery(userId), cancellationToken);
        return Ok(result);
    }

    // POST
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();
        var command = new CreateBookingCommand(request.HotelId, request.RoomId, userId, request.CheckIn, request.CheckOut,
            request.GuestCount, request.FirstName, request.LastName, request.Email, request.PhoneNumber,
            request.SpecialRequest, request.AddOns);
        var result = await _bookingsModule.ExecuteCommandAsync(command, cancellationToken);
        if (result.IsFailure)
            return this.ToProblem(result.Error);
        return StatusCode(StatusCodes.Status201Created, result.Value);
    }

    [HttpPost("quote")]
    [ProducesResponseType(typeof(BookingQuoteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Quote([FromBody] BookingQuoteRequest request, CancellationToken cancellationToken)
    {
        var result = await _bookingsModule.ExecuteQueryAsync(new GetBookingQuoteQuery(request.HotelId, request.RoomId,
            request.CheckIn, request.CheckOut, request.GuestCount,
            request.AddOns?.Select(x => new RequestedHotelAddOn(x.HotelAddOnId, x.Quantity)).ToList()), cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : Ok(result.Value);
    }
    
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelBookingRequest? request, CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();
        var result = await _bookingsModule.ExecuteCommandAsync(new CancelBookingCommand(id, userId, request?.Reason), cancellationToken);
        if (result.IsFailure)
            return this.ToProblem(result.Error);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
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

    private bool TryGetCurrentUserId(out Guid userId) => Guid.TryParse(User.FindFirst("sub")?.Value, out userId);
}

public record CancelBookingRequest(string? Reason);
public sealed record CreateBookingRequest(Guid HotelId, Guid RoomId, DateTime CheckIn, DateTime CheckOut, int GuestCount,
    string FirstName, string LastName, string Email, string PhoneNumber, string? SpecialRequest,
    IReadOnlyCollection<CreateBookingAddOn>? AddOns);
public sealed record BookingQuoteRequest(Guid HotelId, Guid RoomId, DateTime CheckIn, DateTime CheckOut, int GuestCount,
    IReadOnlyCollection<BookingQuoteAddOnRequest>? AddOns);
public sealed record BookingQuoteAddOnRequest(Guid HotelAddOnId, int Quantity);
