using BookingManagement.Application.Command;
using BookingManagement.Application.Contracts;
using BookingManagement.Application.Query.GetBookingDetails;
using BookingManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("booking")]
public class BookingController : ControllerBase
{
private readonly IBookingManagementModule _bookingManagementModule;
    
    public BookingController( IBookingManagementModule bookingManagementModule)
    {
        _bookingManagementModule = bookingManagementModule;
    }
    
    [HttpGet("{reservationId}")]
    public async Task<IActionResult> GetReservationById(Guid reservationId)
    {
        var result = await _bookingManagementModule.ExecuteQueryAsync(
            new GetBookingDetailsQuery(new BookingId(reservationId)));
        if (result == null)
            return NotFound();
        return Ok(result);
    }
    
    // [HttpGet("all-reservations")]
    // public async Task<List<Booking.Domain.Entities.Booking>> GetAllReservations(CancellationToken cancellationToken)
    // {
    //     return await _mediator.Send(new GetAllReservationsQuery());
    // }
    
    [HttpPost("add-reservation")]
    public async Task<IActionResult> AddReservation([FromBody]AddBookingCommand request)
    {
        var result = await _bookingManagementModule.ExecuteCommandAsync(
            new AddBookingCommand(
                request.GuestId,
                request.RoomId, 
                request.Price,
                request.StartDate, 
                request.EndDate));
        return Ok(result);
    }
}