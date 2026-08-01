using BuildingBlock.Domain;
using MediatR;
using Users.Application.Services;
using Users.Domain.Entities;
using Users.Domain.RepositoryContracts;
using Users.Domain.ValueObjects;

namespace Users.Application.Auth.Register;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<AuthSession>>
{
    private readonly IUserRepository _users;
    private readonly IPasswordHashingService _passwords;
    private readonly IRefreshTokenService _refreshTokens;

    public RegisterUserCommandHandler(IUserRepository users, IPasswordHashingService passwords, IRefreshTokenService refreshTokens)
    {
        _users = users;
        _passwords = passwords;
        _refreshTokens = refreshTokens;
    }

    public async Task<Result<AuthSession>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length is < 12 or > 128)
            return Result.Failure<AuthSession>(new Error("User.InvalidPassword", "Password must be between 12 and 128 characters."));

        var personalInfo = UserPersonalInfo.Create(request.FirstName, request.LastName, request.PhoneNumber);
        if (personalInfo.IsFailure)
            return Result.Failure<AuthSession>(personalInfo.Error);

        var userResult = User.Create(request.Username, _passwords.Hash(request.Password), request.Email, personalInfo.Value);
        if (userResult.IsFailure)
            return Result.Failure<AuthSession>(userResult.Error);

        var user = userResult.Value;
        if (await _users.GetByUsernameAsync(user.Username, cancellationToken) is not null)
            return Result.Failure<AuthSession>(new Error("User.UsernameAlreadyExists", "A user with this username already exists."));
        if (await _users.GetByEmailAsync(user.Email, cancellationToken) is not null)
            return Result.Failure<AuthSession>(new Error("User.EmailAlreadyExists", "A user with this email address already exists."));

        var refreshToken = _refreshTokens.CreateToken();
        var tokenResult = user.SetRefreshTokenHash(_refreshTokens.HashToken(refreshToken), _refreshTokens.GetExpiryUtc(DateTime.UtcNow));
        if (tokenResult.IsFailure)
            return Result.Failure<AuthSession>(tokenResult.Error);

        await _users.AddAsync(user, cancellationToken);
        return Result.Success(AuthSession.FromUser(user, refreshToken));
    }
}
