using Accommodations.Domain.Entities.HotelierApplications;
using Application.Events;
using Newtonsoft.Json;

namespace Accommodations.Application.Events.EventNotifications;

public sealed class HotelierApplicationApprovedNotification : DomainNotificationBase<HotelierApplicationApprovedDomainEvent>
{
    [JsonConstructor]
    public HotelierApplicationApprovedNotification(HotelierApplicationApprovedDomainEvent domainEvent, Guid id) : base(domainEvent, id) { }
}
