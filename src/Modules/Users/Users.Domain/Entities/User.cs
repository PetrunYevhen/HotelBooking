using System.Security.Cryptography;
using System.Text;
using BuildingBlock.Domain;
using Users.Domain.Enums;
using Users.Domain.ValueObjects;

namespace Users.Domain.Entities;

public class User : Entity, IAggregateRoot
{
    public UserId UserId { get; private set; } = null!;
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public UserPersonalInfo PersonalInfo { get; private set; } = null!;
    public Role Role { get; private set; }
    public string? RefreshTokenHash { get; private set; }
    public DateTime? RefreshTokenExpiresAt { get; private set; }

    private User()
    {
    }

    private User(string username, string passwordHash, string email, UserPersonalInfo personalInfo)
    {
        UserId = UserId.New();
        Username = username;
        PasswordHash = passwordHash;
        Email = email;
        PersonalInfo = personalInfo;
        Role = Role.User;
    }

    public static Result<User> Create(
        string username,
        string passwordHash,
        string email,
        UserPersonalInfo personalInfo)
    {
        var validationResult = ValidateUserDetails(username, passwordHash, email, personalInfo);
        if (validationResult.IsFailure)
            return Result.Failure<User>(validationResult.Error);

        return Result.Success(new User(username.Trim(), passwordHash, NormalizeEmail(email), personalInfo));
    }

    public Result ChangeUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return Result.Failure(new Error("User.InvalidUsername", "Username is required."));

        Username = username.Trim();
        return Result.Success();
    }

    public Result ChangeEmail(string email)
    {
        if (!IsValidEmail(email))
            return Result.Failure(new Error("User.InvalidEmail", "A valid email address is required."));

        Email = NormalizeEmail(email);
        return Result.Success();
    }

    public Result ChangePasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            return Result.Failure(new Error("User.InvalidPasswordHash", "Password hash is required."));

        PasswordHash = passwordHash;
        return Result.Success();
    }

    public Result UpdatePersonalInfo(UserPersonalInfo personalInfo)
    {
        if (personalInfo is null)
            return Result.Failure(new Error("User.InvalidPersonalInfo", "Personal information is required."));

        PersonalInfo = personalInfo;
        return Result.Success();
    }

    public Result ChangeRole(Role role)
    {
        if (!IsAssignableRole(role))
            return Result.Failure(new Error("User.InvalidRole", "A valid user role is required."));

        Role = role;
        return Result.Success();
    }

    public Result PromoteToAdmin()
    {
        if (Role == Role.Admin)
            return Result.Failure(new Error("User.AlreadyAdmin", "User already has the administrator role."));

        Role = Role.Admin;
        return Result.Success();
    }

    public Result SetRefreshTokenHash(string refreshTokenHash, DateTime expiresAtUtc)
    {
        if (string.IsNullOrWhiteSpace(refreshTokenHash))
            return Result.Failure(new Error("User.InvalidRefreshTokenHash", "Refresh token hash is required."));

        if (expiresAtUtc.Kind != DateTimeKind.Utc || expiresAtUtc <= DateTime.UtcNow)
            return Result.Failure(new Error("User.InvalidRefreshTokenExpiry", "Refresh token expiry must be a future UTC timestamp."));

        RefreshTokenHash = refreshTokenHash;
        RefreshTokenExpiresAt = expiresAtUtc;
        return Result.Success();
    }

    public void RevokeRefreshToken()
    {
        RefreshTokenHash = null;
        RefreshTokenExpiresAt = null;
    }

    public bool MatchesActiveRefreshTokenHash(string refreshTokenHash, DateTime utcNow)
    {
        if (string.IsNullOrWhiteSpace(refreshTokenHash) || string.IsNullOrWhiteSpace(RefreshTokenHash) ||
            !RefreshTokenExpiresAt.HasValue || utcNow.Kind != DateTimeKind.Utc || RefreshTokenExpiresAt.Value <= utcNow)
            return false;

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(RefreshTokenHash),
            Encoding.UTF8.GetBytes(refreshTokenHash));
    }

    public bool HasActiveRefreshToken(DateTime utcNow) =>
        !string.IsNullOrWhiteSpace(RefreshTokenHash)
        && RefreshTokenExpiresAt.HasValue
        && utcNow.Kind == DateTimeKind.Utc
        && RefreshTokenExpiresAt.Value > utcNow;

    private static Result ValidateUserDetails(
        string username,
        string passwordHash,
        string email,
        UserPersonalInfo personalInfo)
    {
        if (string.IsNullOrWhiteSpace(username))
            return Result.Failure(new Error("User.InvalidUsername", "Username is required."));

        if (string.IsNullOrWhiteSpace(passwordHash))
            return Result.Failure(new Error("User.InvalidPasswordHash", "Password hash is required."));

        if (!IsValidEmail(email))
            return Result.Failure(new Error("User.InvalidEmail", "A valid email address is required."));

        if (personalInfo is null)
            return Result.Failure(new Error("User.InvalidPersonalInfo", "Personal information is required."));

        return Result.Success();
    }

    private static bool IsAssignableRole(Role role) =>
        Enum.IsDefined(role) && role != Role.Unknown;

    private static bool IsValidEmail(string email) =>
        !string.IsNullOrWhiteSpace(email)
        && System.Net.Mail.MailAddress.TryCreate(email.Trim(), out _);

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}
