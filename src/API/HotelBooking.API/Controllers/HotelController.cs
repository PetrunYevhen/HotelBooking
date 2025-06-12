using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoomManagment.Application.Command.AddRoom;
using RoomManagment.Domain.Entities;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("hotel")]
public class HotelController : ControllerBase
{
    private readonly IMediator _mediator;

    public HotelController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("add-room")]
    public async Task<Room> AddRoom(AddRoomCommand request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        
        return await _mediator.Send(request);
    }
}