using BuildingBlock.Domain.Events;
using Notifications.Domain.Entities.Enums;

namespace Notifications.Domain.Entities.Events;

public class NotificationFailedDomainEvent : DomainEventBase
{
    public NotificationId NotificationId  { get; set; }
    public string RecipientEmail { get; set; }
    public NotificationType Type { get; set; }
    public string FailureReason  { get; set; }
}
