using MediatR;
using Rooms.Domain.Entities;
using Rooms.Domain.RepositoryContract;

namespace Rooms.Application.Query.CalculatePrice;

public class CalculatePriceQueryHandler : IRequestHandler<CalculatePriceQuery, decimal>
{
    private readonly IRoomsReadRepository _roomReadRepository;

    public CalculatePriceQueryHandler(IRoomsReadRepository roomReadRepository)
    {
        _roomReadRepository = roomReadRepository;
    }

    public async Task<decimal> Handle(CalculatePriceQuery request, CancellationToken cancellationToken)
    {
        var roomId = new RoomId(request.RoomId);
        var room = await _roomReadRepository.GetByIdAsync(roomId, cancellationToken);
        
        var days = (request.CheckOut - request.CheckIn).Days;
        
        return room.PricePerNight * days;
    }
}