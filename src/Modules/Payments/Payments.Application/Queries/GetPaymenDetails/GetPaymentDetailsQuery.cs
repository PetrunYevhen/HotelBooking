using Payments.Application.Contracts;
using Payments.Domain.Entities;

namespace Payments.Application.Queries.GetPaymentById;

public class GetByIdQuery : QueryBase<PaymentDetailsDto>
{
    public Guid PaymentId { get; set; }
    
    public GetByIdQuery(Guid paymentId)
    {
        PaymentId = paymentId;
    }
}