using SharedKernel.ValueObjects;
using Xunit;

namespace HotelBooking.UnitTests.SharedKernel;

public sealed class MoneyTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WhenAmountIsNotPositive_ReturnsFailure(decimal amount)
    {
        var result = Money.Create(amount, "UAH");

        Assert.True(result.IsFailure);
        Assert.Equal("Money.InvalidAmount", result.Error.Code);
    }

    [Fact]
    public void Create_NormalizesCurrencyToUpperCase()
    {
        var result = Money.Create(100, "uah");

        Assert.True(result.IsSuccess);
        Assert.Equal("UAH", result.Value.Currency);
    }
}
