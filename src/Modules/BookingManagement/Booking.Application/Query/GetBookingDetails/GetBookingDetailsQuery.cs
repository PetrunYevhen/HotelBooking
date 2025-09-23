using BookingManagement.Application.Contracts;
using BookingManagement.Domain.Entities;

namespace BookingManagement.Application.Query.GetBookingDetails;

public class GetBookingDetailsQuery : QueryBase<Booking>
{
    public BookingId BookingId { get; }
    
    public GetBookingDetailsQuery(BookingId bookingId)
    {
        BookingId = bookingId;
    }
    
    
}