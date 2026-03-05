using MediatR;
using RoomManagement.Domain.RepositoryContract;

namespace RoomManagement.Application.Query.GetMinPrice;

public class GetMinPriceRoomQueryHandler : IRequestHandler<GetMinPriceQuery, decimal>
{
    private readonly IRoomManagementReadRepository _roomReadRepository;

    public GetMinPriceRoomQueryHandler(IRoomManagementReadRepository roomManagmentReadRepository)
    {
        _roomReadRepository = roomManagmentReadRepository;
    }


    public async Task<decimal> Handle(GetMinPriceQuery request, CancellationToken cancellationToken)
    {
        return await _roomReadRepository.GetMinPriceAsync(request.HotelId, cancellationToken);
    }
}