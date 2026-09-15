using Application.Events;
using Newtonsoft.Json;
using Users.Domain.Entities;

namespace Users.Application.Events;

public sealed class HotelierOnboardingRequestedNotification : DomainNotificationBase<HotelierOnboardingRequestedDomainEvent>
{
    [JsonConstructor]
    public HotelierOnboardingRequestedNotification(HotelierOnboardingRequestedDomainEvent domainEvent, Guid id) : base(domainEvent, id) { }
}
