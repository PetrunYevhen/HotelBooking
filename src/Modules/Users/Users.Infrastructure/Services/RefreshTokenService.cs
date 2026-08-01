using System.Security.Cryptography;
using System.Text;
using Users.Application.Services;

namespace Users.Infrastructure.Services;

public sealed class RefreshTokenService : IRefreshTokenService
{
    public const int LifetimeDays = 30;

    public string CreateToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    public string HashToken(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);
        return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
    }

    public DateTime GetExpiryUtc(DateTime utcNow)
    {
        if (utcNow.Kind != DateTimeKind.Utc)
            throw new ArgumentException("Timestamp must be UTC.", nameof(utcNow));
        return utcNow.AddDays(LifetimeDays);
    }
}
