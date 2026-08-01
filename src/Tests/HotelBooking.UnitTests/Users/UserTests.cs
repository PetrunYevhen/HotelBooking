using Users.Domain.Entities;
using Users.Domain.Enums;
using Users.Domain.ValueObjects;
using Xunit;

namespace HotelBooking.UnitTests.Users;

public sealed class UserTests
{
    [Fact]
    public void Create_WithValidDetails_CreatesDefaultUser()
    {
        var result = User.Create("yevhen", "password-hash", "yevhen@example.com", ValidPersonalInfo());

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value.UserId.Value);
        Assert.Equal(Role.User, result.Value.Role);
    }

    [Fact]
    public void ChangeRole_ToUnknown_ReturnsFailureAndKeepsCurrentRole()
    {
        var user = ValidUser();

        var result = user.ChangeRole(Role.Unknown);

        Assert.True(result.IsFailure);
        Assert.Equal("User.InvalidRole", result.Error.Code);
        Assert.Equal(Role.User, user.Role);
    }

    [Fact]
    public void PromoteToAdmin_WhenUserHasDefaultRole_ChangesRoleToAdmin()
    {
        var user = ValidUser();

        var result = user.PromoteToAdmin();

        Assert.True(result.IsSuccess);
        Assert.Equal(Role.Admin, user.Role);
    }

    [Fact]
    public void UpdatePersonalInfo_WithValidInfo_ReplacesPersonalInfo()
    {
        var user = ValidUser();
        var personalInfo = UserPersonalInfo.Create("Olena", "Petrun", "+380671234567").Value;

        var result = user.UpdatePersonalInfo(personalInfo);

        Assert.True(result.IsSuccess);
        Assert.Equal("Olena", user.PersonalInfo.FirstName);
        Assert.Equal("+380671234567", user.PersonalInfo.PhoneNumber);
    }

    [Fact]
    public void SetRefreshTokenHash_WithFutureUtcExpiry_StoresAnActiveToken()
    {
        var user = ValidUser();
        var expiresAt = DateTime.UtcNow.AddHours(1);

        var result = user.SetRefreshTokenHash("refresh-token-hash", expiresAt);

        Assert.True(result.IsSuccess);
        Assert.Equal("refresh-token-hash", user.RefreshTokenHash);
        Assert.Equal(expiresAt, user.RefreshTokenExpiresAt);
        Assert.True(user.MatchesActiveRefreshTokenHash("refresh-token-hash", DateTime.UtcNow));
    }

    [Fact]
    public void RevokeRefreshToken_ClearsTokenAndExpiry()
    {
        var user = ValidUser();
        user.SetRefreshTokenHash("refresh-token-hash", DateTime.UtcNow.AddHours(1));

        user.RevokeRefreshToken();

        Assert.Null(user.RefreshTokenHash);
        Assert.Null(user.RefreshTokenExpiresAt);
        Assert.False(user.HasActiveRefreshToken(DateTime.UtcNow));
    }

    private static User ValidUser() =>
        User.Create("yevhen", "password-hash", "yevhen@example.com", ValidPersonalInfo()).Value;

    private static UserPersonalInfo ValidPersonalInfo() =>
        UserPersonalInfo.Create("Yevhen", "Petrun", "+380501234567").Value;
}
