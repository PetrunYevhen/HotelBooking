using Accommodations.Domain.Entities.HotelierApplications;
using SharedKernel.Contracts;
using Xunit;

namespace HotelBooking.UnitTests.Accommodations;

public sealed class HotelierApplicationTests
{
    [Fact]
    public void Create_WithMissingBusinessDetails_ReturnsFailure()
    {
        var result = HotelierApplication.Create(new AccountId(Guid.NewGuid()), "", "REG-1", null, "business@example.com", "+380501234567", "Stayora Inn", "1 Main Street");

        Assert.True(result.IsFailure);
        Assert.Equal("HotelierApplication.InvalidDetails", result.Error.Code);
    }

    [Fact]
    public void Approve_PromotesApplicationToApproved()
    {
        var application = ValidApplication();

        var result = application.Approve(new AccountId(Guid.NewGuid()));

        Assert.True(result.IsSuccess);
        Assert.Equal(HotelierApplicationStatus.Approved, application.Status);
        Assert.NotNull(application.ReviewedAt);
    }

    [Fact]
    public void Reject_RequiresReasonAndStoresIt()
    {
        var application = ValidApplication();

        var missingReason = application.Reject(new AccountId(Guid.NewGuid()), " ");
        var rejected = application.Reject(new AccountId(Guid.NewGuid()), "Registration number could not be verified.");

        Assert.True(missingReason.IsFailure);
        Assert.True(rejected.IsSuccess);
        Assert.Equal(HotelierApplicationStatus.Rejected, application.Status);
        Assert.Equal("Registration number could not be verified.", application.RejectionReason);
    }

    private static HotelierApplication ValidApplication() => HotelierApplication.Create(
        new AccountId(Guid.NewGuid()), "Stayora Holdings", "REG-1", null, "business@example.com",
        "+380501234567", "Stayora Inn", "1 Main Street").Value;
}
