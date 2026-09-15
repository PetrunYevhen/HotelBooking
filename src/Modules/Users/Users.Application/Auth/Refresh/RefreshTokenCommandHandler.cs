using BuildingBlock.Domain;
using MediatR;
using Users.Application.Services;
using Users.Domain.RepositoryContracts;

namespace Users.Application.Auth.Refresh;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthSession>>
{
    private static readonly Error InvalidToken = new("Auth.InvalidRefreshToken", "Refresh token is invalid or expired.");
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenService _refreshTokens;

    public RefreshTokenCommandHandler(IUserRepository userRepository, IRefreshTokenService refreshTokens)
    {
        _userRepository = userRepository;
        _refreshTokens = refreshTokens;
    }

    public async Task<Result<AuthSession>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return Result.Failure<AuthSession>(InvalidToken);

        var presentedHash = _refreshTokens.HashToken(request.RefreshToken);
        var user = await _userRepository.GetByRefreshTokenHashAsync(presentedHash, cancellationToken);
        if (user is null || !user.MatchesActiveRefreshTokenHash(presentedHash, DateTime.UtcNow))
            return Result.Failure<AuthSession>(InvalidToken);

        var replacement = _refreshTokens.CreateToken();
        var setResult = user.SetRefreshTokenHash(_refreshTokens.HashToken(replacement), _refreshTokens.GetExpiryUtc(DateTime.UtcNow));
        if (setResult.IsFailure)
            return Result.Failure<AuthSession>(InvalidToken);

        await _userRepository.UpdateAsync(user, cancellationToken);
        return Result.Success(AuthSession.FromUser(user, replacement));
    }
}
