using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.RepositoryContract.Hotels;
using BuildingBlock.Domain;
using MediatR;
using SharedKernel.ValueObjects;

namespace Accommodations.Application.Command.Hotels.CreateHotel;

public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Result<Guid>>
{
    private readonly IHotelRepository _hotelWriteRepository;

    public CreateHotelCommandHandler(IHotelRepository hotelWriteRepository)
    {
        _hotelWriteRepository = hotelWriteRepository;
    }

    public async Task<Result<Guid>> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        var addressResult = Address.Create(request.City, request.Street, request.Country, request.PostalCode);
        if (addressResult.IsFailure)
            return Result.Failure<Guid>(addressResult.Error);

        var hoursResult = OperatingHours.Create(request.CheckIn, request.CheckOut);
        if (hoursResult.IsFailure)
            return Result.Failure<Guid>(hoursResult.Error);

        var hotelResult = Hotel.Create(
            request.Name, 
            request.Description, 
            request.Status,
            addressResult.Value, 
            hoursResult.Value);
        
        if (hotelResult.IsFailure)
            return Result.Failure<Guid>(hotelResult.Error);

        var hotel = await _hotelWriteRepository.AddAsync(hotelResult.Value, cancellationToken);
        return Result.Success(hotel.HotelId.Value);
    }
}