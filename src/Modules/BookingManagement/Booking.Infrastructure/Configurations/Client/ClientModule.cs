using Autofac;
using BookingManagement.Application.Gateways;
using Infrastructure.Client;

namespace BookingManagement.Infrastructure.Configurations.Client;

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
        
        builder.RegisterType<RoomGateway>()
            .As<IRoomGateway>()
            .InstancePerLifetimeScope();
    }
}