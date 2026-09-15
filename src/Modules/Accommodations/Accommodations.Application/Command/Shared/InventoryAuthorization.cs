using Accommodations.Domain.Entities.Hotels;
using Accommodations.Domain.RepositoryContract.Hotels;
using BuildingBlock.Domain;

namespace Accommodations.Application.Command.Shared;

public static class InventoryAuthorization
{
    public static async Task<Result> CheckAsync(IHotelRepository hotels, HotelId hotelId,
        Guid actorId, bool isAdmin, CancellationToken cancellationToken)
    {
        if (actorId == Guid.Empty)
            return Result.Failure(new Error("Inventory.Unauthorized", "Authentication is required."));
        var hotel = await hotels.GetByIdAsync(hotelId, cancellationToken);
        if (hotel is null) return Result.Failure(Error.NotFound("Hotel"));
        return isAdmin || hotel.OwnerUserId == actorId
            ? Result.Success()
            : Result.Failure(new Error("Inventory.Unauthorized", "You do not own this hotel."));
    }
}
