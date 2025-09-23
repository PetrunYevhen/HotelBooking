using HotelManagement.Domain.Entities;
using HotelManagement.Domain.RepositoryContract;
using MediatR;

namespace HotelManagement.Application.Command.AddRoomToHotel;

public class AddRoomToHotelCommandHandler : IRequestHandler<AddRoomToHotelCommand, bool>
{
    private readonly IHotelWriteRepository _hotelWriteRepository;

    public AddRoomToHotelCommandHandler(IHotelWriteRepository hotelWriteRepository)
    {
        _hotelWriteRepository = hotelWriteRepository;
    }

    public async Task<bool> Handle(AddRoomToHotelCommand request, CancellationToken cancellationToken)
    {
        var hotelId = new HotelId(request.HotelId);
        return await _hotelWriteRepository
            .AddRoomToHotelAsync(hotelId, request.RoomId, cancellationToken);
    }
}