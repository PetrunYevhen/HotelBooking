using Bookings.Application.Contracts;
using BuildingBlock.Domain;

namespace Bookings.Application.Command.ConfirmBooking;

public class ConfirmBookingCommand : CommandBase<Result>
{
    public ConfirmBookingCommand(Guid bookingId, decimal totalAmount, string currency, DateTime completedAt)
    {
        BookingId = bookingId;
        TotalAmount = totalAmount;
        Currency = currency;
        CompletedAt = completedAt;
    }

    public Guid BookingId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; }
    public DateTime CompletedAt { get; set; }
}