using BuildingBlock.Domain;
using Notifications.Application.Contracts;

namespace Notifications.Application.Command.SendBookingConfirmedNotification;

public class SendBookingConfirmedNotificationCommand : CommandBase<Result>
{
    public Guid UserId { get; init; }
    public string RecipientEmail { get; init; }
    public Guid BookingId { get; init; }
    public DateTime CheckInDate { get; init; }
    public DateTime CheckOutDate { get; init; }
}