using Accommodations.Domain.Entities.HotelAddOns.Events;
using Application.Events;
using Newtonsoft.Json;

namespace Accommodations.Application.Events.EventNotifications;

public sealed class HotelAddOnUpsertedNotification : DomainNotificationBase<HotelAddOnUpsertedDomainEvent>
{
    [JsonConstructor]
    public HotelAddOnUpsertedNotification(HotelAddOnUpsertedDomainEvent domainEvent, Guid id) : base(domainEvent, id) { }
}
