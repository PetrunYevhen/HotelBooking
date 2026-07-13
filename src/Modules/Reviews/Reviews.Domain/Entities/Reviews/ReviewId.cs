using BuildingBlock.Domain;

namespace Reviews.Domain.Entities.Reviews;

public class ReviewId : TypedIdValueBase
{
    public static ReviewId New() => new(Guid.NewGuid());

    public ReviewId(Guid value) : base(value) { }
}
