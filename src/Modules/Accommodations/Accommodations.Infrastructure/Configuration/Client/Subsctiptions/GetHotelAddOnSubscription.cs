using Accommodations.Application.Query.HotelAddOns;
using Infrastructure.Client;

namespace Accommodations.Infrastructure.Configuration.Client.Subsctiptions;

public sealed record HotelAddOnRequest(Guid HotelId, Guid HotelAddOnId);

public sealed class GetHotelAddOnSubscription : SubscriptionBase<HotelAddOnRequest, HotelAddOnDto?, GetHotelAddOnQuery>
{
    public GetHotelAddOnSubscription() : base("accommodations/hotel-add-on") { }
    protected override GetHotelAddOnQuery MapToQuery(HotelAddOnRequest request) => new(request.HotelId, request.HotelAddOnId);
}
