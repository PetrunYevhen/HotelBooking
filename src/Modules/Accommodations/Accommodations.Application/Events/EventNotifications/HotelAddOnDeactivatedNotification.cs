using Accommodations.Domain.Entities.HotelAddOns.Events;
using Application.Events;
using Newtonsoft.Json;

namespace Accommodations.Application.Events.EventNotifications;

public sealed class HotelAddOnDeactivatedNotification : DomainNotificationBase<HotelAddOnDeactivatedDomainEvent>
{
    [JsonConstructor]
    public HotelAddOnDeactivatedNotification(HotelAddOnDeactivatedDomainEvent domainEvent, Guid id) : base(domainEvent, id) { }
}
