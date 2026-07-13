using Autofac;
using Payments.Application.GatewayContract;
using Payments.Infrastructure.StripeGateway;
using Stripe;

namespace Payments.Infrastructure.Configuration.Gateway;

public class StripeGatewayModule : Module
{
    private readonly PaymentGatewaySettings _settings;

    public StripeGatewayModule(PaymentGatewaySettings settings)
    {
        _settings = settings;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(_settings).AsSelf();

        builder.Register(_ => new StripeClient(
                apiKey: _settings.ApiKey,
                apiBase: _settings.ApiBase))
            .As<IStripeClient>()
            .SingleInstance();

        if (_settings.IsMock)
        {
            builder.RegisterType<StripeMockPaymentGatewayClient>()
                .As<IPaymentGatewayClient>()
                .InstancePerLifetimeScope();
        }
        else
        {
            builder.RegisterType<StripePaymentGatewayClient>()
                .As<IPaymentGatewayClient>()
                .InstancePerLifetimeScope();
        }
    }
}
