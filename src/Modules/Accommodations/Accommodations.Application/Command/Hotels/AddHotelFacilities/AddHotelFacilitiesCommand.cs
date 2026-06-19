using Accommodations.Application.Command.Shared;
using Accommodations.Application.Contracts;
using BuildingBlock.Domain;

namespace Accommodations.Application.Command.Hotels.AddHotelFacilities;

public class AddHotelFacilitiesCommand : CommandBase<Result>
{
    public Guid HotelId { get; init; }
    public List<FacilityRequest> Facilities { get; set; }
}