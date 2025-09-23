using HotelManagement.Domain.Entities;
using HotelManagement.Domain.RepositoryContract;
using MediatR;

namespace HotelManagement.Application.Command.AddNewHotel;

public class AddNewHotelCommandHandler : IRequestHandler<AddNewHotelCommand, Hotel>
{
    private readonly IHotelWriteRepository _hotelWriteRepository;

    public AddNewHotelCommandHandler(IHotelWriteRepository hotelWriteRepository)
    {
        _hotelWriteRepository = hotelWriteRepository;
    }

    public async Task<Hotel> Handle(AddNewHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = new Hotel(
            new HotelId(Guid.NewGuid()),
            request.HotelName,
            request.Description,
            request.ImageUrl,
            request.Rating,
            request.MinRoomPrice 
        );
        return await _hotelWriteRepository.AddHotelAsync(hotel, cancellationToken);
    }
}