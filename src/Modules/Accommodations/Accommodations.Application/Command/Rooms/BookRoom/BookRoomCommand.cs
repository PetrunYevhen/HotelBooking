using Accommodations.Application.Contracts;
using BuildingBlock.Domain;

namespace Accommodations.Application.Command.Rooms.BookRoom;

public class BookRoomCommand : CommandBase<Result>
{
    public BookRoomCommand(Guid roomId)
    {
        RoomId = roomId;
    }

    public Guid RoomId { get; set; }
}