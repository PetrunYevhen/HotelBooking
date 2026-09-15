using Accommodations.Application.Query.Rooms.GetRoomAvailability;
using Infrastructure.Client;
using SharedKernel.Contracts;

namespace Accommodations.Infrastructure.Configuration.Client.Subsctiptions;

public sealed class GetRoomBookingDetailsSubscription()
    : SubscriptionBase<RoomAvailabilityRequest, RoomBookingDetails?, GetRoomBookingDetailsQuery>("accommodations/room-booking-details")
{
    protected override GetRoomBookingDetailsQuery MapToQuery(RoomAvailabilityRequest request) => new(request.RoomId);
}
