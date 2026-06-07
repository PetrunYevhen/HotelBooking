using BuildingBlock.Domain;

namespace Accommodations.Domain.Entities.Pricing;

public class PricingId : TypedIdValueBase
{
    public PricingId(Guid value) : base(value) { }
    public static PricingId New() => new(Guid.NewGuid());
}