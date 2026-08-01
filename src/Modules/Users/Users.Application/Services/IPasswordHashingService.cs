namespace Users.Application.Services;

public interface IPasswordHashingService
{
    string Hash(string password);
    bool Verify(string password, string encodedHash);
}
