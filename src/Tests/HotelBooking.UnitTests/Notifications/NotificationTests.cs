using Notifications.Domain.Entities;
using Notifications.Domain.Entities.Enums;
using Xunit;

namespace HotelBooking.UnitTests.Notifications;

public sealed class NotificationTests
{
    [Fact]
    public void Create_WhenPending_DoesNotSetSentAt()
    {
        var notification = Notification.Create(
            Guid.NewGuid(), "guest@example.com", NotificationType.Email,
            "Confirmed", "Your booking is confirmed.").Value;

        Assert.Equal(NotificationStatus.Pending, notification.Status);
        Assert.Null(notification.SentAt);
    }

    [Fact]
    public void MarkAsSent_SetsSentAt()
    {
        var notification = Notification.Create(
            Guid.NewGuid(), "guest@example.com", NotificationType.Email,
            "Confirmed", "Your booking is confirmed.").Value;

        var result = notification.MarkAsSent();

        Assert.True(result.IsSuccess);
        Assert.NotNull(notification.SentAt);
    }
}
