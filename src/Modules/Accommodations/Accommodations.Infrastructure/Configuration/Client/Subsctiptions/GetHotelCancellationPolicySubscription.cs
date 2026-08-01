using Accommodations.Application.Query.Hotels.GetCancellationPolicy;
using Infrastructure.Client;

namespace Accommodations.Infrastructure.Configuration.Client.Subsctiptions;

public class GetHotelCancellationPolicySubscription
    : SubscriptionBase<HotelCancellationPolicyRequest, HotelCancellationPolicyDto, GetHotelCancellationPolicyQuery>
{
    public GetHotelCancellationPolicySubscription()
        : base("accommodations/hotel-cancellation-policy") { }

    protected override GetHotelCancellationPolicyQuery MapToQuery(HotelCancellationPolicyRequest request)
        => new() { HotelId = request.HotelId };
}
