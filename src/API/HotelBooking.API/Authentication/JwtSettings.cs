namespace HotelBooking.API.Authentication;

public sealed class JwtSettings
{
    public const string SectionName = "Jwt";
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SigningKey { get; init; } = string.Empty;
    public int AccessTokenMinutes { get; init; } = 15;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Issuer) || string.IsNullOrWhiteSpace(Audience))
            throw new InvalidOperationException("JWT issuer and audience must be configured.");
        if (System.Text.Encoding.UTF8.GetByteCount(SigningKey) < 32)
            throw new InvalidOperationException("Jwt:SigningKey must contain at least 256 bits (32 UTF-8 bytes).");
        if (AccessTokenMinutes is < 1 or > 60)
            throw new InvalidOperationException("Jwt:AccessTokenMinutes must be between 1 and 60.");
    }
}
