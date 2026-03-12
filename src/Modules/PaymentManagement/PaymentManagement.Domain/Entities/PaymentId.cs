using BuildingBlock.Domain;

namespace PaymentManagement.Domain.Entities;

public class PaymentId : TypedIdValueBase
{
    public  PaymentId(Guid value) : base(value) {}
}