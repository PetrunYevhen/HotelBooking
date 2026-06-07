using Accommodations.Application.Command.Rooms.AddRoomFacilities;
using Accommodations.Application.Command.Rooms.CreateRooms;
using Accommodations.Application.Command.Shared;
using Accommodations.Application.Contracts;
using Accommodations.Application.Query.Hotels.GetHotelFacilities;
using Accommodations.Application.Query.Rooms.GetRoomDetails;
using Accommodations.Application.Query.Rooms.GetRoomFacilities;
using Accommodations.Application.Query.Rooms.GetRoomsByHotelId;
using Accommodations.Application.Query.Shared;
using Microsoft.AspNetCore.Mvc;
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
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RoomDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _accommodationsModule.ExecuteQueryAsync(new GetRoomDetailsQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:guid}/rooms")]
    [ProducesResponseType(typeof(List<RoomDetailsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRooms(Guid id)
    {
        var result = await _accommodationsModule.ExecuteQueryAsync(new GetRoomsByHotelIdQuery(id));        
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpGet("{id:guid}/facilities")]
    [ProducesResponseType(typeof(List<FacilityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFacilities(Guid id)
    {
        var result = await _accommodationsModule.ExecuteQueryAsync(new GetRoomFacilitiesQuery(id));
        return Ok(result);
    }

    
    // POST
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] List<CreateRoomDto> rooms, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule.ExecuteCommandAsync(new CreateRoomsCommand{Rooms = rooms});
        if (result.IsFailure)
            return BadRequest(new { result.Error.Code, result.Error.Message });

        return StatusCode(StatusCodes.Status201Created, result.Value);
    }

    [HttpPost("{id:guid}/facilities")]
    public async Task<IActionResult> AddFacility(Guid id, [FromBody] List<FacilityRequest> facilities, CancellationToken cancellationToken)
    {
        var result = await _accommodationsModule
            .ExecuteCommandAsync(new AddRoomFacilitiesCommand{RoomId = id, Facilities = facilities});
        
        if (result.IsFailure)
            return BadRequest(new { result.Error.Code, result.Error.Message });

        return NoContent();
    }
}
