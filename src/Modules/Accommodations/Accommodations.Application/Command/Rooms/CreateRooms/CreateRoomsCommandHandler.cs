using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.Entities.Rooms;
using Accommodations.Domain.RepositoryContract.Rooms;
using BuildingBlock.Domain;
using MediatR;
using SharedKernel.ValueObjects;

namespace Accommodations.Application.Command.Rooms.CreateRooms;

public class CreateRoomsCommandHandler : IRequestHandler<CreateRoomsCommand, Result<List<Guid>>>
{
    private readonly IRoomRepository _roomRepository;

    public CreateRoomsCommandHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<Result<List<Guid>>> Handle(CreateRoomsCommand roomuest, CancellationToken cancellationToken)
    {
        var rooms = new List<Room>();
        
        foreach (var room in roomuest.Rooms)
        {
             var basePriceResult = Money.Create(room.BasePriceAmount, room.BasePriceCurrency);
             if (basePriceResult.IsFailure)
                 return Result.Failure<List<Guid>>(basePriceResult.Error);
                    
             
             var roomResult = Room.Create(
                 new HotelId(room.HotelId),
                 room.RoomNumber,
                 room.Type,
                 room.Beds,
                 room.Capacity,
                 room.Description,
                 room.Status,
                 basePriceResult.Value);

             if (roomResult.IsFailure)
                 return Result.Failure<List<Guid>>(roomResult.Error);
             
             rooms.Add(roomResult.Value);
        }
        
        await _roomRepository.AddRangeAsync(rooms, cancellationToken);
        return Result.Success(rooms.Select(r => r.RoomId.Value).ToList());
    }
}