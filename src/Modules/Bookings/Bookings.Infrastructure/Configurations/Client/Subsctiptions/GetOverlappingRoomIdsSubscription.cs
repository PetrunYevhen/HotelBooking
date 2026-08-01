using Bookings.Application.Query.GetOverlappingRoomIds;
using Infrastructure.Client;

namespace Bookings.Infrastructure.Configurations.Client.Subsctiptions;

public class GetOverlappingRoomIdsSubscription
    : SubscriptionBase<OverlappingRoomIdsRequest, List<Guid>, GetOverlappingRoomIdsQuery>
{
    public GetOverlappingRoomIdsSubscription()
        : base("bookings/overlapping-room-ids") { }

    protected override GetOverlappingRoomIdsQuery MapToQuery(OverlappingRoomIdsRequest request)
        => new() { RoomIds = request.RoomIds, CheckIn = request.CheckIn, CheckOut = request.CheckOut };
}
