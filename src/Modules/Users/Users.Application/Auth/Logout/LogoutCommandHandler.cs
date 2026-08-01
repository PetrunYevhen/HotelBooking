using BuildingBlock.Domain;
using MediatR;
using Users.Application.Services;
using Users.Domain.RepositoryContracts;

namespace Users.Application.Auth.Logout;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
{
    private readonly IUserRepository _users;
    private readonly IRefreshTokenService _refreshTokens;

    public LogoutCommandHandler(IUserRepository users, IRefreshTokenService refreshTokens)
    {
        _users = users;
        _refreshTokens = refreshTokens;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return Result.Success();

        var hash = _refreshTokens.HashToken(request.RefreshToken);
        var user = await _users.GetByRefreshTokenHashAsync(hash, cancellationToken);
        if (user is not null && user.MatchesActiveRefreshTokenHash(hash, DateTime.UtcNow))
        {
            user.RevokeRefreshToken();
            await _users.UpdateAsync(user, cancellationToken);
        }

        return Result.Success();
    }
}
