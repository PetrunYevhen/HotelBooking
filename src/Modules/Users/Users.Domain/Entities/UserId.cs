using BuildingBlock.Domain;

namespace Users.Domain.Entities;

public class UserId : TypedIdValueBase
{
    public UserId(Guid value) : base(value)
    {}

    public static UserId New() => new(Guid.NewGuid());
}
