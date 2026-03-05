using MediatR;
using RoomManagement.Domain.Entities;
using RoomManagement.Domain.RepositoryContract;

namespace RoomManagement.Application.Query.CalculatePrice;

public class CalculatePriceQueryHandler : IRequestHandler<CalculatePriceQuery, decimal>
{
    private readonly IRoomManagementReadRepository _roomReadRepository;

    public CalculatePriceQueryHandler(IRoomManagementReadRepository roomReadRepository)
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