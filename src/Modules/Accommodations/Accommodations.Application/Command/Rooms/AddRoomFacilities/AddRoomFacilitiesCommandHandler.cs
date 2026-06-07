using Accommodations.Application.Command.Hotels.AddHotelFacilities;
using Accommodations.Domain.Entities.Rooms;
using Accommodations.Domain.RepositoryContract.Rooms;
using BuildingBlock.Domain;
using MediatR;

namespace Accommodations.Application.Command.Rooms.AddRoomFacilities;

public class AddRoomFacilitiesCommandHandler : IRequestHandler<AddRoomFacilitiesCommand, Result>
{
    private readonly IRoomRepository _roomRepository;

    public AddRoomFacilitiesCommandHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<Result> Handle(AddRoomFacilitiesCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(new RoomId(request.RoomId), cancellationToken);
        if (room is null)
            return Result.Failure(new Error("Room.NotFound", "Room not found."));
        
        foreach (var facility in request.Facilities) 
            room.AddFacility(facility.Name, facility.Category);
        
        return Result.Success();
    }
}