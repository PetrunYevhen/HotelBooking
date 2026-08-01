using Bookings.Application.Contracts;

namespace Bookings.Application.Query.GetBookingUserId;

public sealed class GetBookingUserIdQuery(Guid bookingId) : QueryBase<Guid?>
{
    public Guid BookingId { get; } = bookingId;
}
