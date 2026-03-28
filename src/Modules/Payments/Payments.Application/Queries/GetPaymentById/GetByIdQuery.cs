using Payments.Application.Contracts;
using Payments.Domain.Entities;

namespace Payments.Application.Queries.GetPaymentById;

public class GetByIdQuery : QueryBase<Payment>
{
    public Guid PaymentId { get; set; }
    
    public GetByIdQuery(Guid paymentId)
    {
        PaymentId = paymentId;
    }
}