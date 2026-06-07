using Accommodations.Application.Command.Hotels.AddHotelFacilities;
using Accommodations.Application.Command.Hotels.CreateHotel;
using Accommodations.Application.Command.Shared;
using Accommodations.Application.Contracts;
using Accommodations.Application.Query.Hotels.GetHotelDetails;
using Accommodations.Application.Query.Hotels.GetHotelFacilities;
using Accommodations.Application.Query.Shared;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/hotels")] 
public class HotelsController : ControllerBase
{
    private readonly IAccommodationsModule _accommodationsModule;

    public HotelsController(
        IAccommodationsModule accommodationsModule)
    {
        _accommodationsModule = accommodationsModule;
    }

    // GET
    [HttpGet("{id:guid}")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _accommodationsModule.ExecuteQueryAsync(new GetHotelDetailsQuery(id));
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpGet("{id:guid}/facilities")]
    [ProducesResponseType(typeof(List<FacilityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFacilities(Guid id)
    {
        var result = await _accommodationsModule.ExecuteQueryAsync(new GetHotelFacilitiesQuery(id));
        return Ok(result);
    }

    // [HttpGet]
    // [ProducesResponseType(typeof(List<HotelSummaryDto>), StatusCodes.Status200OK)]
    // public async Task<IActionResult> GetAll([FromQuery] GetHotelsRequest request)
    // {
    //     var result = await _accommodationsModule.ExecuteQueryAsync(
    //         new GetHotelsQuery(request.City, request.Status, request.Page, request.PageSize));
    //     return Ok(result);
    // }
    
    
    // POST
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateHotelCommand command)
    {
        var result = await _accommodationsModule.ExecuteCommandAsync(command);
        if (result.IsFailure)
            return BadRequest(new { result.Error.Code, result.Error.Message });

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });    
    }
    
    [HttpPost("{id:guid}/facilities")]
    public async Task<IActionResult> AddFacility(Guid id, [FromBody] List<FacilityRequest> facilities, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule
            .ExecuteCommandAsync(new AddHotelFacilitiesCommand(){HotelId = id, Facilities = facilities});
        
        if (result.IsFailure)
            return BadRequest(new { result.Error.Code, result.Error.Message });

        return NoContent();
    }
}