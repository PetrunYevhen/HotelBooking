using Microsoft.AspNetCore.Mvc;
using Rooms.Application.Command.AddRoom;
using Rooms.Application.Contracts;
using Rooms.Application.Query.GetMinPrice;
using Rooms.Application.Query.GetPrice;
using Rooms.Application.Query.GetRoomDetails;
using Rooms.Application.Query.GetRoomsByHotelId;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("rooms")]
public class RoomController : ControllerBase
{
    private readonly IRoomsModule _roomsModule;

    public RoomController(IRoomsModule roomsModule)
    {
        _roomsModule = roomsModule;
    }

    [HttpGet("{roomId:guid}")]
    public async Task<IActionResult> GetRoomDetails(Guid roomId)
    {
        var result = await _roomsModule.ExecuteQueryAsync(new GetRoomDetailsQuery(roomId));
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("hotel/{id:guid}/min-price")]
    public async Task<IActionResult> GetHotelMinPrice(Guid id)
    {
        var result = await _roomsModule.ExecuteQueryAsync(new GetMinPriceQuery(id));
        return Ok(result);
    }

    [HttpGet("hotel/{id:guid}")]
    public async Task<IActionResult> GetRoomsByHotel(Guid id)
    {
        var result = await _roomsModule.ExecuteQueryAsync(new GetRoomsByIdQuery(id));
        return Ok(result);
    }

    [HttpGet("room-price/{roomId:guid}")]
    public async Task<IActionResult> GetRoomPrice(Guid roomId)
    {
        var result = await _roomsModule.ExecuteQueryAsync(new GetPriceQuery(roomId));
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddRoom([FromBody] AddRoomCommand command)
    {
        var result = await _roomsModule.ExecuteCommandAsync(command);
        return Ok(result);
    }
}