using Payments.Application.Contracts;

namespace Payments.Application.Queries.GetPaymenDetails;

public class GetPaymentDetailsQuery : QueryBase<PaymentDetailsDto>
{
    public Guid PaymentId { get; set; }
    
    public GetPaymentDetailsQuery(Guid paymentId)
    {
        PaymentId = paymentId;
    }
}