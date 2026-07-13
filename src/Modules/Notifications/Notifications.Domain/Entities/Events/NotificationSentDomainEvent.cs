using BuildingBlock.Domain.Events;
using Notifications.Domain.Entities.Enums;

namespace Notifications.Domain.Entities.Events;

public class NotificationSentDomainEvent : DomainEventBase
{
    public NotificationId NotificationId  { get; set; }
    public string RecipientEmail  { get; set; }
    public NotificationType Type { get; set; }
}
