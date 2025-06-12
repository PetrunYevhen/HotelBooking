using Application.Command;
using Application.Query;
using Application.Query.GetRoomBookingDetails;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoomManagment.Application.Dto;
using RoomManagment.Domain.Entities;

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
    
    [HttpGet("all-reservations")]
    public async Task<List<Reservation>> GetAllReservations(CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetAllReservationsQuery());
    }
    
    [HttpGet("room-booking-details/{roomId}")]
    public async Task<RoomBookingDetailsDto> GetRoomBookingDetails(Guid roomId)
    {
        return await _mediator.Send(new GetRoomBookingDetailsQuery(new RoomId(roomId)));
    }
    
    [HttpPost("add-reservation")]
    public async Task<Reservation> AddReservation(AddReservationCommand request)
    {
        return await _mediator.Send(request);
    }
}