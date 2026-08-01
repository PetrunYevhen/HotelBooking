using Bookings.Application.Contracts;
using BuildingBlock.Domain;

namespace Bookings.Application.Command.CancelBooking;

public class CancelBookingCommand : CommandBase<Result>
{
    public CancelBookingCommand(Guid bookingId, Guid userId, string? reason = null)
    {
        BookingId = bookingId;
        UserId = userId;
        Reason = reason;
    }

    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }
    public string? Reason { get; set; }
}
