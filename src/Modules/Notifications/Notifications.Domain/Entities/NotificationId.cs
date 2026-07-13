using BuildingBlock.Domain;

namespace Notifications.Domain.Entities;

public class NotificationId : TypedIdValueBase
{
    public static NotificationId New() => new(Guid.NewGuid());

    public NotificationId(Guid value) : base(value) { }
}
