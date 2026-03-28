
using Bookings.Application.Command.CancelBooking;
using Bookings.Application.Command.CreateBooking;
using Bookings.Application.Contracts;
using Bookings.Application.Query.GetBookingById;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("booking")]
public class BookingController : ControllerBase
{
private readonly IBookingsModule _bookingManagementModule;
    
    public BookingController( IBookingsModule bookingManagementModule)
    {
        _bookingManagementModule = bookingManagementModule;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBookingAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _bookingManagementModule.ExecuteQueryAsync(new GetByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBookingAsync([FromBody] CreateBookingCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _bookingManagementModule.ExecuteCommandAsync(command);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> CancelBookingAsync(Guid id)
    {
        var result = await _bookingManagementModule.ExecuteCommandAsync(new CancelBookingCommand(id));
        return Ok(result);
    }
}