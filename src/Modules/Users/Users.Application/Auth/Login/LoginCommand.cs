using BuildingBlock.Domain;
using Users.Application.Contracts;

namespace Users.Application.Auth.Login;

public sealed class LoginCommand : CommandBase<Result<AuthSession>>
{
    public string UsernameOrEmail { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
