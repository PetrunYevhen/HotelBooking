using MediatR;
using RoomManagement.Domain.Entities;
using RoomManagement.Domain.RepositoryContract;

namespace RoomManagement.Application.Query.GetPrice;

public class GetPriceQueryHandler : IRequestHandler<GetPriceQuery, decimal>
{
    private readonly IRoomManagementReadRepository _roomReadRepository;

    public GetPriceQueryHandler(IRoomManagementReadRepository roomReadRepository)
    {
        _roomReadRepository = roomReadRepository;
    }

    public async Task<decimal> Handle(GetPriceQuery request, CancellationToken cancellationToken)
    {
        var roomId = new RoomId(request.RoomId);

        var room = await _roomReadRepository.GetByIdAsync(roomId, cancellationToken);

        return room.PricePerNight;
    }
}