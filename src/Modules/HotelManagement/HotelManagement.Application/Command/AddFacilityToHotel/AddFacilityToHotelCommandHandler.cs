using HotelManagement.Domain.Entities;
using HotelManagement.Domain.RepositoryContract;
using MediatR;

namespace HotelManagement.Application.Command.AddFacilityToHotel;

public class AddFacilityToHotelCommandHandler : IRequestHandler<AddFacilityToHotelCommand, bool>
{
    private readonly IHotelWriteRepository _hotelWriteRepository;

    public AddFacilityToHotelCommandHandler(IHotelWriteRepository hotelWriteRepository)
    {
        _hotelWriteRepository = hotelWriteRepository;
    }

    public async Task<bool> Handle(AddFacilityToHotelCommand request, CancellationToken cancellationToken)
    {
        var hotelId = new HotelId(request.HotelId);
        return await _hotelWriteRepository.AddFacilitiesToHotelAsync(hotelId, request.FacilityIds, cancellationToken);
        
    }
}