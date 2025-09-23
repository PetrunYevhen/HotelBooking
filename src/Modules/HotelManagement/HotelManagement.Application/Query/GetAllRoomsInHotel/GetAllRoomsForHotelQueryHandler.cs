using HotelManagement.Domain.Entities;
using HotelManagement.Domain.RepositoryContract;
using MediatR;

namespace HotelManagement.Application.Query.GetAllRoomsInHotel;

public class GetAllRoomsForHotelQueryHandler : IRequestHandler<GetAllRoomsForHotelQuery, List<Guid>>
{
    private readonly IHotelReadRepository _hotelReadRepository;

    public GetAllRoomsForHotelQueryHandler(IHotelReadRepository hotelRepository)
    {
        _hotelReadRepository = hotelRepository;
    }

    public async Task<List<Guid>> Handle(GetAllRoomsForHotelQuery request, CancellationToken cancellationToken)
    {
        var hotelId = new HotelId(request.HotelId);
        var rooms = await _hotelReadRepository.GetRoomIdsByHotelIdAsync(hotelId, cancellationToken);

        if (rooms == null || !rooms.Any())
        {
            throw new InvalidOperationException($"No rooms found for hotel with id {request.HotelId}");
        }

        return rooms;
    }
}