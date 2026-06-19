using Accommodations.Application.Query.Rooms.GetRoomPrice;
using BuildingBlock.Domain;
using Infrastructure.Client;
using SharedKernel.ValueObjects;

namespace Accommodations.Infrastructure.Configuration.Client.Subsctiptions;

public class GetRoomPriceSubscription
    : SubscriptionBase<RoomPriceRequest, Result<Money>, GetRoomPriceQuery>
{
    public GetRoomPriceSubscription()
        : base("accommodations/room-price") { }

    protected override GetRoomPriceQuery MapToQuery(RoomPriceRequest request)
        => new(request.RoomId, request.Start, request.End);
}