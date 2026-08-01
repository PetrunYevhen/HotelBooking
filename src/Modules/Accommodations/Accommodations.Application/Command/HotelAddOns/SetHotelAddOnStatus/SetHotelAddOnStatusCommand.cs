using Accommodations.Application.Contracts;
using BuildingBlock.Domain;

namespace Accommodations.Application.Command.HotelAddOns.SetHotelAddOnStatus;

public sealed class SetHotelAddOnStatusCommand : CommandBase<Result>
{
    public SetHotelAddOnStatusCommand(Guid hotelId, Guid hotelAddOnId, bool isActive)
    {
        HotelId = hotelId;
        HotelAddOnId = hotelAddOnId;
        IsActive = isActive;
    }
    public Guid HotelId { get; }
    public Guid HotelAddOnId { get; }
    public bool IsActive { get; }
}
