using Infrastructure.Client;
using Rooms.Application.Query.CalculatePrice;

namespace Rooms.Infrastructure.Configuration.Client.Subscriptions;

public class RoomPriceSubscription : SubscriptionBase<RoomPriceRequestDto, decimal, CalculatePriceQuery>
{
    public RoomPriceSubscription() : base("room-price")
    {
    }

    protected override CalculatePriceQuery MapToQuery(RoomPriceRequestDto request)
    {
        return new CalculatePriceQuery(request.RoomId, request.CheckIn,  request.CheckOut);
    }
}