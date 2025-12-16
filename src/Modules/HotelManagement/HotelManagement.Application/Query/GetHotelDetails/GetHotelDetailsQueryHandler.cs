using HotelManagement.Application.CachingContract;
using HotelManagement.Domain.Entities;
using HotelManagement.Domain.RepositoryContract;
using MediatR;
using RoomManagement.Application.Query.GetMinPriceRoom;

namespace HotelManagement.Application.Query.GetHotelDetails;

public class GetHotelDetailsQueryHandler : IRequestHandler<GetHotelDetailsQuery, HotelDetailDto>
{
    private readonly IHotelReadRepository _hotelReadRepository;
    private readonly IHotelRoomsCache _hotelRoomsCache;
    private readonly IMediator _mediator;
    
    public GetHotelDetailsQueryHandler(IHotelReadRepository hotelReadRepository, IMediator mediator, IHotelRoomsCache hotelRoomsCache)
    {
        _hotelReadRepository = hotelReadRepository;
        _mediator = mediator;
        _hotelRoomsCache = hotelRoomsCache;
    }


    public async Task<HotelDetailDto> Handle(
        GetHotelDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var hotelId = new HotelId(request.HotelId);

        var hotel = await _hotelReadRepository
            .GetHotelByIdAsync(hotelId, cancellationToken);
        
        if (hotel == null)
            throw new KeyNotFoundException(
                $"Готель з ID {request.HotelId} не знайдено.");

        var minPriceRoom = await _mediator.Send(new GetMinPriceRoomQuery(hotelId.Value));
        
        
        return new HotelDetailDto
        {
            HotelId = hotel.HotelId.Value,  
            HotelName = hotel.HotelName,
            Description = hotel.Description,
            ImageUrl = hotel.ImageUrl,
            Rating = hotel.Rating,
            MinRoomPrice = minPriceRoom
        };
    }


}