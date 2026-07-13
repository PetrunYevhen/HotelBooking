using Bookings.Application.Contracts;
using BuildingBlock.Domain;

namespace Bookings.Application.Command.CheckInBooking;

public class CheckInBookingCommand : CommandBase<Result>
{
    public CheckInBookingCommand(Guid bookingId)
    {
        BookingId = bookingId;
    }

    public Guid BookingId { get; set; }
}
