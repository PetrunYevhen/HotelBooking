using Users.Infrastructure.Services;
using Xunit;

namespace HotelBooking.UnitTests.Users;

public sealed class Argon2idPasswordHashingServiceTests
{
    [Fact]
    public void Hash_StoresEncodedArgon2idHash_AndOnlyVerifiesTheCorrectPassword()
    {
        var service = new Argon2idPasswordHashingService();
        const string password = "correct horse battery staple";

        var hash = service.Hash(password);

        Assert.NotEqual(password, hash);
        Assert.StartsWith("argon2id$v=19$m=65536,t=3,p=1$", hash);
        Assert.True(service.Verify(password, hash));
        Assert.False(service.Verify("incorrect password", hash));
    }
}
