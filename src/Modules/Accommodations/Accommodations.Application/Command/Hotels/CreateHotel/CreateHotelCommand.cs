using Accommodations.Application.Contracts;
using Accommodations.Domain.Entities.Hotels.Enums;
using BuildingBlock.Domain;

namespace Accommodations.Application.Command.Hotels.CreateHotel;

public class CreateHotelCommand : CommandBase<Result<Guid>>
{
    public string Name { get; init; }
    public string Description { get; init; }
    public HotelStatus Status { get; init; }
    
    public string Street { get; init; }
    public string City { get; init; }
    public string Country { get; init; }
    public string PostalCode { get; init; }
    
    public TimeOnly CheckIn { get; init; }
    public TimeOnly CheckOut { get; init; } 
}
