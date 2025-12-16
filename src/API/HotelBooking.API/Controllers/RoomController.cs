using Microsoft.AspNetCore.Mvc;
using RoomManagement.Application.Command.AddRoom;
using RoomManagement.Application.Contracts;
using RoomManagement.Application.Query.GetMinPriceForRoom;
using RoomManagement.Application.Query.GetRoomDetails;

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

    [HttpGet("get-room-details/{roomId}")]
    public async Task<IActionResult> GetRoomDetails(Guid roomId)
    {
        var result = await _roomManagementModule.ExecuteQueryAsync(
            new GetRoomDetailsQuery(roomId));
        return Ok(result);
    }
    
    [HttpGet("get-min-price-room/{hotelId}")]
    public async Task<IActionResult> GetMinPriceRoom(Guid hotelId)
    {
        var result = await _roomManagementModule.ExecuteQueryAsync(
            new GetMinPriceForRoomQuery(hotelId));
        return Ok(result);
    }

    [HttpPost("add-room")]
    public async Task<IActionResult> AddRoom(AddRoomCommand request)
    {
        var result = await _roomManagementModule.ExecuteCommandAsync(
            new AddRoomCommand(
            request.RoomNumber,
            request.Capacity,
            request.Description,
            request.RoomCount,
            request.Beds,
            request.Status,
            request.PricePerNight
        ));
        return Ok(result);
    }
    
}