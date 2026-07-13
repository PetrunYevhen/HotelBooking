using Bookings.Application.Contracts;
using Bookings.Application.Query.GetBookingById;

namespace Bookings.Application.Query.GetBookingsByUserId;

public class GetBookingsByUserIdQuery : QueryBase<List<BookingDto>>
{
    public GetBookingsByUserIdQuery(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}
