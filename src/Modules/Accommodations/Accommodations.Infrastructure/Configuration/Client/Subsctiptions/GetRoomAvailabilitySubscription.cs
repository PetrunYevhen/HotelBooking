using Accommodations.Application.Query.Rooms.GetRoomAvailability;
using Infrastructure.Client;

namespace Accommodations.Infrastructure.Configuration.Client.Subsctiptions;

public class GetRoomAvailabilitySubscription 
    
    : SubscriptionBase<RoomAvailabilityRequest, bool, GetRoomAvailabilityQuery>
{
    public GetRoomAvailabilitySubscription() 
        : base("accommodations/room-available") { }

    protected override GetRoomAvailabilityQuery MapToQuery(RoomAvailabilityRequest request) 
        => new(request.RoomId);
}