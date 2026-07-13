using Bookings.Application.Contracts;
using BuildingBlock.Domain;

namespace Bookings.Application.Command.CheckOutBooking;

public class CheckOutBookingCommand : CommandBase<Result>
{
    public CheckOutBookingCommand(Guid bookingId)
    {
        BookingId = bookingId;
    }

    public Guid BookingId { get; set; }
}
