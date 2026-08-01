using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using Users.Application.Services;

namespace Users.Infrastructure.Services;

/// <summary>Argon2id hashes encoded as argon2id$v=19$m=65536,t=3,p=1$salt$hash.</summary>
public sealed class Argon2idPasswordHashingService : IPasswordHashingService
{
    private const int SaltLength = 16;
    private const int HashLength = 32;
    private const int MemorySizeKb = 65_536;
    private const int Iterations = 3;
    private const int DegreeOfParallelism = 1;

    public string Hash(string password)
    {
        ValidatePassword(password);
        var salt = RandomNumberGenerator.GetBytes(SaltLength);
        var hash = DeriveKey(password, salt);
        return $"argon2id$v=19$m={MemorySizeKb},t={Iterations},p={DegreeOfParallelism}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    public bool Verify(string password, string encodedHash)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(encodedHash))
            return false;

        try
        {
            var parts = encodedHash.Split('$');
            if (parts.Length != 5 || parts[0] != "argon2id" || parts[1] != "v=19" ||
                parts[2] != $"m={MemorySizeKb},t={Iterations},p={DegreeOfParallelism}")
                return false;

            var salt = Convert.FromBase64String(parts[3]);
            var expectedHash = Convert.FromBase64String(parts[4]);
            if (salt.Length != SaltLength || expectedHash.Length != HashLength)
                return false;

            var actualHash = DeriveKey(password, salt);
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static byte[] DeriveKey(string password, byte[] salt)
    {
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            MemorySize = MemorySizeKb,
            Iterations = Iterations,
            DegreeOfParallelism = DegreeOfParallelism
        };
        return argon2.GetBytes(HashLength);
    }

    private static void ValidatePassword(string password)
    {
        if (password is null || password.Length is < 12 or > 128)
            throw new ArgumentException("Password must be between 12 and 128 characters.", nameof(password));
    }
}
