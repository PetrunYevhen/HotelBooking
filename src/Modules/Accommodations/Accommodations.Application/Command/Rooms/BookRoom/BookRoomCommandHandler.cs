using Accommodations.Domain.Entities.Rooms;
using Accommodations.Domain.RepositoryContract.Rooms;
using BuildingBlock.Domain;
using MediatR;

namespace Accommodations.Application.Command.Rooms.BookRoom;

public class BookRoomCommandHandler : IRequestHandler<BookRoomCommand, Result>
{
    private readonly IRoomRepository _roomRepository;

    public BookRoomCommandHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<Result> Handle(BookRoomCommand request, CancellationToken cancellationToken)
    {
        var roomId = new RoomId(request.RoomId);
        var room = await _roomRepository.GetByIdAsync(roomId, cancellationToken);
        if (room is null)
            return Result.Failure(new Error("Room.NotFound", $"Room {request.RoomId} not found."));

        room.Booked();
        await _roomRepository.UpdateAsync(room, cancellationToken);
        return Result.Success();
    }
}