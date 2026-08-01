using BuildingBlock.Domain;
using Users.Application.Contracts;

namespace Users.Application.Auth.Logout;

public sealed class LogoutCommand : CommandBase<Result>
{
    public string RefreshToken { get; init; } = string.Empty;
}
