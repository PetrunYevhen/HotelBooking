using Accommodations.Application.Contracts;
using BuildingBlock.Domain;

namespace Accommodations.Application.Command.Rooms.CreateRooms;

public class CreateRoomsCommand : CommandBase<Result<List<Guid>>>
{
    public List<CreateRoomDto> Rooms { get; set; }
    
}