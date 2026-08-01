using Users.Domain.Enums;

namespace Users.Application.Services;

public interface IJwtTokenService
{
    string CreateAccessToken(Guid userId, string username, Role role);
}
