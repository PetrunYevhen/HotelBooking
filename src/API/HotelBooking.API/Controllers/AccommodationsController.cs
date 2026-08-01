using Accommodations.Application.Command.Hotels.AddHotelFacilities;
using Accommodations.Application.Command.Hotels.CreateHotel;
using Accommodations.Application.Command.Hotels.SetPolicies;
using Accommodations.Application.Command.HotelAddOns.CreateHotelAddOn;
using Accommodations.Application.Command.HotelAddOns.SetHotelAddOnStatus;
using Accommodations.Application.Command.HotelAddOns.UpdateHotelAddOn;
using Accommodations.Application.Command.Shared;
using Accommodations.Application.Contracts;
using Accommodations.Application.Query.Hotels.GetAllHotels;
using Accommodations.Application.Query.Hotels.GetHotelDetails;
using Accommodations.Application.Query.Hotels.GetHotelFacilities;
using Accommodations.Application.Query.Hotels.SearchHotels;
using Accommodations.Application.Query.HotelAddOns;
using Accommodations.Application.Query.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

    [HttpGet("{id:guid}/add-ons")]
    [ProducesResponseType(typeof(IReadOnlyList<HotelAddOnDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAddOns(Guid id, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule.ExecuteQueryAsync(new GetHotelAddOnsQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/add-ons")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAddOn(Guid id, [FromBody] CreateHotelAddOnRequest request, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule.ExecuteCommandAsync(new CreateHotelAddOnCommand(
            id, request.Code, request.Name, request.Description, request.PriceAmount, request.PriceCurrency, request.PricingType), cancellationToken);
        if (result.IsFailure)
            return this.ToProblem(result.Error);
        return StatusCode(StatusCodes.Status201Created, result.Value);
    }

    [HttpPut("{hotelId:guid}/add-ons/{addOnId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateAddOn(Guid hotelId, Guid addOnId, [FromBody] UpdateHotelAddOnRequest request, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule.ExecuteCommandAsync(new UpdateHotelAddOnCommand(
            hotelId, addOnId, request.Code, request.Name, request.Description, request.PriceAmount, request.PriceCurrency, request.PricingType), cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : NoContent();
    }

    [HttpPost("{hotelId:guid}/add-ons/{addOnId:guid}/activate")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> ActivateAddOn(Guid hotelId, Guid addOnId, CancellationToken cancellationToken) =>
        SetAddOnStatus(hotelId, addOnId, true, cancellationToken);

    [HttpPost("{hotelId:guid}/add-ons/{addOnId:guid}/deactivate")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> DeactivateAddOn(Guid hotelId, Guid addOnId, CancellationToken cancellationToken) =>
        SetAddOnStatus(hotelId, addOnId, false, cancellationToken);

    private async Task<IActionResult> SetAddOnStatus(Guid hotelId, Guid addOnId, bool isActive, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule.ExecuteCommandAsync(new SetHotelAddOnStatusCommand(hotelId, addOnId, isActive), cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<HotelDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule.ExecuteQueryAsync(new GetAllHotelsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(List<HotelDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromQuery] string? destination,
        [FromQuery] DateTime? checkIn,
        [FromQuery] DateTime? checkOut,
        [FromQuery] int guests,
        [FromQuery] int rooms,
        CancellationToken cancellationToken)
    {
        var query = new SearchHotelsQuery
        {
            Destination = destination,
            CheckIn = checkIn,
            CheckOut = checkOut,
            Guests = guests > 0 ? guests : 1,
            Rooms = rooms > 0 ? rooms : 1
        };
        var result = await _accommodationsModule.ExecuteQueryAsync(query, cancellationToken);
        return Ok(result);
    }


    // POST
    [HttpPost]
    [Authorize(Roles = "Admin,Hotelier")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateHotelCommand command, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var currentUserId))
            return Unauthorized();

        var ownerUserId = User.IsInRole("Admin") ? command.OwnerUserId : currentUserId;
        var commandWithOwner = new CreateHotelCommand
        {
            Name = command.Name, Description = command.Description, Status = command.Status,
            Street = command.Street, City = command.City, Country = command.Country,
            PostalCode = command.PostalCode, CheckIn = command.CheckIn, CheckOut = command.CheckOut,
            OwnerUserId = ownerUserId ?? currentUserId
        };
        var result = await _accommodationsModule.ExecuteCommandAsync(commandWithOwner, cancellationToken);
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

public sealed record CreateHotelAddOnRequest(string Code, string Name, string? Description, decimal PriceAmount, string PriceCurrency, Accommodations.Domain.Entities.HotelAddOns.Enums.PricingType PricingType);
public sealed record UpdateHotelAddOnRequest(string Code, string Name, string? Description, decimal PriceAmount, string PriceCurrency, Accommodations.Domain.Entities.HotelAddOns.Enums.PricingType PricingType);
