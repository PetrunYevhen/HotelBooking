using Accommodations.Application.Command.Hotels.CreateHotel;
using Accommodations.Application.Contracts;
using Accommodations.Application.Query.Hotels.GetHotelDetails;
using Accommodations.Application.Query.Hotels.GetHotelFacilities;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("hotels")] 
public class AccommodationsController : ControllerBase
{
    private readonly ILogger<AccommodationsController> _logger;
    private readonly IAccommodationsModule _accommodationsModule;

    public AccommodationsController(
        IAccommodationsModule accommodationsModule, 
        ILogger<AccommodationsController> logger)
    {
        _accommodationsModule = accommodationsModule;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var result = await _accommodationsModule.ExecuteQueryAsync(new GetHotelDetailsQuery(id));
        return result != null ? Ok(result) : NotFound();
    }
    
    [HttpGet("{id:guid}/facilities")]
    public async Task<IActionResult> GetFacilities(Guid id)
    {
        var result = await _accommodationsModule.ExecuteQueryAsync(new GetHotelFacilitiesQuery(id));
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHotelCommand command)
    {
        var result = await _accommodationsModule.ExecuteCommandAsync(command);
        return Ok(result);
    }
}