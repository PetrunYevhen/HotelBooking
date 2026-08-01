using BuildingBlock.Domain;
using Users.Application.Contracts;

namespace Users.Application.Auth.Refresh;

public sealed class RefreshTokenCommand : CommandBase<Result<AuthSession>>
{
    public string RefreshToken { get; init; } = string.Empty;
}
