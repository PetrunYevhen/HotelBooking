using Users.Application.Contracts;
using Users.Domain.Entities;

namespace Users.Application.Query.GetUserById;

public sealed class GetUserByIdQuery : QueryBase<UserDto?>
{
    public GetUserByIdQuery(UserId userId)
    {
        UserId = userId;
    }

    public UserId UserId { get; }
}
