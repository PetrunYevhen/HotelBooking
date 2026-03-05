using Microsoft.AspNetCore.Mvc;
using RoomManagement.Application.Command.AddRoom;
using RoomManagement.Application.Contracts;
using RoomManagement.Application.Query.GetMinPrice;
using RoomManagement.Application.Query.GetPrice;
using RoomManagement.Application.Query.GetRoomDetails;
using RoomManagement.Application.Query.GetRoomsByHotelId;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("rooms")]
public class RoomController : ControllerBase
{
    private readonly IRoomManagementModule _roomManagementModule;

    public RoomController(IRoomManagementModule roomManagementModule)
    {
        _roomManagementModule = roomManagementModule;
    }

    [HttpGet("{roomId:guid}")]
    public async Task<IActionResult> GetRoomDetails(Guid roomId)
    {
        var result = await _roomManagementModule.ExecuteQueryAsync(new GetRoomDetailsQuery(roomId));
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("hotel/{id:guid}/min-price")]
    public async Task<IActionResult> GetHotelMinPrice(Guid id)
    {
        var result = await _roomManagementModule.ExecuteQueryAsync(new GetMinPriceQuery(id));
        return Ok(result);
    }

    [HttpGet("hotel/{id:guid}")]
    public async Task<IActionResult> GetRoomsByHotel(Guid id)
    {
        var result = await _roomManagementModule.ExecuteQueryAsync(new GetRoomsByIdQuery(id));
        return Ok(result);
    }

    [HttpGet("room-price/{roomId:guid}")]
    public async Task<IActionResult> GetRoomPrice(Guid roomId)
    {
        var result = await _roomManagementModule.ExecuteQueryAsync(new GetPriceQuery(roomId));
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddRoom([FromBody] AddRoomCommand command)
    {
        var result = await _roomManagementModule.ExecuteCommandAsync(command);
        return Ok(result);
    }
}