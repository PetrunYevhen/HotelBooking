using BuildingBlock.Domain;
using Users.Application.Contracts;

namespace Users.Application.Command.Users.CreateUser;

public class CreateUserCommand : CommandBase<Result<Guid>>
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
}
