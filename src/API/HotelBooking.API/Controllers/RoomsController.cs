using Accommodations.Application.Command.Rooms.AddRoomFacilities;
using Accommodations.Application.Command.Rooms.CreateRooms;
using Accommodations.Application.Command.Rooms.DeactivateRoom;
using Accommodations.Application.Command.Shared;
using Accommodations.Application.Contracts;
using Accommodations.Application.Query.Rooms.GetRoomDetails;
using Accommodations.Application.Query.Rooms.GetRoomFacilities;
using Accommodations.Application.Query.Rooms.GetRoomPrice;
using Accommodations.Application.Query.Rooms.GetRoomsByHotelId;
using Accommodations.Application.Query.Shared;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.ValueObjects;
using RoomDetailsDto = Accommodations.Application.Query.Shared.RoomDetailsDto;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomsController : ControllerBase
{
    private readonly IAccommodationsModule _accommodationsModule;

    public RoomsController(IAccommodationsModule accommodationsModule)
    {
        _accommodationsModule = accommodationsModule;
    }

    // GET
    [HttpGet("{roomId:guid}")]
    [ProducesResponseType(typeof(RoomDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid roomId, DateTime checkIn, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule.ExecuteQueryAsync(new GetRoomDetailsQuery(roomId, checkIn), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{hotelId:guid}/rooms")]
    [ProducesResponseType(typeof(List<RoomDetailsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRooms(Guid hotelId, DateTime checkIn, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule.ExecuteQueryAsync(new GetRoomsByHotelIdQuery(hotelId, checkIn), cancellationToken);        
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpGet("{id:guid}/facilities")]
    [ProducesResponseType(typeof(List<FacilityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFacilities(Guid id, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule.ExecuteQueryAsync(new GetRoomFacilitiesQuery(id), cancellationToken);
        return Ok(result);
    }
    
    [HttpGet("{roomId:guid}/price")]
    [ProducesResponseType(typeof(Money), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRoomPrice(
        Guid roomId,
        [FromQuery] DateTime checkIn,
        [FromQuery] DateTime checkOut,
        CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule.ExecuteQueryAsync(
            new GetRoomPriceQuery(roomId, checkIn, checkOut), cancellationToken);

        if (result.IsFailure)
            return this.ToProblem(result.Error);

        return Ok(result.Value);
    }


    
    // POST
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] List<CreateRoomDto> rooms, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule.ExecuteCommandAsync(new CreateRoomsCommand{Rooms = rooms}, cancellationToken);
        if (result.IsFailure)
            return this.ToProblem(result.Error);

        return StatusCode(StatusCodes.Status201Created, result.Value);
    }

    [HttpPost("{id:guid}/facilities")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddFacility(Guid id, [FromBody] List<FacilityRequest> facilities, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule
            .ExecuteCommandAsync(new AddRoomFacilitiesCommand{RoomId = id, Facilities = facilities}, cancellationToken);
        
        if (result.IsFailure)
            return this.ToProblem(result.Error);

        return NoContent();
    }
    
    [HttpPost("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule
            .ExecuteCommandAsync(new DeactivateRoomCommand(id), cancellationToken);

        if (result.IsFailure)
            return this.ToProblem(result.Error);

        return NoContent();
    }
}
