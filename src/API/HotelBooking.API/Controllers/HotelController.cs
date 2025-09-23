using HotelManagement.Application.Command.AddFacilityToHotel;
using HotelManagement.Application.Command.AddNewHotel;
using HotelManagement.Application.Command.AddRoomToHotel;
using HotelManagement.Application.Contracts;
using HotelManagement.Application.Query.GetAllFacilitiesInHotel;
using HotelManagement.Application.Query.GetAllRoomsInHotel;
using HotelManagement.Application.Query.GetHotelDetails;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("hotel")]
public class HotelController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IHotelManagementModule _hotelManagementModule;

    public HotelController(ILogger logger, IHotelManagementModule hotelManagementModule)
    {
        _logger = logger;
        _hotelManagementModule = hotelManagementModule;
    }

    [HttpGet("hotel-details/{hotelId}")]
    public async Task<IActionResult> GetHotelDetails(Guid hotelId)
    {
        var result = await _hotelManagementModule.ExecuteQueryAsync(new GetHotelDetailsQuery(hotelId));
        return Ok(result);
    }
    
    [HttpGet("all-rooms/{hotelId}")]
    public async Task<IActionResult> GetAllRoomsForHotel(Guid hotelId)
    {
        var result = await _hotelManagementModule.ExecuteQueryAsync(new GetAllRoomsForHotelQuery(hotelId));
        
        return Ok(result);
    }
    
    [HttpGet("all-facilities/{hotelId}")]
    public async Task<IActionResult> GetAllFacilitiesForHotel(Guid hotelId)
    {
        var result = await _hotelManagementModule.ExecuteQueryAsync(new GetAllFacilitiesInHotelQuery(hotelId));
        return Ok(result);
    }
    
    [HttpPost("add-hotel")]
    public async Task<IActionResult> AddHotel([FromBody]AddNewHotelCommand request)
    {
        var result = await _hotelManagementModule.ExecuteCommandAsync(
            new AddNewHotelCommand(
                request.HotelName,
                request.Description,
                request.ImageUrl,
                request.Rating,
                request.MinRoomPrice));
        return Ok(result);
    }

    [HttpPost("add-room-to-hotel")]
    public async Task<IActionResult> AddRoomToHotel([FromBody] AddRoomToHotelCommand request)
    {
        _logger.Warning("Adding room {RoomId} to hotel {HotelId}", request.HotelId, request.RoomId);
        var result = await _hotelManagementModule.ExecuteCommandAsync(
            new AddRoomToHotelCommand(request.HotelId, request.RoomId));
        return Ok(result);
    }

    [HttpPost("add-facility-to-hotel")]
    public async Task<IActionResult> AddFacilityToHotel([FromBody] AddFacilityToHotelCommand request)
    {
        _logger.Warning("Adding facilities {FacilityIds} to hotel {HotelId}", request.FacilityIds, request.HotelId);
        var result = await _hotelManagementModule.ExecuteCommandAsync(
            new AddFacilityToHotelCommand(request.HotelId, request.FacilityIds));
        return Ok(result);
    }
}