using BuildingBlock.Domain;
using MediatR;
using Users.Application.Services;
using Users.Domain.RepositoryContracts;

namespace Users.Application.Auth.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthSession>>
{
    private static readonly Error InvalidCredentials = new("Auth.InvalidCredentials", "Invalid credentials.");
    private readonly IUserRepository _users;
    private readonly IPasswordHashingService _passwords;
    private readonly IRefreshTokenService _refreshTokens;

    public LoginCommandHandler(IUserRepository users, IPasswordHashingService passwords, IRefreshTokenService refreshTokens)
    {
        _users = users;
        _passwords = passwords;
        _refreshTokens = refreshTokens;
    }

    public async Task<Result<AuthSession>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var identifier = request.UsernameOrEmail?.Trim();
        if (string.IsNullOrWhiteSpace(identifier))
            return Result.Failure<AuthSession>(InvalidCredentials);
        var user = identifier.Contains('@')
            ? await _users.GetByEmailAsync(identifier.ToLowerInvariant(), cancellationToken)
            : await _users.GetByUsernameAsync(identifier, cancellationToken);

        if (user is null || !_passwords.Verify(request.Password ?? string.Empty, user.PasswordHash))
            return Result.Failure<AuthSession>(InvalidCredentials);

        var refreshToken = _refreshTokens.CreateToken();
        var setResult = user.SetRefreshTokenHash(_refreshTokens.HashToken(refreshToken), _refreshTokens.GetExpiryUtc(DateTime.UtcNow));
        if (setResult.IsFailure)
            return Result.Failure<AuthSession>(InvalidCredentials);

        await _users.UpdateAsync(user, cancellationToken);
        return Result.Success(AuthSession.FromUser(user, refreshToken));
    }
}
