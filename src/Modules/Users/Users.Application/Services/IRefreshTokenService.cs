namespace Users.Application.Services;

public interface IRefreshTokenService
{
    string CreateToken();
    string HashToken(string token);
    DateTime GetExpiryUtc(DateTime utcNow);
}
