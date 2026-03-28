using Hotels.Application.Command.AddHotel;
using Hotels.Application.Contracts;
using Hotels.Application.Query.GetFacilities;
using Hotels.Application.Query.GetHotelDetails;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("hotels")] 
public class HotelController : ControllerBase
{
    private readonly ILogger<HotelController> _logger;
    private readonly IHotelsModule _hotelManagementModule;

    public HotelController(
        IHotelsModule hotelManagementModule, 
        ILogger<HotelController> logger)
    {
        _hotelManagementModule = hotelManagementModule;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var result = await _hotelManagementModule.ExecuteQueryAsync(new GetHotelDetailsQuery(id));
        return result != null ? Ok(result) : NotFound();
    }
    
    // [HttpGet("{Id:guid}/rooms-details")]
    // public async Task<IActionResult> GetHotelWithRooms(Guid Id)
    // {
    //     var result = await _getHotelRoomsCompositionService.GetHotelRooms(Id);
    //     return result != null ? Ok(result) : NotFound();
    // }
    
    [HttpGet("{id:guid}/facilities")]
    public async Task<IActionResult> GetFacilities(Guid id)
    {
        var result = await _hotelManagementModule.ExecuteQueryAsync(new GetFacilitiesQuery(id));
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddHotelCommand command)
    {
        var result = await _hotelManagementModule.ExecuteCommandAsync(command);
        return Ok(result);
    }
}