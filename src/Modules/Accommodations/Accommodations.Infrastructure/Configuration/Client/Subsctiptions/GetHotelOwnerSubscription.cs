using Accommodations.Application.Query.Hotels.GetHotelOwner;
using Infrastructure.Client;

namespace Accommodations.Infrastructure.Configuration.Client.Subsctiptions;

public class GetHotelOwnerSubscription
    : SubscriptionBase<HotelOwnerRequest, Guid?, GetHotelOwnerQuery>
{
    public GetHotelOwnerSubscription()
        : base("accommodations/hotel-owner") { }

    protected override GetHotelOwnerQuery MapToQuery(HotelOwnerRequest request)
        => new(request.HotelId);
}
