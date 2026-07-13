using Reviews.Domain.ValueObjects;
using Xunit;

namespace HotelBooking.UnitTests.Reviews;

public sealed class RatingScoreTests
{
    [Theory]
    [InlineData(0.9)]
    [InlineData(5.1)]
    public void Create_WhenOutsideAllowedRange_ReturnsFailure(double value)
    {
        var result = RatingScore.Create(value);

        Assert.True(result.IsFailure);
        Assert.Equal("RatingScore.Invalid", result.Error.Code);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3.5)]
    [InlineData(5)]
    public void Create_WhenInsideAllowedRange_ReturnsSuccess(double value)
    {
        var result = RatingScore.Create(value);

        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value.Value);
    }
}
