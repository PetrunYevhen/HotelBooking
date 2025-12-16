using HotelBooking.API.Composition.HotelCompositionServices.GetAllRoomsForHotel;
using HotelManagement.Application.Command.AddNewHotel;
using HotelManagement.Application.Contracts;
using HotelManagement.Application.Query.GetAllFacilitiesInHotel;
using HotelManagement.Application.Query.GetHotelDetails;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("hotel")]
public class HotelController : ControllerBase
{
    private readonly ILogger<HotelController> _logger;
    private readonly IHotelDetailsCompositionService _hotelDetailsCompositionService;
    private readonly IHotelManagementModule _hotelManagementModule;

    public HotelController( IHotelManagementModule hotelManagementModule, ILogger<HotelController> logger, IHotelDetailsCompositionService hotelDetailsCompositionService)
    {
        _hotelManagementModule = hotelManagementModule;
        _logger = logger;
        _hotelDetailsCompositionService = hotelDetailsCompositionService;
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
        var result = await _hotelDetailsCompositionService.GetHotelDetailsAsync(hotelId);
        
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
}