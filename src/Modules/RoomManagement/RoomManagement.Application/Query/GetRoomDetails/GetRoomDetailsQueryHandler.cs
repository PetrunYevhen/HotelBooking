using MediatR;
using RoomManagement.Domain.Entities;
using RoomManagement.Domain.RepositoryContract;

namespace RoomManagement.Application.Query.GetRoomDetails;

public class GetRoomDetailsQueryHandler : IRequestHandler<GetRoomDetailsQuery, RoomBookingDetailsDto>
{
    private readonly IRoomManagmentReadRepository _roomManagmentReadRepository;

    public GetRoomDetailsQueryHandler(IRoomManagmentReadRepository roomManagmentReadRepository)
    {
        _roomManagmentReadRepository = roomManagmentReadRepository;
    }

    public async Task<RoomBookingDetailsDto> Handle(GetRoomDetailsQuery request, CancellationToken cancellationToken)
    {
        var roomId = new RoomId(request.RoomId);
        
        var room = await _roomManagmentReadRepository.GetRoomByIdAsync(roomId, cancellationToken);

        if (room == null) throw new KeyNotFoundException("Room not found");

        return new RoomBookingDetailsDto
        {
            RoomId = room.RoomId.Value,
            RoomNumber = room.RoomNumber,
            Beds = room.Beds,
            Capacity = room.Capacity,
            Description = room.Description,
            PricePerNight = room.PricePerNight,
            Status = room.Status
        };
    }
}