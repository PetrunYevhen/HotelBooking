using Application.Command;
using Application.Query;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("booking")]
public class BookingController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public BookingController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{reservationId}")]
    public async Task<Reservation> GetReservationById(Guid  reservationId)
    {
        return await _mediator.Send(new GetReservationByIdQuery(new ReservationId(reservationId)));
    }
    
    [HttpPost("add-reservation")]
    public async Task<Reservation> AddReservation(AddReservationCommand request)
    {
        return await _mediator.Send(request);
    }
}