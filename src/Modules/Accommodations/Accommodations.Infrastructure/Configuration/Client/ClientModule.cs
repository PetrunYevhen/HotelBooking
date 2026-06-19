using Accommodations.Infrastructure.Configuration.Client.Subsctiptions;
using Autofac;
using Infrastructure.Client;

namespace Accommodations.Infrastructure.Configuration.Client;

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
    }
    
    public void RegisterSubscriptions(ILifetimeScope scope)
    {
        new GetRoomAvailabilitySubscription().Subscride(_client, scope);
        new GetRoomPriceSubscription().Subscride(_client, scope);
    }
}