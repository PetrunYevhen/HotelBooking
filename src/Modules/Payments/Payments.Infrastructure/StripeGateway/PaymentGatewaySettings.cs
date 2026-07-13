namespace Payments.Infrastructure.StripeGateway;

public class PaymentGatewaySettings
{
    public PaymentGatewaySettings(string apiBase, string apiKey, bool isMock)
    {
        ApiBase = apiBase;
        ApiKey = apiKey;
        IsMock = isMock;
    }

    public string ApiBase { get; init; }
    public string ApiKey { get; init; }
    public bool IsMock { get; init; }
}