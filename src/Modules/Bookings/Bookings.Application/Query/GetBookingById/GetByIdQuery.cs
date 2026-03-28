using Bookings.Application.Contracts;

namespace Bookings.Application.Query.GetBookingById;

public class GetByIdQuery : QueryBase<BookingDto>
{
    public GetByIdQuery(Guid bookingId)
    {
        BookingId = bookingId;
    }

    public Guid BookingId { get; init; }
}