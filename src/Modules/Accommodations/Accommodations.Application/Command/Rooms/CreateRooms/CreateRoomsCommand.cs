using Accommodations.Application.Contracts;
using BuildingBlock.Domain;

namespace Accommodations.Application.Command.Rooms.CreateRooms;

public class CreateRoomsCommand : CommandBase<Result<List<Guid>>>
{
    public Guid ActorId { get; init; }
    public bool IsAdmin { get; init; }
    public List<CreateRoomDto> Rooms { get; set; }
    
}
