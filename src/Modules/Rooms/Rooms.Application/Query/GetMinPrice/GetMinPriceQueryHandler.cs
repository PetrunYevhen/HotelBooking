using MediatR;
using Rooms.Domain.RepositoryContract;

namespace Rooms.Application.Query.GetMinPrice;

public class GetMinPriceRoomQueryHandler : IRequestHandler<GetMinPriceQuery, decimal>
{
    private readonly IRoomsReadRepository _roomReadRepository;

    public GetMinPriceRoomQueryHandler(IRoomsReadRepository roomManagmentReadRepository)
    {
        _roomReadRepository = roomManagmentReadRepository;
    }


    public async Task<decimal> Handle(GetMinPriceQuery request, CancellationToken cancellationToken)
    {
        return await _roomReadRepository.GetMinPriceAsync(request.HotelId, cancellationToken);
    }
}