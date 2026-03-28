using MediatR;
using Rooms.Domain.Entities;
using Rooms.Domain.RepositoryContract;

namespace Rooms.Application.Query.GetPrice;

public class GetPriceQueryHandler : IRequestHandler<GetPriceQuery, decimal>
{
    private readonly IRoomsReadRepository _roomReadRepository;

    public GetPriceQueryHandler(IRoomsReadRepository roomReadRepository)
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