using MediatR;
using RoomManagement.Domain.Entities;
using RoomManagement.Domain.RepositoryContract;

namespace RoomManagement.Application.Query.GetRoomDetails;

public class GetRoomDetailsQueryHandler : IRequestHandler<GetRoomDetailsQuery, RoomBookingDetailsDto>
{
    private readonly IRoomManagementReadRepository _roomReadRepository;

    public GetRoomDetailsQueryHandler(IRoomManagementReadRepository roomManagementReadRepository)
    {
        _roomReadRepository = roomManagementReadRepository;
    }

    public async Task<RoomBookingDetailsDto> Handle(GetRoomDetailsQuery request, CancellationToken cancellationToken)
    {
        var roomId = new RoomId(request.Id);
        
        var room = await _roomReadRepository.GetByIdAsync(roomId, cancellationToken);

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