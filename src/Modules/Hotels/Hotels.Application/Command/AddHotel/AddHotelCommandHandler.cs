using Hotels.Domain.Entities;
using Hotels.Domain.RepositoryContract;
using MediatR;

namespace Hotels.Application.Command.AddHotel;

public class AddHotelCommandHandler : IRequestHandler<AddHotelCommand, Hotel>
{
    private readonly IHotelWriteRepository _hotelWriteRepository;

    public AddHotelCommandHandler(IHotelWriteRepository hotelWriteRepository)
    {
        _hotelWriteRepository = hotelWriteRepository;
    }

    public async Task<Hotel> Handle(AddHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = new Hotel(
            new HotelId(Guid.NewGuid()),
            request.HotelName,
            request.Description,
            request.ImageUrl,
            request.Rating,
            request.MinRoomPrice 
        );
        return await _hotelWriteRepository.AddAsync(hotel, cancellationToken);
    }
}