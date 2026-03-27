using PaymentManagement.Application.Contracts;
using PaymentManagement.Domain.Entities;

namespace PaymentManagement.Application.Queries.GetPaymentById;

public class GetByIdQuery : QueryBase<Payment>
{
    public Guid PaymentId { get; set; }
    
    public GetByIdQuery(Guid paymentId)
    {
        PaymentId = paymentId;
    }
}