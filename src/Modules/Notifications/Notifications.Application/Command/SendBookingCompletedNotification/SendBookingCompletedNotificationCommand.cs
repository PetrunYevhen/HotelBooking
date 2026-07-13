using BuildingBlock.Domain;
using Notifications.Application.Contracts;

namespace Notifications.Application.Command.SendBookingCompletedNotification;

public class SendBookingCompletedNotificationCommand : CommandBase<Result>
{
    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }
    public string RecipientEmail { get; set; }
}