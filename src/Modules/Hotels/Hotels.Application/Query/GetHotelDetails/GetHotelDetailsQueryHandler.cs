using DTO.DTOs.HotelDto;
using Hotels.Domain.Entities;
using Hotels.Domain.RepositoryContract;
using MediatR;

namespace Hotels.Application.Query.GetHotelDetails;

public class GetHotelDetailsQueryHandler : IRequestHandler<GetHotelDetailsQuery, HotelDetailsDto>
{
    private readonly IHotelReadRepository _hotelReadRepository;
    private readonly IMediator _mediator;
    
    public GetHotelDetailsQueryHandler(IHotelReadRepository hotelReadRepository, IMediator mediator)
    {
        _hotelReadRepository = hotelReadRepository;
        _mediator = mediator;
    }


    public async Task<HotelDetailsDto> Handle(
        GetHotelDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var id = new HotelId(request.Id);

        var hotel = await _hotelReadRepository
            .GetByHotelIdAsync(id, cancellationToken);
        
        if (hotel == null)
            throw new KeyNotFoundException(
                $"Готель з ID {request.Id} не знайдено.");

        // var minPriceRoom = await _mediator.Send(new GetMinPriceRoomQuery(Id.Value));
        
        
        return new HotelDetailsDto
        (
            hotel.HotelId.Value,  
            hotel.HotelName,
            hotel.Description,
            hotel.ImageUrl,
            hotel.Rating,
            0,
            null
        );
    }


}