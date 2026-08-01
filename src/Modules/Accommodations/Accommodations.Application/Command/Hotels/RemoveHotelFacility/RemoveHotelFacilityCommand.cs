using Accommodations.Application.Contracts;
using BuildingBlock.Domain;

namespace Accommodations.Application.Command.Hotels.RemoveHotelFacility;

public sealed class RemoveHotelFacilityCommand(Guid hotelId, Guid facilityId) : CommandBase<Result>
{
    public Guid HotelId { get; } = hotelId;
    public Guid FacilityId { get; } = facilityId;
}
