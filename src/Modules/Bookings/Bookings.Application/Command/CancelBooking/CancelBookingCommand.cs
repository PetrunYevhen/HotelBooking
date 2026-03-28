using Bookings.Application.Contracts;

namespace Bookings.Application.Command.CancelBooking;

public class CancelBookingCommand : CommandBase<Guid>
{
    public CancelBookingCommand(Guid bookingId)
    {
        BookingId = bookingId;
    }

    public Guid BookingId { get; set; }
}