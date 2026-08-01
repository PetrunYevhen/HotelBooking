using Users.Domain.Entities;

namespace Users.Application.Auth;

/// <summary>Contains a raw refresh token only while it is being returned to the API cookie writer.</summary>
public sealed record AuthSession(Guid UserId, string Username, string Role, string RefreshToken)
{
    public static AuthSession FromUser(User user, string refreshToken) =>
        new(user.UserId.Value, user.Username, user.Role.ToString(), refreshToken);
}
