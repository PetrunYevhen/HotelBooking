using HotelManagement.Application.Command.AddHotel;
using HotelManagement.Application.Contracts;
using HotelManagement.Application.Query.GetFacilities;
using HotelManagement.Application.Query.GetHotelDetails;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("hotels")] 
public class HotelController : ControllerBase
{
    private readonly ILogger<HotelController> _logger;
    private readonly IHotelManagementModule _hotelManagementModule;

    public HotelController(
        IHotelManagementModule hotelManagementModule, 
        ILogger<HotelController> logger)
    {
        _hotelManagementModule = hotelManagementModule;
        _logger = logger;
    }

    [HttpGet("{Id:guid}")]
    public async Task<IActionResult> GetDetails(Guid Id)
    {
        var result = await _hotelManagementModule.ExecuteQueryAsync(new GetHotelDetailsQuery(Id));
        return result != null ? Ok(result) : NotFound();
    }
    
    // [HttpGet("{Id:guid}/rooms-details")]
    // public async Task<IActionResult> GetHotelWithRooms(Guid Id)
    // {
    //     var result = await _getHotelRoomsCompositionService.GetHotelRooms(Id);
    //     return result != null ? Ok(result) : NotFound();
    // }
    
    [HttpGet("{Id:guid}/facilities")]
    public async Task<IActionResult> GetFacilities(Guid Id)
    {
        var result = await _hotelManagementModule.ExecuteQueryAsync(new GetFacilitiesQuery(Id));
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddHotelCommand command)
    {
        var result = await _hotelManagementModule.ExecuteCommandAsync(command);
        return Ok(result);
    }
}