using BookingManagement.Application.Contracts;

namespace BookingManagement.Application.Query.GetBookingById;

public class GetByIdQuery : QueryBase<BookingDto>
{
    public GetByIdQuery(Guid bookingId)
    {
        BookingId = bookingId;
    }

    public Guid BookingId { get; init; }
}