using Payments.Application.Contracts;
using Payments.Application.Queries.GetPaymenDetails;

namespace Payments.Application.Queries.GetPaymentByBookingId;

public class GetPaymentByBookingIdQuery : QueryBase<PaymentDetailsDto>
{
    public Guid BookingId { get; set; }

    public GetPaymentByBookingIdQuery(Guid bookingId)
    {
        BookingId = bookingId;
    }
}
