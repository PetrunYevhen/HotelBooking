using Accommodations.Application.Contracts;
using BuildingBlock.Domain;

namespace Accommodations.Application.Command.Hotels.AssignHotelOwner;

public sealed class AssignHotelOwnerCommand(Guid hotelId, Guid? ownerUserId) : CommandBase<Result>
{
    public Guid HotelId { get; } = hotelId;
    public Guid? OwnerUserId { get; } = ownerUserId;
}
