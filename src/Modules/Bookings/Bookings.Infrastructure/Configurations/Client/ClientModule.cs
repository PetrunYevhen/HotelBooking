using Autofac;
using Bookings.Application.ClientContracts;
using Bookings.Application.Services.AddOns;
using Bookings.Application.Services.Quotes;
using Bookings.Infrastructure.Configurations.Client.Subsctiptions;
using Infrastructure.Client;

namespace Bookings.Infrastructure.Configurations.Client;

public class ClientModule : Module
{
    private readonly IClient _client;
    public ClientModule(IClient client)
    {
        _client = client;
    }

    protected override void Load(ContainerBuilder builder)
    {
        if (_client != null)
        {
            builder.RegisterInstance(_client);
        }

        builder.RegisterType<AccommodationsClient>()
            .As<IAccommodationsClient>()
            .SingleInstance();
        builder.RegisterType<AddOnPriceCalculationService>()
            .As<IAddOnPriceCalculationService>()
            .InstancePerLifetimeScope();
        builder.RegisterType<BookingQuoteService>()
            .As<IBookingQuoteService>()
            .InstancePerLifetimeScope();
    }

    public void RegisterSubscriptions(ILifetimeScope scope)
    {
        new GetOverlappingRoomIdsSubscription().Subscride(_client, scope);
    }
}
