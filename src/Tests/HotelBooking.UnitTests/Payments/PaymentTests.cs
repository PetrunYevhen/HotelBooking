using Payments.Domain.Entities;
using Payments.Domain.Entities.Enums;
using SharedKernel.ValueObjects;
using Xunit;

namespace HotelBooking.UnitTests.Payments;

public sealed class PaymentTests
{
    [Fact]
    public void Create_WhenPending_HasNoCompletionTimestampOrGatewayReference()
    {
        var payment = Payment.Create(Guid.NewGuid(), Money.Create(500, "UAH").Value).Value;

        Assert.Null(payment.CompletedAt);
        Assert.Null(payment.ExternalTransactionId);
    }

    [Fact]
    public void Complete_WhenPending_StoresTransactionAndChangesStatus()
    {
        var payment = Payment.Create(Guid.NewGuid(), Money.Create(500, "UAH").Value).Value;

        var result = payment.Complete("transaction-123");

        Assert.True(result.IsSuccess);
        Assert.Equal(PaymentStatus.Completed, payment.Status);
        Assert.Equal("transaction-123", payment.ExternalTransactionId);
    }

    [Fact]
    public void Refund_WhenPaymentIsPending_ReturnsFailure()
    {
        var payment = Payment.Create(Guid.NewGuid(), Money.Create(500, "UAH").Value).Value;

        var result = payment.Refund();

        Assert.True(result.IsFailure);
        Assert.Equal(PaymentStatus.Pending, payment.Status);
    }
}
