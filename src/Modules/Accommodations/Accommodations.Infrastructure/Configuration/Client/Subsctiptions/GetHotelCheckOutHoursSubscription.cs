using Accommodations.Application.Query.Hotels.GetCheckOutHours;
using Infrastructure.Client;

namespace Accommodations.Infrastructure.Configuration.Client.Subsctiptions;

public class GetHotelCheckOutHoursSubscription
    : SubscriptionBase<HotelCheckOutHoursRequest, int, GetHotelCheckOutHoursQuery>
{
    public GetHotelCheckOutHoursSubscription()
        : base("accommodations/hotel-checkout-hours") { }

    protected override GetHotelCheckOutHoursQuery MapToQuery(HotelCheckOutHoursRequest request)
        => new() { HotelId = request.HotelId };
}