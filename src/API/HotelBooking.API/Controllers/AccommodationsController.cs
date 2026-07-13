using Accommodations.Application.Command.Hotels.AddHotelFacilities;
using Accommodations.Application.Command.Hotels.CreateHotel;
using Accommodations.Application.Command.Hotels.SetPolicies;
using Accommodations.Application.Command.Shared;
using Accommodations.Application.Contracts;
using Accommodations.Application.Query.Hotels.GetAllHotels;
using Accommodations.Application.Query.Hotels.GetHotelDetails;
using Accommodations.Application.Query.Hotels.GetHotelFacilities;
using Accommodations.Application.Query.Shared;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/hotels")] 
public class AccommodationsController : ControllerBase
{
    private readonly IAccommodationsModule _accommodationsModule;

    public AccommodationsController(
        IAccommodationsModule accommodationsModule)
    {
        _accommodationsModule = accommodationsModule;
    }

    // GET
    [HttpGet("{id:guid}")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule.ExecuteQueryAsync(new GetHotelDetailsQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpGet("{id:guid}/facilities")]
    [ProducesResponseType(typeof(List<FacilityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFacilities(Guid id, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule.ExecuteQueryAsync(new GetHotelFacilitiesQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<HotelDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule.ExecuteQueryAsync(new GetAllHotelsQuery(), cancellationToken);
        return Ok(result);
    }
    
    
    // POST
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateHotelCommand command, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule.ExecuteCommandAsync(command, cancellationToken);
        if (result.IsFailure)
            return this.ToProblem(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });    
    }
    
    [HttpPost("{id:guid}/facilities")]
    public async Task<IActionResult> AddFacility(Guid id, [FromBody] List<FacilityRequest> facilities, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule
            .ExecuteCommandAsync(new AddHotelFacilitiesCommand(){HotelId = id, Facilities = facilities}, cancellationToken);
        
        if (result.IsFailure)
            return this.ToProblem(result.Error);

        return NoContent();
    }
    
    [HttpPut("{id:guid}/policies")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetPolicies(Guid id, [FromBody] SetHotelPoliciesCommand command, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule.ExecuteCommandAsync(new SetHotelPoliciesCommand
        (
            id,
            command.CancellationPolicyType,
            command.DeadlineDays,
            command.PercentagePenalty,
            command.PetPolicy,
            command.SmokingPolicy,
            command.CheckOutHoursPolicy
        ), cancellationToken);
        if (result.IsFailure)
            return this.ToProblem(result.Error);

        return NoContent();
    }
}
